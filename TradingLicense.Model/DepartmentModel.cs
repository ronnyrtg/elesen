using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class DepartmentModel
    {
        public int DEP_ID { get; set; }

        [Display(Name = "Kod Jabatan/Unit")]
        [Required(ErrorMessage = "Masukkan kod jabatan")]
        [StringLength(10)]
        public string DEP_CODE { get; set; }

        [Display(Name = "Nama Jabatan/Unit")]
        [Required(ErrorMessage = "Masukkan nama penuh Jabatan/Unit")]
        [StringLength(255)]
        public string DEP_DESC { get; set; }
        [Display(Name = "Dalam atau Luar PL?")]
        public int INTERNAL { get; set; }
        [Display(Name = "Boleh Route?")]
        public int ROUTE { get; set; }
        [Display(Name = "Active")]
        public bool ACTIVE { get; set; }
    }
}
