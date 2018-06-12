using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class IndLinkAttModel
    {
        public int IndLinkAttID { get; set; }
        public int IndividualID { get; set; }

        public string AttachmentDesc { get; set; }
        public int AttachmentID { get; set; }


        public virtual IndividualModel Individual { get; set; }
        public virtual AttachmentModel Attachment { get; set; }
    }
}
