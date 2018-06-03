using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class EntmtGroupModel
    {
        public int EntmtGroupID { get; set; }
        [Display(Name = "Entertainment Group Code")]
        [Required(ErrorMessage = "Please enter Entertainment Group Code")]
        [StringLength(10)]
        public string EntmtGroupCode { get; set; }
        [Display(Name = "Entertainment Group Description")]
        [Required(ErrorMessage = "Please enter Entertainment Group Description")]
        [StringLength(255)]
        public string EntmtGroupDesc { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
