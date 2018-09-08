using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class TUTORIAL
    {
        [Key]
        public int TUTORIAL_ID { get; set; }
        public int? BT_ID { get; set; }
        public float? LUAS { get; set; }
        public DateTime? TARIKH { get; set; }

        //For string type variable
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string T_DESC { get; set; }

        public bool ACTIVE { get; set; }
    }
}
