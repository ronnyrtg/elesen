﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class RequiredDoc
    {
        [Key]
        public int RequiredDocID { get; set; }

        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string RequiredDocDesc { get; set; }

        public bool Active { get; set; }
        public RequiredDoc()
        {
            Active = true;
        }
    }
}
