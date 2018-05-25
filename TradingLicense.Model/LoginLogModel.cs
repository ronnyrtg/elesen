using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class LoginLogModel
    {
        [Key]
        public int LoginLogID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
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
