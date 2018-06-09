using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class PaymentReceivedModel
    {
        public int PaymentReceivedID { get; set; }        
        public int IndividualID { get; set; }
        [Display(Name = "Tujuan Pembayaran")]
        [Required(ErrorMessage = "Sila masukkan tujuan pembayaran")]
        public string PaymentFor { get; set; }
        [Display(Name = "Jumlah Bayaran")]
        [Required(ErrorMessage = "Sila masukkan nilai bayaran")]
        public float AmountPaid { get; set; }
        public DateTime DatePaid { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ReceivedBy { get; set; }
    }
}
