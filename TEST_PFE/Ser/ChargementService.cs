using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TEST_PFE.Ser;




public class SqlColumn
{
    public string Name { get; set; }
    public string DataType { get; set; }
}

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

    public async Task<List<string>> GetTableNames()
    {
        var tables = new List<string>();
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception and return a more detailed error message
            Console.WriteLine("Error: " + ex.Message);  // Tu peux loguer l'exception ici
            throw new Exception("Erreur lors de la récupération des tables SQL", ex);  // Relance l'exception pour propagation
        }
        return tables;
    }

    public async Task<List<SqlColumn>> GetSqlTableStructure(string tableName)
    {
        var columns = new List<SqlColumn>();

        using (var connection = new SqlConnection("Votre chaîne de connexion"))
        {
            await connection.OpenAsync();
            var query = $"SELECT COLUMN_NAME, DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";

            using (var command = new SqlCommand(query, connection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    columns.Add(new SqlColumn
                    {
                        Name = reader.GetString(0),
                        DataType = reader.GetString(1)
                    });
                }
            }
        }

        return columns;
    }


    public async Task<List<Dictionary<string, object>>> GetDataFromTable(string tableName)
    {
        var result = new List<Dictionary<string, object>>();

        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var command = new SqlCommand($"SELECT * FROM {tableName}", connection);

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row.Add(reader.GetName(i), reader.GetValue(i));
                    }
                    result.Add(row);
                }
            }
        }

        return result;
    }






}