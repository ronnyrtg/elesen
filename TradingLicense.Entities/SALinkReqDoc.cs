using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class SALinkReqDoc
    {
        [Key]
        public int SALinkReqDocID { get; set; }

        public int StallApplicationID { get; set; }

        public int RequiredDocID { get; set; }

        public int? AttachmentID { get; set; }

        public virtual StallApplication StallApplication { get; set; }

    }
}
