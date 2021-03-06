using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class PremiseApplicationModel
    {
        public int PremiseApplicationID { get; set; }
        [Required(ErrorMessage = "Sila pilih jenis kelulusan")]
        public int Mode { get; set; }
        [Required(ErrorMessage = "Sila pilih jenis perniagaan")]
        public int BusinessTypeID { get; set; }
        [Required(ErrorMessage = "Sila pilih Kumpulan Kod")]
        public int SectorID { get; set; }
        [Required(ErrorMessage = "Sila pilih Syarikat")]
        public int CompanyID { get; set; }
        [Required(ErrorMessage = "Sila pilih nama pemohon")]
        public string Addra1 { get; set; }
        public string Addra2 { get; set; }
        public string Addra3 { get; set; }
        public string Addra4 { get; set; }
        public string PcodeA { get; set; }
        public string StateA { get; set; }
        [Required(ErrorMessage = "Sila pilih pemilikan premis")]
        public int PremiseOwnership { get; set; }
        public DateTime StartRent { get; set; }
        public DateTime StopRent { get; set; }
        public string WhichFloor { get; set; }       
        public int PremiseTypeID { get; set; }
        [Required(ErrorMessage = "Sila masukkan luas premis dalam meter persegi")]
        public float PremiseArea { get; set; }
        public float? TotalFee { get; set; }
        public int AppStatusID { get; set; }

        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DateApproved { get; set; }
        public DateTime DatePaid { get; set; }
        public string ReferenceNo { get; set; }
        public string LicenseStatus { get; set; }
        public DateTime ExpireDate { get; set; }

        public string BusinessCodeids { get; set; }
        public string Individualids { get; set; }
        public string RequiredDocIds { get; set; }
        public string AdditionalDocIds { get; set; }

        public int UserRollTemplate { get; set; }
        public string BusinessTypeDesc { get; set; }
        public string BusinessCodeDesc { get; set; }
        public string SectorDesc { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string StatusDesc { get; set; }
        public string PremiseDesc { get; set; }
        public bool IsDraft { get; set; }
        public string UploadRequiredDocids { get; set; }
        public string UploadAdditionalDocids { get; set; }
        public string newIndividualsList { get; set; }

        public string newComment { get; set; }
        public string Supported { get; set; }
        public string SubmitType { get; set; }
        public bool HasPADepSupp { get; set; }

        public float AmountDue { get; set; }

        public List<Select2ListItem> selectedbusinessCodeList = new List<Select2ListItem>();
        public List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();

        //For ViewPremiseApplication page
        public List<string> businessCodeList = new List<string>();
        public List<IndividualLink> indLinkList = new List<IndividualLink>();
        public List<reqDocLink> RequiredDocNames = new List<reqDocLink>();
        public List<addDocLink> AdditionalDocDescs = new List<addDocLink>();

        public static string GetReferenceNo(int premiseApplicationId, DateTime submittedDateTime)
        {
            return $"{submittedDateTime.Year}/PA/NEW/{premiseApplicationId.ToString().PadLeft(6, '0')}";
        }

        public override string ToString()
        {
            return this.ReferenceNo;
        }
    }

    public class NewIndividualModel
    {
        public string fullName { get; set; }
        public string passportNo { get; set; }
    }

    public class IndividualLink
    {
        public int IndListID { get; set; }
        public string IndName { get; set; }
    }

    public class reqDocLink
    {
        public string AttName { get; set; }
        public string reqDocDesc { get; set; }
    }

    public class addDocLink
    {
        public string AttName { get; set; }
        public string addDocDesc { get; set; }
    }
}