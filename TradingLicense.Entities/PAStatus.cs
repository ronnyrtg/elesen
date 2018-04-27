using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class PAStatus
    {
        [Key]
        public int PAStatusID { get; set; }

        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string StatusDesc { get; set; }

        public float PercentProgress { get; set; }
    }
}
