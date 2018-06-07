using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class PALinkAddDoc
    {
        [Key]
        public int PALinkAddDocID { get; set; }

        public int PremiseApplicationID { get; set; }

        public int AdditionalDocID { get; set; }

        public int? AttachmentID { get; set; }

        public virtual PremiseApplication PremiseApplication { get; set; }

        public virtual AdditionalDoc AdditionalDoc { get; set; }
    }
}
