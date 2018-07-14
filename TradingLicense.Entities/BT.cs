using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    //Business Type
    public class BT
    {
        [Key]
        public int BT_ID { get; set; }
        [Required]
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string BT_CODE { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string BT_DESC { get; set; }
        public bool ACTIVE { get; set; }
        public BT()
        {
            ACTIVE = true;
        }
    }
}
