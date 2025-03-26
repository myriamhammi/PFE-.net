namespace TEST_PFE.Models
{
    public class KPI
    {
        public string Name { get; set; }
        public TableData Data { get; set; }

        public KPI(string name, TableData data)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }
    }
}
