using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class LiquorCodeModel
    {
        public int LiquorCodeID { get; set; }

        [Display(Name = "Liquor License Code")]
        [Required(ErrorMessage = "Sila masukkan kod lesen minuman keras")]
        [StringLength(5)]
        public string LCodeNumber { get; set; }
        [Display(Name = "Liquor Type Description")]
        [Required(ErrorMessage = "Sila masukkan penerangan jenis minuman keras")]
        [StringLength(255)]
        public string LiquorCodeDesc { get; set; }
        [Display(Name = "Start & Stop time")]
        [StringLength(255)]
        public string DefaultHours { get; set; }
        [Display(Name = "Extra hourly fee after allocated time")]
        public float ExtraHourFee { get; set; }
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
}
