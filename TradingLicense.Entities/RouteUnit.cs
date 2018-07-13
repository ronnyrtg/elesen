using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class RouteUnit
    {
        [Key]
        public int RouteUnitID { get; set; }
        public int ApplicationType { get; set; }
        public int ApplicationID { get; set; }
        public int DepartmentID { get; set; }
        public bool IsSupported { get; set; }
        public int? UsersID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string Comment { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public bool Active { get; set; }
        public RouteUnit()
        {
            Active = true;
        }

        public virtual Users Users { get; set; }
        public virtual Department Department { get; set; }
    }
}
