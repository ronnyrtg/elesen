using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class ATTACHMENT
    {
        [Key]
        public int ATT_ID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string FILENAME { get; set; }
    }
}
