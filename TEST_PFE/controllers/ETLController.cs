using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

//public class TableRequest
//{
//    public string TableName { get; set; }
//}


namespace TEST_PFE.Controllers
{
    public class ETLController : Controller
    {
        private readonly string _fileUploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        public ETLController()
        {
            if (!Directory.Exists(_fileUploadDirectory))
            {
                Directory.CreateDirectory(_fileUploadDirectory);
            }
        }
        // Affichage de la vue Upload.cshtml
        public IActionResult Upload()
        {
            return View();  // Cela cherchera par défaut la vue Upload.cshtml sous Views/ETL
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(string selectedTable, IFormFile uploadedFile)
        {

            if (selectedTable == "Toutes")
            {
                return RunAllPackages();
            }

            // Vérification du fichier
            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                ViewBag.Message = "Fichier invalide.";
                return View("Upload");  // Retourner la vue Upload.cshtml sous Views/ETL
            }

            if (uploadedFile != null && uploadedFile.Length > 0)
            {
                var fileUploadPath = Path.Combine(_fileUploadDirectory, uploadedFile.FileName);  // Renommage de 'filePath'
                using (var fileStream = new FileStream(fileUploadPath, FileMode.Create))
                {
                    uploadedFile.CopyTo(fileStream);
                }

                // Stockage du nom de fichier pour l'afficher dans la vue
                var recentFiles = GetRecentFiles();

                TempData["RecentFiles"] = GetRecentFiles();
                ViewBag.Message = "Fichier téléchargé avec succès.";
            }

            // Sauvegarder le fichier dans wwwroot/uploads
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            Directory.CreateDirectory(uploadsPath); // Crée le dossier s'il n'existe pas
            var filePath = Path.Combine(uploadsPath, uploadedFile.FileName);

            // Vérifier s'il existe déjà un fichier avec le même nom
            if (System.IO.File.Exists(filePath))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(uploadedFile.FileName);
                var extension = Path.GetExtension(uploadedFile.FileName);
                var newFileName = $"{fileNameWithoutExtension}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                filePath = Path.Combine(uploadsPath, newFileName);  // Nouveau nom avec suffixe
            }

            // Sauvegarde du fichier
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

            // 1. LoadToSA
            bool loadResult = false;
            try
            {
                loadResult = RunSSISPackage(selectedTable, "LoadToSA");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Erreur pendant le chargement vers SA : {ex.Message}";
                return View("Upload");
            }

            if (!loadResult)
            {
                ViewBag.Message = "Erreur pendant le chargement vers SA.";
                return View("Upload");
            }

            // 2. TransformToDW
            string transformPackageTable = selectedTable;
            if (selectedTable == "QuoteProduct" || selectedTable == "OpportunityProduct" || selectedTable == "SalesProduct")
            {
                transformPackageTable = "Product";
            }

            bool transformResult = false;
            try
            {
                transformResult = RunSSISPackage(transformPackageTable, "TransformToDW");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Erreur pendant la transformation vers DW : {ex.Message}";
                return View("Upload");
            }

            if (!transformResult)
            {
                ViewBag.Message = "Erreur pendant la transformation vers DW.";
                return View("Upload");
            }

            // 3. LoadToCRM
            //bool crmLoadResult1 = false;
            //bool crmLoadResult2 = true; // Par défaut à true (si non concerné)

            //try
            //{
            //    crmLoadResult1 = RunSSISPackage(selectedTable, "LoadToCRM");

            //    if (selectedTable == "QuoteProduct" || selectedTable == "OpportunityProduct" || selectedTable == "SalesProduct")
            //    {
            //        crmLoadResult2 = RunSSISPackage("Product", "LoadToCRM");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Message = $"Erreur pendant le chargement vers CRM : {ex.Message}";
            //    return View("Upload");
            //}

            //if (!crmLoadResult1 || !crmLoadResult2)
            //{
            //    ViewBag.Message = "Erreur pendant le chargement vers CRM.";
            //    return View("Upload");
            //}


            ViewBag.Message = "✅ ETL terminé avec succès !";
            return View("Upload");

        }

        // Fonction pour récupérer les fichiers récents
        public List<string> GetRecentFiles()
        {
            var files = Directory.GetFiles(_fileUploadDirectory)
                                 .Select(file => new FileInfo(file))
                                 .OrderByDescending(file => file.CreationTime)
                                 .Take(5) // Limiter à 5 fichiers récents
                                 .Select(file => file.Name)
                                 .ToList();

            return files;
        }


        //exécuter Package1 et Package2
        public IActionResult RunAllPackages()
        {
            try
            {
                string package0 = @"C:\Users\meria\source\repos\PFE_EY\PFE_EY\package_SA.dtsx";

                string package1 = @"C:\Users\meria\source\repos\PFE_EY\PFE_EY\Package1.dtsx";
              //  string package2 = @"C:\Users\meria\source\repos\PFE_EY\PFE_EY\Package2.dtsx";

                bool result0 = RunSSISPackageFromPath(package0);
                if (!result0)
                {
                    ViewBag.Message = "Erreur pendant l'exécution de Staging Area (SA).";
                    return View("Upload");
                }

                bool result1 = RunSSISPackageFromPath(package1);
                if (!result1)
                {
                    ViewBag.Message = "Erreur pendant l'exécution de Data Warehouse (DW).";
                    return View("Upload");
                }

                //bool result2 = RunSSISPackageFromPath(package2);
                //if (!result2)
                //{
                //    ViewBag.Message = "Erreur pendant l'exécution de Package2 (CRM).";
                //    return View("Upload");
                //}

                ViewBag.Message = "✅ Tous les packages ont été exécutés avec succès.";
                return View("Upload");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Erreur globale pendant l'exécution des packages : {ex.Message}";
                return View("Upload");
            }
        }

        // Helper pour lancer un package à partir d’un chemin direct
        private bool RunSSISPackageFromPath(string packagePath)
        {
            try
            {
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = "dtexec";
                process.StartInfo.Arguments = $"/f \"{packagePath}\"";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // Afficher les logs dans la console ou dans la vue
                Console.WriteLine($"Sortie du package : {output}");
                Console.WriteLine($"Erreur du package : {error}");
                ViewBag.PackageOutput = output;
                ViewBag.PackageError = error;

                // Vérifier le code de sortie
                if (process.ExitCode != 0)
                {
                    ViewBag.Message = $"Erreur lors de l'exécution du package. Erreur : {error}";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Erreur lors de l'exécution du package : {ex.Message}";
                return false;
            }
        }




        private bool RunSSISPackage(string selectedTable, string packageName)
        {
            try
            {
                // Emplacement des SSIS Packages
                var ssisPackagesPath = @"C:\Users\meria\source\repos\PFE_EY\PFE_EY";

                // Chemin complet vers le package (LoadToSA ou TransformToDW)
                var fullPackagePath = Path.Combine(ssisPackagesPath, $"{selectedTable}_{packageName}.dtsx");

                // Vérification de l'existence du fichier
                if (!System.IO.File.Exists(fullPackagePath))
                {
                    Console.WriteLine($"Le fichier {selectedTable}_{packageName}.dtsx est introuvable.");
                    ViewBag.Message = $"Le fichier {selectedTable}_{packageName}.dtsx est introuvable.";
                    return false;
                }

                // Exécution du package sans option /LOG
                var startInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Program Files (x86)\Microsoft SQL Server\150\DTS\Binn\DTExec.exe",
                    Arguments = $"/F \"{fullPackagePath}\" /Rep E",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    // Lecture des flux de sortie et d'erreur
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    // Affichage des messages dans la console
                    Console.WriteLine($"{packageName} Sortie : {output}");
                    Console.WriteLine($"{packageName} Erreur : {error}");

                    // Vérification du code de sortie du processus
                    if (process.ExitCode != 0)
                    {
                        ViewBag.Message = $"Erreur lors de l'exécution du package {packageName} : {error}";
                        return false;
                    }
                }

                // Si le processus a réussi
                return true;
            }
            catch (Exception ex)
            {
                // En cas d'exception, gestion de l'erreur
                Console.WriteLine($"Erreur lors de l'exécution du package {packageName}: {ex.Message}");
                ViewBag.Message = $"Erreur lors de l'exécution du package {packageName}: {ex.Message}";
                return false;
            }
        }









        //private readonly IDataTransformationService _transformationService;
        //private readonly IChargementService _chargementService;

        //public ETLController(IDataTransformationService transformationService, IChargementService chargementService)
        //{
        //    _transformationService = transformationService;
        //    _chargementService = chargementService;
        //}

        //[HttpPost("ChargerDonneesTransformees")]
        //public async Task<IActionResult> ChargerDonneesTransformees([FromForm] IFormFile file, [FromForm] string tableName, [FromForm] string mapping)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest(new { message = "Le fichier est vide ou manquant." });
        //    }

        //    if (string.IsNullOrWhiteSpace(tableName))
        //    {
        //        return BadRequest(new { message = "Le nom de la table est requis." });
        //    }

        //    if (string.IsNullOrWhiteSpace(mapping))
        //    {
        //        return BadRequest(new { message = "Le mapping est requis." });
        //    }

        //    try
        //    {
        //        using var memoryStream = new MemoryStream();
        //        await file.CopyToAsync(memoryStream);
        //        var fileBytes = memoryStream.ToArray();

        //        var mappingDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapping);
        //        if (mappingDict == null || mappingDict.Count == 0)
        //        {
        //            return BadRequest(new { message = "Le mapping fourni est invalide ou vide." });
        //        }

        //        // Utilisation directe de la méthode de transformation + nettoyage
        //        var dynamicData = await _transformationService.TransformAndNormalizeDataAsync(fileBytes, mappingDict);

        //        await _chargementService.CreateTableDynamically(dynamicData, tableName, mappingDict);
        //        await _chargementService.InsertDataDynamically(dynamicData, tableName);

        //        return Ok(new
        //        {
        //            message = "La table a été créée avec succès et les données ont été chargées.",
        //            data = dynamicData
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = $"Erreur lors du chargement : {ex.Message}" });
        //    }
        //}





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



