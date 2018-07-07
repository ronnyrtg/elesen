using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class IndLinkCom
    {
        [Key]
        public int IndLinkComID { get; set; }
        public int IndividualID { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string IndPosition { get; set; }
        public int CompanyID { get; set; }


        public virtual Individual Individual { get; set; }
        public virtual Company Company { get; set; }
    }
}
