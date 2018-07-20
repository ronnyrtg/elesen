using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class SECTOR
    {
        [Key]
        public int SECTORID { get; set; }

        [Column(TypeName = "VARCHAR2")]
        [StringLength(30)]
        public string SECTORDESC { get; set; }

        public bool ACTIVE { get; set; }
        public SECTOR()
        {
            ACTIVE = true;
        }
    }
}
