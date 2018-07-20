using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class USERS
    {

        public int USERSID { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string FULLNAME { get; set; }

        [StringLength(30)]
        [Column(TypeName = "VARCHAR2")]
        public string USERNAME { get; set; }

        [StringLength(200)]
        [Column(TypeName = "VARCHAR2")]
        public string EMAIL { get; set; }

        [StringLength(32)]
        [Column(TypeName = "VARCHAR2")]
        public string PASSWORD { get; set; }
        public int? ROLEID { get; set; }
        public int? DEP_ID { get; set; }

        public int LOCKED { get; set; }
        public bool ACTIVE { get; set; }
        public USERS()
        {
            ACTIVE = true;
        }

        public virtual ROLE ROLE { get; set; }
        public virtual DEPARTMENT DEPARTMENT { get; set; }

    }
}
