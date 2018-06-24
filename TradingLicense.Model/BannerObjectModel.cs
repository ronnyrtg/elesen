using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class BannerObjectModel
    {
        public int BannerObjectID { get; set; }
        public int BannerApplicationID { get; set; }
        public int BQuantity { get; set; }
        public float BSize { get; set; }
        [Required(ErrorMessage = "Sila masukkan lokasi iklan")]
        [StringLength(255)]
        public int LocationID { get; set; }
        [Required(ErrorMessage = "Sila pilih kod iklan")]
        public int BannerCodeID { get; set; }
    }
    public class SelectedBannerObjectModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
