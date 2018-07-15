using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    //Required Documents
    public class RDModel
    {
        public int RD_ID { get; set; }
        [Required(ErrorMessage = "Sila masukkan jenis dokumen wajib")]
        public string RD_DESC { get; set; }
        public bool ACTIVE { get; set; }
    }
}
