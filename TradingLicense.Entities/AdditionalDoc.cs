using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class AdditionalDoc
    {
        [Key]
        public int AdditionalDocID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string DocDesc { get; set; }
        public bool Active { get; set; }
        public AdditionalDoc()
        {
            Active = true;
        }
    }
}
