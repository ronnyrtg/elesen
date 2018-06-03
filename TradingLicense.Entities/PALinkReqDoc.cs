using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class PALinkReqDoc
    {
        [Key]
        public int PALinkReqDocID { get; set; }

        public int PremiseApplicationID { get; set; }

        public int RequiredDocID { get; set; }

        public int? AttachmentID { get; set; }

        public virtual PremiseApplication PremiseApplication { get; set; }

        public virtual RequiredDoc RequiredDoc { get; set; }
    }
}
