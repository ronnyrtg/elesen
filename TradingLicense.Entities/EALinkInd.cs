using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class EALinkInd
    {
        [Key]
        public int EALinkIndID { get; set; }

        public int EntmtApplicationID { get; set; }

        public int IndividualID { get; set; }

        public virtual Individual Individual { get; set; }

        public virtual EntmtApplication EntmtApplication { get; set; }
    }
}
