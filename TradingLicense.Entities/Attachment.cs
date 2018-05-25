using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class Attachment
    {
        [Key]

        public int AttachmentID { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string FileName { get; set; }
    }
}
