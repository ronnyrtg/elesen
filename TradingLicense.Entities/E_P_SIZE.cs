using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class E_P_SIZE
    {
        [Key]
        public int E_P_SIZEID { get; set; }
        public int E_PREMISEID { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string E_S_DESC { get; set; }
        public float? E_S_FEE { get; set; }
        //Base Fee
        public float? E_S_B_FEE { get; set; }
        //Object Fee
        public float? E_S_O_FEE { get; set; }
        //Object name
        public string E_S_O_NAME { get; set; }
        public int? E_S_PERIOD { get; set; }
        public int? E_S_PERIOD_Q { get; set; }
        public bool ACTIVE { get; set; }
        public E_P_SIZE()
        {
            ACTIVE = true;
        }
        public virtual E_PREMISE E_PREMISE { get; set; }
    }
}