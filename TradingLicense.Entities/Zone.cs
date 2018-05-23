using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class Zone
    {
        [Key]
        public int ZoneID { get; set; }
        [Required]
        [StringLength(4)]
        [Column(TypeName = "VARCHAR2")]
        public string ZoneCode { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string ZoneDesc { get; set; }
        public int? UsersID { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool Active { get; set; }
        public Zone()
        {
            Active = true;
        }

        public virtual Users Users { get; set; }
    }
}
