using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TEST_PFE.Ser;

[ApiController]
[Route("api/[controller]")]
public class DynamicsController : ControllerBase
{
    private readonly DataSyncService _dataSyncService;
    private readonly ILogger<DynamicsController> _logger; // Déclare une instance de ILogger

    // Injecte ILogger dans le constructeur
    public DynamicsController(DataSyncService dataSyncService, ILogger<DynamicsController> logger)
    {
        _dataSyncService = dataSyncService;
        _logger = logger; // Initialisation de _logger
    }

    [HttpPost("{tableName}")]
    public async Task<IActionResult> PushToDynamics(string tableName)
    {
        try
        {
            // Appel à la méthode SyncSqlTableToDataverse et récupération du message de réponse
            string result = await _dataSyncService.SyncSqlTableToDataverse(tableName);

            // Retourne la réponse avec le message du résultat
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            // Gestion des exceptions
            return StatusCode(500, new { message = "An error occurred while syncing the table.", error = ex.Message });
        }
    }

    /// <summary>
    /// Crée une table personnalisée dans Dynamics 365 à partir du nom donné.
    /// </summary>
    /// <param name="tableName">Nom de la table (sans espaces ni caractères spéciaux)</param>
    [HttpPost("create-table/{tableName}")]
    public async Task<IActionResult> CreateTableInDynamics(string tableName)
    {
        try
        {
            // Log de démarrage de la création
            _logger.LogInformation($"Demande de création de la table: {tableName}");

            // Appel à la méthode SyncSqlTableToDataverse et récupération du message de réponse
            string result = await _dataSyncService.SyncSqlTableToDataverse(tableName);

            // Log de succès
            _logger.LogInformation($"Table {tableName} créée avec succès.");

            // Retourner la réponse avec le message du résultat
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            // Log d'erreur
            _logger.LogError($"Erreur lors de la création de la table {tableName}: {ex.Message}");

            // Gestion des exceptions
            return StatusCode(500, new { message = "Une erreur est survenue lors de la création de la table.", error = ex.Message });
        }
    }
}
