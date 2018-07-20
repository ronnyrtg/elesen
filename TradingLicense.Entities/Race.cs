using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TradingLicense.Entities
{
    public class RACE
    {
        [Key]
        public int RACEID { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string RACEDESC { get; set; }
        public bool ACTIVE { get; set; }
        public RACE()
        {
            ACTIVE = true;
        }
    }
}
