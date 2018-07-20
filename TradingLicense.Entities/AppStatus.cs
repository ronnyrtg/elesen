using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class APPSTATUS
    {
        [Key]
        public int APPSTATUSID { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string STATUSDESC { get; set; }
        public float PROGRESS { get; set; }
    }
}
