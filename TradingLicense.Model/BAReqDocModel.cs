using System.Collections.Generic;
namespace TradingLicense.Model
{
    public class BAReqDocModel
    {
        public int BAReqDocID { get; set; }

        public int RequiredDocID { get; set; }

        public string RequiredDocDesc { get; set; }

        public string IsChecked { get; set; }

        public string AttachmentFileName { get; set; }

        public int AttachmentId { get; set; }

        public List<int> RequiredDocs { get; set; }

        public int BannerApplicationID { get; set; }
    }
}
