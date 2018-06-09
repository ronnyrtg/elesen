using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class HawkerApplication
    {
        [Key]
        public int HawkerApplicationID { get; set; }
        //Filled by Desk Officer
        public int HawkerCodeID { get; set; }
        public string NamaPemohon { get; set; }
        public string ICPaspot { get; set; }
        public string Lokasi { get; set; }

        //Filled by Clerk
        public int IndividualID { get; set; }        
        public string OperationHours { get; set; }
        public string HawkerLocation { get; set; }
        public int AppStatusID { get; set; }

        //The user who creates this application
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        //The staff processing this application
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UpdatedBy { get; set; }
        public DateTime LastUpdated { get; set; }

        public virtual AppStatus AppStatus { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual HawkerCode HawkerCode { get; set; }
        public virtual Users Users { get; set; }
    }
}
