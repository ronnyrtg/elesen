using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class BCLinkDep
    {
        [Key]
        public int BussCodLinkDepID { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BusinessCodeID { get; set; }
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentID { get; set; }

        public virtual BusinessCode BusinessCode { get; set; }
        public virtual Department Department { get; set; }

    }
}
