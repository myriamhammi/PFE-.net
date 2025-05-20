namespace TEST_PFE.Models
{
    public class PredictionInput
    {
        public string ClientName { get; set; } // nouveau champ

        public string Industry { get; set; }
        public string Lead_Source { get; set; }
        public double Revenue_Potential { get; set; }
        public int Days_to_Close { get; set; }
        public string Model { get; set; } // logreg, rf, xgb
    }
}
