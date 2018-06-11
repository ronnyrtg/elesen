using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class SALinkReqDoc
    {
        [Key]
        public int LAReqDocID { get; set; }
        public int LiquorApplicationID { get; set; }
        public int RequiredDocID { get; set; }
        public int AttachmentID { get; set; }

        public virtual RequiredDoc RequiredDoc { get; set; }
        public virtual LiquorApplication LiquorApplication { get; set; }
    }
}
