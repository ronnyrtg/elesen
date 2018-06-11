using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class PaymentReceived
    {
        [Key]
        public int PaymentReceivedID { get; set; }
        public int IndividualID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string PaymentFor { get; set; }
        public float AmountPaid { get; set; }
        public DateTime DatePaid { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ReceivedBy { get; set; }

        public virtual Individual Individual { get; set; }
    }
}
