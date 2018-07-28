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
        public bool SUPPORT { get; set; }
        public int? USERSID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string COMMENT { get; set; }
        public DateTime? SUBMITTED { get; set; }
        public bool ACTIVE { get; set; }
        public ROUTEUNIT()
        {
            ACTIVE = true;
        }

        public virtual LIC_TYPE LIC_TYPE { get; set; }
        public virtual USERS USERS { get; set; }
        public virtual DEPARTMENT DEPARTMENT { get; set; }
    }
}
