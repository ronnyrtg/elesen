namespace TradingLicense.Model
{
    public class RD_L_LTModel
    {
        public int RD_L_LTID { get; set; }
        public int RD_ID { get; set; }
        public int LIC_TYPEID { get; set; }

        public string RD_DESC { get; set; }
        public string IsChecked { get; set; }
        public string AttachmentFileName { get; set; }
        public int AttachmentId { get; set; }
    }
}
