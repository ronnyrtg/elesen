using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class E_P_FEE
    {
        //Entertainment Premise Fee
        [Key]
        public int E_P_FEEID { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string E_P_DESC { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string E_S_DESC { get; set; }
        public float? E_S_FEE { get; set; }
        public float? E_S_B_FEE { get; set; }
        public float? E_S_O_FEE { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(50)]
        public string E_S_O_NAME { get; set; }
        public int E_S_PERIOD { get; set; }
        public int E_S_P_QTY { get; set; }
        public bool ACTIVE { get; set; }
        public E_P_FEE()
        {
            ACTIVE = true;
        }
    }
}
