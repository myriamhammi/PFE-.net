using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using TEST_PFE.Ser;

public class DataTransformationService : IDataTransformationService
{
    public async Task<List<Dictionary<string, string>>> TransformDataAsync(byte[] fileBytes, Dictionary<string, string> mapping)
    {
        // Enregistrer le fournisseur d'encodage pour Windows-1252
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        var transformedData = new List<Dictionary<string, string>>();

        try
        {
            using (var stream = new MemoryStream(fileBytes))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var headerRead = false;
                string[] columnNames = null;

                // Lire chaque feuille du fichier
                do
                {
                    while (reader.Read())
                    {
                        // Lecture de l'en-tête (noms de colonnes)
                        if (!headerRead)
                        {
                            columnNames = new string[reader.FieldCount];
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                columnNames[i] = reader.GetValue(i)?.ToString() ?? $"Colonne{i}";
                            }
                            headerRead = true;
                            continue; // On saute cette ligne d'en-tête
                        }

                        // Traiter chaque ligne de données après l'en-tête
                        var row = new Dictionary<string, string>();

                        // Vérifier la cohérence du nombre de colonnes
                        if (reader.FieldCount == columnNames.Length)
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var columnName = columnNames[i];
                                var value = reader.GetValue(i)?.ToString() ?? "";  // Gérer les valeurs nulles ou vides

                                // Appliquer le mapping des types (si disponible)
                                if (mapping.ContainsKey(columnName))
                                {
                                    // Par exemple, on peut ajouter une logique de conversion en fonction du type (ex: "texte", "date", etc.)
                                    var columnType = mapping[columnName];

                                    // Logique de transformation par type si nécessaire, exemple simple ici
                                    if (columnType == "date" && DateTime.TryParse(value, out var parsedDate))
                                    {
                                        row[columnName] = parsedDate.ToString("yyyy-MM-dd");
                                    }
                                    else
                                    {
                                        row[columnName] = value; // Conserver la valeur brute par défaut
                                    }
                                }
                                else
                                {
                                    row[columnName] = value; // Si pas de mapping, on conserve la valeur brute
                                }
                            }

                            // Ajouter la ligne transformée à la liste
                            transformedData.Add(row);
                        }
                        else
                        {
                            // Log ou gestion d'erreur si les lignes ne sont pas cohérentes
                            Console.WriteLine($"Ligne avec un nombre de colonnes incohérent (attendu {columnNames.Length}, trouvé {reader.FieldCount}), ignorée.");
                            continue; // Ignorer les lignes incohérentes
                        }
                    }
                } while (reader.NextResult());  // Passer à la feuille suivante si applicable
            }
        }
        catch (Exception ex)
        {
            // Log l'erreur et retourne une liste vide en cas d'exception
            Console.WriteLine($"Erreur lors de la transformation des données : {ex.Message}");
            return new List<Dictionary<string, string>>(); // Retourne une liste vide si une erreur survient
        }

        return transformedData;
    }



    //public async Task<DataTable> TransformWithMapping(Stream stream, Dictionary<string, string> typeMapping)
    //{
    //    stream.Position = 0;
    //    DataTable transformedTable = null;

    //    try
    //    {
    //        using var reader = ExcelReaderFactory.CreateReader(stream);
    //        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
    //        {
    //            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
    //            {
    //                UseHeaderRow = true
    //            }
    //        });

    //        var table = result.Tables[0];
    //        transformedTable = new DataTable(table.TableName ?? "TempTable");

    //        // Appliquer le mapping de types
    //        foreach (DataColumn col in table.Columns)
    //        {
    //            var columnName = col.ColumnName;
    //            var typeStr = typeMapping.ContainsKey(columnName) ? typeMapping[columnName] : "string";
    //            var type = MapToType(typeStr);

    //            transformedTable.Columns.Add(columnName, type);
    //        }

    //        foreach (DataRow row in table.Rows)
    //        {
    //            var newRow = transformedTable.NewRow();
    //            foreach (DataColumn col in table.Columns)
    //            {
    //                var value = row[col.ColumnName];
    //                var typeStr = typeMapping.ContainsKey(col.ColumnName) ? typeMapping[col.ColumnName] : "string";
    //                var type = MapToType(typeStr);

    //                try
    //                {
    //                    newRow[col.ColumnName] = Convert.ChangeType(value, type);
    //                }
    //                catch (Exception ex)
    //                {
    //                    // Log l'erreur et affecte DBNull en cas d'erreur de conversion
    //                    Console.WriteLine($"Erreur de conversion de {value} pour la colonne {col.ColumnName} : {ex.Message}");
    //                    newRow[col.ColumnName] = DBNull.Value;
    //                }
    //            }
    //            transformedTable.Rows.Add(newRow);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log l'erreur si le processus de transformation échoue
    //        Console.WriteLine($"Erreur lors de la transformation avec mappage : {ex.Message}");
    //        return null; // Retourne null en cas d'erreur
    //    }

    //    return transformedTable;
    //}

    //private Type MapToType(string typeString)
    //{
    //    return typeString.ToLower() switch
    //    {
    //        "int" or "int32" => typeof(int),
    //        "long" or "int64" => typeof(long),
    //        "decimal" => typeof(decimal),
    //        "double" => typeof(double),
    //        "datetime" => typeof(DateTime),
    //        "bool" or "boolean" => typeof(bool),
    //        _ => typeof(string),
    //    };
    //}
}
