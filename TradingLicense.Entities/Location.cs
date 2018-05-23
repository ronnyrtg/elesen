using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }
        [Required]
        [StringLength(4)]
        [Column(TypeName = "VARCHAR2")]
        public string LocationCode { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string LocationDesc { get; set; }
        public int? UsersID { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool Active { get; set; }
        public Location()
        {
            Active = true;
        }

        public virtual Users Users { get; set; }
    }
}
