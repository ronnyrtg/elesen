using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TradingLicense.Entities
{
    public class StallCode
    {
        [Key]
        public int StallCodeID { get; set; }
        [Required]
        [StringLength(5)]
        [Column(TypeName = "VARCHAR2")]
        public string SCodeNumber { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string StallCodeDesc { get; set; }
        public float Fee { get; set; }
        public int Period { get; set; }
        public int PeriodQuantity { get; set; }
        public int Mode { get; set; }
        public bool Active { get; set; }
        public StallCode()
        {
            Active = true;
        }
    }
}
