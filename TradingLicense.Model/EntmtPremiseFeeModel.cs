using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class EntmtPremiseFeeModel
    {
        public int E_P_FEEID { get; set; }
        public string E_P_DESC { get; set; }
        [Display(Name = "Jenis Kod Hiburan")]
        [Required(ErrorMessage = "Sila masukkan tajuk Kod Hiburan")]
        public string E_S_DESC { get; set; }
        public float? E_S_FEE { get; set; }
        public float? E_S_B_FEE { get; set; }
        public float? E_S_O_FEE { get; set; }
        public string E_S_O_NAME { get; set; }
        public int E_S_PERIOD { get; set; }
        public int E_S_P_QTY { get; set; }
        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }
    }
    public class SelectedEntmtCodeModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
