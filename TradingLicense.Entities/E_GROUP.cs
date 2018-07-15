using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class E_GROUP
    {
        //Entertainment Group Code
        [Key]
        public int E_GROUPID { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(10)]
        public string E_G_CODE { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string E_G_DESC { get; set; }

        public bool ACTIVE { get; set; }
        public E_GROUP()
        {
            ACTIVE = true;
        }
    }
}
