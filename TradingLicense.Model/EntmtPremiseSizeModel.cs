using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class EntmtPremiseSizeModel
    {
        public int E_P_SIZEID { get; set; }
        public int E_PREMISEID { get; set; }
        [Display(Name = "Jenis Kod Hiburan")]
        [Required(ErrorMessage = "Sila masukkan tajuk Kod Hiburan")]
        [StringLength(255)]
        public string E_S_DESC { get; set; }
        public float? E_S_FEE { get; set; }
        public float? E_S_B_FEE { get; set; }
        public float? E_S_O_FEE { get; set; }
        public string E_S_O_NAME { get; set; }
        public int E_S_PERIOD { get; set; }
        public int E_S_PERIOD_Q { get; set; }
        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }

        public string E_P_DESC { get; set; }
    }
    public class SelectedEntmtCodeModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
