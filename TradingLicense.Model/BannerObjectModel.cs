using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class BannerObjectModel
    {
        public int BannerObjectID { get; set; }

        [Display(Name = "Banner Location")]
        [Required(ErrorMessage = "Sila masukkan lokasi iklan")]
        [StringLength(5)]
        public int LocationID { get; set; }
        [Display(Name = "Banner Zone")]
        [Required(ErrorMessage = "Sila masukkan lokasi zon iklan")]
        [StringLength(255)]
        public int ZoneID { get; set; }
        [Display(Name = "Kod Iklan")]
        [Required(ErrorMessage = "Sila pilih kod iklan")]
        public int BannerCodeID { get; set; }
    }
    public class SelectedBannerObjectModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
