using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class E_CODEModel
    {
        public int E_CODEID { get; set; }
        public int E_GROUPID { get; set; }
        [Display(Name = "Jenis Kod Hiburan")]
        [Required(ErrorMessage = "Sila masukkan tajuk Kod Hiburan")]
        [StringLength(255)]
        public string E_C_DESC { get; set; }
        public float? E_FEE { get; set; }
        public float? E_BASE_FEE { get; set; }
        public float? E_OBJECT_FEE { get; set; }
        public string E_OBJECT_NAME { get; set; }
        public int E_PERIOD { get; set; }
        public int E_PERIOD_Q { get; set; }
        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }

        public string E_G_DESC { get; set; }
    }
    public class SelectedEntmtCodeModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
