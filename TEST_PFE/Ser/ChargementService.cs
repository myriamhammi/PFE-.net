using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TEST_PFE.Ser;

public class ChargementService : IChargementService
{
    private readonly string _connectionString;

    // Injection de la chaîne de connexion via le constructeur
    public ChargementService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SQL_Connection");
    }

   
    public async Task CreateTableDynamically(List<dynamic> transformedData, string tableName, Dictionary<string, string> mapping)
    {
        var columnDefinitions = new List<string>();

        if (mapping == null || mapping.Count == 0)
        {
            throw new Exception("Le mapping des colonnes est vide ou invalide.");
        }

        foreach (var column in mapping)
        {
            var columnName = column.Key;
            var columnType = ConvertToSqlType(column.Value); // à ajouter ci-dessous

            // Échapper les noms de colonnes qui contiennent des espaces ou des caractères spéciaux
            columnName = columnName.Replace(" ", "_"); // Remplacer les espaces par des underscores
            columnName = $"[{columnName}]"; // Ajouter des crochets autour du nom de la colonne

            columnDefinitions.Add($"{columnName} {columnType}");
        }

        var createTableQuery = $"CREATE TABLE [{tableName}] ({string.Join(", ", columnDefinitions)});";

        await ExecuteSqlAsync(createTableQuery);
    }




    private string ConvertToSqlType(string type)
    {
        return type.ToLower() switch
        {
            "texte" => "NVARCHAR(MAX)",
            "entier" => "INT",
            "decimal" => "DECIMAL(18,2)",
            "date" => "DATETIME",
            _ => "NVARCHAR(MAX)"
        };
    }




    private async Task ExecuteSqlAsync(string query)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand(query, connection))
            {
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task InsertDataDynamically(List<dynamic> transformedData, string tableName)
    {
        if (transformedData.Count == 0)
            return;

        foreach (var row in transformedData)
        {
            var rowDict = (IDictionary<string, object>)row;

            // Colonnes échappées : [CODE FACTURE], [DATE FACTURE], ...
            var columnNames = rowDict.Keys.Select(name => $"[{name}]");
            var columns = string.Join(", ", columnNames);

            // Paramètres : @param0, @param1, ...
            var parameters = rowDict.Keys.Select((_, index) => $"@param{index}");
            var values = string.Join(", ", parameters);

            var sqlParameters = rowDict.Select((kvp, index) =>
                new SqlParameter($"@param{index}", kvp.Value ?? DBNull.Value)).ToArray();

            var insertQuery = $"INSERT INTO [{tableName}] ({columns}) VALUES ({values});";

            await ExecuteSqlAsync(insertQuery, sqlParameters);
        }
    }


    public async Task ExecuteSqlAsync(string query, SqlParameter[] parameters)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters);
                await command.ExecuteNonQueryAsync();
            }
        }
    }


}
