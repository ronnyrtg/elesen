using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class AppStatusModel
    {
        public int APPSTATUSID { get; set; }

        [Required(ErrorMessage = "Please Enter Status Description")]
        [Display(Name = "Status Description")]
        [StringLength(100)]
        public string STATUSDESC { get; set; }
        [Required(ErrorMessage = "Please Enter Percent Progress")]
        [Display(Name = "Percent Progress")]
        public float PROGRESS { get; set; }
    }
}
