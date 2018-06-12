using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace TradingLicense.Model
{
    public class AttachmentModel
    {
        public int AttachmentID { get; set; }

        [Display(Name = "Attachment File")]
        public string FileName { get; set; }

        public string FileNameFullPath { get; set; }
        
    }
}
