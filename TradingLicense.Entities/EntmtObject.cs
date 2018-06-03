using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class EntmtObject
    {
        [Key]
        public int EntmtObjectID { get; set; }

        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string EntmtObjectDesc { get; set; }
        public float ObjectFee { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string ObjectName { get; set; }
        public float? BaseFee { get; set; }
        public int? Period { get; set; }
        public int? PeriodQuantity { get; set; }
        public bool Active { get; set; }
        public EntmtObject()
        {
            Active = true;
        }
    }
}
