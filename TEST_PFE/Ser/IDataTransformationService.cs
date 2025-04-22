using ExcelDataReader;

namespace TEST_PFE.Ser
{
    public interface IDataTransformationService
    {
        Task<List<Dictionary<string, string>>> TransformDataAsync(byte[] fileBytes);
    }

   
}
