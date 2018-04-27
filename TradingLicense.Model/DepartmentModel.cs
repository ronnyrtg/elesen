using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class DepartmentModel
    {
        public int DepartmentID { get; set; }

        [Display(Name = "Department Code")]
        [Required(ErrorMessage = "Please Enter Department Code")]
        [StringLength(10)]
        public string DepartmentCode { get; set; }

        [Display(Name = "Department Description")]
        [Required(ErrorMessage = "Please Enter Department Description")]
        [StringLength(255)]
        public string DepartmentDesc { get; set; }
        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
