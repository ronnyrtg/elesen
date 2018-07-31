using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class PaymentModel
    {
        public int PAYTRAN_MID { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string CENTER_CODE { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string COUNTER_NO { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string RCPT_FROM { get; set; }
        public DateTime TX_DATE { get; set; }
        [StringLength(20)]
        [Column(TypeName = "VARCHAR2")]
        public string RCPT_NO { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string PAYMENT_TYPE { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string PAYMENT_REF { get; set; }
        public float TOTAL_AMOUNT { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string REMARKS { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string RCPT_REF { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ID_STAMP { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string TIME_STAMP { get; set; }
        public float OVERPYMT { get; set; }
        [StringLength(5)]
        [Column(TypeName = "VARCHAR2")]
        public string SYSTEM_ID { get; set; }
        [StringLength(5)]
        [Column(TypeName = "VARCHAR2")]
        public string TARGET_ID { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string TELLER_ID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string LIC_TYPE { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string LICENSE { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string FILE_NO { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string LICENSE_NO { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string PERSON_ID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ACCT_NO { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string BANK_CODE { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string CHQ_TYPE { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string REV_VOTE_HEAD { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string BANK_VOTE_HEAD { get; set; }
        [StringLength(5)]
        [Column(TypeName = "VARCHAR2")]
        public string VOID_IND { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string VOID_ID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string VOID_TIME { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string CARD_NO { get; set; }
        public float OTOTAL_AMOUNT { get; set; }
        public float RTOTAL_AMOUNT { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string PARTICULAR { get; set; }
        [StringLength(5)]
        [Column(TypeName = "VARCHAR2")]
        public string UPLOAD { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string EXT_RCPT_NO { get; set; }
        public int CPD_ID { get; set; }
        public float DISCOUNT { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string CPD_REFERENCE { get; set; }
        public int RECORD_ID { get; set; }
    }
}
