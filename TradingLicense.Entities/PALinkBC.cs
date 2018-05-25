using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class PALinkBC
    {
        [Key]
        public int PALinkBCID { get; set; }

        public int PremiseApplicationID { get; set; }

        public int BusinessCodeID { get; set; }

        public virtual BusinessCode BusinessCode { get; set; }
        public virtual PremiseApplication PremiseApplication { get; set; }
    }
}
