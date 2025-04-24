using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TEST_PFE.Ser;

public class DataTransformationService : IDataTransformationService
{
    public async Task<List<dynamic>> TransformAndNormalizeDataAsync(byte[] fileBytes, Dictionary<string, string> mapping)
    {
        var rawData = await TransformDataAsync(fileBytes, mapping);
        return NettoyerColonnes(rawData);
    }

    public async Task<List<Dictionary<string, string>>> TransformDataAsync(byte[] fileBytes, Dictionary<string, string> mapping)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var transformedData = new List<Dictionary<string, string>>();

        try
        {
            Console.WriteLine("Mapping des colonnes :");
            foreach (var key in mapping.Keys)
            {
                Console.WriteLine($"Mapping clé attendue : '{key}'");
            }

            using var stream = new MemoryStream(fileBytes);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var headerRead = false;
            string[] columnNames = null;

            do
            {
                while (reader.Read())
                {
                    if (!headerRead)
                    {
                        columnNames = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columnNames[i] = NettoyerNomColonne(reader.GetValue(i)?.ToString() ?? $"Colonne{i}");
                            Console.WriteLine($"Nom de colonne extrait et nettoyé : '{columnNames[i]}'");
                        }
                        headerRead = true;
                        continue;
                    }

                    var row = new Dictionary<string, string>();
                    if (reader.FieldCount == columnNames.Length)
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = columnNames[i];
                            var value = reader.GetValue(i)?.ToString() ?? "";

                            if (mapping.ContainsKey(columnName))
                            {
                                var columnType = mapping[columnName];
                                if (columnType == "date" && DateTime.TryParse(value, out var parsedDate))
                                {
                                    row[columnName] = parsedDate.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    row[columnName] = value;
                                }
                            }
                            else
                            {
                                row[columnName] = value; // garder la donnée même si non mappée
                            }
                        }

                        transformedData.Add(row);
                    }
                }
            } while (reader.NextResult());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la transformation des données : {ex.Message}");
            return new List<Dictionary<string, string>>();
        }

        return transformedData;
    }

    private List<dynamic> NettoyerColonnes(List<Dictionary<string, string>> data)
    {
        var cleanedList = new List<dynamic>();

        foreach (var item in data)
        {
            var cleanedRow = new ExpandoObject() as IDictionary<string, object>;

            foreach (var kvp in item)
            {
                var cleanedKey = NettoyerNomColonne(kvp.Key);
                cleanedRow[cleanedKey] = kvp.Value;
            }

            cleanedList.Add(cleanedRow);
        }

        return cleanedList;
    }

    // Retirer les accents et caractères spéciaux
    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    private string NettoyerNomColonne(string colonne)
    {
        if (string.IsNullOrWhiteSpace(colonne))
            return string.Empty;

        // On retire les accents uniquement
        var withoutDiacritics = RemoveDiacritics(colonne);

        // On remplace les caractères spéciaux et on nettoie
        var cleaned = withoutDiacritics
            .Trim()
            .Replace(" ", "_")
            .Replace(".", "_")
            .Replace("-", "_")
            .Replace("(", "")
            .Replace(")", "")
            .Replace("/", "_");

        return cleaned.ToUpperInvariant(); // Finalement on met en majuscule
    }




}