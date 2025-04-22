using ExcelDataReader;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TEST_PFE.Ser;

public class DataTransformationService : IDataTransformationService
{
    public async Task<List<Dictionary<string, string>>> TransformDataAsync(byte[] fileBytes)
    {
        // Enregistrer le fournisseur d'encodage pour Windows-1252
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        var transformedData = new List<Dictionary<string, string>>();

        using (var stream = new MemoryStream(fileBytes))
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
            var headerRead = false;
            string[] columnNames = null;

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
                        continue; // on saute cette ligne d'en-tête
                    }

                    var row = new Dictionary<string, string>();
                    if (reader.FieldCount == columnNames.Length) // Vérifie si le nombre de colonnes est cohérent
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = columnNames[i];
                            var value = reader.GetValue(i)?.ToString() ?? "";
                            row[columnName] = value;
                        }
                    }
                    else
                    {
                        // Optionnel : log ou gestion d'erreur si les lignes ne sont pas cohérentes
                        continue; // ou handle l'erreur selon ton besoin
                    }

                    transformedData.Add(row);
                }
            } while (reader.NextResult());
        }

        return transformedData;
    }


}
