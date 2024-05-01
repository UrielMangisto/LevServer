namespace LevServer.Services
{
    public class OmdbApiService
    {
        private static readonly string _apiKey = "cae43c7a";
        private static readonly string _baseUrl = "http://www.omdbapi.com/";
        private static readonly string _posterUrl = "https://img.omdbapi.com/?apikey=cae43c7a&";
        public static readonly string _sampleId = "tt0050083";

        public static async Task<string> GetMovieAsync(string title)
        {
            using (var httpClient = new HttpClient())
            {
                var query = $"?apikey={_apiKey}&t={title}&type=movie";
                var url = _baseUrl + query;

                Console.WriteLine(url);

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var movieData = await response.Content.ReadAsStringAsync();

                return movieData;
            }
        }

        public static async Task<string> GetMovieByIDAsync(string id)
        {
            using (var httpClient = new HttpClient())
            {
                var query = $"?apikey={_apiKey}&i={id}&type=movie";
                var url = _baseUrl + query;

                Console.WriteLine(url);

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var movieData = await response.Content.ReadAsStringAsync();

                return movieData;
            }
        }

        public static async Task<string> GetMoviesBySearchAsync(string s, int? y = null, int page = 1)
        {
            using var httpClient = new HttpClient();
            var query = $"?apikey={_apiKey}&type=movie&s=\"{s}\"&page={page}";

            if (y != null)
            {
                query += $"&y={y}";
            }

            var url = _baseUrl + query;

            var response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var movieData = await response.Content.ReadAsStringAsync();

            return movieData;
        }
    }
}
