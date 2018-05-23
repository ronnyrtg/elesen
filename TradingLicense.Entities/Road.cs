using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class Road
    {
        [Key]
        public int RoadID { get; set; }
        [Required]
        [StringLength(4)]
        [Column(TypeName = "VARCHAR2")]
        public string RoadCode { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string RoadDesc { get; set; }
        public int? UsersID { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool Active { get; set; }
        public Road()
        {
            Active = true;
        }

        public virtual Users Users { get; set; }
    }
}
