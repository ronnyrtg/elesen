using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class PremiseAddress
    {
        [Key]
        public int PremiseAddressID { get; set; }
        public int? PremiseApplicationID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string Addra1 { get; set; }
        [StringLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string Addra2 { get; set; }
        [StringLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string Addra3 { get; set; }
        [StringLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string Addra4 { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string PcodeA { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string AreaA { get; set; }
        [StringLength(30)]
        [Column(TypeName = "VARCHAR2")]
        public string TownA { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string DistrictA { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string StateA { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string CountryA { get; set; }
    }
}
