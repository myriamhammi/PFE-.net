using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using TEST_PFE.Ser;

namespace TEST_PFE.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataTransformationService _dataTransformationService;

        public HomeController(IDataTransformationService dataTransformationService)
        {
            _dataTransformationService = dataTransformationService;
        }

        // Afficher la vue principale
        public IActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }

        // Action pour traiter le fichier
        [HttpPost]
        public async Task<IActionResult> ProcessFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "Aucun fichier téléchargé." });
            }

            try
            {
                // Lire le fichier téléchargé
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var fileBytes = stream.ToArray();

                // Appeler le service de transformation
                var transformedData = await _dataTransformationService.TransformDataAsync(fileBytes);

                // Ici, tu peux insérer les données dans ta base de données
                // await _dataTransformationService.LoadDataToDatabaseAsync(transformedData);

                return Json(new { success = true, message = "Données extraites et transformées avec succès." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Erreur : {ex.Message}" });
            }
        }
    }
}
