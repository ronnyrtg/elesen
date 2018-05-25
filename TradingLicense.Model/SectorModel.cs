using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class SectorModel
    {
        public int SectorID { get; set; }

        [Display(Name = "Sector Description")]
        [Required(ErrorMessage = "Please enter Sector Description")]
        [StringLength(30)]
        public string SectorDesc { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
