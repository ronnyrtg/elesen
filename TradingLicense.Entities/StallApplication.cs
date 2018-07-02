using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class StallApplication
    {
        [Key]
        public int StallApplicationID { get; set; }
        public int Mode { get; set; }
        public int IndividualID { get; set; }
        public int StallCodeID { get; set; }
        public DateTime ValidStart { get; set; }
        public DateTime ValidStop { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string OperationHours { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string StallLocation { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string GoodsType { get; set; }
        public int HelperA { get; set; }
        public int? HelperB { get; set; }
        public int? HelperC { get; set; }
        public int AppStatusID { get; set; }

        //The user who creates this application
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        //The staff processing this application
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

        public virtual AppStatus AppStatus { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual StallCode StallCode { get; set; }
        public virtual Users Users { get; set; }
    }
}
