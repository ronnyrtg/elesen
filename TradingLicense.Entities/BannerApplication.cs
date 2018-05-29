using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class BannerApplication
    {
        [Key]
        public int BannerApplicationID { get; set; }
        public int? CompanyID { get; set; }
        public int IndividualID { get; set; }
        public int AppStatusID { get; set; }

        //The user who creates this application
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }
        
        //The staff processing this application
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UpdatedBy { get; set; }

        public virtual AppStatus AppStatus { get; set; }
        public virtual Company Company { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual Users Users { get; set; }
    }
}
