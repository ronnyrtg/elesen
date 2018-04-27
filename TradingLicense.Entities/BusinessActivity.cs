using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class BusinessActivity
    {
        [Key]
        public int BusinessActivityID { get; set; }
        public int BusinessCodeID { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UnitNo { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]

        public string Floor { get; set; }
        public float PremiseArea { get; set; }

        public virtual BusinessCode BusinessCode { get; set; }
        
    }
}
