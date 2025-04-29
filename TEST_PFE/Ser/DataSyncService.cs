using System;
using TEST_PFE.Ser;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace TEST_PFE.Ser
{
    

    public class DataSyncService
    {
        private readonly DynamicsService _dynamicsService;
        private readonly IChargementService _chargementService;
        private readonly ILogger<DataSyncService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;


        // Constructeur avec l'injection des services nécessaires
        public DataSyncService(DynamicsService dynamicsService, IChargementService chargementService, ILogger<DataSyncService> logger, IHttpClientFactory httpClientFactory)
        {
            _dynamicsService = dynamicsService;
            _chargementService = chargementService;
            _logger = logger;
            _httpClientFactory = httpClientFactory;


        }

        public async Task<string> SyncSqlTableToDataverse(string tableName)
        {
            try
            {
                var tableData = await _chargementService.GetDataFromTable(tableName);

                if (tableData == null || tableData.Count == 0)
                {
                    _logger.LogWarning($"No data found to sync for table {tableName}.");
                    return "No data to sync.";
                }

                var response = await _dynamicsService.SendDataToDynamicsAsync(tableData);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Successfully synced table {tableName} to Dynamics.");
                    return "Sync successful!";
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error syncing to Dynamics: {errorDetails}");
                    return $"Error syncing: {errorDetails}";
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"HTTP error during sync: {httpEx.Message}");
                return $"HTTP error during sync: {httpEx.Message}";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception during sync: {ex.Message}");
                return $"Server error during sync: {ex.Message}";
            }
        }

        public async Task<string> CreateTableFromSqlToDataverse(string tableName, string accessToken)
        {
            try
            {
                // Récupération de la structure de la table depuis SQL Server
                var sqlTableStructure = await _chargementService.GetSqlTableStructure(tableName);  // Méthode pour récupérer les colonnes

                if (sqlTableStructure == null || sqlTableStructure.Count == 0)
                {
                    _logger.LogWarning($"La structure de la table {tableName} est vide.");
                    return "Structure de la table non disponible.";
                }

                // Créer la table dans Dynamics
                var response = await CreateTableInDataverse(tableName, tableName + "s", accessToken); // Utilisation de la méthode existante pour créer la table

                // Ensuite, ajouter les champs à la table Dynamics
                foreach (var column in sqlTableStructure)
                {
                    await AddFieldToDynamicsTable(tableName, column, accessToken);
                }

                return $"Table {tableName} créée et structure ajoutée avec succès.";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la création de la table {tableName} : {ex.Message}");
                return $"Erreur lors de la création de la table : {ex.Message}";
            }
        }

        private async Task AddFieldToDynamicsTable(string tableName, SqlColumn column, string accessToken)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string typeMetadata = GetMetadataType(column.DataType);

                var schemaName = $"{column.Name.ToLower()}_field"; // Evite les conflits de nom avec des mots réservés

                var payload = new Dictionary<string, object>
            {
                { "@odata.type", typeMetadata },
                { "SchemaName", schemaName },
                { "DisplayName", new Dictionary<string, object>
                    {
                        { "LocalizedLabels", new[] { new Dictionary<string, object>
                            {
                                { "Label", column.Name },
                                { "LanguageCode", 1033 }
                            } }
                        }
                    }
                },
                { "Description", new Dictionary<string, object>
                    {
                        { "LocalizedLabels", new[] { new Dictionary<string, object>
                            {
                                { "Label", $"Field for {column.Name}" },
                                { "LanguageCode", 1033 }
                            } }
                        }
                    }
                },
                { "RequiredLevel", new Dictionary<string, object>
                    {
                        { "Value", "None" }
                    }
                }
            };

                // Ajout spécifique selon le type
                if (column.DataType.ToLower() == "varchar" || column.DataType.ToLower() == "nvarchar")
                {
                    payload.Add("MaxLength", 255); // Ou personnaliser selon besoin
                }
                else if (column.DataType.ToLower() == "int" || column.DataType.ToLower() == "bigint")
                {
                    payload.Add("MinValue", 0);
                    payload.Add("MaxValue", 1000000);
                }

                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                var apiUrl = $"https://org97d7668c.api.crm4.dynamics.com/api/data/v9.2/EntityDefinitions(LogicalName='{tableName.ToLower()}')/Attributes";

                var response = await httpClient.PostAsync(apiUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Champ {column.Name} ajouté à la table {tableName} avec succès.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erreur création champ {column.Name} : {errorContent}");
                    throw new Exception($"Échec de l'ajout du champ {column.Name}. Détails : {errorContent}");
                }
            }
        }

        private string GetMetadataType(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "varchar":
                case "nvarchar":
                    return "Microsoft.Dynamics.CRM.StringAttributeMetadata";
                case "int":
                case "bigint":
                case "smallint":
                case "tinyint":
                    return "Microsoft.Dynamics.CRM.IntegerAttributeMetadata";
                case "datetime":
                case "date":
                case "timestamp":
                    return "Microsoft.Dynamics.CRM.DateTimeAttributeMetadata";
                case "bit":
                    return "Microsoft.Dynamics.CRM.BooleanAttributeMetadata";
                case "float":
                case "decimal":
                    return "Microsoft.Dynamics.CRM.DecimalAttributeMetadata";
                default:
                    throw new Exception($"Type SQL '{sqlType}' non pris en charge pour Dataverse.");
            }
        }



        public async Task<string> CreateTableInDataverse(string schemaName, string displayName, string accessToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Définir le PrimaryAttribute (généralement un attribut comme DISPLAYNAME ou MAIL dans ce cas)
                var primaryAttribute = $"{schemaName}_name";

                // Payload ajusté avec PrimaryAttribute
                var payload = new Dictionary<string, object>
            {
                { "SchemaName", schemaName },
                { "DisplayName", new Dictionary<string, object>
                    {
                        { "@odata.type", "Microsoft.Dynamics.CRM.Label" },
                        { "LocalizedLabels", new[] { new Dictionary<string, object>
                        {
                            { "Label", displayName },
                            { "LanguageCode", 1033 }  // Code de langue pour l'anglais
                        } } }
                    }
                },
                { "Description", new Dictionary<string, object>
                    {
                        { "@odata.type", "Microsoft.Dynamics.CRM.Label" },
                        { "LocalizedLabels", new[] { new Dictionary<string, object>
                        {
                            { "Label", $"Table for {displayName}" },
                            { "LanguageCode", 1033 }  // Code de langue pour l'anglais
                        } } }
                    }
                },
                { "OwnershipType", "UserOwned" }, // OwnershipType
                { "HasActivities", false },  // Pas d'activités
                { "HasNotes", true },  // Autoriser les notes
                { "PrimaryNameAttribute", $"{schemaName}_name" }
            };

                // Sérialisation du payload en JSON avec Newtonsoft.Json
                var jsonContent = new StringContent(
                    JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    "application/json"
                );

                // Envoi de la requête HTTP POST
                var response = await httpClient.PostAsync(
                    "https://org97d7668c.api.crm4.dynamics.com/api/data/v9.2/EntityDefinitions",
                    jsonContent
                );

                // Vérification de la réponse HTTP
                if (response.IsSuccessStatusCode)
                {
                    return $"✅ Table {displayName} created successfully.";
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to create table. Details: {errorContent}");
                }
            }
        }





    }
}