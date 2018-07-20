using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class LOGINLOG
    {
        [Key]
        public int LOGINLOGID { get; set; }

        [Required]
        public DateTime LOGDATE { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string LOGDESC { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "VARCHAR2")]
        public string IPADDRESS { get; set; }

        [Required]
        public bool LOGINSTATUS { get; set; }

        public int USERSID { get; set; }

        public virtual USERS USERS { get; set; }
    }
}
