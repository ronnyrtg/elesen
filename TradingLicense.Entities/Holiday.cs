﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class Holiday
    {
        [Key]
        public int HolidayID { get; set; }

        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string HolidayDesc { get; set; }
        public DateTime HolidayDate { get; set; }
    }
}
