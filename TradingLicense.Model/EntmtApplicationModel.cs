using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class EntmtApplicationModel
    {
        public int EntmtApplicationID { get; set; }

        [Required(ErrorMessage = "Please Select Business Type")]
        public int BusinessTypeID { get; set; }

        public int UsersID { get; set; }

        [Required(ErrorMessage = "Please Select Mykad/Passport No")]
        public int IndividualID { get; set; }

        public int EntmtGroupID { get; set; }

        public DateTime DateSubmitted { get; set; }

        public string UpdatedBy { get; set; }

        public int AppStatusID { get; set; }

        public string EntmtGroupids { get; set; }

        public string EntmtObjectids { get; set; }

        public string EntmtCodeids { get; set; }

        public string Individualids { get; set; }

        public string RequiredDocIds { get; set; }

        public int UserRollTemplate { get; set; }

        public string StatusDesc { get; set; }

        public bool IsDraft { get; set; }

        public string UploadRequiredDocids { get; set; }

        public List<SelectedEntmtCodeModel> selectedEntmtCodeList = new List<SelectedEntmtCodeModel>();

        public List<SelectedIndividualModel> selectedIndividualList = new List<SelectedIndividualModel>();
    }
}