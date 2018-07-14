using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class AddressModel
    {
        public int ADDRESS_ID { get; set; }
        [Required(ErrorMessage = "Sila masukkan alamat")]
        public string ADDRA1 { get; set; }
        public string ADDRA2 { get; set; }
        public string ADDRA3 { get; set; }
        public string ADDRA4 { get; set; }
        public string PCODEA { get; set; }
        public string STATEA { get; set; }
    }
}
