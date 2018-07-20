using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class LOCATION
    {
        [Key]
        public int LOCATIONID { get; set; }
        [Required]
        [StringLength(4)]
        [Column(TypeName = "VARCHAR2")]
        public string L_CODE { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string L_DESC { get; set; }
        public bool ACTIVE { get; set; }
        public LOCATION()
        {
            ACTIVE = true;
        }
    }
}
