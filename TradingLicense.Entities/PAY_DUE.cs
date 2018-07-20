using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class PAY_DUE
    {
        [Key]
        public int PAY_DUEID { get; set; }
        public string IND_IDS { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string PAY_FOR { get; set; }
        public float AMT_DUE { get; set; }
        public DateTime DATE_BILL { get; set; }
        public DateTime DUE_DATE { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string BILL_BY { get; set; }
        public int BILL_STATUS { get; set; }

        public PAY_DUE() 
        {
            BILL_STATUS = 0;
        }
    }
}
