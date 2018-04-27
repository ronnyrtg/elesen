using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class UnitLuar
    {
        [Key]
        public int UnitLuarID { get; set; }

        [Column(TypeName = "VARCHAR2")]
        [StringLength(50)]
        public string UnitLuarCode { get; set; }

        [Column(TypeName = "VARCHAR2")]
        [StringLength(255)]
        public string UnitLuarDesc { get; set; }

        public bool active { get; set; }

    }
}
