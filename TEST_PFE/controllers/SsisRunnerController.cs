using Microsoft.AspNetCore.Mvc;
using TEST_PFE.Models;

namespace TEST_PFE.Controllers  
{
    public class SsisRunnerController : Controller
    {
        [HttpGet]
        public IActionResult SsisRunner()
        {
            return View("~/Views/Data/SsisRunner.cshtml"); // ← Charge la bonne vue
        }

        [HttpPost]
       //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RunETL()
        {
            string package1 = @"C:\Users\meria\source\repos\PFE_EY\PFE_EY\Package1.dtsx";
            string package2 = @"C:\Users\meria\source\repos\PFE_EY\PFE_EY\Package2.dtsx";

            bool success1 = await SsisPackageRunner.RunPackageAsync(package1);
            bool success2 = await SsisPackageRunner.RunPackageAsync(package2);

            string message = (success1 && success2)
                ? "✅ ETL terminé avec succès !"
                : "❌ Erreur lors de l'exécution des packages.";

            return Content(message); // Retourne juste le message texte
        }

    }
}
