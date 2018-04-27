using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool Active { get; set; }
        public Department()
        {
            Active = true;
        }
    }
}
