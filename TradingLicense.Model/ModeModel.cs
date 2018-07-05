using System.ComponentModel.DataAnnotations;
namespace TradingLicense.Model
{
    public class ModeModel
    {
        public int ModeID { get; set; }

        [Display(Name = "Kelulusan")]
        [Required(ErrorMessage = "Masukkan jenis kelulusan")]
        [StringLength(50)]
        public string ModeDesc { get; set; }

        [Display(Name = "Aktif?")]
        public bool Active { get; set; }
    }
}
