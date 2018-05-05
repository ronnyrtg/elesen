using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class BusinessType
    {
        [Key]
        public int BusinessTypeID { get; set; }
        [Required]
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string BusinessTypeCode { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string BusinessTypeDesc { get; set; }
        public bool Active { get; set; }
        public BusinessType()
        {
            Active = true;
        }
    }
}
