using Microsoft.AspNetCore.Mvc;
using System.Text;
using TEST_PFE.Models;
using System.Text.Json;

namespace TEST_PFE.Controllers
{
   
    public class PredictionController : Controller
    {
        private readonly HttpClient _httpClient;

        public PredictionController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://127.0.0.1:5001/"); // Flask API
        }

        public IActionResult gain_opp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Predict([FromBody] PredictionInput input)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(new
            {
                model = input.Model,
                features = new object[]
                {
                input.Industry, input.Lead_Source,
                input.Revenue_Potential, input.Days_to_Close
                }
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("predict", jsonContent);
            if (!response.IsSuccessStatusCode)
                return BadRequest("Erreur lors de l'appel de l'API Flask.");

            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }
    }
}
