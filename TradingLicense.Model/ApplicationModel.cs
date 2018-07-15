using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class ApplicationModel
    {
        public int APP_ID { get; set; }
        public int APP_TYPE_ID { get; set; }
        public int MODE { get; set; }
        public int BUSINESSTYPEID { get; set; }
        public int COMPANYID { get; set; }
        public int ADDRESS_ID { get; set; }
        public string P_OWN { get; set; }
        public float P_AREA { get; set; }
        public DateTime? START_RENT { get; set; }
        public DateTime? STOP_RENT { get; set; }
        public string FLOOR { get; set; }
        public int PREMISETYPEID { get; set; }
        public string UPDATED_BY { get; set; }
        public float? TOTAL_FEE { get; set; }
        public int APPSTATUSID { get; set; }

        //Premise Application
        [Required(ErrorMessage = "Sila pilih Kumpulan Kod")]
        public int SectorID { get; set; }
        public string BusinessCodeids { get; set; }
        public string BusinessCodeDesc { get; set; }
        public string SectorDesc { get; set; }
        public List<Select2ListItem> selectedbusinessCodeList = new List<Select2ListItem>();

        //Banner Application has Processing Fee
        public float? PRO_FEE { get; set; }

        //Hawker Application & Stall Application
        public DateTime? V_START { get; set; }
        public DateTime? V_STOP { get; set; }
        public string GOODS_TYPE { get; set; }
        public int H_START { get; set; }
        public int H_STOP { get; set; }
        public int? HELPERA { get; set; }
        public int? HELPERB { get; set; }
        public int? HELPERC { get; set; }

        //Entertainment Application
        public int E_GROUPID { get; set; }
        public string E_G_DESC { get; set; }
        public int E_CODEID { get; set; }
        public string E_C_DESC { get; set; }
        public float? E_FEE { get; set; }
        public float? E_BASE_FEE { get; set; }
        public float? E_OBJECT_FEE { get; set; }
        public string E_OBJECT_NAME { get; set; }
        public int E_PERIOD { get; set; }
        public int E_PERIOD_Q { get; set; }

        //Save to Address Table
        public string ADDRA1 { get; set; }
        public string ADDRA2 { get; set; }
        public string ADDRA3 { get; set; }
        public string ADDRA4 { get; set; }
        public string PCODEA { get; set; }
        public string STATEA { get; set; }

        //User that creates the application, either Public user or Desk Officer
        public int USERSID { get; set; }
        public DateTime SUBMIT { get; set; }
        public DateTime? APPROVE { get; set; }
        public DateTime? PAID { get; set; }
        public string REF_NO { get; set; }
        public string L_STATUS { get; set; }
        public DateTime? EXPIRE { get; set; }

        
        public string Individualids { get; set; }
        public string RequiredDocIds { get; set; }
        public string AdditionalDocIds { get; set; }

        public int UserRollTemplate { get; set; }
        public string BusinessTypeDesc { get; set; }
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

        
        public List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
        
    }
}