using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class E_GROUPModel
    {
        public int E_GROUPID { get; set; }
        [Display(Name = "Kumpulan Hiburan")]
        [Required(ErrorMessage = "Sila masukkan nama kumpulan hiburan")]
        [StringLength(10)]
        public string E_G_CODE { get; set; }
        [Display(Name = "Jenis Hiburan")]
        [Required(ErrorMessage = "Sila masukkan jenis hiburan")]
        [StringLength(255)]
        public string E_G_DESC { get; set; }

        [Display(Name = "Aktif?")]
        public bool ACTIVE { get; set; }
    }
}
