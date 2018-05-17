using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TradingLicense.Entities
{
    public class HawkerType
    {
        [Key]
        public int HawkerTypeID { get; set; }
        [Required]
        [StringLength(60)]
        [Column(TypeName = "VARCHAR2")]
        public string HawkerTypeDesc { get; set; }
        public float Fee { get; set; }
        public int Period { get; set; }
        public int PeriodQuantity { get; set; }
        public bool Active { get; set; }
        public HawkerType()
        {
            Active = true;
        }
    }
}
