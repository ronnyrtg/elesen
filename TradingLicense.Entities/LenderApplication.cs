using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class LenderApplication
    {
        [Key]
        public int LenderApplicationID { get; set; }
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
        //For Business Types 2 and above
        public float? AuthorizedCapital { get; set; }
        public float? PaidUpCapital { get; set; }
        public float? IssuedCapital { get; set; }
        public float? CashCapital { get; set; }
        public float? OtherCapital { get; set; }
        public float? BankSource { get; set; }
        public float? SavingSource { get; set; }
        public float? LoanSource { get; set; }
        public float? OtherSource { get; set; }
        public int AppStatusID { get; set; }

        //The user who creates this application
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        //The staff processing this application
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UpdatedBy { get; set; }
        public DateTime DateApproved { get; set; }
        public float? ProcessingFee { get; set; }
        public DateTime DatePaid { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ReferenceNo { get; set; }
        public DateTime LExpireDate { get; set; }

        public virtual AppStatus AppStatus { get; set; }
        public virtual BusinessType BusinessType { get; set; }
        public virtual Company Company { get; set; }
        public virtual Users Users { get; set; }
    }
}
