using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using TEST_PFE.Ser;

namespace TEST_PFE.Controllers
{
    [Route("ETL")]
    [ApiController]
    public class ETLController : ControllerBase
    {
        private readonly string _connectionString = "Server=LAPTOP-A8G9933C;Database=Data_TRY;Trusted_Connection=True;TrustServerCertificate=True";
        private readonly string _nomTable = "DonneesETL";
        private readonly IDataTransformationService _transformationService;

        public ETLController(IDataTransformationService transformationService)
        {
            _transformationService = transformationService;
        }

        /// <summary>
        /// Endpoint pour charger un fichier Excel, le transformer et insérer les données dans SQL Server
        /// </summary>
        [HttpPost("ChargerDonnees")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ChargerDonneesDepuisFichier([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Aucun fichier reçu.");

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                var transformedData = await _transformationService.TransformDataAsync(fileBytes);

                var convertedData = transformedData
                    .Select(d => d.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value))
                    .ToList();

                return InsererDonneesDansSql(convertedData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors du traitement du fichier : {ex.Message}");
            }
        }

        /// <summary>
        /// Méthode privée pour insérer des lignes dans la base de données SQL Server
        /// </summary>
        private IActionResult InsererDonneesDansSql(List<Dictionary<string, object>> donnees)
        {
            if (donnees == null || !donnees.Any())
                return BadRequest("Aucune donnée à insérer.");

            try
            {
                using SqlConnection conn = new SqlConnection(_connectionString);
                conn.Open();

                var premiereLigne = donnees.First();

                // Création correcte de la table sans @ variables
                string colonnesSql = string.Join(", ", premiereLigne.Select(kvp => $"[{kvp.Key}] NVARCHAR(MAX)"));
                string createTableQuery = $@"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{_nomTable}' AND xtype='U')
            BEGIN
                CREATE TABLE [{_nomTable}] ({colonnesSql})
            END";

                using (SqlCommand createCmd = new SqlCommand(createTableQuery, conn))
                    createCmd.ExecuteNonQuery();

                // Insertion des données ligne par ligne
                foreach (var ligne in donnees)
                {
                    var colonnes = string.Join(", ", ligne.Keys.Select(k => $"[{k}]"));

                    // Remplacement des espaces pour les paramètres
                    var parametres = ligne.Keys.Select(k => $"@{k.Replace(" ", "_")}");
                    var valeurs = string.Join(", ", parametres);

                    var insertQuery = $"INSERT INTO [{_nomTable}] ({colonnes}) VALUES ({valeurs})";

                    using SqlCommand cmd = new SqlCommand(insertQuery, conn);
                    foreach (var kvp in ligne)
                    {
                        // Remplacement identique ici
                        string paramName = $"@{kvp.Key.Replace(" ", "_")}";
                        cmd.Parameters.AddWithValue(paramName, kvp.Value ?? DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                }


                return Ok(new { message = "Données chargées avec succès dans SQL Server." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Erreur : {ex.Message}");
            }
        }


    }
}
