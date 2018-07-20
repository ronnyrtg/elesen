using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class ZONE
    {
        [Key]
        public int ZONEID { get; set; }
        [Required]
        [StringLength(4)]
        [Column(TypeName = "VARCHAR2")]
        public string ZONECODE { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string ZONEDESC { get; set; }
        public bool ACTIVE { get; set; }
        public ZONE()
        {
            ACTIVE = true;
        }
    }
}
