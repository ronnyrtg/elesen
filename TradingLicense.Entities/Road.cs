using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class ROAD
    {
        [Key]
        public int ROADID { get; set; }
        [Required]
        [StringLength(4)]
        [Column(TypeName = "VARCHAR2")]
        public string ROADCODE { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string ROADDESC { get; set; }
        public bool ACTIVE { get; set; }
        public ROAD()
        {
            ACTIVE = true;
        }
    }
}
