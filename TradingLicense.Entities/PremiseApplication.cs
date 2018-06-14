using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public  class PremiseApplication
    {
        [Key]
        public int PremiseApplicationID { get; set; }
        public int IndividualID { get; set; }
        public int Mode { get; set; }
        public int SectorID { get; set; }
        public int BusinessTypeID { get; set; }
        public int AppStatusID { get; set; }
        public int CompanyID { get; set; }
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
        public int PremiseOwnership { get; set; }
        public float PremiseArea { get; set; }
        public DateTime StartRent { get; set; }
        public DateTime StopRent { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string WhichFloor { get; set; }
        public int PremiseTypeID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string OtherPremiseType { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UpdatedBy { get; set; }

        //User that creates the application, either Public user or Desk Officer
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        public virtual PremiseType PremiseType { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual AppStatus AppStatus { get; set; }
        public virtual BusinessType BusinessType { get; set; }
        public virtual Users Users { get; set; }
        public virtual Company Company { get; set; }
        public virtual Sector Sector { get; set; }
    }
}
