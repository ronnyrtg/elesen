using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class HawkerApplicationModel
    {
        public int HawkerApplicationID { get; set; }
        public int IndividualID { get; set; }
        public int HawkerCodeID { get; set; }
        public DateTime? ValidStart { get; set; }
        public DateTime? ValidStop { get; set; }
        public string GoodsType { get; set; }
        [Required(ErrorMessage = "Sila masukkan masa perniagaan")]
        [StringLength(255)]
        public string OperationHours { get; set; }
        [Required(ErrorMessage = "Sila masukkan lokasi perniagaan")]
        [StringLength(255)]
        public string HawkerLocation { get; set; }
        public int HelperA { get; set; }
        public int? HelperB { get; set; }
        public int? HelperC { get; set; }
        public int AppStatusID { get; set; }

        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime? DateApproved { get; set; }
        public DateTime? DatePaid { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string ReferenceNo { get; set; }
        public string LicenseStatus { get; set; }

        public string Individualids { get; set; }
        public string RequiredDocIds { get; set; }
        public string HCodeNumber { get; set; }
        public string HawkerCodeDesc { get; set; }
        public string FullName { get; set; }
        public string StatusDesc { get; set; }
        public int UserRollTemplate { get; set; }
        public bool IsDraft { get; set; }
        public string UploadRequiredDocids { get; set; }
        public string newIndividualsList { get; set; }

        public string newComment { get; set; }

        public List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();

        public static string GetReferenceNo(int hawkerApplicationId, DateTime submittedDateTime)
        {
            return $"{submittedDateTime.Year}/HA/{hawkerApplicationId.ToString().PadLeft(6, '0')}";
        }

        public override string ToString()
        {
            return this.ReferenceNo;
        }
    }
}