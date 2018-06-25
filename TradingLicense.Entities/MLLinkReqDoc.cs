using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class MLLinkReqDoc
    {
        [Key]
        public int MLLinkReqDocID { get; set; }

        public int MLPremiseApplicationID { get; set; }

        public int RequiredDocID { get; set; }

        public int? AttachmentID { get; set; }

        public virtual MLPremiseApplication MLPremiseApplication { get; set; }

    }
}
