using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class DepartmentModel
    {
        public int DepartmentID { get; set; }

        [Display(Name = "Kod Jabatan/Unit")]
        [Required(ErrorMessage = "Masukkan kod jabatan")]
        [StringLength(10)]
        public string DepartmentCode { get; set; }

        [Display(Name = "Nama Jabatan/Unit")]
        [Required(ErrorMessage = "Masukkan nama penuh Jabatan/Unit")]
        [StringLength(255)]
        public string DepartmentDesc { get; set; }
        [Display(Name = "Dalam atau Luar PL?")]
        public int Internal { get; set; }
        [Display(Name = "Boleh Route?")]
        public int Routeable { get; set; }
        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
