using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class LiquorApplicationModel
    {
        public int LiquorApplicationID { get; set; }
        public int Mode { get; set; }
        [Required(ErrorMessage = "Sila pilih jenis perniagaan")]
        public int BusinessTypeID { get; set; }
        public int LiquorCodeID { get; set; }
        [Required(ErrorMessage = "Please Select Mykad/Passport No")]
        public int IndividualID { get; set; }
        public int CompanyID { get; set; }
        public string Addra1 { get; set; }
        public string Addra2 { get; set; }
        public string Addra3 { get; set; }
        public string Addra4 { get; set; }
        public string PcodeA { get; set; }
        public string StateA { get; set; }
        public int AppStatusID { get; set; }
        public string PreviousLicense { get; set; }
        public DateTime? PrevLicDate { get; set; }
        public DateTime? PrevLicExp { get; set; }

        public string PreviousLiquorLicenseNo { get; set; }
        public DateTime? PreviousLiquorIssueDate { get; set; }
        public DateTime? PreviousLiquorExpDate { get; set; }

        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime DateApproved { get; set; }
        public float? ProcessingFee { get; set; }
        public DateTime DatePaid { get; set; }
        public string ReferenceNo { get; set; }
        public string LicenseStatus { get; set; }
        public DateTime ExpireDate { get; set; }


        public string FullName { get; set; }
        public string MykadNo { get; set; }
        public string BusinessTypeDesc { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyAddress { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime SSMRegDate { get; set; }
        public DateTime SSMExpDate { get; set; }
        public string LCodeNumber { get; set; }
        public string LiquorCodeDesc { get; set; }
        public string DefaultHours { get; set; }
        public string RequiredDocIds { get; set; }
        public int UserRollTemplate { get; set; }
        public string StatusDesc { get; set; }
        public bool IsDraft { get; set; }
        public string UploadRequiredDocids { get; set; }
    }
}