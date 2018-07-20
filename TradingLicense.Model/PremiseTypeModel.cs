using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class PremiseTypeModel
    {
        public int PT_ID { get; set; }

        [Display(Name = "Premise Description")]
        [Required(ErrorMessage = "Please enter Premise Description")]
        [StringLength(255)]
        public string PT_DESC { get; set; }

        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }
    }
}
