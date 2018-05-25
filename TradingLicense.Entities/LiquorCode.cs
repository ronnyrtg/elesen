using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TradingLicense.Entities
{
    public class LiquorCode
    {
        [Key]
        public int LiquorCodeID { get; set; }
        [Required]
        [StringLength(5)]
        [Column(TypeName = "VARCHAR2")]
        public string LCodeNumber { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string LiquorCodeDesc { get; set; }
        public string DefaultHours { get; set; }
        public float ExtraHourFee { get; set; }
        public int Period { get; set; }
        public int PeriodQuantity { get; set; }
        public float PeriodFee { get; set; }
        public int Mode { get; set; }
        public bool Active { get; set; }
        public LiquorCode()
        {
            Active = true;
        }
    }
}
