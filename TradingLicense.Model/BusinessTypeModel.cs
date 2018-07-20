using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    //Business Type
    public class BusinessTypeModel
    {
        public int BT_ID { get; set; }
        [Required(ErrorMessage = "Sila masukkan kod perniagaan")]
        public string BT_CODE { get; set; }
        [Required(ErrorMessage = "Sila masukkan jenis perniagaan")]
        public string BT_DESC { get; set; }
        public bool ACTIVE { get; set; }

        public string RequiredDocs { get; set; }
        
    }
}
