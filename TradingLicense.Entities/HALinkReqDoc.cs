using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class HALinkReqDoc
    {
        [Key]
        public int HALinkReqDocID { get; set; }

        public int HawkerApplicationID { get; set; }

        public int RequiredDocID { get; set; }

        public int? AttachmentID { get; set; }

        public virtual HawkerApplication HawkerApplication { get; set; }

    }
}
