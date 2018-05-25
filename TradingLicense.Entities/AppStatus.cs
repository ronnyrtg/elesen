using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class AppStatus
    {
        [Key]
        public int AppStatusID { get; set; }

        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string StatusDesc { get; set; }

        public float PercentProgress { get; set; }
    }
}
