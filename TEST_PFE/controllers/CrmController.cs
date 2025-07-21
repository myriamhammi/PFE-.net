using Microsoft.AspNetCore.Mvc;
using TEST_PFE.Ser;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk;

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
        public IActionResult GetRecords(string entityName)
        {
            try
            {
                var entities = _crmService.GetEntitiesByLogicalName(entityName);

                // Convertir en liste de dictionnaires
                var records = entities.Select(entity =>
                    entity.Attributes.ToDictionary(attr => attr.Key, attr => attr.Value)
                ).ToList();

                ViewData["EntityName"] = entityName;
                return View("~/Views/Data/Records.cshtml", records);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération des enregistrements de l'entité {entityName}");
                return StatusCode(500, new { message = "Erreur lors de la récupération des enregistrements", error = ex.Message });
            }
        }







        [HttpGet("record/{entityName}/{id}")]
        public IActionResult GetRecordForUpdate(string entityName, Guid id)
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
                var record = _crmService.GetRecord(entityName, id, new List<string> { "name", "description", "createdon", "modifiedon" });
                if (record == null)
                {
                    return NotFound($"Enregistrement non trouvé pour l'ID {id} dans l'entité {entityName}.");
                }
                var model = new
                {
                    EntityName = entityName,
                    Id = id,
                    Fields = record.Attributes
                };
                ViewData["EntityName"] = entityName;
                return View("~/Views/Data/UpdateRecordForm.cshtml", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération de l'enregistrement {id} pour l'entité {entityName}");
                return StatusCode(500, "Erreur serveur lors de la récupération de l'enregistrement.");
            }
        }



        //[HttpPost("record/{entityName}")]
        //public async Task<IActionResult> CreateRecord(string entityName, [FromBody] Dictionary<string, object> fields)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(entityName))
        //        {
        //            return BadRequest(new { message = "Le nom de l'entité est requis." });
        //        }

        //        if (fields == null || fields.Count == 0)
        //        {
        //            return BadRequest(new { message = "Aucun champ à créer." });
        //        }

        //        Guid newRecordId =  _crmService.CreateRecord(entityName, fields); // Ajoutez await ici

        //        TempData["SuccessMessage"] = $"Enregistrement créé avec succès (ID : {newRecordId}).";
        //        return RedirectToAction("GetRecords", new { entityName = entityName });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error creating record for entity {entityName}");
        //        return StatusCode(500, new { message = $"Erreur lors de la création d'un enregistrement pour {entityName}", error = ex.Message });
        //    }
        //}



        //[HttpPost("record/{entityName}/{id}")]
        //public async Task<IActionResult> UpdateRecord(string entityName, Guid id, [FromForm] Dictionary<string, object> fields)
        //{
        //    try
        //    {
        //        // Log des paramètres reçus
        //        _logger.LogInformation($"Mise à jour de l'enregistrement : EntityName = {entityName}, ID = {id}");
        //        _logger.LogInformation($"Champs reçus : {string.Join(", ", fields.Keys)}");

        //        if (string.IsNullOrWhiteSpace(entityName))
        //        {
        //            return BadRequest("Le nom de l'entité est requis.");
        //        }

        //        if (fields == null || fields.Count == 0)
        //        {
        //            return BadRequest("Aucun champ à mettre à jour.");
        //        }

        //        // Ajouter la date de modification actuelle
        //        fields["modifiedon"] = DateTime.UtcNow; // Utilisez DateTime.Now si vous préférez l'heure locale

        //        // Utilisez le bon nom d'attribut pour l'identifiant
        //        string idAttributeName = entityName.ToLower() + "id"; // Par exemple, "accountid" pour l'entité "Account"

        //        // Vérifiez si l'enregistrement existe avant de le mettre à jour
        //        var existingRecord = _crmService.GetRecord(entityName, id, new List<string> { idAttributeName });
        //        if (existingRecord == null)
        //        {
        //            return NotFound($"Enregistrement non trouvé pour l'ID {id} dans l'entité {entityName}.");
        //        }

        //        // Log avant la mise à jour
        //        _logger.LogInformation("Tentative de mise à jour de l'enregistrement...");

        //        // Mettre à jour l'enregistrement
        //        _crmService.UpdateRecord(entityName, id, fields);

        //        // Message de succès
        //        TempData["SuccessMessage"] = $"L'enregistrement {id} a été mis à jour avec succès.";

        //        // Redirection vers la liste des enregistrements
        //        return RedirectToAction("GetRecords", new { entityName = entityName });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log de l'erreur
        //        _logger.LogError(ex, $"Erreur lors de la mise à jour de l'enregistrement {id} de {entityName}");

        //        // Message d'erreur pour l'utilisateur
        //        TempData["ErrorMessage"] = $"Erreur serveur lors de la mise à jour : {ex.Message}";

        //        // Redirection vers le formulaire de mise à jour
        //        return RedirectToAction("GetRecordForUpdate", new { entityName = entityName, id = id });
        //    }
        //}

        //[HttpDelete("record/{entityName}/{id}")]
        //public async Task<IActionResult> DeleteRecord(string entityName, Guid id)
        //{
        //    try
        //    {
        //        // Utilisez le bon nom d'attribut pour l'identifiant
        //        string idAttributeName = entityName.ToLower() + "id"; // Par exemple, "accountid" pour l'entité "Account"

        //        // Vérifiez si l'enregistrement existe avant de le supprimer
        //        var existingRecord = _crmService.GetRecord(entityName, id, new List<string> { idAttributeName });
        //        if (existingRecord == null)
        //        {
        //            return NotFound(new { message = $"Enregistrement non trouvé pour l'ID {id} dans l'entité {entityName}." });
        //        }

        //        // Supprimer l'enregistrement
        //        _crmService.DeleteRecord(entityName, id);

        //        // Message de succès
        //        TempData["SuccessMessage"] = $"L'enregistrement {id} a été supprimé avec succès.";

        //        // Redirection vers la liste des enregistrements
        //        return RedirectToAction("GetRecords", new { entityName = entityName });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log de l'erreur
        //        _logger.LogError(ex, $"Erreur lors de la suppression de l'enregistrement {id} de {entityName}");

        //        // Message d'erreur pour l'utilisateur
        //        TempData["ErrorMessage"] = $"Erreur lors de la suppression de l'enregistrement : {ex.Message}";

        //        // Redirection vers la liste des enregistrements
        //        return RedirectToAction("GetRecords", new { entityName = entityName });
        //    }
        //}
        [HttpPost]
        public IActionResult CreateRecord(string EntityName, [FromForm] Dictionary<string, string> Fields)
        {
            if (string.IsNullOrWhiteSpace(EntityName) || Fields == null || Fields.Count == 0)
            {
                TempData["ErrorMessage"] = "Les données de l'enregistrement sont incomplètes.";
                return RedirectToAction("GetRecords", new { entityName = EntityName });
            }
            try
            {
                var fields = Fields.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
                var recordId = _crmService.CreateRecord(EntityName, fields);
                TempData["SuccessMessage"] = $"Enregistrement créé avec succès (ID : {recordId}).";
                return RedirectToAction("GetRecords", new { entityName = EntityName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de l'enregistrement");
                TempData["ErrorMessage"] = $"Erreur lors de la création : {ex.Message}";
                return RedirectToAction("GetRecords", new { entityName = EntityName });
            }
        }

        [HttpPost("record/{EntityName}/{RecordId}")]
        public IActionResult UpdateRecord(string EntityName, string RecordId, [FromForm] Dictionary<string, string> Fields)
        {
            if (string.IsNullOrWhiteSpace(EntityName) || string.IsNullOrWhiteSpace(RecordId) || Fields == null || Fields.Count == 0)
            {
                TempData["ErrorMessage"] = "Les données de mise à jour sont incomplètes.";
                return RedirectToAction("GetRecords", new { entityName = EntityName });
            }
            try
            {
                var fields = Fields.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
                fields["modifiedon"] = DateTime.UtcNow;
                _crmService.UpdateRecord(EntityName, Guid.Parse(RecordId), fields);
                TempData["SuccessMessage"] = $"L'enregistrement {RecordId} a été mis à jour avec succès.";
                return RedirectToAction("GetRecords", new { entityName = EntityName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la mise à jour de l'enregistrement");
                TempData["ErrorMessage"] = $"Erreur lors de la mise à jour : {ex.Message}";
                return RedirectToAction("GetRecords", new { entityName = EntityName });
            }
        }

        [HttpPost]
        public IActionResult DeleteRecord(string EntityName, string Id)
        {
            if (string.IsNullOrWhiteSpace(EntityName) || string.IsNullOrWhiteSpace(Id))
            {
                TempData["ErrorMessage"] = "Paramètres de suppression invalides.";
                return RedirectToAction("GetRecords", new { entityName = EntityName });
            }
            try
            {
                _crmService.DeleteRecord(EntityName, Guid.Parse(Id));
                TempData["SuccessMessage"] = $"L'enregistrement {Id} a été supprimé avec succès.";
                return RedirectToAction("GetRecords", new { entityName = EntityName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de l'enregistrement");
                TempData["ErrorMessage"] = $"Erreur lors de la suppression : {ex.Message}";
                return RedirectToAction("GetRecords", new { entityName = EntityName });
            }
        }







    }
}

