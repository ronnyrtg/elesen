using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class LiquorApplication
    {
        [Key]
        public int LiquorApplicationID { get; set; }
        public int Mode { get; set; }
        public int BusinessTypeID { get; set; }
        public int LiquorCodeID { get; set; }
        public int IndividualID { get; set; }
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
        public int AppStatusID { get; set; }
       

        //The user who creates this application
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        //The staff processing this application
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UpdatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime? DateApproved { get; set; }
        public float? ProcessingFee { get; set; }
        public float? TotalFee { get; set; }
        public DateTime? DatePaid { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ReferenceNo { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string LicenseStatus { get; set; }
        public DateTime? ExpireDate { get; set; }

        public virtual BusinessType BusinessType { get; set; }
        public virtual Company Company { get; set; }
        public virtual AppStatus AppStatus { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual LiquorCode LiquorCode { get; set; }
        public virtual Users Users { get; set; }
    }
}
