using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class IndLinkAtt
    {
        [Key]
        public int IndLinkAttID { get; set; }
        public int IndividualID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string AttachmentDesc { get; set; }
        public int AttachmentID { get; set; }


        public virtual Individual Individual { get; set; }
        public virtual Attachment Attachment { get; set; }
    }
}
