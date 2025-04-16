using Microsoft.Xrm.Sdk;

namespace TEST_PFE.Models
{
    public class EntityRecordResult

    {
        public List<Entity> Records { get; set; }
        public Dictionary<string, string> ColumnDisplayNames { get; set; } // LogicalName → DisplayName
    }
}
