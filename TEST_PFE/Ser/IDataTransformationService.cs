using ExcelDataReader;
using System.Data;

namespace TEST_PFE.Ser
{
    public interface IDataTransformationService
    {
        Task<List<Dictionary<string, string>>> TransformDataAsync(byte[] fileBytes, Dictionary<string, string> mapping);

    }


}
