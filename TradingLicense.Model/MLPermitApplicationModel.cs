using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class MLPermitApplicationModel
    {
        public int MLPermitApplicationID { get; set; }
        public int Mode { get; set; }
        public int IndividualID { get; set; }
        public int CompanyID { get; set; }
        public int LenderApplicationID { get; set; }
        public bool Brochure { get; set; }
        public bool Newspaper { get; set; }
        public bool SignBoard { get; set; }
        public bool Radio { get; set; }
        public bool Internet { get; set; }
        public bool Television { get; set; }
        public bool VCD { get; set; }
        public bool Cinema { get; set; }
        public bool Others { get; set; }
        public string SpecifyOthers { get; set; }
        public int AppStatusID { get; set; }

        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime DateApproved { get; set; }
        public float? ProcessingFee { get; set; }
        public DateTime DatePaid { get; set; }
        public DateTime ExpireDate { get; set; }

        public string FullName { get; set; }
        public string MykadNo { get; set; }
        public string CompanyName { get; set; }
        public string BusinessTypeDesc { get; set; }
        public string RegistrationNo { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyAddress { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime LExpireDate { get; set; }
        public string RequiredDocIds { get; set; }
        public int UserRollTemplate { get; set; }
        public string StatusDesc { get; set; }
        public bool IsDraft { get; set; }
        public string UploadRequiredDocids { get; set; }
    }
}