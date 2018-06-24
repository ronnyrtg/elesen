using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TradingLicense.Entities
{
    public class Race
    {
        [Key]
        public int RaceID { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string RaceDesc { get; set; }
        public bool Active { get; set; }
        public Race()
        {
            Active = true;
        }
    }
}
