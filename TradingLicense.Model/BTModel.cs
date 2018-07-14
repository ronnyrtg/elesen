using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    //Business Type
    public class BT
    {
        public int BT_ID { get; set; }
        [Required(ErrorMessage = "Sila masukkan kod perniagaan")]
        public string BT_CODE { get; set; }
        [Required(ErrorMessage = "Sila masukkan jenis perniagaan")]
        public string BT_DESC { get; set; }
        public bool ACTIVE { get; set; }
        
    }
}
