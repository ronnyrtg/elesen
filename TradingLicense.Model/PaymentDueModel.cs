using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class PaymentDueModel
    {
        public int PAY_DUEID { get; set; }
        public string IND_IDs { get; set; }
        [Display(Name = "Tujuan Pembayaran")]
        [Required(ErrorMessage = "Sila masukkan tujuan pembayaran")]
        public string PAY_FOR { get; set; }
        [Display(Name = "Jumlah Bayaran")]
        [Required(ErrorMessage = "Sila masukkan nilai yang perlu dibayar")]
        public float AMT_DUE { get; set; }
        public DateTime DATE_BILL { get; set; }
        public DateTime DUE_DATE { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string BILL_BY { get; set; }
        public int BILL_STATUS { get; set; }
    }
}
