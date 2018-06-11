namespace TradingLicense.Model
{
    public class BTLinkReqDocModel
    {
        public int BTLinkReqDocID { get; set; }

        public int BusinessTypeID { get; set; }

        public int RequiredDocID { get; set; }

        public string RequiredDocDesc { get; set; }

        public string IsChecked { get; set; }

        public string AttachmentFileName { get; set; }

        public int AttachmentId { get; set; }

        public int PremiseApplicationID { get; set; }
        public int LiquorApplicationID { get; set; }
        public int EntmtApplicationID { get; set; }
    }
}
