using Microsoft.AspNetCore.Mvc;
using System.Text;
using TEST_PFE.Models;
using System.Text.Json;
using TEST_PFE.Ser;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Net.Http;
using TEST_PFE.controllers;

namespace TEST_PFE.Controllers
{
    [Route("api/prediction")]
    [ApiController]
    public class PredictionController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _connectionString;
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger<PredictionController> _logger;



        public PredictionController(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<PredictionController> logger)
        {
            _httpClient = new HttpClient();
            _httpClientFactory = httpClientFactory;
            _httpClient.BaseAddress = new Uri("http://127.0.0.1:5001/"); // Flask API

            _connectionString = configuration.GetValue<string>("Prediction_Connection");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));


        }

        public IActionResult gain_opp()
        {
            return View();
        }

        public IActionResult Sales_Total()
        {
            return View();
        }
        [HttpGet("/Prediction/Order_status")]
        public IActionResult Order_status()
        {
            return View("Order_status");
        }


        [HttpPost]
        public async Task<IActionResult> Predict([FromBody] PredictionInput input)
        {
            // Construction du json pour Flask API
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

            var resultString = await response.Content.ReadAsStringAsync();

            // Supposons que le résultat JSON est { "prediction": 0.85, "model": "rf" }
            var result = JsonSerializer.Deserialize<PredictionResultResponse>(resultString);

            // Enregistre la prédiction dans la base SQL
            await SavePredictionInDbAsync(input, result);

            return Content(resultString, "application/json");
        }
        private async Task SavePredictionInDbAsync(PredictionInput input, PredictionResultResponse result)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
            INSERT INTO PredictionResults
            (ClientName, Industry, Lead_Source, Revenue_Potential, Days_To_Close, Model, PredictionResult, CreatedAt)
            VALUES
            (@ClientName, @Industry, @Lead_Source, @Revenue_Potential, @Days_To_Close, @Model, @PredictionResult, @CreatedAt)";

            using SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ClientName", input.ClientName ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Industry", input.Industry);
            cmd.Parameters.AddWithValue("@Lead_Source", input.Lead_Source);
            cmd.Parameters.AddWithValue("@Revenue_Potential", input.Revenue_Potential);
            cmd.Parameters.AddWithValue("@Days_To_Close", input.Days_to_Close);
            cmd.Parameters.AddWithValue("@Model", input.Model);
            cmd.Parameters.AddWithValue("@PredictionResult", result.Prediction);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<Prediction_2_Response> PredictAsync(Prediction_2 request)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5001/predict", request);

            if (response.IsSuccessStatusCode)
            {
                var predictionResult = await response.Content.ReadFromJsonAsync<Prediction_2_Response>();
                return predictionResult;
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erreur lors de l’appel à l’API Flask : {error}");
            }
        }

        // Action HTTP POST exposée à ton front
        [HttpPost("predict")]
        public async Task<IActionResult> Post([FromBody] Prediction_2 request)
        {
            try
            {
                var result = await PredictAsync(request);
                return Ok(result); // Renvoie : { client_status, model, prediction }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Endpoint pour Order status

        // GET: /api/prediction/stats/overview
        [HttpGet("stats/overview")]
        public async Task<IActionResult> GetStats()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5001/stats/overview");
            var json = await response.Content.ReadAsStringAsync();

            // Désérialise la chaîne JSON pour la retourner comme objet JSON propre
            var data = JsonSerializer.Deserialize<object>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Json(data);
        }


        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentPredictions()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5001/predictions/recent");
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5001/clients");
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        [HttpPost]
        public async Task<IActionResult> PostPrediction([FromBody] Prediction3 data)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();


                // Sérialisation correcte en JSON
                var json = JsonSerializer.Serialize(data);
                Console.WriteLine("==== JSON envoyé à Flask ====");
                Console.WriteLine(json);
                //Console.WriteLine("Payload envoyé à Flask : " + json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:5001/predict", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("==== Réponse Flask ====");
                Console.WriteLine($"Code HTTP : {(int)response.StatusCode}");
                Console.WriteLine($"Contenu : {responseContent}");


                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, $"Erreur depuis Flask API : {responseContent}");
                }

                return Content(responseContent, "application/json");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
                return StatusCode(500, $"Erreur interne : {ex.Message}");
            }


        }



        [HttpGet("stats/by_client")]
        public async Task<IActionResult> GetClientStats()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://localhost:5001/stats/by_client");
            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        public async Task<PredictionSummary?> GetPredictionSummaryAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5000/api/prediction/summary");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var summary = JsonSerializer.Deserialize<PredictionSummary>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return summary;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'appel à l'API Flask : {ex.Message}");
                return null;
            }



        }
        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await GetPredictionSummaryAsync();

            if (summary == null)
                return StatusCode(500, "Erreur lors de l'appel à l'API Flask.");

            return Ok(summary);
        }
    }


    // Modèle pour désérialiser la réponse Flask
    public class PredictionResultResponse
    {
        public double Prediction { get; set; }
        public string Model { get; set; }
    }

        public class PredictionSummary
        {
            public int total_prediction { get; set; }
            public int validations { get; set; }
            public int annulations { get; set; }
            public double taux_annulation { get; set; }
        }

}
