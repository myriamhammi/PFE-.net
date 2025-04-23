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

       
    }
}
