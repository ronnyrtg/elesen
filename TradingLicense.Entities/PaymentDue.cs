using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class PaymentDue
    {
        [Key]
        public int PaymentDueID { get; set; }
        public int IndividualID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string PaymentFor { get; set; }
        public float AmountDue { get; set; }
        public DateTime DateBilled { get; set; }
        public DateTime DueDate { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string BilledBy { get; set; }
        public int BillStatus { get; set; }

        public PaymentDue() 
        {
            BillStatus = 0;
        }
        
        public virtual Individual Individual { get; set; } 
    }
}
