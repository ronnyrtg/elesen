using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class RaceModel
    {
        public int RACEID { get; set; }
        public string RACE_CODE { get; set; }
        [Display(Name = "Race Description")]
        [Required(ErrorMessage = "Sila masukkan nama bangsa/etnik/sukukaum")]
        [StringLength(255)]
        public string RACE_DESC { get; set; }

        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }
    }
}
