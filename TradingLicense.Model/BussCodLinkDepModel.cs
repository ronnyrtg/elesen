using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class BussCodLinkDepModel
    {
        public int BussCodLinkDepID { get; set; }

        [Display(Name ="Business Code Description")]
        [Required(ErrorMessage = "Please Select Business Code Description")]
        public int BusinessCodeID { get; set; }

        [Display(Name = "Department Code")]
        [Required(ErrorMessage = "Please Select Department Code")]
        public int DepartmentID { get; set; }

        public string CodeDesc { get; set; }

        public string DepartmentDesc { get; set; }
    }
}
