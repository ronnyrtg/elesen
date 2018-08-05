using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class ROUTEUNIT
    {
        [Key]
        public int ROUTEUNITID { get; set; }
        public int APP_ID { get; set; }
        public int DEP_ID { get; set; }
        public int? SUPPORT { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string SENDER { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string QUESTION { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string RECEIVER { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string ANSWER { get; set; }
        public DateTime? SUBMITTED { get; set; }
        public DateTime? REPLIED { get; set; }

        public virtual APPLICATION APPLICATION { get; set; }
        public virtual DEPARTMENT DEPARTMENT { get; set; }
    }
}
