using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class AttachmentModel
    {
        public int AttachmentID { get; set; }

        [Display(Name = "Attachment File")]
        public string FileName { get; set; }
    }
}
