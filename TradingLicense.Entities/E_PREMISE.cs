using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class E_PREMISE
    {
        //Entertainment Premise Type
        [Key]
        public int E_PREMISEID { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(10)]
        public string E_P_CODE { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string E_P_DESC { get; set; }

        public bool ACTIVE { get; set; }
        public E_PREMISE()
        {
            ACTIVE = true;
        }
    }
}
