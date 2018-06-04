using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class EntmtCode
    {
        [Key]
        public int EntmtCodeID { get; set; }
        public int EntmtGroupID { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string EntmtCodeDesc { get; set; }
        public float? Fee { get; set; }
        public float? BaseFee { get; set; }
        public float? ObjectFee { get; set; }
        public string ObjectName { get; set; }
        public int? Period { get; set; }
        public int? PeriodQuantity { get; set; }
        public int Mode { get; set; }
        public bool Active { get; set; }
        public EntmtCode()
        {
            Active = true;
            Mode = 3;
        }
        public virtual EntmtGroup EntmtGroup { get; set; }
    }
}
