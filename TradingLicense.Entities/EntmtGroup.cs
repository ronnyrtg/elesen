using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class EntmtGroup
    {
        [Key]
        public int EntmtGroupID { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(10)]
        public string EntmtGroupCode { get; set; }
        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string EntmtGroupDesc { get; set; }

        public bool Active { get; set; }
        public EntmtGroup()
        {
            Active = true;
        }
    }
}
