using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class MLPremiseApplicationModel
    {
        public int MLPremiseApplicationID { get; set; }
        public int Mode { get; set; }
        public int BusinessTypeID { get; set; }
        public int CompanyID { get; set; }
        public string Addra1 { get; set; }
        public string Addra2 { get; set; }
        public string Addra3 { get; set; }
        public string Addra4 { get; set; }
        public string PcodeA { get; set; }
        public string StateA { get; set; }

        //For Business Types 2 and above
        public float? AuthorizedCapital { get; set; }
        public float? PaidUpCapital { get; set; }
        public float? IssuedCapital { get; set; }
        public float? CashCapital { get; set; }
        public float? OtherCapital { get; set; }
        public float? BankSource { get; set; }
        public float? SavingSource { get; set; }
        public float? LoanSource { get; set; }
        public float? OtherSource { get; set; }
        public string Individualids { get; set; }
        public int AppStatusID { get; set; }

        //The user who creates this application
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        public string RequiredDocIds { get; set; }
        public string UploadRequiredDocids { get; set; }
        public int UserRollTemplate { get; set; }
        public bool IsDraft { get; set; }

        public string newComment { get; set; }
        public string Supported { get; set; }
        public string SubmitType { get; set; }
        public bool HasPADepSupp { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime? DateApproved { get; set; }
        public float? ProcessingFee { get; set; }
        public DateTime? DatePaid { get; set; }
       
        public string ReferenceNo { get; set; }
        public DateTime? LExpireDate { get; set; }
        public string AdditionalDocIds { get; set; }
        public string UploadAdditionalDocIds { get; set; }
        public string newIndividualsList { get; set; }
        public string PremiseArea { get; set; }
        public string PremiseOwnership { get; set; }
        public string BusinessCodeIds { get; set; }

        public List<Select2ListItem> selectedBusinessCodeList = new List<Select2ListItem>();
        public List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
    }
}
