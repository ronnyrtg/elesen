using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class BCLinkDep
    {
        [Key]
        public int BussCodLinkDepID { get; set; }
        [Required]
        public int BusinessCodeID { get; set; }
        [Required]
        public int DepartmentID { get; set; }

        public virtual BusinessCode BusinessCode { get; set; }
        public virtual Department Department { get; set; }

    }
}
