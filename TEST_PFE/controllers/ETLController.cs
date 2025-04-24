using Microsoft.AspNetCore.Mvc;
using TEST_PFE.Ser;
using Newtonsoft.Json;
using System.Dynamic;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Text;

public class TableRequest
{
    public string TableName { get; set; }
}

namespace TEST_PFE.Controllers
{
    [Route("ETL")]
    [ApiController]
    public class ETLController : ControllerBase
    {
        private readonly IDataTransformationService _transformationService;
        private readonly IChargementService _chargementService;

        public ETLController(IDataTransformationService transformationService, IChargementService chargementService)
        {
            _transformationService = transformationService;
            _chargementService = chargementService;
        }

        [HttpPost("ChargerDonneesTransformees")]
        public async Task<IActionResult> ChargerDonneesTransformees([FromForm] IFormFile file, [FromForm] string tableName, [FromForm] string mapping)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Le fichier est vide ou manquant." });
            }

            if (string.IsNullOrWhiteSpace(tableName))
            {
                return BadRequest(new { message = "Le nom de la table est requis." });
            }

            if (string.IsNullOrWhiteSpace(mapping))
            {
                return BadRequest(new { message = "Le mapping est requis." });
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                var mappingDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapping);
                if (mappingDict == null || mappingDict.Count == 0)
                {
                    return BadRequest(new { message = "Le mapping fourni est invalide ou vide." });
                }

                // Utilisation directe de la méthode de transformation + nettoyage
                var dynamicData = await _transformationService.TransformAndNormalizeDataAsync(fileBytes, mappingDict);

                await _chargementService.CreateTableDynamically(dynamicData, tableName, mappingDict);
                await _chargementService.InsertDataDynamically(dynamicData, tableName);

                return Ok(new
                {
                    message = "La table a été créée avec succès et les données ont été chargées.",
                    data = dynamicData
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erreur lors du chargement : {ex.Message}" });
            }
        }





        // Méthode pour convertir les données en objets dynamiques (ExpandoObject)
        //private List<dynamic> ConvertToDynamic(List<Dictionary<string, string>> transformedData)
        //{
        //    var dynamicData = new List<dynamic>();

        //    foreach (var row in transformedData)
        //    {
        //        var expandoObject = new ExpandoObject() as IDictionary<string, object>;

        //        foreach (var column in row)
        //        {
        //            expandoObject.Add(column.Key, column.Value);
        //        }

        //        dynamicData.Add(expandoObject);
        //    }

        //    return dynamicData;
        //}





        //private List<dynamic> ApplyMapping(List<dynamic> data, Dictionary<string, string> mapping)
        //{
        //    var mappedData = new List<dynamic>();

        //    foreach (var item in data)
        //    {
        //        var mappedItem = new ExpandoObject() as IDictionary<string, object>;

        //        // Appliquer le mappage à chaque propriété de l'élément
        //        foreach (var kvp in item)
        //        {
        //            if (mapping.ContainsKey(kvp.Key))
        //            {
        //                var mappedPropertyName = mapping[kvp.Key];
        //                mappedItem[mappedPropertyName] = kvp.Value;
        //            }
        //        }

        //        mappedData.Add(mappedItem);
        //    }

        //    return mappedData;
        //}

        // Méthode de conversion : retourne une liste de dynamic (ExpandoObject)
        //private List<dynamic> ConvertToObjectList(List<Dictionary<string, string>> stringData)
        //{
        //    var objectData = new List<dynamic>();

        //    foreach (var row in stringData)
        //    {
        //        dynamic newRow = new ExpandoObject();
        //        var rowDict = (IDictionary<string, object>)newRow;

        //        // Convertir chaque ligne en dynamic (ExpandoObject)
        //        foreach (var kvp in row)
        //        {
        //            rowDict[kvp.Key] = kvp.Value;  // Ajouter à l'ExpandoObject
        //        }

        //        objectData.Add(newRow);
        //    }

        //    return objectData;
        //}

        // Méthode pour convertir les données en objets dynamiques (ExpandoObject)
        //[HttpPost("ETL/ChargerDonneesEtEnvoyerCRM")]
        //public async Task<IActionResult> ChargerEtEnvoyerVersCRM(
        //   IFormFile file,
        //   [FromQuery] string nomEntite, // <- Ici on récupère dynamiquement
        //   [FromBody] Dictionary<string, string> mapping)
        //{
        //    if (string.IsNullOrWhiteSpace(nomEntite))
        //        return BadRequest("Le nom de l'entité CRM est requis.");

        //    using var memoryStream = new MemoryStream();
        //    await file.CopyToAsync(memoryStream);

        //    var donneesNettoyees = await _transformationService.TransformAndNormalizeDataAsync(memoryStream.ToArray(), mapping);

        //    // Envoi vers CRM avec nom d'entité dynamique
        //    await _crmExportService.EnvoyerVersCRM(donneesNettoyees, nomEntite.ToLower()); // Utilisation de await

        //    return Ok(new { message = $"Données envoyées avec succès vers l'entité CRM '{nomEntite}'." });
        //}

        //    [Route("ETL/SendTableToCRM")]
        //    [HttpPost]
        //    public async Task<IActionResult> SendTableToCRM([FromBody] TableRequest tableRequest)
        //    {
        //        if (tableRequest == null || string.IsNullOrEmpty(tableRequest.TableName))
        //        {
        //            return BadRequest(new { message = "Le nom de la table est requis." });
        //        }

        //        try
        //        {
        //            // Connexion à la base de données SQL pour récupérer les données de la table spécifiée
        //            using (var connection = new SqlConnection("SQL_Connection"))
        //            {
        //                await connection.OpenAsync();

        //                // Requête SQL avec paramètres pour éviter les injections SQL
        //                var query = "SELECT * FROM @TableName";
        //                var command = new SqlCommand(query, connection);
        //                command.Parameters.AddWithValue("@TableName", tableRequest.TableName); // Utilisation d'un paramètre SQL sécurisé

        //                using (var reader = await command.ExecuteReaderAsync())
        //                {
        //                    // Récupération des données sous forme de liste d'objets
        //                    var tableData = new List<ExpandoObject>();
        //                    while (await reader.ReadAsync())
        //                    {
        //                        var row = new ExpandoObject() as IDictionary<string, object>;
        //                        for (int i = 0; i < reader.FieldCount; i++)
        //                        {
        //                            row.Add(reader.GetName(i), reader.GetValue(i));
        //                        }
        //                        tableData.Add((ExpandoObject)row);
        //                    }

        //                    // Vérification si des données ont été récupérées
        //                    if (tableData.Count == 0)
        //                    {
        //                        return NotFound(new { message = "Aucune donnée trouvée pour la table spécifiée." });
        //                    }

        //                    // Envoi des données vers CRM
        //                    await _crmExportService.EnvoyerVersCRM(tableData, tableRequest.TableName);

        //                    return Ok(new { message = "Table envoyée avec succès au CRM." });
        //                }
        //            }
        //        }
        //        catch (SqlException sqlEx)
        //        {
        //            return StatusCode(500, new { message = $"Erreur SQL lors de l'envoi de la table au CRM : {sqlEx.Message}" });
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, new { message = $"Erreur lors de l'envoi de la table au CRM : {ex.Message}" });
        //        }
        //    }
        //}
    }
}



