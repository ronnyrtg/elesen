using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class ROAD_M
    {
        [Key]
        public int ROADID { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string ROAD_CODE { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string ROAD_DESC { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ID_STAMP { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string TIME_STAMP { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string REC_STATUS { get; set; }
    }
}
