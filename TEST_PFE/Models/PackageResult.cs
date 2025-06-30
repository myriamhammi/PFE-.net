namespace TEST_PFE.Models
{
    public class PackageResult

    {

        public bool Success { get; set; }
        public string Output { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public int ExitCode { get; set; }
    }
}
