using Microsoft.AspNetCore.Mvc;
using System.Text;
using TEST_PFE.Models;
using System.Text.Json;
using TEST_PFE.Ser;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace TEST_PFE.Controllers
{
    
    public class PredictionController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _connectionString;




        public PredictionController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://127.0.0.1:5001/"); // Flask API

            _connectionString = configuration.GetValue<string>("Prediction_Connection");
        }

        public IActionResult gain_opp()
        {
            return View();
        }

        public IActionResult Sales_Total()
        {
            return View();
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

    }


    // Modèle pour désérialiser la réponse Flask
    public class PredictionResultResponse
    {
        public double Prediction { get; set; }
        public string Model { get; set; }
    }
}
