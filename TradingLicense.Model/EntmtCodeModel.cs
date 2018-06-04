using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class EntmtCodeModel
    {
        public int EntmtCodeID { get; set; }
        public int EntmtGroupID { get; set; }
        [Display(Name = "Entertainment Code Description")]
        [Required(ErrorMessage = "Please enter Entertainment Object Description")]
        [StringLength(255)]
        public string EntmtCodeDesc { get; set; }
        public float? Fee { get; set; }
        public float? BaseFee { get; set; }
        public float? ObjectFee { get; set; }
        public string ObjectName { get; set; }
        public int Period { get; set; }
        public int PeriodQuantity { get; set; }
        public int Mode { get; set; }
        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        public string EntmtGroupDesc { get; set; }
    }
    public class SelectedEntmtCodeModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
