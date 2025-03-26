////using Microsoft.AspNetCore.Mvc;
////using TEST_PFE.Ser;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using Microsoft.Extensions.Logging;

////namespace TEST_PFE.Controllers
////{
////    public class CrmController : Controller
////    {
////        private readonly CrmService _crmService;
////        private readonly ILogger<CrmController> _logger;

////        public CrmController(CrmService crmService, ILogger<CrmController> logger)
////        {
////            _crmService = crmService;
////            _logger = logger;
////        }

////        [HttpGet("entities")]
////        public IActionResult GetEntities()
////        {
////            try
////            {
////                var entityNames = _crmService.GetAllEntities();

////                if (entityNames == null || !entityNames.Any())
////                {
////                    _logger.LogWarning("No entities found.");
////                    return NotFound(new { message = "Aucune entité disponible." });
////                }

////                return View("~/Views/Data/EntitiesList.cshtml", entityNames);
////            }
////            catch (Exception ex)
////            {
////                _logger.LogError(ex, "Error retrieving entities.");
////                return StatusCode(500, new { message = "Erreur lors de la récupération des entités", error = ex.Message });
////            }
////        }

////        [HttpGet("records/{entityName}")]
////        public IActionResult GetRecords(string entityName)
////        {
////            var records = _crmService.GetEntitiesByLogicalName(entityName);
////            var recordList = records as IEnumerable<dynamic>; // Typage explicite

////            if (recordList == null || !recordList.Any())
////            {
////                _logger.LogWarning($"No records found for entity: {entityName}");
////                return NotFound(new { message = $"Aucun enregistrement trouvé pour l'entité {entityName}." });
////            }
////            return View("~/Views/Data/RecordsList.cshtml", recordList);


////        }

////        [HttpGet("record/{entityName}/{id}")]
////        public IActionResult GetRecordForUpdate(string entityName, Guid id)
////        {
////            try
////            {
////                // Validation des paramètres
////                if (string.IsNullOrWhiteSpace(entityName))
////                {
////                    return BadRequest(new { message = "Le nom de l'entité est requis." });
////                }

////                var fields = new List<string> { "name", "createdon" };
////                var record = _crmService.GetRecord(entityName, id, fields);

////                if (record == null)
////                {
////                    _logger.LogWarning($"Record not found for entity {entityName} with ID {id}");
////                    return NotFound(new { message = $"Enregistrement non trouvé pour l'ID {id} dans l'entité {entityName}." });
////                }

////                var result = new
////                {
////                    Id = record.Id,
////                    LogicalName = record.LogicalName,
////                    Attributes = record.Attributes.ToDictionary(a => a.Key, a => a.Value?.ToString())
////                };

////                return View("~/Views/Data/UpdateRecord.cshtml", result);
////            }
////            catch (Exception ex)
////            {
////                _logger.LogError(ex, $"Error retrieving record {id} for entity {entityName}");
////                return StatusCode(500, new { message = $"Erreur lors de la récupération de l'enregistrement {id} de {entityName}", error = ex.Message });
////            }
////        }

////        [HttpPost("record/{entityName}")]
////        public IActionResult CreateRecord(string entityName, [FromBody] Dictionary<string, object> fields)
////        {
////            try
////            {
////                // Validation des paramètres
////                if (string.IsNullOrWhiteSpace(entityName))
////                {
////                    return BadRequest(new { message = "Le nom de l'entité est requis." });
////                }

////                if (fields == null || fields.Count == 0)
////                {
////                    return BadRequest(new { message = "Aucun champ à créer." });
////                }

////                Guid newRecordId = _crmService.CreateRecord(entityName, fields);

////                // Redirection vers la liste des enregistrements
////                return RedirectToAction("GetRecords", new { entityName = entityName });
////            }
////            catch (Exception ex)
////            {
////                _logger.LogError(ex, $"Error creating record for entity {entityName}");
////                return StatusCode(500, new { message = $"Erreur lors de la création d'un enregistrement pour {entityName}", error = ex.Message });
////            }
////        }

////        [HttpPost("record/{entityName}/{id}")]
////        public IActionResult UpdateRecord(string entityName, Guid id, [FromBody] Dictionary<string, object> fields)
////        {
////            try
////            {
////                // Validation des paramètres
////                if (string.IsNullOrWhiteSpace(entityName))
////                {
////                    return BadRequest(new { message = "Le nom de l'entité est requis." });
////                }

////                if (fields == null || fields.Count == 0)
////                {
////                    return BadRequest(new { message = "Aucun champ à mettre à jour." });
////                }

////                // Validation des valeurs des champs
////                if (!fields.Any(f => f.Value != null))
////                {
////                    return BadRequest(new { message = "Au moins une valeur doit être fournie." });
////                }

////                // Mise à jour de l'enregistrement
////                _crmService.UpdateRecord(entityName, id, fields);

////                // Redirection avec message de succès
////                TempData["SuccessMessage"] = $"L'enregistrement {id} a été mis à jour avec succès.";
////                return RedirectToAction("GetRecords", new { entityName = entityName });
////            }
////            catch (KeyNotFoundException ex)
////            {
////                _logger.LogWarning(ex, $"Champ manquant pour l'enregistrement {id} de {entityName}");
////                return BadRequest(new { message = "Champ manquant dans la mise à jour", error = ex.Message });
////            }
////            catch (InvalidOperationException ex)
////            {
////                _logger.LogWarning(ex, $"Opération invalide pour l'enregistrement {id} de {entityName}");
////                return BadRequest(new { message = "Opération invalide", error = ex.Message });
////            }
////            catch (Exception ex)
////            {
////                _logger.LogError(ex, $"Erreur lors de la mise à jour de l'enregistrement {id} de {entityName}");
////                return StatusCode(500, new { message = "Erreur serveur lors de la mise à jour", error = ex.Message });
////            }
////        }

////        [HttpDelete("record/{entityName}/{id}")]
////        public IActionResult DeleteRecord(string entityName, Guid id)
////        {
////            try
////            {
////                _crmService.DeleteRecord(entityName, id);
////                // Redirige vers la liste des enregistrements pour cette entité après suppression
////                return RedirectToAction("GetRecords", new { entityName = entityName });
////            }
////            catch (Exception ex)
////            {
////                _logger.LogError(ex, $"Error deleting record {id} from entity {entityName}");
////                return StatusCode(500, new { message = $"Erreur lors de la suppression de l'enregistrement {id} de {entityName}", error = ex.Message });
////            }
////        }
////    }
////}
//using Microsoft.AspNetCore.Mvc;
//using TEST_PFE.Ser;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Extensions.Logging;

//namespace TEST_PFE.Controllers
//{
//    public class CrmController : Controller
//    {
//        private readonly CrmService _crmService;
//        private readonly ILogger<CrmController> _logger;

//        public CrmController(CrmService crmService, ILogger<CrmController> logger)
//        {
//            _crmService = crmService ?? throw new ArgumentNullException(nameof(crmService));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        [HttpGet("entities")]
//        public IActionResult GetEntities()
//        {
//            try
//            {
//                var entityNames = _crmService.GetAllEntities();

//                if (entityNames == null || !entityNames.Any())
//                {
//                    _logger.LogWarning("No entities found.");
//                    return NotFound(new { message = "Aucune entité disponible." });
//                }

//                return View("~/Views/Data/EntitiesList.cshtml", entityNames);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error retrieving entities.");
//                return StatusCode(500, new { message = "Erreur lors de la récupération des entités", error = ex.Message });
//            }
//        }

//        [HttpGet("records/{entityName}")]
//        public IActionResult GetRecords(string entityName)
//        {
//            try
//            {
//                if (string.IsNullOrWhiteSpace(entityName))
//                {
//                    return BadRequest(new { message = "Le nom de l'entité est requis." });
//                }

//                var records = _crmService.GetEntitiesByLogicalName(entityName);
//                var recordList = records as IEnumerable<dynamic>;

//                if (recordList == null || !recordList.Any())
//                {
//                    _logger.LogWarning($"No records found for entity: {entityName}");
//                    return NotFound(new { message = $"Aucun enregistrement trouvé pour l'entité {entityName}." });
//                }

//                return View("~/Views/Data/RecordsList.cshtml", recordList);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error retrieving records for entity {entityName}");
//                return StatusCode(500, new { message = "Erreur lors de la récupération des enregistrements", error = ex.Message });
//            }
//        }

//        [HttpGet("record/{entityName}/{id}")]
//        public IActionResult GetRecordForUpdate(string entityName, Guid id)
//        {
//            try
//            {
//                if (string.IsNullOrWhiteSpace(entityName))
//                {
//                    return BadRequest(new { message = "Le nom de l'entité est requis." });
//                }

//                var fields = new List<string> { "name", "createdon" };
//                var record = _crmService.GetRecord(entityName, id, fields);

//                if (record == null)
//                {
//                    _logger.LogWarning($"Record not found for entity {entityName} with ID {id}");
//                    return NotFound(new { message = $"Enregistrement non trouvé pour l'ID {id} dans l'entité {entityName}." });
//                }

//                var result = new
//                {
//                    Id = record.Id,
//                    LogicalName = record.LogicalName,
//                    Attributes = record.Attributes.ToDictionary(a => a.Key, a => a.Value?.ToString())
//                };

//                return View("~/Views/Data/UpdateRecordForm.cshtml", result);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Erreur lors de la récupération de l'enregistrement {id} pour l'entité {entityName}");
//                return StatusCode(500, new { message = "Erreur serveur lors de la récupération de l'enregistrement", error = ex.Message });
//            }
//        }


//        [HttpPost("record/{entityName}")]
//        public IActionResult CreateRecord(string entityName, [FromBody] Dictionary<string, object> fields)
//        {
//            try
//            {
//                if (string.IsNullOrWhiteSpace(entityName))
//                {
//                    return BadRequest(new { message = "Le nom de l'entité est requis." });
//                }

//                if (fields == null || fields.Count == 0)
//                {
//                    return BadRequest(new { message = "Aucun champ à créer." });
//                }

//                Guid newRecordId = _crmService.CreateRecord(entityName, fields);

//                return RedirectToAction("GetRecords", new { entityName = entityName });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error creating record for entity {entityName}");
//                return StatusCode(500, new { message = $"Erreur lors de la création d'un enregistrement pour {entityName}", error = ex.Message });
//            }
//        }

//        [HttpPost("record/{entityName}/{id}")]
//        public IActionResult UpdateRecord(string entityName, Guid id, [FromForm] Dictionary<string, object> fields)
//        {
//            try
//            {
//                if (string.IsNullOrWhiteSpace(entityName))
//                {
//                    return BadRequest(new { message = "Le nom de l'entité est requis." });
//                }

//                var fieldsList = fields.Keys.ToList();
//                var existingRecord = _crmService.GetRecord(entityName, id, fieldsList);

//                if (existingRecord == null)
//                {
//                    _logger.LogWarning($"Record not found for entity {entityName} with ID {id}");
//                    return NotFound(new { message = $"Enregistrement non trouvé pour l'ID {id} dans l'entité {entityName}." });
//                }

//                if (fields == null || fields.Count == 0)
//                {
//                    return BadRequest(new { message = "Aucun champ à mettre à jour." });
//                }

//                if (!fields.Any(f => f.Value != null))
//                {
//                    return BadRequest(new { message = "Au moins une valeur doit être fournie." });
//                }

//                _crmService.UpdateRecord(entityName, id, fields);

//                TempData["SuccessMessage"] = $"L'enregistrement {id} a été mis à jour avec succès.";
//                return RedirectToAction("GetRecords", new { entityName = entityName });
//            }
//            catch (KeyNotFoundException ex)
//            {
//                _logger.LogWarning(ex, $"Champ manquant pour l'enregistrement {id} de {entityName}");
//                return BadRequest(new { message = "Champ manquant dans la mise à jour", error = ex.Message });
//            }
//            catch (InvalidOperationException ex)
//            {
//                _logger.LogWarning(ex, $"Opération invalide pour l'enregistrement {id} de {entityName}");
//                return BadRequest(new { message = "Opération invalide", error = ex.Message });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Erreur lors de la mise à jour de l'enregistrement {id} de {entityName}");
//                return StatusCode(500, new { message = "Erreur serveur lors de la mise à jour", error = ex.Message });
//            }
//        }



//        [HttpDelete("record/{entityName}/{id}")]
//        public IActionResult DeleteRecord(string entityName, Guid id)
//        {
//            try
//            {
//                _crmService.DeleteRecord(entityName, id);
//                return RedirectToAction("GetRecords", new { entityName = entityName });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, $"Error deleting record {id} from entity {entityName}");
//                return StatusCode(500, new { message = $"Erreur lors de la suppression de l'enregistrement {id} de {entityName}", error = ex.Message });
//            }
//        }
//    }
//}
using Microsoft.AspNetCore.Mvc;
using TEST_PFE.Ser;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace TEST_PFE.controllers
{

    public class CrmController : Controller
    {
        private readonly CrmService _crmService;
        private readonly ILogger<CrmController> _logger;

        public CrmController(CrmService crmService, ILogger<CrmController> logger)
        {
            _crmService = crmService ?? throw new ArgumentNullException(nameof(crmService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpGet("entities")]
        public async Task<IActionResult> GetEntities()
        {
            try
            {
                var entityNames = _crmService.GetAllEntities(); // Ajoutez await ici

                if (entityNames == null || !entityNames.Any())
                {
                    _logger.LogWarning("No entities found.");
                    return NotFound(new { message = "Aucune entité disponible." });
                }

                return View("~/Views/Data/EntitiesList.cshtml", entityNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entities.");
                return StatusCode(500, new { message = "Erreur lors de la récupération des entités", error = ex.Message });
            }
        }

        [HttpGet("records/{entityName}")]
        //public IActionResult GetRecords(string entityName)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(entityName))
        //        {
        //            return BadRequest(new { message = "Le nom de l'entité est requis." });
        //        }

        //        var records = _crmService.GetEntitiesByLogicalName(entityName);
        //        var recordList = records as IEnumerable<dynamic>;

        //        if (recordList == null || !recordList.Any())
        //        {
        //            _logger.LogWarning($"No records found for entity: {entityName}");
        //            return NotFound(new { message = $"Aucun enregistrement trouvé pour l'entité {entityName}." });
        //        }

        //        return View("~/Views/Data/RecordsList.cshtml", recordList);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error retrieving records for entity {entityName}");
        //        return StatusCode(500, new { message = "Erreur lors de la récupération des enregistrements", error = ex.Message });
        //    }
        //}

        public IActionResult GetRecords(string entityName)
        {
            var records = _crmService.GetEntitiesByLogicalName(entityName);
            ViewData["EntityName"] = entityName; // Passer entityName à la vue
            return View("~/Views/Data/RecordsList.cshtml", records);
        }



        [HttpGet("record/{entityName}/{id}")]
        public async Task<IActionResult> GetRecordForUpdate(string entityName, Guid id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entityName))
                {
                    return BadRequest("Le nom de l'entité est requis.");
                }

                if (id == Guid.Empty)
                {
                    return BadRequest("L'ID de l'enregistrement est invalide.");
                }

                var fields = new List<string> { "name", "description", "createdon", "modifiedon" };
                var record = _crmService.GetRecord(entityName, id, fields);

                if (record == null)
                {
                    return NotFound($"Enregistrement non trouvé pour l'ID {id} dans l'entité {entityName}.");
                }

                // Passer entityName à la vue
                ViewData["EntityName"] = entityName;

                return View("~/Views/Data/UpdateRecordForm.cshtml", record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération de l'enregistrement {id} pour l'entité {entityName}");
                return StatusCode(500, "Erreur serveur lors de la récupération de l'enregistrement.");
            }
        }



        [HttpPost("record/{entityName}")]
        public async Task<IActionResult> CreateRecord(string entityName, [FromBody] Dictionary<string, object> fields)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entityName))
                {
                    return BadRequest(new { message = "Le nom de l'entité est requis." });
                }

                if (fields == null || fields.Count == 0)
                {
                    return BadRequest(new { message = "Aucun champ à créer." });
                }

                Guid newRecordId =  _crmService.CreateRecord(entityName, fields); // Ajoutez await ici

                TempData["SuccessMessage"] = $"Enregistrement créé avec succès (ID : {newRecordId}).";
                return RedirectToAction("GetRecords", new { entityName = entityName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating record for entity {entityName}");
                return StatusCode(500, new { message = $"Erreur lors de la création d'un enregistrement pour {entityName}", error = ex.Message });
            }
        }



        [HttpPost("record/{entityName}/{id}")]
        public async Task<IActionResult> UpdateRecord(string entityName, Guid id, [FromForm] Dictionary<string, object> fields)
        {
            try
            {
                // Log des paramètres reçus
                _logger.LogInformation($"Mise à jour de l'enregistrement : EntityName = {entityName}, ID = {id}");
                _logger.LogInformation($"Champs reçus : {string.Join(", ", fields.Keys)}");

                if (string.IsNullOrWhiteSpace(entityName))
                {
                    return BadRequest("Le nom de l'entité est requis.");
                }

                if (fields == null || fields.Count == 0)
                {
                    return BadRequest("Aucun champ à mettre à jour.");
                }

                // Ajouter la date de modification actuelle
                fields["modifiedon"] = DateTime.UtcNow; // Utilisez DateTime.Now si vous préférez l'heure locale

                // Utilisez le bon nom d'attribut pour l'identifiant
                string idAttributeName = entityName.ToLower() + "id"; // Par exemple, "accountid" pour l'entité "Account"

                // Vérifiez si l'enregistrement existe avant de le mettre à jour
                var existingRecord = _crmService.GetRecord(entityName, id, new List<string> { idAttributeName });
                if (existingRecord == null)
                {
                    return NotFound($"Enregistrement non trouvé pour l'ID {id} dans l'entité {entityName}.");
                }

                // Log avant la mise à jour
                _logger.LogInformation("Tentative de mise à jour de l'enregistrement...");

                // Mettre à jour l'enregistrement
                _crmService.UpdateRecord(entityName, id, fields);

                // Message de succès
                TempData["SuccessMessage"] = $"L'enregistrement {id} a été mis à jour avec succès.";

                // Redirection vers la liste des enregistrements
                return RedirectToAction("GetRecords", new { entityName = entityName });
            }
            catch (Exception ex)
            {
                // Log de l'erreur
                _logger.LogError(ex, $"Erreur lors de la mise à jour de l'enregistrement {id} de {entityName}");

                // Message d'erreur pour l'utilisateur
                TempData["ErrorMessage"] = $"Erreur serveur lors de la mise à jour : {ex.Message}";

                // Redirection vers le formulaire de mise à jour
                return RedirectToAction("GetRecordForUpdate", new { entityName = entityName, id = id });
            }
        }

        [HttpDelete("record/{entityName}/{id}")]
        public async Task<IActionResult> DeleteRecord(string entityName, Guid id)
        {
            try
            {
                // Utilisez le bon nom d'attribut pour l'identifiant
                string idAttributeName = entityName.ToLower() + "id"; // Par exemple, "accountid" pour l'entité "Account"

                // Vérifiez si l'enregistrement existe avant de le supprimer
                var existingRecord = _crmService.GetRecord(entityName, id, new List<string> { idAttributeName });
                if (existingRecord == null)
                {
                    return NotFound(new { message = $"Enregistrement non trouvé pour l'ID {id} dans l'entité {entityName}." });
                }

                // Supprimer l'enregistrement
                _crmService.DeleteRecord(entityName, id);

                // Message de succès
                TempData["SuccessMessage"] = $"L'enregistrement {id} a été supprimé avec succès.";

                // Redirection vers la liste des enregistrements
                return RedirectToAction("GetRecords", new { entityName = entityName });
            }
            catch (Exception ex)
            {
                // Log de l'erreur
                _logger.LogError(ex, $"Erreur lors de la suppression de l'enregistrement {id} de {entityName}");

                // Message d'erreur pour l'utilisateur
                TempData["ErrorMessage"] = $"Erreur lors de la suppression de l'enregistrement : {ex.Message}";

                // Redirection vers la liste des enregistrements
                return RedirectToAction("GetRecords", new { entityName = entityName });
            }
        }


    }
}