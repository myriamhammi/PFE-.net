namespace TEST_PFE.Ser
{
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class DataSyncService
    {
        private readonly DynamicsService _dynamicsService;
        private readonly IChargementService _chargementService;
        private readonly ILogger<DataSyncService> _logger;

        // Constructeur avec l'injection des services nécessaires
        public DataSyncService(DynamicsService dynamicsService, IChargementService chargementService, ILogger<DataSyncService> logger)
        {
            _dynamicsService = dynamicsService;
            _chargementService = chargementService;
            _logger = logger;
        }

        public async Task<string> SyncSqlTableToDataverse(string tableName)
        {
            try
            {
                // Récupération des données de la table à synchroniser depuis le service de chargement
                var tableData = await _chargementService.GetTableNames();

                // Vérification que les données sont bien récupérées
                if (tableData == null || tableData.Count == 0)
                {
                    _logger.LogWarning($"Aucune donnée à synchroniser pour la table {tableName}.");
                    return "Aucune donnée à synchroniser.";
                }

                // Envoi des données vers Dynamics (Dataverse)
                var response = await _dynamicsService.SendDataToDynamicsAsync(tableData);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Synchronisation réussie de la table {tableName} vers Dynamics.");
                    return "Synchronisation réussie !";
                }
                else
                {
                    // Gestion des erreurs d'API Dynamics
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erreur lors de la synchronisation vers Dynamics : {errorDetails}");
                    return $"Erreur lors de la synchronisation : {errorDetails}";
                }
            }
            catch (Exception ex)
            {
                // Gestion des exceptions non prévues
                _logger.LogError($"Exception lors de la synchronisation de la table {tableName} vers Dynamics : {ex.Message}");
                return $"Erreur de serveur lors de la synchronisation : {ex.Message}";
            }
        }

        public async Task<string> CreateTableInDataverse(string schemaName, string displayName, string accessToken)
        {
            return await _dynamicsService.CreateCustomTableAsync(accessToken, schemaName, displayName);
        }
    }
}
