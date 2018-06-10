using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class StallCodeModel
    {
        public int StallCodeID { get; set; }

        [Display(Name = "Stall Code")]
        [Required(ErrorMessage = "Please enter Stall Code")]
        [StringLength(5)]
        public string SCodeNumber { get; set; }
        [Display(Name = "Stall Type Description")]
        [Required(ErrorMessage = "Please enter Stall Type Description")]
        [StringLength(255)]
        public string StallCodeDesc { get; set; }
        [Display(Name = "Stall Type Fee")]
        [Required(ErrorMessage = "Please enter Stall Type Fee")]
        public float Fee { get; set; }
        [Display(Name = "License Validity Period")]
        [Required(ErrorMessage = "Please enter valid period")]
        public int Period { get; set; }
        [Display(Name = "Valid Period Multiplier")]
        [Required(ErrorMessage = "Please enter period multiplier, default is 1")]
        public int PeriodQuantity { get; set; }
        [Display(Name = "Mode")]
        [Required(ErrorMessage = "Please enter mode, default is 1")]
        public int Mode { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
    public class SelectedStallCodeModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
