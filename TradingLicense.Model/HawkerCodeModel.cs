using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class HawkerCodeModel
    {
        public int HawkerCodeID { get; set; }

        [Display(Name = "Hawker Code")]
        [Required(ErrorMessage = "Please enter Hawker Code")]
        [StringLength(5)]
        public string HCodeNumber { get; set; }
        [Display(Name = "Hawker Type Description")]
        [Required(ErrorMessage = "Please enter Hawker Type Description")]
        [StringLength(60)]
        public string HawkerCodeDesc { get; set; }
        [Display(Name = "Hawker Type Fee")]
        [Required(ErrorMessage = "Please enter Hawker Type Fee")]
        public float Fee { get; set; }
        [Display(Name = "License Validity Period")]
        [Required(ErrorMessage = "Please enter valid period")]
        public int Period { get; set; }
        [Display(Name = "Valid Period Multiplier")]
        [Required(ErrorMessage = "Please enter period multiplier, default is 1")]
        public int PeriodQuantity { get; set; }
        [Required(ErrorMessage = "Sila pilih jenis kelulusan")]
        [Display(Name = "Mode")]
        public int Mode { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
