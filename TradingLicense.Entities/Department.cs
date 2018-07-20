using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class DEPARTMENT
    {
        [Key]
        public int DEP_ID { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string DEP_CODE { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string DEP_DESC { get; set; }
        public int INTERNAL { get; set; }
        public int ROUTE { get; set; }
        public bool ACTIVE { get; set; }
        public DEPARTMENT()
        {
            ACTIVE = true;
        }
    }
}
