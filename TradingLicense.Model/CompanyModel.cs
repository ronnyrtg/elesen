using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class CompanyModel
    {
        public int CompanyID { get; set; }
        public int BusinessTypeID { get; set; }
        public string RegistrationNo { get; set; }
        [Required(ErrorMessage = "Sila masukkan nama syarikat atau pertubuhan")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Sila masukkan nombor telefon pejabat")]
        public string CompanyPhone { get; set; }
        [Required(ErrorMessage = "Please Enter Company Address")]       
        public string Addra1 { get; set; }        
        public string Addra2 { get; set; }        
        public string Addra3 { get; set; }        
        public string Addra4 { get; set; }        
        public string PcodeA { get; set; }        
        public string StateA { get; set; }
        public DateTime? SSMRegDate { get; set; }
        public DateTime? SSMExpDate { get; set; }
        public float? AuthorisedCapital { get; set; }
        public float? IssuedCapital { get; set; }
        public float? PaidUpCapitalCash { get; set; }
        public float? PaidUpCapitalOther { get; set; }
        public float? BankSource { get; set; }
        public float? DepositSource { get; set; }
        public float? LoanSource { get; set; }
        public string LoanSourceName { get; set; }
        public float? OtherSource { get; set; }
        public string OtherSourceName { get; set; }

        public string BusinessTypeDesc { get; set; }

        public bool Active { get; set; }
    }
}
