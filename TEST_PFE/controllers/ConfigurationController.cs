using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEST_PFE.Models;

namespace TEST_PFE.controllers
{
    public class ConfigurationController : Controller
    {
        private readonly AppDbContext _context;

        public ConfigurationController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _context.Accounts.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var configurations = await _context.ConfigurationTables.ToListAsync();

            ViewData["Accounts"] = accounts;
            ViewData["Products"] = products;
            ViewData["Configurations"] = configurations;

            return View("~/Views/Data/Index.cshtml"); // Spécifier le chemin exact
        }

    }
}
