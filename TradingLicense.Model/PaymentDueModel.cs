using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class PaymentDueModel
    {
        public int PaymentDueID { get; set; }
        public int IndividualID { get; set; }
        [Display(Name = "Tujuan Pembayaran")]
        [Required(ErrorMessage = "Sila masukkan tujuan pembayaran")]
        public string PaymentFor { get; set; }
        [Display(Name = "Jumlah Bayaran")]
        [Required(ErrorMessage = "Sila masukkan nilai yang perlu dibayar")]
        public float AmountDue { get; set; }
        public DateTime DateBilled { get; set; }
        public DateTime DueDate { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string BilledBy { get; set; }
        public int BillStatus { get; set; }
    }
}
