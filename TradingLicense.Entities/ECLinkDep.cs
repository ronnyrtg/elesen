using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class ECLinkDep
    {
        [Key]
        public int EntCodLinkDepID { get; set; }
        [Required]
        public int EntmtCodeID { get; set; }
        [Required]
        public int DepartmentID { get; set; }

        public virtual EntmtCode EntmtCode { get; set; }
        public virtual Department Department { get; set; }

    }
}
