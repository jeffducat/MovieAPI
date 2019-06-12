using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace MovieAPIProjectV2.Models
{
    public class MovieDAL
    {
        private static readonly string APIKey = GetAPIKey()[0];
        private static readonly string TMDbAPIKey = GetAPIKey()[1];
        public static JObject GetMovieAPI(string options)
        {

            HttpWebRequest request = WebRequest.CreateHttp($"http://www.omdbapi.com/?apikey={APIKey}{options}");

            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader data = new StreamReader(response.GetResponseStream());

                JObject movieData = JObject.Parse(data.ReadToEnd());
                return movieData;
            }
            return null;
        }

        public static JObject GetVideoAPI(string options, string externalSource = null)
        {
            //options should be:
            //find/{external_id} to find the movie Id
            //movie/{movie_id}/videos to find the video trailer
            //externalSource should equal "&external_source=imdb_id"
            string url = $"https://api.themoviedb.org/3/{options}?api_key={TMDbAPIKey}&language=en-US{externalSource}";

            HttpWebRequest request = WebRequest.CreateHttp(url);

            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader data = new StreamReader(response.GetResponseStream());
                
                JObject movieData = JObject.Parse(data.ReadToEnd());
                return movieData;
            }
            else if(response.StatusCode == HttpStatusCode.NotFound)
            {
                StreamReader data = new StreamReader(response.GetResponseStream());

                JObject movieData = JObject.Parse(data.ReadToEnd());
                return movieData;
            }
            return null;
        }
       
        private static string[] GetAPIKey()
        {
            int count = 0;
            string APIKeyFile;
            string[] APIKeys = new string[2];
            string fileName = @"APIKey.txt";
            string binaryPath = AppDomain.CurrentDomain.BaseDirectory + fileName;

            StreamReader myFile = new StreamReader(binaryPath);

            try
            {
                APIKeyFile = myFile.ReadToEnd();
                foreach (string line in APIKeyFile.Split('\n'))
                {
                    if (line.Contains("\r"))
                    {
                        APIKeys[count] = line.Split('\r')[0];
                    }
                    else
                    {
                        APIKeys[count] = line;
                    }                    
                    count++;
                }
            }
            finally
            {
                myFile.Close();
            }

            return APIKeys;
        }
    }
}