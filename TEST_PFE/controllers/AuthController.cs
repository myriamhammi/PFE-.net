using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TEST_PFE.Controllers
{
    public class AuthController : Controller
    {
        private const string ValidEmail = "myriam.hammi@esprit.tn";
        private const string ValidPassword = "Meriam123+";

        [HttpGet]
        public IActionResult Login()
        {
            // Si l'utilisateur est déjà authentifié, rediriger vers la page d'accueil
            if (HttpContext.Session.GetString("IsAuthenticated") == "true")
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (email == ValidEmail && password == ValidPassword)
            {
                // Stocker l'état d'authentification dans la session
                HttpContext.Session.SetString("IsAuthenticated", "true");
                HttpContext.Session.SetString("UserEmail", email);

                return RedirectToAction("Index", "Home");
            }

            TempData["ErrorMessage"] = "Email ou mot de passe incorrect";
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Supprimer les données de session
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}