using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class LiquorApplicationModel
    {
        public int LiquorApplicationID { get; set; }

        [Required(ErrorMessage = "Please Select Business Type")]
        public int BusinessTypeID { get; set; }
        public string NamaPemohon { get; set; }
        public string ICPaspot { get; set; }
        public string NamaSyarikat { get; set; }
        public int UsersID { get; set; }

        [Required(ErrorMessage = "Please Select Mykad/Passport No")]
        public int IndividualID { get; set; }

        public int LiquorCodeID { get; set; }

        public DateTime DateSubmitted { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime AprovedDate { get; set; }
        public DateTime ExpireDate { get; set; }

        public int AppStatusID { get; set; }

        public string RequiredDocIds { get; set; }

        public int UserRollTemplate { get; set; }

        public string StatusDesc { get; set; }

        public bool IsDraft { get; set; }

        public string UploadRequiredDocids { get; set; }
    }
}