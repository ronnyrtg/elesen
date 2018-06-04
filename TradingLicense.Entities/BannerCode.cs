using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TradingLicense.Entities
{
    public class BannerCode
    {
        [Key]
        public int BannerCodeID { get; set; }
        [Required]
        [StringLength(5)]
        [Column(TypeName = "VARCHAR2")]
        public string BCodeNumber { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string BannerCodeDesc { get; set; }
        public float ProcessingFee { get; set; }
        public float ExtraFee { get; set; }
        public int Period { get; set; }
        public int PeriodQuantity { get; set; }
        public float PeriodFee { get; set; }
        public int Mode { get; set; }
        public bool Active { get; set; }
        public BannerCode()
        {
            Active = true;
            Mode = 2;
        }

        public virtual BannerApplication BannerApplication { get; set; }
    }
}
