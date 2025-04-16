using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo.Wmi;
using TEST_PFE.Models;

namespace TEST_PFE.Ser
{
    public class CrmService
    {

        private readonly ServiceClient _serviceClient;
        private readonly ILogger<CrmService> _logger;

        public CrmService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("Dataverse");

            try
            {
                _serviceClient = new ServiceClient(connectionString);

                if (_serviceClient.IsReady)
                {
                    Console.WriteLine("✅ Connexion réussie à Dynamics CRM !");
                }
                else
                {
                    throw new Exception("Échec de connexion : " + _serviceClient.LastError);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la connexion à Dynamics CRM : " + ex.Message);
                throw;
            }
        }




        public List<string> GetAllEntities()
        {
            List<string> entityNames = new List<string>();

            try
            {
                var request = new RetrieveAllEntitiesRequest
                {
                    EntityFilters = EntityFilters.Entity,
                    RetrieveAsIfPublished = true
                };

                var response = (RetrieveAllEntitiesResponse)_serviceClient.Execute(request);

                if (response?.EntityMetadata != null)
                {
                    foreach (var entity in response.EntityMetadata)
                    {
                        entityNames.Add(entity.LogicalName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des entités : {ex.Message}");
            }

            return entityNames;
        }





        //public Dictionary<string, string> GetAllEntitiesWithDisplayNames()
        //{
        //    var request = new RetrieveAllEntitiesRequest
        //    {
        //        EntityFilters = EntityFilters.Entity,
        //        RetrieveAsIfPublished = true
        //    };

        //    var response = (RetrieveAllEntitiesResponse)_serviceClient.Execute(request);

        //    var entityDict = new Dictionary<string, string>();

        //    foreach (var metadata in response.EntityMetadata)
        //    {
        //        var logicalName = metadata.LogicalName;
        //        var displayName = metadata.DisplayName?.UserLocalizedLabel?.Label ?? logicalName;

        //        if (!entityDict.ContainsKey(logicalName))
        //        {
        //            entityDict.Add(logicalName, displayName);
        //        }
        //    }

        //    return entityDict.OrderBy(e => e.Value).ToDictionary(kv => kv.Key, kv => kv.Value);
        //}


        public IEnumerable<Entity> GetEntitiesByLogicalName(string entityName)
        {
            try
            {
                // Vérification que la connexion est prête
                if (_serviceClient.IsReady)
                {
                    // Création de la requête pour récupérer tous les enregistrements de l'entité spécifiée
                    var query = new QueryExpression(entityName)
                    {
                        ColumnSet = new ColumnSet(true)  // Récupère toutes les colonnes de l'entité
                    };

                    // Exécution de la requête
                    var result = _serviceClient.RetrieveMultiple(query);

                    // Retourne la liste des entités récupérées
                    return result.Entities;
                }
                else
                {
                    _logger.LogError("La connexion à Dataverse n'est pas prête.");
                    return Enumerable.Empty<Entity>();  // Retourne une collection vide si la connexion échoue
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erreur lors de la récupération des enregistrements de l'entité {entityName}");
                return Enumerable.Empty<Entity>();  // Retourne une collection vide en cas d'erreur
            }

        }


        public EntityMetadata GetEntityMetadata(string entityLogicalName)
        {
            var request = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = entityLogicalName
            };

            var response = (RetrieveEntityResponse)_serviceClient.Execute(request);
            return response.EntityMetadata;
        }




        public Guid CreateRecord(string entityName, Dictionary<string, object> fields)
        {
            try
            {
                if (fields == null || fields.Count == 0)
                {
                    throw new ArgumentException("Aucun champ à créer.");
                }

                Entity newRecord = new Entity(entityName);

                foreach (var field in fields)
                {
                    if (newRecord.Attributes.ContainsKey(field.Key))
                    {
                        newRecord[field.Key] = field.Value;
                    }
                    else
                    {
                        Console.WriteLine($"Avertissement : Le champ {field.Key} n'existe pas pour l'entité {entityName}.");
                    }
                }

                Guid recordId = _serviceClient.Create(newRecord);
                Console.WriteLine($"✅ {entityName} créé avec ID : {recordId}");
                return recordId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création de {entityName}: " + ex.Message);
                throw new InvalidOperationException("Erreur lors de la création de l'enregistrement.", ex);
            }
        }

        public Entity? GetRecord(string entityName, Guid recordId, List<string> fields)
        {
            try
            {
                // Utilisez le bon nom d'attribut pour l'identifiant
                string idAttributeName = entityName.ToLower() + "id"; // Par exemple, "accountid" pour l'entité "Account"

                // Ajoutez l'attribut d'identifiant à la liste des champs
                fields.Add(idAttributeName);

                ColumnSet columns = new ColumnSet(fields.ToArray());
                Entity record = _serviceClient.Retrieve(entityName, recordId, columns);

                return record;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération de l'enregistrement {entityName} avec ID {recordId}: " + ex.Message);
                throw new InvalidOperationException("Erreur lors de la récupération de l'enregistrement.", ex);
            }
        }


        public void UpdateRecord(string entityName, Guid recordId, Dictionary<string, object> fields)
        {
            try
            {
                Entity updatedRecord = new Entity(entityName, recordId);
                foreach (var field in fields)
                {
                    updatedRecord[field.Key] = field.Value;
                }

                _serviceClient.Update(updatedRecord);
                Console.WriteLine($"✅ {entityName} mis à jour !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour de {entityName} avec ID {recordId}: " + ex.Message);
                throw;
            }
        }

        public void DeleteRecord(string entityName, Guid recordId)
        {
            try
            {
                _serviceClient.Delete(entityName, recordId);
                Console.WriteLine($"✅ {entityName} supprimé avec succès !");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la suppression de {entityName} avec ID {recordId}: " + ex.Message);
                throw;
            }
        }
    }
}
