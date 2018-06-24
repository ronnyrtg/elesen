using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class RaceModel
    {
        public int RaceID { get; set; }

        [Display(Name = "Race Description")]
        [Required(ErrorMessage = "Sila masukkan nama bangsa/etnik/sukukaum")]
        [StringLength(255)]
        public string RaceDesc { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
