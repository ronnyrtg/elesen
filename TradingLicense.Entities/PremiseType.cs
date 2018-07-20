using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TradingLicense.Entities
{
    public class PREMISETYPE
    {
        [Key]
        public int PT_ID { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string PT_DESC { get; set; }
        public bool ACTIVE { get; set; }
        public PREMISETYPE()
        {
            ACTIVE = true;
        }
    }
}
