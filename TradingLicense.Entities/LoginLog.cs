using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class LoginLog
    {
        [Key]
        public int LoginLogID { get; set; }

        [Required]
        public DateTime LogDate { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string LogDesc { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "VARCHAR2")]
        public string IpAddress { get; set; }

        [Required]
        public bool LoginStatus { get; set; }

        public int UsersID { get; set; }

        public virtual Users Users { get; set; }
    }
}
