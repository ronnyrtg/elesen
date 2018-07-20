using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class LoginLogModel
    {
        [Key]
        public int LOGINLOGID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime LOGDATE { get; set; }

        [Required]
        [StringLength(100)]
        public string LOGDESC { get; set; }

        [Required]
        [StringLength(20)]
        public string IPADDRESS { get; set; }

        public bool LOGINSTATUS { get; set; }
    }
}
