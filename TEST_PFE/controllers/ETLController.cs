using Microsoft.AspNetCore.Mvc;
using TEST_PFE.Ser;
using Newtonsoft.Json;
using System.Dynamic;

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
                // Lire le fichier en mémoire
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();

                // Désérialiser le mapping (clé: nom de colonne, valeur: type)
                var mappingDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapping);
                if (mappingDict == null || mappingDict.Count == 0)
                {
                    return BadRequest(new { message = "Le mapping fourni est invalide ou vide." });
                }

                // Transformer les données et convertir au format dynamique
                var transformedData = await _transformationService.TransformDataAsync(fileBytes, mappingDict);
                var dynamicData = ConvertToDynamic(transformedData);

                // Création de la table puis insertion des données
                await _chargementService.CreateTableDynamically(dynamicData, tableName, mappingDict); // Ajout de mappingDict ici
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
        private List<dynamic> ConvertToDynamic(List<Dictionary<string, string>> transformedData)
        {
            var dynamicData = new List<dynamic>();

            foreach (var row in transformedData)
            {
                var expandoObject = new ExpandoObject() as IDictionary<string, object>;

                foreach (var column in row)
                {
                    expandoObject.Add(column.Key, column.Value);
                }

                dynamicData.Add(expandoObject);
            }

            return dynamicData;
        }





        private List<dynamic> ApplyMapping(List<dynamic> data, Dictionary<string, string> mapping)
        {
            var mappedData = new List<dynamic>();

            foreach (var item in data)
            {
                var mappedItem = new ExpandoObject() as IDictionary<string, object>;

                // Appliquer le mappage à chaque propriété de l'élément
                foreach (var kvp in item)
                {
                    if (mapping.ContainsKey(kvp.Key))
                    {
                        var mappedPropertyName = mapping[kvp.Key];
                        mappedItem[mappedPropertyName] = kvp.Value;
                    }
                }

                mappedData.Add(mappedItem);
            }

            return mappedData;
        }

        // Méthode de conversion : retourne une liste de dynamic (ExpandoObject)
        private List<dynamic> ConvertToObjectList(List<Dictionary<string, string>> stringData)
        {
            var objectData = new List<dynamic>();

            foreach (var row in stringData)
            {
                dynamic newRow = new ExpandoObject();
                var rowDict = (IDictionary<string, object>)newRow;

                // Convertir chaque ligne en dynamic (ExpandoObject)
                foreach (var kvp in row)
                {
                    rowDict[kvp.Key] = kvp.Value;  // Ajouter à l'ExpandoObject
                }

                objectData.Add(newRow);
            }

            return objectData;
        }
    }
}






