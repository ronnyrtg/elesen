using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class HolidayModel
    {
        public int HOLIDAYID { get; set; }

        [Display(Name = "Holiday Name")]
        [Required(ErrorMessage = "Please Enter Holiday Name")]
        [StringLength(100)]
        public string H_DESC { get; set; }

        [Display(Name = "Holiday Date")]
        [Required(ErrorMessage = "Please Enter Holiday Date")]
        public DateTime H_DATE { get; set; }
    }
}