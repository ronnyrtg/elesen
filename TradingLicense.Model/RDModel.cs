using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    //Required Documents
    public class RD
    {
        public int RD_ID { get; set; }
        [Required(ErrorMessage = "Sila masukkan jenis dokumen wajib")]
        public string RD_DESC { get; set; }
        public bool ACTIVE { get; set; }
    }
}
