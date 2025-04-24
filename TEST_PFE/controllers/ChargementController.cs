using Microsoft.AspNetCore.Mvc;
using TEST_PFE.Ser;

namespace TEST_PFE.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChargementController : Controller
    {
        private readonly IChargementService _chargementService;

        // Injection de dépendance via le constructeur
        public ChargementController(IChargementService chargementService)
        {
            _chargementService = chargementService;
        }

        [HttpGet("tables")]
        public async Task<IActionResult> GetSqlTables()
        {
            try
            {
                var tables = await _chargementService.GetTableNames();
                return Ok(tables);  // Renvoie un JSON avec les tables
            }
            catch (Exception ex)
            {
                // Log de l'erreur pour débogage
                Console.WriteLine($"Erreur lors de la récupération des tables : {ex.Message}");
                return StatusCode(500, new { message = "Erreur serveur", error = ex.Message });
            }
        }




    }

}
