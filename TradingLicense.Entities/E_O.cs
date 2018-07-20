using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class E_O
    {
        [Key]
        public int E_OID { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string E_O_DESC { get; set; }
        public float? E_O_FEE { get; set; }
        //Base Fee
        public float? E_O_B_FEE { get; set; }
        //Object name
        public string E_O_NAME { get; set; }
        public int? E_O_PERIOD { get; set; }
        public int? E_O_PERIOD_Q { get; set; }
        public bool ACTIVE { get; set; }
        public E_O()
        {
            ACTIVE = true;
        }
        public virtual E_PREMISE E_PREMISE { get; set; }
    }
}