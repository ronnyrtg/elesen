using System.ComponentModel.DataAnnotations;

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
