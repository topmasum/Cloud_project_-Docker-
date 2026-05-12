using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace RecipeAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class YouTubeController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        // In professional apps, we use IHttpClientFactory to manage connections efficiently
        public YouTubeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("suggest")]
        public async Task<IActionResult> GetVideo([FromQuery] string query)
        {
            // PRO TIP: In your thesis, mention you'd move this to environment variables for security
            string apiKey = "AIzaSyDsdW9VgUmfbmyiu4ABBYp17p04Gee2bCs"; 
            string url = $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={query}+recipe&type=video&key={apiKey}&maxResults=1";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error calling YouTube API");
            }

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var root = doc.RootElement;

            // Check if items list is empty
            if (root.GetProperty("items").GetArrayLength() == 0)
            {
                return NotFound("No recipe videos found for this query.");
            }

            var firstVideo = root.GetProperty("items")[0];
            
            return Ok(new
            {
                title = firstVideo.GetProperty("snippet").GetProperty("title").GetString(),
                videoId = firstVideo.GetProperty("id").GetProperty("videoId").GetString(),
                thumbnail = firstVideo.GetProperty("snippet").GetProperty("thumbnails").GetProperty("high").GetProperty("url").GetString()
            });
        }
    }
}