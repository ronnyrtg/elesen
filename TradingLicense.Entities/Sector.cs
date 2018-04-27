using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class Sector
    {
        [Key]
        public int SectorID { get; set; }

        [Column(TypeName = "VARCHAR2")]
        [StringLength(30)]
        public string SectorDesc { get; set; }

        public bool Active { get; set; }
        public Sector()
        {
            Active = true;
        }
    }
}
