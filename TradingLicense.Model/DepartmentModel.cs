using System.ComponentModel.DataAnnotations;

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
        [Display(Name = "Internal?")]
        public int Internal { get; set; }
        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
