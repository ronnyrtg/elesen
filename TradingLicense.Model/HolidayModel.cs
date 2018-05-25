using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class HolidayModel
    {
        public int HolidayID { get; set; }

        [Display(Name = "Holiday Name")]
        [Required(ErrorMessage = "Please Enter Holiday Name")]
        [StringLength(100)]
        public string HolidayDesc { get; set; }

        [Display(Name = "Holiday Date")]
        [Required(ErrorMessage = "Please Enter Holiday Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime HolidayDate { get; set; }
    }
}