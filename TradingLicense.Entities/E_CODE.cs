using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class E_CODE
    {
        [Key]
        public int E_CODEID { get; set; }
        public int E_GROUPID { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string E_C_DESC { get; set; }
        public float? E_C_FEE { get; set; }
        //Base Fee
        public float? E_C_B_FEE { get; set; }
        //Object Fee
        public float? E_C_O_FEE { get; set; }
        //Object name
        public string E_C_O_NAME { get; set; }
        public int? E_C_PERIOD { get; set; }
        public int? E_C_PERIOD_Q { get; set; }
        public bool ACTIVE { get; set; }
        public E_CODE()
        {
            ACTIVE = true;
        }
        public virtual E_GROUP E_GROUP { get; set; }
    }
}
