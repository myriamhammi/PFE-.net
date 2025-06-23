namespace TEST_PFE.Models
{
    public class AssistantMemory
    {
        public string UserId { get; set; }  // à lier avec un cookie ou identifiant
        public List<string> History { get; set; } = new();
        public DateTime LastVisit { get; set; }
    }

}
