﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class CitizenModel
    {
        [Key]
        public int CITIZENID { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string CITIZEN_CODE { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string CITIZEN_DESC { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ID_STAMP { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string TIME_STAMP { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string REC_STATUS { get; set; }
    }
}