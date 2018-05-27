using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class BannerApplicationModel
    {
        public int BannerApplicationID { get; set; }
        public int IndividualID { get; set; }
        public int CompanyID { get; set; }
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string UpdatedBy { get; set; }

        public int AppStatusID { get; set; }

        public string BannerObjectids { get; set; }

        public string RequiredDocIds { get; set; }

        public int UserRollTemplate { get; set; }

        public bool IsDraft { get; set; }

        public string UploadRequiredDocids { get; set; }

        public List<SelectedBannerObjectModel> selectedbannerobjectList = new List<SelectedBannerObjectModel>();
    }
}