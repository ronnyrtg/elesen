using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class PADepSupp
    {
        [Key]
        public int PADepSuppID { get; set; }
        public int PremiseApplicationID { get; set; }
        public int DepartmentID { get; set; }
        public bool IsSupported { get; set; }
        public int UserId { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string SubmittedBy { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string Comment { get; set; }
        public DateTime SubmittedDate { get; set; }
        public bool IsActive { get; set; }

        public virtual PremiseApplication PremiseApplication { get; set; }

        public virtual Department Department { get; set; }
    }
}
