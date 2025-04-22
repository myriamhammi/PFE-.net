using System.Data.SqlClient;
using FileHelpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace TEST_PFE.Ser
{
    public class DataService
    {
        private readonly string _sqlConnectionString;

        public DataService(IConfiguration configuration)
        {
            _sqlConnectionString = configuration.GetConnectionString("sql_Connection");
        }

        // Traitement dynamique du fichier, sans pré-défini comme Contact
        public async Task<List<Dictionary<string, string>>> ProcessFileAsync(IFormFile file, List<Func<Dictionary<string, string>, Dictionary<string, string>>> transformations = null)
        {
            var records = new List<Dictionary<string, string>>();
            var engine = new FileHelperEngine<DelimitedRecord>();

            // Lire le fichier et le convertir en dictionnaire dynamique
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                // Lire le fichier CSV et créer des enregistrements dynamiques
                var rows = engine.ReadStream(reader).ToList();

                // Transformation dynamique des lignes
                foreach (var row in rows)
                {
                    var record = row.ToDictionary(); // Utiliser ToDictionary pour extraire les valeurs
                    if (transformations != null)
                    {
                        foreach (var transform in transformations)
                        {
                            record = transform(record); // Appliquer les transformations
                        }
                    }
                    records.Add(record);
                }
            }
            return records;
        }

        // Charger les données dans la base de données
        public async Task LoadDataToSQLAsync(List<Dictionary<string, string>> records)
        {
            using (var connection = new SqlConnection(_sqlConnectionString))
            {
                await connection.OpenAsync();

                foreach (var record in records)
                {
                    var columns = string.Join(",", record.Keys);
                    var parameters = string.Join(",", record.Keys.Select(k => $"@{k}"));
                    var query = $"INSERT INTO Contacts ({columns}) VALUES ({parameters})";

                    using (var cmd = new SqlCommand(query, connection))
                    {
                        foreach (var kvp in record)
                        {
                            cmd.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                        }

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
        }
    }

    // Déléguer l'analyse de chaque ligne en "enregistrement dynamique"
    [DelimitedRecord(",")] // spécifier que les champs sont séparés par des virgules
    public class DelimitedRecord
    {
        // Les propriétés dynamiques pour chaque colonne du CSV
        // Cela sera rempli automatiquement lors de l'analyse du fichier CSV
        public string[] Fields { get; set; }

        // Convertir l'enregistrement en dictionnaire clé/valeur
        public Dictionary<string, string> ToDictionary()
        {
            var dict = new Dictionary<string, string>();
            for (int i = 0; i < Fields.Length; i++)
            {
                dict.Add($"Column{i + 1}", Fields[i]);
            }
            return dict;
        }
    }
}
