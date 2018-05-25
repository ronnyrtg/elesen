using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class BTLinkReqDoc
    {
        [Key]
        public int BTLinkReqDocID { get; set; }

        public int BusinessTypeID { get; set; }

        public int RequiredDocID { get; set; }

        public virtual BusinessType BusinessType { get; set; }

        public virtual RequiredDoc RequiredDoc { get; set; }
    }
}
