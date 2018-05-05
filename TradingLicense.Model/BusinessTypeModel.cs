using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class BusinessTypeModel
    {
        public int BusinessTypeID { get; set; }

        [Display(Name = "Business Type Code")]
        [Required(ErrorMessage = "Please enter Business Type Code")]
        [StringLength(1)]
        public string BusinessTypeCode { get; set; }

        [Display(Name = "Business Description")]
        [Required(ErrorMessage = "Please enter Business Description")]
        [StringLength(255)]
        public string BusinessTypeDesc { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
