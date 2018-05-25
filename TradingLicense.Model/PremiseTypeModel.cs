using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class PremiseTypeModel
    {
        public int PremiseTypeID { get; set; }

        [Display(Name = "Premise Description")]
        [Required(ErrorMessage = "Please enter Premise Description")]
        [StringLength(255)]
        public string PremiseDesc { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
