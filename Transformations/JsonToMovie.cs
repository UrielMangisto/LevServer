using Newtonsoft.Json;

using LevServer.models;
namespace LevServer.Transformations
{
    public class JsonToMovie
    {
        public  IEnumerable<MovieDto>? GetMovieObjFromJson(string json)
        {
            return JsonMovieDeserialization.GetSearchResults(json);
        }

        public  Movie? GetMovieFromJson(string json)
        {
            return JsonMovieDeserialization.GetMovieResult(json);
        }

      
    }

    internal class SearchResponse
    {
        public List<MovieResult>? Search { get; set; }
        public string? TotalResult { get; set; }
        public bool Response { get; set; }
    }

    //internal class MediaResult
    //{
    //    public string? Title { get; set; }
    //    public string? Year { get; set; }
    //    public string? ImdbID { get; set; }
    //    public string? Poster { get; set; }
    //}

    internal class MovieResult
    {
        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? Genre { get; set; }
        public string? ImdbID { get; set; }
        public string? ImdbRating { get; set; }
        public string? Poster { get; set; }
        public string? Runtime { get; set; }
    }

    internal static class JsonMovieDeserialization
    {
        public static IEnumerable<MovieDto>? GetSearchResults(string json)
        {
            var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(json);

            if(searchResponse?.Search == null)
            {
                return new List<MovieDto>{ };
            }

            return (
                from response in searchResponse?.Search
                select new MovieDto { 
                    Title = response.Title,
                    Year = response.Year,
                    Genre = response.Genre,
                    ImdbID = response.ImdbID,
                    Rating= response.ImdbRating,
                    PosterURL = response.Poster,
                    Time = response.Runtime
                }
                ).ToList();
        }

        public static Movie? GetMovieResult(string json)
        {
            var movieResult = JsonConvert.DeserializeObject<MovieResult>(json);

            if (movieResult == null || movieResult.Title == null)
            {
                return null;
            }

            // parsing the year for movie
            if (!int.TryParse(movieResult.Year, out int year))
            {
                year = 1900;
            }


            // parsing the rating for movie
            if (!double.TryParse(movieResult.ImdbRating, out double rating))
            {
                rating = 0;
            }


            if (!int.TryParse(movieResult.Runtime?.Split()[0], out int time))
            {
                time = 0;
            }

            return new Movie
            { 
                Title = movieResult.Title,
                Genre = movieResult.Genre,
                PosterURL = movieResult.Poster,
                Year = year,
                ImdbID = movieResult.ImdbID,
                Rating = rating,
                Time = time
            };
        }
    }
}
