using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class IND_L_ATT
    {
        [Key]
        public int IND_L_ATTID { get; set; }
        public int IND_ID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string ATT_DESC { get; set; }
        public int ATT_ID { get; set; }


        public virtual INDIVIDUAL INDIVIDUAL { get; set; }
        public virtual ATTACHMENT ATTACHMENT { get; set; }
    }
}
