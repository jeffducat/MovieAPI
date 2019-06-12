using MovieAPIProjectV2.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieAPIProjectV2.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("../Account/Login");
            }
            if (TempData["SearchResults"] != null)
            {
                JObject jData = (JObject)TempData["SearchResults"];
                if(jData["Response"].ToString() == "False")
                {
                    return View();
                }
                List<MovieDisplayContent> movies = new List<MovieDisplayContent>();
                for (int i = 0; i < (int)jData["totalResults"]; i++)
                {
                    MovieDisplayContent newMovie = new MovieDisplayContent((JObject)jData["Search"][i], "search");
                    movies.Add(newMovie);
                    if (i >= 4)
                    {
                        return View(movies);
                    }

                }
                return View(movies);
            }
            return View();           
        }

        public ActionResult SearchDatabase(string searchQuery, string searchYear)
        {
            if(searchQuery == "Grant Chirpus")
            {
                ViewBag.Grant = "https://media0.giphy.com/media/ZZYfo0y5L5XyeV1kzk/giphy.gif";
                return View("Index");
            }
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("../Account/Login");
            }
            string options = string.Empty;
            if (searchQuery != null)
            {
                options += $"&s={searchQuery}";
            }
            if (searchYear != null)
            {
                options += $"&y={searchYear}";
            }
            JObject jData = MovieDAL.GetMovieAPI(options);
            TempData["SearchResults"] = jData;
            return RedirectToAction("Index");
        }
    }
}