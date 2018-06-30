using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class EntmtApplication
    {
        [Key]
        public int EntmtApplicationID { get; set; }
        public int Mode { get; set; }
        public int BusinessTypeID { get; set; }
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
        public string StateA { get; set; }
        public int PremiseTypeID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string OtherPremiseType { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string WhichFloor { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string PremiseLocation { get; set; }
        public float PremiseArea { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string PremiseOwnership { get; set; }
        public int EntmtGroupID { get; set; }
        public int EntmtCodeID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string StartStopTime { get; set; }
        public int PeriodQuantity { get; set; }
        public int Period { get; set; }
        public int EntmtObjectID { get; set; }
        public int AppStatusID { get; set; }
        

        //The user who creates this application
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UpdatedBy { get; set; }
        public DateTime? DateApproved { get; set; }
        public float? ProcessingFee { get; set; }
        public float? TotalFee { get; set; }
        public DateTime? DatePaid { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ReferenceNo { get; set; }
        public string LicenseStatus { get; set; }
        public DateTime? ExpireDate { get; set; }

        public virtual BusinessType BusinessType { get; set; }
        public virtual PremiseType PremiseType { get; set; }
        public virtual AppStatus AppStatus { get; set; }
        public virtual Company Company { get; set; }
        public virtual Users Users { get; set; }
    }
}
