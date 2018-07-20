using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class HOLIDAY
    {
        [Key]
        public int HOLIDAYID { get; set; }

        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string H_DESC { get; set; }
        public DateTime H_DATE { get; set; }
    }
}
