using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class PAY_REC
    {
        [Key]
        public int PAY_RECID { get; set; }
        public int IND_ID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string PAY_FOR { get; set; }
        public float AMT_PAID { get; set; }
        public DateTime DATE_PAID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string REC_BY { get; set; }

        public virtual INDIVIDUAL INDIVIDUAL { get; set; }
    }
}
