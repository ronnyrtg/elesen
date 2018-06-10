using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class LiquorApplication
    {
        [Key]
        public int LiquorApplicationID { get; set; }
        //Filled by Desk Officer
        public string NamaPemohon { get; set; }
        public string ICPaspot { get; set; }
        public string NamaSyarikat { get; set; }
        public int LiquorCodeID { get; set; }

        //Filled later by Clerk
        public int IndividualID { get; set; }
        public int CompanyID { get; set; }
        public string OperationHours { get; set; }
        public string LiquorLocation { get; set; }
        public int AppStatusID { get; set; }

        //The user who creates this application
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        //The staff processing this application
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UpdatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime AprovedDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public virtual Company Company { get; set; }
        public virtual AppStatus AppStatus { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual LiquorCode LiquorCode { get; set; }
        public virtual Users Users { get; set; }
    }
}
