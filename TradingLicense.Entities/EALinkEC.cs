using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class EALinkEC
    {
        [Key]
        public int EALinkECID { get; set; }

        public int EntmtApplicationID { get; set; }

        public int EntmtCodeID { get; set; }

        public virtual EntmtCode EntmtCode { get; set; }
        public virtual EntmtApplication EntmtApplication { get; set; }
    }
}
