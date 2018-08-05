using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class SUBZONE_M
    {
        [Key]
        public int SUBZONEID { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string SUBZONE_CODE { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string SUBZONE_DESC { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ID_STAMP { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string TIME_STAMP { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string ZONE_CODE { get; set; }
    }
}
