using Microsoft.AspNetCore.Mvc;
using TEST_PFE.Ser;

namespace TEST_PFE.controllers
{

    [ApiController]
    [Route("api/assistant")]
    [Produces("application/json")]
    public class AssistantController : ControllerBase
    {
        private readonly IAssistantMemoryService _memoryService;

        public AssistantController(IAssistantMemoryService memoryService)
        {
            _memoryService = memoryService;
        }

        [HttpPost("respond")]
        public IActionResult Respond([FromBody] AssistantRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Choice))
                return BadRequest(new { response = "Choix non spécifié." });

            var userId = GetUserId();
            _memoryService.SaveChoice(userId, request.Choice);

            string response = request.Choice switch
            {
                "Rapports" => "Voici les rapports disponibles 📊",
                "Analyser" => "Démarrons l’analyse de vos données 🧠",
                "Predire" => "Lançons la prédiction de vos KPIs 🔮",
                _ => "Option non reconnue."
            };

            return Ok(new { response });
        }

        [HttpGet("welcome")]
        public IActionResult GetWelcomeMessage()
        {
            var userId = GetUserId();
            var memory = _memoryService.GetMemory(userId);

            string suggestion = memory.History.LastOrDefault() switch
            {
                "Rapports" => "Souhaitez-vous revoir vos derniers rapports ?",
                "Analyser" => "On continue l’analyse des données ?",
                "Predire" => "Prêt(e) à prédire vos futurs KPIs ?",
                _ => "Que souhaitez-vous faire aujourd’hui ?"
            };

            string greeting = DateTime.Now.Hour switch
            {
                <= 11 => "☀️ Bonjour",
                <= 17 => "👋 Bon après-midi",
                _ => "🌙 Bonsoir"
            };

            return Ok(new { message = $"{greeting} ! {suggestion}" });
        }

        private string GetUserId()
        {
            // À adapter avec une vraie authentification ou cookie sécurisé
            return Request.Cookies["userId"] ?? "Myriam Hammi";
        }
    }

    public class AssistantRequest
    {
        public string Choice { get; set; }
    }


}
