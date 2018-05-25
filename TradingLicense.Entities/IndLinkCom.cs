using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class IndLinkCom
    {
        [Key]
        public int IndLinkComID { get; set; }
        public int IndividualID { get; set; }
        public int CompanyID { get; set; }


        public virtual Individual Individual { get; set; }
        public virtual Company Company { get; set; }
    }
}
