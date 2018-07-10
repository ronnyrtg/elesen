using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class HawkerApplication
    {
        [Key]
        public int HawkerApplicationID { get; set; }
        public int Mode { get; set; }
        public int IndividualID { get; set; }
        public int HawkerCodeID { get; set; }
        public DateTime? ValidStart { get; set; }
        public DateTime? ValidStop { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string GoodsType { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string OperationHours { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string HawkerLocation { get; set; }
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
        public DateTime LastUpdated { get; set; }
        public DateTime? DateApproved { get; set; }
        public float? TotalFee { get; set; }
        public DateTime? DatePaid { get; set; }
        public DateTime? ExpireDate { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ReferenceNo { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string LicenseStatus { get; set; }

        public virtual AppStatus AppStatus { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual HawkerCode HawkerCode { get; set; }
        public virtual Users Users { get; set; }
    }
}
