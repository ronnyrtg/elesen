using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class BALinkReqDoc
    {
        [Key]
        public int BALinkReqDocID { get; set; }

        public int BannerApplicationID { get; set; }

        public int RequiredDocID { get; set; }

        public int? AttachmentID { get; set; }

        public virtual BannerApplication BannerApplication { get; set; }

        public virtual Attachment Attachment { get; set; }

        public virtual BAReqDoc BAReqDoc { get; set; }

    }
}
