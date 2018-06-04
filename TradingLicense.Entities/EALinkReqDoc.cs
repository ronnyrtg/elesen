using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class EALinkReqDoc
    {

        [Key]
        public int EALinkReqDocID { get; set; }

        public int EntmtApplicationID { get; set; }

        public int RequiredDocID { get; set; }

        public int? AttachmentID { get; set; }

        public virtual EntmtApplication EntmtApplication { get; set; }

    }
}
