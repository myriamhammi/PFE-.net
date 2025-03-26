namespace TEST_PFE.Models
{
    public class TableData
    {
        public string TableName { get; set; }
        public List<string> Attributes { get; set; }
        public List<Dictionary<string, object>> Records { get; set; }

        // Constructor to initialize TableData
        public TableData(string tableName, List<string> attributes, List<Dictionary<string, object>> records)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            Records = records ?? new List<Dictionary<string, object>>(); // Initialize an empty list if records are not provided
        }

        // Optional: Add a parameterless constructor if needed for model binding
        public TableData()
        {
            Attributes = new List<string>();
            Records = new List<Dictionary<string, object>>();
        }
    }
}
