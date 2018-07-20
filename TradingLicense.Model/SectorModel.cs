using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class SectorModel
    {
        public int SECTORID { get; set; }

        [Display(Name = "Sector Description")]
        [Required(ErrorMessage = "Please enter Sector Description")]
        [StringLength(30)]
        public string SECTORDESC { get; set; }

        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }
    }
}
