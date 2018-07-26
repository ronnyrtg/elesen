using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class ZONE_M
    {
        [Key]
        public int ZONEID { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string ZONE_CODE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string GEN_FLAG { get; set; }
        public int GEN_RATE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string SEW_FLAG { get; set; }
        public int SEW_RATE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string DRAIN_FLAG { get; set; }
        public int DRAIN_RATE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string WATER_FLAG { get; set; }
        public int WATER_RATE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string GRASS_FLAG { get; set; }
        public int GRASS_RATE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string LIGHT_FLAG { get; set; }
        public int LIGHT_RATE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string SLABS_FLAG { get; set; }
        public int SLABS_RATE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string ROAD_FLAG { get; set; }
        public int ROAD_RATE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string PLAYGRD_FLAG { get; set; }
        public int PLAYGRD_RATE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string SCAV_FLAG { get; set; }
        public int SCAV_RATE { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string CLEAN_FLAG { get; set; }
        public int CLEAN_RATE { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string ZONE_DESC { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ID_STAMP { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string TIME_STAMP { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string REC_STATUS { get; set; }
        public int NXT_PAS_NO { get; set; }
        public int NXT_PRS_NO { get; set; }
        public int IS_PAS_INUSE { get; set; }
        public int IS_PRS_INUSE { get; set; }
    }
}
