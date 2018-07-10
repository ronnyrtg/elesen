using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class EntmtApplicationModel
    {
        public int EntmtApplicationID { get; set; }
        public int Mode { get; set; }
        [Required(ErrorMessage = "Please Select Business Type")]
        public int BusinessTypeID { get; set; }
        public int CompanyID { get; set; }
        public string Addra1 { get; set; }
        public string Addra2 { get; set; }
        public string Addra3 { get; set; }
        public string Addra4 { get; set; }
        public string PcodeA { get; set; }
        public string StateA { get; set; }
        public int PremiseTypeID { get; set; }
        public string OtherPremiseType { get; set; }
        public string WhichFloor { get; set; }
        public string PremiseLocation { get; set; }
        [Required(ErrorMessage = "Sila masukkan luas premis dalam meter persegi")]
        public float PremiseArea { get; set; }
        [Required(ErrorMessage = "Sila pilih pemilikan premis")]
        public string PremiseOwnership { get; set; }
        public int EntmtGroupID { get; set; }
        public int EntmtCodeID { get; set; }
        public string StartStopTime { get; set; }
        public int PeriodQuantity { get; set; }
        public int Period { get; set; }
        public int EntmtObjectID { get; set; }
        public int AppStatusID { get; set; }

        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DateApproved { get; set; }
        public DateTime DatePaid { get; set; }
        public string ReferenceNo { get; set; }
        public string LicenseStatus { get; set; }
        public DateTime ExpireDate { get; set; }

        public string EntmtGroupids { get; set; }
        public string EntmtObjectids { get; set; }
        public string EntmtCodeids { get; set; }
        public string Individualids { get; set; }
        public string RequiredDocIds { get; set; }
        public string newIndividualsList { get; set; }

        public string EntmtGroupCode { get; set; }
        public string EntmtGroupDesc { get; set; }
        public string EntmtCodeDesc { get; set; }
        public string EntmtObjectDesc { get; set; }
        public int UserRollTemplate { get; set; }
        public string StatusDesc { get; set; }
        public bool IsDraft { get; set; }
        public string UploadRequiredDocids { get; set; }

        public List<SelectedEntmtCodeModel> selectedEntmtCodeList = new List<SelectedEntmtCodeModel>();
        public List<SelectedEntmtObjectModel> selectedEntmtObjectList = new List<SelectedEntmtObjectModel>();
        public List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();

        public class NewIndividualModel
        {
            public string fullName { get; set; }
            public string passportNo { get; set; }
        }
    }
}