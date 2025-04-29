using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

//public class DynamicsService
//{

//    private readonly ServiceClient _serviceClient;
//    private readonly string _sqlConnectionString;

//    public DynamicsService(IConfiguration config)
//    {
//        _sqlConnectionString = config.GetConnectionString("SQL_Connection");
//        _serviceClient = new ServiceClient(config.GetConnectionString("Dataverse"));
//    }

//    public async Task<bool> TestSqlConnection()
//    {
//        try
//        {
//            using var conn = new SqlConnection(_sqlConnectionString);
//            await conn.OpenAsync();
//            return true;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Erreur de connexion SQL : {ex.Message}");
//            return false;
//        }
//    }


//    public async Task<List<Dictionary<string, object>>> ReadFromSql(string tableName)
//    {
//        var result = new List<Dictionary<string, object>>();
//        try
//        {
//            using var conn = new SqlConnection(_sqlConnectionString);
//            await conn.OpenAsync();
//            using var cmd = new SqlCommand($"SELECT * FROM {tableName}", conn);
//            using var reader = await cmd.ExecuteReaderAsync();

//            while (await reader.ReadAsync())
//            {
//                var row = new Dictionary<string, object>();
//                for (int i = 0; i < reader.FieldCount; i++)
//                {
//                    row[reader.GetName(i).ToLower()] = reader.GetValue(i);
//                }
//                result.Add(row);
//            }
//        }
//        catch (SqlException ex)
//        {
//            Console.WriteLine($"Erreur SQL : {ex.Message}");
//            throw new ApplicationException("Erreur lors de la lecture des données depuis SQL.", ex);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Erreur générale : {ex.Message}");
//            throw;
//        }

//        return result;
//    }


//    public async Task CreateEntityIfNotExists(string entityName, List<string> columns)
//    {
//        // Vérifier si l'entité existe déjà
//        var retrieveReq = new RetrieveEntityRequest
//        {
//            LogicalName = entityName,
//            EntityFilters = EntityFilters.Entity
//        };

//        try
//        {
//            _serviceClient.Execute(retrieveReq);
//            return; // Déjà existante
//        }
//        catch { }

//        // Créer l'entité
//        var entityMetadata = new EntityMetadata
//        {
//            SchemaName = entityName,
//            LogicalName = entityName,
//            DisplayName = new Label(entityName, 1033),
//            Description = new Label($"Entity for {entityName}", 1033),
//            OwnershipType = OwnershipTypes.UserOwned,
//            IsActivity = false
//        };

//        var createReq = new CreateEntityRequest
//        {
//            Entity = entityMetadata,
//            PrimaryAttribute = new StringAttributeMetadata
//            {
//                SchemaName = "new_name",
//                DisplayName = new Label("Name", 1033),
//                MaxLength = 100,
//                RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None)
//            }
//        };

//        _serviceClient.Execute(createReq);

//        // Ajouter les champs
//        foreach (var column in columns)
//        {
//            var attr = new StringAttributeMetadata
//            {
//                SchemaName = "new_" + column,
//                DisplayName = new Label(column, 1033),
//                MaxLength = 100
//            };

//            var attrReq = new CreateAttributeRequest
//            {
//                EntityName = entityName,
//                Attribute = attr
//            };

//            _serviceClient.Execute(attrReq);
//        }
//    }

//    public async Task InsertIntoDataverse(List<Dictionary<string, object>> rows, string entityName)
//    {
//        foreach (var row in rows)
//        {
//            var entity = new Entity(entityName);

//            foreach (var col in row)
//            {
//                entity["new_" + col.Key] = col.Value;
//            }

//            try
//            {
//                _serviceClient.Create(entity);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Erreur insertion: {ex.Message}");
//            }
//        }
//    }

//}

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

//public class DynamicsService
//{

//    private readonly ServiceClient _serviceClient;
//    private readonly string _sqlConnectionString;

//    public DynamicsService(IConfiguration config)
//    {
//        _sqlConnectionString = config.GetConnectionString("SQL_Connection");
//        _serviceClient = new ServiceClient(config.GetConnectionString("Dataverse"));
//    }

//    public async Task<bool> TestSqlConnection()
//    {
//        try
//        {
//            using var conn = new SqlConnection(_sqlConnectionString);
//            await conn.OpenAsync();
//            return true;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Erreur de connexion SQL : {ex.Message}");
//            return false;
//        }
//    }


//    public async Task<List<Dictionary<string, object>>> ReadFromSql(string tableName)
//    {
//        var result = new List<Dictionary<string, object>>();
//        try
//        {
//            using var conn = new SqlConnection(_sqlConnectionString);
//            await conn.OpenAsync();
//            using var cmd = new SqlCommand($"SELECT * FROM {tableName}", conn);
//            using var reader = await cmd.ExecuteReaderAsync();

//            while (await reader.ReadAsync())
//            {
//                var row = new Dictionary<string, object>();
//                for (int i = 0; i < reader.FieldCount; i++)
//                {
//                    row[reader.GetName(i).ToLower()] = reader.GetValue(i);
//                }
//                result.Add(row);
//            }
//        }
//        catch (SqlException ex)
//        {
//            Console.WriteLine($"Erreur SQL : {ex.Message}");
//            throw new ApplicationException("Erreur lors de la lecture des données depuis SQL.", ex);
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Erreur générale : {ex.Message}");
//            throw;
//        }

//        return result;
//    }


//    public async Task CreateEntityIfNotExists(string entityName, List<string> columns)
//    {
//        // Vérifier si l'entité existe déjà
//        var retrieveReq = new RetrieveEntityRequest
//        {
//            LogicalName = entityName,
//            EntityFilters = EntityFilters.Entity
//        };

//        try
//        {
//            _serviceClient.Execute(retrieveReq);
//            return; // Déjà existante
//        }
//        catch { }

//        // Créer l'entité
//        var entityMetadata = new EntityMetadata
//        {
//            SchemaName = entityName,
//            LogicalName = entityName,
//            DisplayName = new Label(entityName, 1033),
//            Description = new Label($"Entity for {entityName}", 1033),
//            OwnershipType = OwnershipTypes.UserOwned,
//            IsActivity = false
//        };

//        var createReq = new CreateEntityRequest
//        {
//            Entity = entityMetadata,
//            PrimaryAttribute = new StringAttributeMetadata
//            {
//                SchemaName = "new_name",
//                DisplayName = new Label("Name", 1033),
//                MaxLength = 100,
//                RequiredLevel = new AttributeRequiredLevelManagedProperty(AttributeRequiredLevel.None)
//            }
//        };

//        _serviceClient.Execute(createReq);

//        // Ajouter les champs
//        foreach (var column in columns)
//        {
//            var attr = new StringAttributeMetadata
//            {
//                SchemaName = "new_" + column,
//                DisplayName = new Label(column, 1033),
//                MaxLength = 100
//            };

//            var attrReq = new CreateAttributeRequest
//            {
//                EntityName = entityName,
//                Attribute = attr
//            };

//            _serviceClient.Execute(attrReq);
//        }
//    }

//    public async Task InsertIntoDataverse(List<Dictionary<string, object>> rows, string entityName)
//    {
//        foreach (var row in rows)
//        {
//            var entity = new Entity(entityName);

//            foreach (var col in row)
//            {
//                entity["new_" + col.Key] = col.Value;
//            }

//            try
//            {
//                _serviceClient.Create(entity);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Erreur insertion: {ex.Message}");
//            }
//        }
//    }

//}

public class DynamicsService
{
    private readonly HttpClient _httpClient;

    public DynamicsService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> SendDataToDynamicsAsync(object data)
    {
        // Implémentation pour envoyer les données à Dynamics
        var requestUri = "https://org97d7668c.api.crm4.dynamics.com";
        var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(requestUri, content);
        return response;
    }

    public async Task<string> CreateCustomTableAsync(string accessToken, string schemaName, string displayName)
    {
        string url = "https://org97d7668c.api.crm4.dynamics.com/api/data/v9.2/EntityDefinitions";

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
        _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var requestBody = new
        {
            SchemaName = schemaName,
            DisplayName = new { localizedLabels = new[] { new { label = displayName, languageCode = 1036 } } },
            DisplayCollectionName = new { localizedLabels = new[] { new { label = displayName + "s", languageCode = 1036 } } },
            Description = new { localizedLabels = new[] { new { label = "Entité créée dynamiquement", languageCode = 1036 } } },
            OwnershipType = "UserOwned",
            IsActivity = false,
            PrimaryNameAttribute = "name"  // Assurez-vous que "name" est un attribut valide dans l'entité.

        };

        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
            return "✅ Table créée avec succès !";
        else
            return $"❌ Erreur: {response.StatusCode}\n{responseBody}";
    }
}