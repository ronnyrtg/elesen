using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TradingLicense.Entities
{
    public class HawkerCode
    {
        [Key]
        public int HawkerCodeID { get; set; }
        [Required]
        [StringLength(5)]
        [Column(TypeName = "VARCHAR2")]
        public string HCodeNumber { get; set; }
        [Required]
        [StringLength(60)]
        [Column(TypeName = "VARCHAR2")]
        public string HawkerCodeDesc { get; set; }
        public float Fee { get; set; }
        public int Period { get; set; }
        public int PeriodQuantity { get; set; }
        public int Mode { get; set; }
        public bool Active { get; set; }
        public HawkerCode()
        {
            Active = true;
        }
    }
}
