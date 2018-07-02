using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class BannerApplicationModel
    {
        public int BannerApplicationID { get; set; }
        public int? CompanyID { get; set; }
        public int IndividualID { get; set; }
        public int AppStatusID { get; set; }

        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime DateApproved { get; set; }
        public float? TotalFee { get; set; }
        public float? ProcessingFee { get; set; }
        public DateTime DatePaid { get; set; }
        public string ReferenceNo { get; set; }
        public string LicenseStatus { get; set; }
        public DateTime ExpireDate { get; set; }

        public string BannerObjectids { get; set; }
        public string BannerCodeids { get; set; }
        public string RequiredDocIds { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string StatusDesc { get; set; }
        public int UserRollTemplate { get; set; }
        public bool IsDraft { get; set; }
        public string UploadRequiredDocids { get; set; }

        public int BannerCodeID { get; set; }
        public int LocationID { get; set; }
        public float BSize { get; set; }
        public int BQuantity { get; set; }
        public float Fee { get; set; }

        public string newComment { get; set; }
        public string Supported { get; set; }
        public string SubmitType { get; set; }
        public bool HasPADepSupp { get; set; }

        public float AmountDue { get; set; }

        public static string GetReferenceNo(int bannerApplicationId, DateTime submittedDateTime)
        {
            return $"{submittedDateTime.Year}/BA/NEW/{bannerApplicationId.ToString().PadLeft(6, '0')}";
        }
    }
   
}