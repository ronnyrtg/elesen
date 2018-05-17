using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class AppStatusModel
    {
        public int AppStatusID { get; set; }

        [Required(ErrorMessage = "Please Enter Status Description")]
        [Display(Name = "Status Description")]
        [StringLength(100)]
        public string StatusDesc { get; set; }

        [Required(ErrorMessage = "Please Enter Percent Progress")]
        [Display(Name = "Percent Progress")]
        public float PercentProgress { get; set; }
    }
}
