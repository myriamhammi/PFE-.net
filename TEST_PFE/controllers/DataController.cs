using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using TEST_PFE.Ser;

namespace TEST_PFE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly DataService _dataService;

        public DataController(DataService dataService)
        {
            _dataService = dataService;
        }

        // Endpoint pour charger un fichier et appliquer des transformations
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Aucun fichier téléchargé.");
            }

            try
            {
                // Traitement dynamique du fichier
                var records = await _dataService.ProcessFileAsync(file);

                // Vous pouvez également appliquer des transformations, si nécessaire
                // var transformations = new List<Func<Dictionary<string, string>, Dictionary<string, string>>>
                // {
                //     row => { row["Column1"] = row["Column1"].ToUpper(); return row; }
                // };
                // var records = await _dataService.ProcessFileAsync(file, transformations);

                // Charger les données dans la base de données
                await _dataService.LoadDataToSQLAsync(records);

                return Ok(new { message = "Fichier traité et données insérées avec succès." });
            }
            catch (System.Exception ex)
            {
                // Gérer les erreurs et retourner un message d'erreur
                return StatusCode(500, new { message = $"Une erreur s'est produite : {ex.Message}" });
            }
        }
    }
}
