using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class EntmtObjectModel
    {
        public int E_OID { get; set; }

        [Display(Name = "Entertainment Object Description")]
        [Required(ErrorMessage = "Please enter Entertainment Object Description")]
        [StringLength(255)]
        public string E_O_DESC { get; set; }
        [Required(ErrorMessage = "Please enter Fee")]
        public float E_O_FEE { get; set; }
        public float? E_O_B_FEE { get; set; }
        [Display(Name = "Object Name")]
        [Required(ErrorMessage = "Please enter Object Name")]
        [StringLength(255)]
        public string E_O_NAME { get; set; }       
        public int? E_O_PERIOD { get; set; }
        public int? E_O_PERIOD_Q { get; set; }
        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
    public class SelectedEntmtObjectModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
