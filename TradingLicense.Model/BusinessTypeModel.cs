using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    //Business Type
    public class BusinessTypeModel
    {
        public int BT_ID { get; set; }
        [Display(Name = "Kod Jenis Perniagaan")]
        [Required(ErrorMessage = "Sila masukkan kod jenis perniagaan")]
        public string BT_CODE { get; set; }
        [Display(Name = "Nama Jenis Perniagaan")]
        [Required(ErrorMessage = "Sila masukkan jenis perniagaan")]
        public string BT_DESC { get; set; }
        public bool ACTIVE { get; set; }

        public List<int> RequiredDocs { get; set; }
        
    }
}
