using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class B_O_Model
    {
        public int B_O_ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int APP_ID { get; set; }
        public int B_QTY { get; set; }
        public float B_SIZE { get; set; }
        public float? FEE { get; set; }
        [Required(ErrorMessage = "Sila masukkan lokasi iklan")]
        [StringLength(255)]
        public int ADDRESS_ID { get; set; }
        [Required(ErrorMessage = "Sila pilih kod iklan")]
        public int BC_ID { get; set; }

        public string C_R_DESC { get; set; }
        public string ADDRA1 { get; set; }
        public string ADDRA2 { get; set; }
        public string ADDRA3 { get; set; }
        public string ADDRA4 { get; set; }
    }
    public class SelectedBannerObjectModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
