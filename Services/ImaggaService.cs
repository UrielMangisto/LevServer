using Microsoft.AspNetCore.Mvc;

namespace LevServer.Services
{
    public class ImaggaService
    {
        private readonly static HttpClient _httpClient;
        private const string ApiBaseUrl = "https://api.imagga.com";
        private readonly static string _key = "acc_1eaa87190489d9d";
        private readonly static string _secret = "2a43ee0d2c36374d43fd6c35be810664";

        public async Task<string> GetImageTagsAsync(string imageUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiBaseUrl}/tags?image_url={imageUrl}");
                response.EnsureSuccessStatusCode(); // Throw an exception if the request fails

                var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException ex)
            {
                return ex.Message; // Handle the exception as needed
            }
        }
    }
}
