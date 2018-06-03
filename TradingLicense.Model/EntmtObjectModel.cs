using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class EntmtObjectModel
    {
        public int EntmtObjectID { get; set; }

        [Display(Name = "Entertainment Object Description")]
        [Required(ErrorMessage = "Please enter Entertainment Object Description")]
        [StringLength(255)]
        public string EntmtObjectDesc { get; set; }
        [Required(ErrorMessage = "Please enter Fee")]
        public float ObjectFee { get; set; }
        [Display(Name = "Object Name")]
        [Required(ErrorMessage = "Please enter Object Name")]
        [StringLength(255)]
        public string ObjectName { get; set; }
        public float? BaseFee { get; set; }
        public int? Period { get; set; }
        public int? PeriodQuantity { get; set; }
        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
