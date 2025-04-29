using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using TEST_PFE.Models;

namespace TEST_PFE.controllers
{
    public class SalesKPIController : Controller
    {
        //private readonly ServiceClient _serviceClient;
        //private readonly ILogger<SalesKPIController> _logger;
        //private const string DATAVERSE_CONNECTION = "Dataverse";
        //private readonly List<string> validTableNames = new List<string> { "opportunity", "lead", "account" };

        //public SalesKPIController(IConfiguration configuration, ILogger<SalesKPIController> logger)
        //{
        //    if (configuration == null)
        //        throw new ArgumentNullException(nameof(configuration));

        //    string connectionString = configuration.GetConnectionString(DATAVERSE_CONNECTION);
        //    if (string.IsNullOrEmpty(connectionString))
        //        throw new InvalidOperationException($"La chaîne de connexion '{DATAVERSE_CONNECTION}' n'a pas été trouvée.");

        //    _serviceClient = new ServiceClient(connectionString);
        //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //}

        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        // Récupérer les KPI avec leurs données associées
        //        var kpis = await GetSalesKPIsAsync();

        //        // Passer les KPI à la vue via ViewData
        //        ViewData["KPIs"] = kpis;

        //        // Renvoyer la vue principale
        //        return View("~/Views/Data/KPI_Pred.cshtml");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Erreur lors du chargement des KPI");
        //        return StatusCode(500, "Une erreur est survenue lors du traitement de votre demande.");
        //    }
        //}

        //private async Task<List<KPI>> GetSalesKPIsAsync()
        //{
        //    var kpiTasks = new List<Task<TableData>>
        //    {
        //        GetTableMetadataAsync("opportunity"),
        //        GetTableMetadataAsync("lead"),
        //        GetTableMetadataAsync("account")
        //    };

        //    try
        //    {
        //        var metadataResults = await Task.WhenAll(kpiTasks);
        //        return new List<KPI>
        //        {
        //            new KPI("Prédiction du chiffre d'affaires futur", metadataResults[0]),
        //            new KPI("Taux de conversion des leads", metadataResults[1]),
        //            new KPI("Segmentation des clients", metadataResults[2])
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Erreur lors de la récupération des KPIs.");
        //        throw;
        //    }
        //}

        //private async Task<TableData> GetTableMetadataAsync(string tableName)
        //{
        //    if (!validTableNames.Contains(tableName.ToLower()))
        //    {
        //        throw new ArgumentException($"La table '{tableName}' n'est pas valide.");
        //    }

        //    var request = new RetrieveEntityRequest
        //    {
        //        EntityFilters = EntityFilters.Attributes,
        //        LogicalName = tableName,
        //    };

        //    try
        //    {
        //        var response = (RetrieveEntityResponse)await _serviceClient.ExecuteAsync(request);
        //        var entityMetadata = response.EntityMetadata;

        //        List<string> requiredAttributes = GetRequiredAttributesForTable(tableName);

        //        var attributes = entityMetadata.Attributes
        //            .Where(a => a.IsCustomAttribute == false && !string.IsNullOrEmpty(a.LogicalName) && requiredAttributes.Contains(a.LogicalName))
        //            .Select(a => a.LogicalName)
        //            .ToList();

        //        // Pagination: Limiter à un maximum de 100 enregistrements pour cet exemple
        //        List<Dictionary<string, object>> records = await LoadTableRecords(tableName, attributes, 100);

        //        return new TableData(tableName, attributes, records);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Échec de la récupération des métadonnées pour la table {tableName}");
        //        throw;
        //    }
        //}

        //private List<string> GetRequiredAttributesForTable(string tableName)
        //{
        //    return tableName.ToLower() switch
        //    {
        //        "opportunity" => new List<string> { "estimatedvalue", "closeprobability", "name", "createdon", "actualvalue", "opportunityid", "stageid" },
        //        "lead" => new List<string> { "subject", "leadsourcecode", "statuscode", "createdon", "convertedfromleadid", "convertedon" },
        //        "account" => new List<string> { "name", "industrycode", "accountnumber", "revenue", "numberofemployees", "primarycontactid", "address1_city", "address1_country" },
        //        _ => new List<string>()
        //    };
        //}

        //private async Task<List<Dictionary<string, object>>> LoadTableRecords(string tableName, List<string> attributes, int maxRecords)
        //{
        //    QueryExpression query = new QueryExpression(tableName)
        //    {
        //        ColumnSet = new ColumnSet(attributes.ToArray()),
        //        TopCount = maxRecords // Limiter à un nombre maximum de 100 enregistrements
        //    };

        //    var result = await _serviceClient.RetrieveMultipleAsync(query);

        //    return result.Entities.Select(entity =>
        //    {
        //        var record = new Dictionary<string, object>();
        //        foreach (var attribute in attributes)
        //        {
        //            // Vérifier si l'attribut existe avant de l'ajouter
        //            if (entity.Attributes.ContainsKey(attribute))
        //            {
        //                record[attribute] = entity[attribute];
        //            }
        //            else
        //            {
        //                record[attribute] = null;  // Ou une autre valeur par défaut
        //            }
        //        }
        //        return record;
        //    }).ToList();
        //}
        public IActionResult Descriptifs()
        {
            return View("~/Views/SalesKPI/Descriptifs.cshtml");
        }

        public IActionResult Explicatifs()
        {
            return View("~/Views/SalesKPI/Explicatifs.cshtml");
        }

        public IActionResult Predictifs()
        {
            return View();
        }

    }
}

