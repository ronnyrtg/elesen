using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class RequiredDocModel
    {
        public int RequiredDocID { get; set; }

        [Display(Name = "Required Document Description")]
        [Required]
        [StringLength(255)]
        public string RequiredDocDesc { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }

    public class RequiredDocList
    {
        public int RequiredDocID { get; set; }

        public int AttachmentID { get; set; }
    }

    public class AdditionalDocList
    {
        public int AdditionalDocID { get; set; }

        public int AttachmentID { get; set; }
    }
}
