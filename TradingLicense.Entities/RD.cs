using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    //Required Documents
    public class RD
    {
        [Key]
        public int RD_ID { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string RD_DESC { get; set; }
    }
}
