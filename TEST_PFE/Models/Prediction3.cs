using System.Text.Json.Serialization;
namespace TEST_PFE.Models
{
  

    public class Prediction3
    {
        public string Full_Name { get; set; }
        public string Industry { get; set; }
        public float Order_Total { get; set; }
        public int delai_livraison_jours { get; set; }
        public int Order_Month { get; set; }
        public int Order_Weekday { get; set; }
        public int Order_Quarter { get; set; }
        public int nb_lignes_produits { get; set; }
        public int quantite_totale { get; set; }
        public float prix_unitaire_moyen { get; set; }
        public int nb_produits_uniques { get; set; }
        public float prix_total_recalcule { get; set; }
        public float quantite_par_ligne { get; set; }
        public float prix_moyen_par_ligne { get; set; }
    }

}
