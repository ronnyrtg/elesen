namespace TradingLicense.Entities
{
    public class BCLinkAD
    {
        public int BCLinkADID { get; set; }

        public int BusinessCodeID { get; set; }

        public int AdditionalDocID { get; set; }

        public virtual AdditionalDoc AdditionalDoc { get; set; }
    }
}
