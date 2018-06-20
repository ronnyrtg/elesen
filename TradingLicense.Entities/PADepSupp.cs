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
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string SubmittedBy { get; set; }
        public DateTime SubmittedDate { get; set; }
    }
}
