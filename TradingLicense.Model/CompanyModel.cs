using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class CompanyModel
    {
        public int COMPANYID { get; set; }
        public int BT_ID { get; set; }
        public string REG_NO { get; set; }
        [Required(ErrorMessage = "Sila masukkan nama syarikat atau pertubuhan")]
        public string C_NAME { get; set; }
        [Required(ErrorMessage = "Sila masukkan nombor telefon pejabat")]
        public string C_PHONE { get; set; }
        [Required(ErrorMessage = "Please Enter Company Address")]       
        public string ADDRA1 { get; set; }        
        public string ADDRA2 { get; set; }        
        public string ADDRA3 { get; set; }        
        public string ADDRA4 { get; set; }        
        public string PCODEA { get; set; }        
        public string STATEA { get; set; }
        
        public DateTime? SSMREGDATE { get; set; }
        
        public DateTime? SSMEXPDATE { get; set; }
        public float? A_CAPITAL { get; set; }
        public float? I_CAPITAL { get; set; }
        public float? PU_C_CASH { get; set; }
        public float? PU_C_O { get; set; }
        public float? BANK_S { get; set; }
        public float? DEPOSIT_S { get; set; }
        public float? LOAN_S { get; set; }
        public string LOAN_S_NAME { get; set; }
        public float? OTHER_S { get; set; }
        public string OTHER_S_NAME { get; set; }

        public string BusinessTypeDesc { get; set; }
        public bool Active { get; set; }
    }
}
