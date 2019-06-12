using Microsoft.AspNet.Identity;
using MovieAPIProjectV2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieAPIProject.Controllers
{
    public class UserController : Controller
    {
        private MovieDb ORM = new MovieDb();
        // GET: User        
        public ActionResult Movies()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("../Account/Login");
            }
            string thisUser = User.Identity.GetUserId();
            List<FavMovie> favMovies = ORM.FavMovies.SqlQuery("Select * from FavMovie where UserId=@id", new SqlParameter("@id", thisUser)).ToList();                        
            List<MovieDisplayContent> movies = new List<MovieDisplayContent>();
            foreach(FavMovie m in favMovies)
            {
                MovieDisplayContent thisMovie = new MovieDisplayContent(MovieDAL.GetMovieAPI($"&i={m.imdbId}"), "imdbId");
                movies.Add(thisMovie);
            }
            return View(movies);
        }

        public ActionResult RemoveFavorite(string ImdbId)
        {
            string thisUser = User.Identity.GetUserId();
            List<FavMovie> favMovies = ORM.FavMovies.SqlQuery("Select * from FavMovie where UserId=@id", new SqlParameter("@id", thisUser)).ToList();
            FavMovie found = favMovies.Find(x => x.imdbId == ImdbId);
            ORM.FavMovies.Remove(found);
            ORM.SaveChanges();
            return RedirectToAction("Movies");
        }

        public ActionResult AddFavorite(string ImdbId)
        {
            FavMovie addMe = new FavMovie();
            addMe.imdbId = ImdbId;
            addMe.UserId = User.Identity.GetUserId();
            ORM.FavMovies.Add(addMe);
            ORM.SaveChanges();
            return RedirectToAction("Movies");
        }
        
    }
}