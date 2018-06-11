using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class PALinkInd
    {
        [Key]
        public int PALinkIndID { get; set; }

        public int PremiseApplicationID { get; set; }

        public int IndividualID { get; set; }

        public virtual Individual Individual { get; set; }

        public virtual PremiseApplication PremiseApplication { get; set; }
    }
}
