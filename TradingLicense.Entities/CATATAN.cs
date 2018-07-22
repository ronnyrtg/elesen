using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class CATATAN
    {
        [Key]
        public int CATATANID { get; set; }
        public int APP_ID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string CONTENT { get; set; }

        public virtual APPLICATION APPLICATION { get; set; }
    }
}
