using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class RequiredDocModel
    {
        public int RD_ID { get; set; }

        [Display(Name = "Required Document Description")]
        [Required]
        [StringLength(255)]
        public string RD_DESC { get; set; }
    }

    public class RequiredDocList
    {
        public int RequiredDocID { get; set; }

        public int AttachmentID { get; set; }
    }
}
