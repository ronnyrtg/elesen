using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class BannerObjectModel
    {
        public int B_O_ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int APP_ID { get; set; }
        public int B_QTY { get; set; }
        public float B_SIZE { get; set; }
        public float? FEE { get; set; }
        [Required(ErrorMessage = "Sila masukkan lokasi iklan")]
        [StringLength(255)]
        public string ADDRB1 { get; set; }
        public string ADDRB2 { get; set; }
        public string ADDRB3 { get; set; }
        public string ADDRB4 { get; set; }
        [Required(ErrorMessage = "Sila pilih kod iklan")]
        public int BC_ID { get; set; }

        public string CodeRefDesc { get; set; }
    }
    public class SelectedBannerObjectModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
