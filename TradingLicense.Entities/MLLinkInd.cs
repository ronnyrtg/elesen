using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class MLLinkInd
    {
        [Key]
        public int MLLinkIndID { get; set; }

        public int MLPremiseApplicationID { get; set; }

        public int IndividualID { get; set; }

        public virtual Individual Individual { get; set; }

        public virtual MLPremiseApplication MLPremiseApplication { get; set; }
    }
}
