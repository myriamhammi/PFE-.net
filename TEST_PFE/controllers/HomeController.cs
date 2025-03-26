using Microsoft.AspNetCore.Mvc;

namespace TEST_PFE.controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Home/Index.cshtml");
        }
       

    }
}