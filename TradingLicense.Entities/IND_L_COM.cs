using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class IND_L_COM
    {
        [Key]
        public int ILC_ID { get; set; }
        public int IND_ID { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string IND_POST { get; set; }
        public int COMPANYID { get; set; }


        public virtual INDIVIDUAL INDIVIDUAL { get; set; }
        public virtual COMPANY COMPANY { get; set; }
    }
}
