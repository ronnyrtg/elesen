using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class LoginLogModel
    {

        public int LoginLogID { get; set; }

        [Required]
        public DateTime LogDate { get; set; }

        [Required]
        [StringLength(100)]
        public string LogDesc { get; set; }

        [Required]
        [StringLength(20)]
        public string IpAddress { get; set; }

        public bool LoginStatus { get; set; }
    }
}
