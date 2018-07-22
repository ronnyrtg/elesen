using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class CatatanModel
    {
        public int CATATANID { get; set; }
        public int APP_ID { get; set; }
        [Required(ErrorMessage = "Sila masukkan catatan")]
        public string CONTENT { get; set; }
    }
}
