using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public  class PremiseApplication
    {
        [Key]
        public int PremiseApplicationID { get; set; }
        public int BusinessTypeID { get; set; }
        //public int IndividualID { get; set; }
        public int SectorID { get; set; }
        public int CompanyID { get; set; }

        public int UsersID { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string PremiseAddress { get; set; }
        public int? PremiseStatus { get; set; }

        public float? PremiseArea { get; set; }

        public int? PremiseTypeID { get; set; }
        public int? PremiseModification { get; set; }
        public DateTime DateSubmitted { get; set; }
    
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UpdatedBy { get; set; }

        public int AppStatusID { get; set; }

        public virtual PremiseType PremiseType { get; set; }
        public virtual AppStatus AppStatus { get; set; }

        //public virtual Individual Individual { get; set; }

        public virtual BusinessType BusinessType { get; set; }

        public virtual Users Users { get; set; }

        public virtual Company Company { get; set; }

        public virtual Sector Sector { get; set; }
    }
}
