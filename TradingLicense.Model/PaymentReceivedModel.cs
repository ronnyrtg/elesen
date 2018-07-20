using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class PaymentReceivedModel
    {
        public int PAY_RECID { get; set; }        
        public int IND_ID { get; set; }
        [Display(Name = "Tujuan Pembayaran")]
        [Required(ErrorMessage = "Sila masukkan tujuan pembayaran")]
        public string PAY_FOR { get; set; }
        [Display(Name = "Jumlah Bayaran")]
        [Required(ErrorMessage = "Sila masukkan nilai bayaran")]
        public float AMT_PAID { get; set; }
        public DateTime DATE_PAID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string REC_BY { get; set; }
    }
}
