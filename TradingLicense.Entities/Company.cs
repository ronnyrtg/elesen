using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class COMPANY
    {
        [Key]
        public int COMPANYID { get; set; }
        public int? BT_ID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string REG_NO { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string C_NAME { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string C_PHONE { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string C_ADDRESS { get; set; }        
        public DateTime? SSMREGDATE { get; set; }
        public DateTime? SSMEXPDATE { get; set; }
        //Authorised Capital
        public float? A_CAPITAL { get; set; }
        //Issued Capital
        public float? I_CAPITAL { get; set; }
        //Paid Up Capital Cash
        public float? PU_C_CASH { get; set; }
        //Paid Up Capital Other Source
        public float? PU_C_O { get; set; }
        //Bank Source
        public float? BANK_S  { get; set; }
        //Deposit Source
        public float? DEPOSIT_S { get; set; }
        //Loan Source in RM
        public float? LOAN_S { get; set; }
        //Loan Source Name
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string LOAN_S_NAME { get; set; }
        //Other Source in RM
        public float? OTHER_S { get; set; }
        //Other Source Name
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string OTHER_S_NAME { get; set; }
        public bool ACTIVE { get; set; }

        public virtual BT BT { get; set; }

        public COMPANY()
        {
            ACTIVE = true;
        }
    }
}
