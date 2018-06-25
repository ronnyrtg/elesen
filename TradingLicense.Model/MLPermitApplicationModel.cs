using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class MLPermitApplicationModel
    {
        public int MLPermitApplicationID { get; set; }
        public int IndividualID { get; set; }
        public int CompanyID { get; set; }
        public int LenderApplicationID { get; set; }
        public int Brochure { get; set; }
        public int Newspaper { get; set; }
        public int SignBoard { get; set; }
        public int Radio { get; set; }
        public int Internet { get; set; }
        public int Television { get; set; }
        public int VCD { get; set; }
        public int Cinema { get; set; }
        public int Others { get; set; }
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