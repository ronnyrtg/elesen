using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace TradingLicense.Model
{
    public class AttachmentModel
    {
        public int ATT_ID { get; set; }
        [Display(Name = "Attachment File Name")]
        public string FILENAME { get; set; }

        public string FileNameFullPath { get; set; }
        
    }
}
