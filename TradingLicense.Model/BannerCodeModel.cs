using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class BannerCodeModel
    {
        public int BannerCodeID { get; set; }

        [Display(Name = "Banner License Code")]
        [Required(ErrorMessage = "Sila masukkan kod lesen iklan")]
        [StringLength(5)]
        public string BCodeNumber { get; set; }
        [Display(Name = "Banner Type Description")]
        [Required(ErrorMessage = "Sila masukkan penerangan jenis iklan")]
        [StringLength(255)]
        public string BannerCodeDesc { get; set; }
        [Display(Name = "Processing Fee")]
        [Required(ErrorMessage = "Sila masukkan Yuran Pemprosesan")]
        public float ProcessingFee { get; set; }
        [Display(Name = "Extra Fee after first 8 meter square")]
        public float ExtraFee { get; set; }
        [Display(Name = "Period")]
        [Required(ErrorMessage = "Sila pilih tahunan,bulanan,mingguan atau harian")]
        public int Period { get; set; }
        [Display(Name = "Valid Period Multiplier")]
        [Required(ErrorMessage = "Sila masukkan pekali bagi tempoh")]
        public int PeriodQuantity { get; set; }
        [Display(Name = "Fee per Period")]
        [Required(ErrorMessage = "Sila masukkan Fi mengikut tempoh")]
        public float PeriodFee { get; set; }
        [Required(ErrorMessage = "Sila pilih jenis kelulusan")]
        [Display(Name = "Mode")]
        public int Mode { get; set; }

        [Display(Name = "Aktif?")]
        public bool Active { get; set; }
    }
    public class SelectedBannerCodeModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
