using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class AttachmentModel
    {
        public int AttachmentID { get; set; }

        [Display(Name = "Attachment File")]
        public string FileName { get; set; }
    }
}
