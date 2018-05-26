using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string DepartmentCode { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string DepartmentDesc { get; set; }
        public int Internal { get; set; }
        public bool Active { get; set; }
        public Department()
        {
            Active = true;
        }
    }
}
