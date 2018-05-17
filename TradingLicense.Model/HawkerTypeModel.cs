using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class HawkerTypeModel
    {
        public int HawkerTypeID { get; set; }

        [Display(Name = "Hawker Type Description")]
        [Required(ErrorMessage = "Please enter Hawker Type Description")]
        [StringLength(60)]
        public string HawkerTypeDesc { get; set; }
        [Display(Name = "Hawker Type Fee")]
        [Required(ErrorMessage = "Please enter Hawker Type Fee")]
        public float Fee { get; set; }
        [Display(Name = "License Validity Period")]
        [Required(ErrorMessage = "Please enter valid period")]
        public int Period { get; set; }
        [Display(Name = "Valid Period Multiplier")]
        [Required(ErrorMessage = "Please enter period multiplier, default is 1")]
        public int PeriodQuantity { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
