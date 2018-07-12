using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class BannerObjectModel
    {
        public int BannerObjectID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BannerApplicationID { get; set; }
        public int BQuantity { get; set; }
        public float BSize { get; set; }
        public float Fee { get; set; }
        [Required(ErrorMessage = "Sila masukkan lokasi iklan")]
        [StringLength(255)]
        public int LocationID { get; set; }
        [Required(ErrorMessage = "Sila pilih kod iklan")]
        public int BannerCodeID { get; set; }

        public string BannerCodeDesc { get; set; }
        public string LocationDesc { get; set; }
    }
    public class SelectedBannerObjectModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
