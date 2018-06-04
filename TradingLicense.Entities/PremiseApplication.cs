using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public  class PremiseApplication
    {
        [Key]
        public int PremiseApplicationID { get; set; }
        //Added by Desk Officer
        public int SectorID { get; set; }
        public int BusinessTypeID { get; set; }
        public string NamaPemohon { get; set; }
        public string ICPaspot { get; set; }
        public string NamaSyarikat { get; set; }
        public int AppStatusID { get; set; }

        //Added later by Clerk
        public int? IndividualID { get; set; }
        public int? CompanyID { get; set; }
        public int? PremiseAddressID { get; set; }
        public int? PremiseOwnership { get; set; }
        public float? PremiseArea { get; set; }
        public DateTime? StartRent { get; set; }
        public DateTime? StopRent { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string WhichFloor { get; set; }
        public int? PremiseTypeID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string OtherPremiseType { get; set; }
        public int? UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UpdatedBy { get; set; }

        public virtual PremiseType PremiseType { get; set; }
        public virtual PremiseAddress PremiseAddress { get; set; }
        public virtual AppStatus AppStatus { get; set; }
        public virtual Individual Individual { get; set; }
        public virtual BusinessType BusinessType { get; set; }
        public virtual Users Users { get; set; }
        public virtual Company Company { get; set; }
        public virtual Sector Sector { get; set; }
    }
}
