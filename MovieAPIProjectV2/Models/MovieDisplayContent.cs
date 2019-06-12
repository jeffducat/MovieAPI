using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieAPIProjectV2.Models
{
    public class MovieDisplayContent
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Rated { get; set; }
        public string Genre { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public string Metascore { get; set; }
        public string ImdbId { get; set; }
        public string VideoUrl { get; set; }

        public MovieDisplayContent(JObject jData, string code)
        {

            ImdbId = jData["imdbID"].ToString();
            if (code == "search")
            {
                jData = MovieDAL.GetMovieAPI($"&i={ImdbId}");
            }
            if(jData["Response"].ToString() == "False")
            {
                Title = null;
                Year = null;
                Poster = null;
                Rated = null;
                Genre = null;
                Plot = null;
                Metascore = null;
                ImdbId = null;
                VideoUrl = null;
            }
            else
            {
                Title = jData["Title"].ToString();
                Year = jData["Year"].ToString();
                Poster = jData["Poster"].ToString();              
                Rated = jData["Rated"].ToString();
                Genre = jData["Genre"].ToString();
                Plot = jData["Plot"].ToString();
                Metascore = jData["Metascore"].ToString();
                VideoUrl = GetVideoUrl(ImdbId);
            }           

        }

        private string GetVideoUrl(string ImdbId)
        {
            JObject TMDbMovie = MovieDAL.GetVideoAPI($"find/{ImdbId}", "&external_source=imdb_id");
            JArray movies = (JArray)TMDbMovie["movie_results"];
            if(movies.Count == 0)
            {
                return null;
            }
            string TMDbId = TMDbMovie["movie_results"][0]["id"].ToString();
            //JObject TMDbMovieInfo = MovieDAL.GetVideoAPI($"movie/{TMDbId}");
            JObject TMDbVideo = MovieDAL.GetVideoAPI($"movie/{TMDbId}/videos");
            JArray results = (JArray)TMDbVideo["results"];
            if(results.Count == 0)
            {
                return null;
            }
            return "https://www.youtube.com/embed/" + TMDbVideo["results"][0]["key"].ToString();

        }
    }
}