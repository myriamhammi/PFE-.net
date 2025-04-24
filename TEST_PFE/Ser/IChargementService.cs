using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TEST_PFE.Ser
{
    public interface IChargementService
    {
        Task CreateTableDynamically(List<dynamic> transformedData, string tableName, Dictionary<string, string> mapping);
        Task InsertDataDynamically(List<dynamic> transformedData, string tableName);
        Task<List<string>> GetTableNames();
    }
}