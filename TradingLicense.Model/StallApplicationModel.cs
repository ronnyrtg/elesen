using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class StallApplicationModel
    {
        public int StallApplicationID { get; set; }
        public int IndividualID { get; set; }
        public int StallCodeID { get; set; }
        [Required(ErrorMessage = "Sila masukkan masa perniagaan")]
        [StringLength(255)]
        public string OperationHours { get; set; }
        [Required(ErrorMessage = "Sila masukkan lokasi perniagaan")]
        [StringLength(255)]
        public string StallLocation { get; set; }

        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string UpdatedBy { get; set; }

        public int AppStatusID { get; set; }

        public string RequiredDocIds { get; set; }

        public string FullName { get; set; }

        public string StatusDesc { get; set; }

        public int UserRollTemplate { get; set; }

        public bool IsDraft { get; set; }

        public string UploadRequiredDocids { get; set; }

        public List<SelectedStallCodeModel> selectedstallCodeList = new List<SelectedStallCodeModel>();
    }
}