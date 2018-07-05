using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class Mode
    {
        [Key]
        public int ModeID { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ModeDesc { get; set; }
        public bool Active { get; set; }
        public Mode()
            {
                Active = true;
            }
    }
}
