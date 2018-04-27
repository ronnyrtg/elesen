using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class AdditionalInfo
    {
        [Key]
        public int AdditionalInfoID { get; set; }
        public int BusinessCodeID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string InfoDesc { get; set; }
        [StringLength(30)]
        [Column(TypeName = "VARCHAR2")]
        public string InfoQuantity { get; set; }

        public virtual BusinessCode BusinessCode { get; set; }
    }
}
