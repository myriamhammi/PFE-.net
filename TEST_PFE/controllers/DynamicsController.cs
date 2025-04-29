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

            // Obtenir le token d'accès OAuth
            string accessToken = await OAuthHelper.GetAccessTokenAsync();

            // Générer le nom au pluriel (exemple : "Customer" -> "Customers")
            string pluralName = tableName + "s";  // Logique de base, tu peux l'adapter selon tes besoins

            // Appel de la méthode de création de table
            string result = await _dataSyncService.CreateTableInDataverse(
                schemaName: tableName,           // nom technique de la table
                displayName: tableName,          // nom visible dans l’interface
                accessToken: accessToken        // Token d'accès
            );

            _logger.LogInformation($"Table {tableName} créée avec succès.");
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erreur lors de la création de la table {tableName}: {ex.Message}");
            return StatusCode(500, new { message = "Une erreur est survenue lors de la création de la table.", error = ex.Message });
        }
    }

    [HttpPost("create-table-from-sql/{tableName}")]
    public async Task<IActionResult> CreateTableFromSqlToDynamics(string tableName)
    {
        try
        {
            _logger.LogInformation($"Demande de création de la table depuis SQL: {tableName}");

            // Obtenir le token d'accès OAuth
            string accessToken = await OAuthHelper.GetAccessTokenAsync();

            // Appel de la méthode pour créer la table dans Dynamics
            string result = await _dataSyncService.CreateTableFromSqlToDataverse(tableName, accessToken);

            _logger.LogInformation($"Table {tableName} créée depuis SQL avec succès.");
            return Ok(new { message = result });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erreur lors de la création de la table {tableName} depuis SQL : {ex.Message}");
            return StatusCode(500, new { message = "Une erreur est survenue lors de la création de la table.", error = ex.Message });
        }
    }



}