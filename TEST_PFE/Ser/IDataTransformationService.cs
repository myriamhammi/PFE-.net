using ExcelDataReader;
using System.Data;
using System.Dynamic;

namespace TEST_PFE.Ser
{
    public interface IDataTransformationService
    {
        Task<List<Dictionary<string, string>>> TransformDataAsync(byte[] fileBytes, Dictionary<string, string> mapping);
        Task<List<dynamic>> TransformAndNormalizeDataAsync(byte[] fileBytes, Dictionary<string, string> mapping);
    }


}
