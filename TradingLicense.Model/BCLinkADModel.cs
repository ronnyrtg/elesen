namespace TradingLicense.Model
{
    public class BCLinkADModel
    {
        public int BCLinkADID { get; set; }

        public int BusinessCodeID { get; set; }

        public int AdditionalDocID { get; set; }

        public string DocDesc { get; set; }

        public string IsChecked { get; set; }

        public string AttachmentFileName { get; set; }

        public int AttachmentId { get; set; }

        public int PremiseApplicationID { get; set; }
    }
}
