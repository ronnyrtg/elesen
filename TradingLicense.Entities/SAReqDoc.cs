using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class SAReqDoc
    {
        [Key]
        public int SAReqDocID { get; set; }

        public int RequiredDocID { get; set; }

        public virtual RequiredDoc RequiredDoc { get; set; }
    }
}
