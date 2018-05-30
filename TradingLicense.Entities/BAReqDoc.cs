using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class BAReqDoc
    {
        [Key]
        public int BAReqDocID { get; set; }

        public int RequiredDocID { get; set; }

        public virtual RequiredDoc RequiredDoc { get; set; }
    }
}
