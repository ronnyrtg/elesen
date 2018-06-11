using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class LALinkReqDoc
    {
        [Key]
        public int LALinkReqDocID { get; set; }

        public int LiquorApplicationID { get; set; }

        public int RequiredDocID { get; set; }

        public int? AttachmentID { get; set; }

        public virtual LiquorApplication LiquorApplication { get; set; }

    }
}
