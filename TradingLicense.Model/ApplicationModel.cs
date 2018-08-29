using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class Select2ListItem
    {
        public int id { get; set; }
        public string text { get; set; }
    }


    public class ApplicationModel
    {
        public int APP_ID { get; set; }
        public int LIC_TYPEID { get; set; }
        public int MODE { get; set; }
        public int? BT_ID { get; set; }
        public int? IND_ID { get; set; }
        public int? COMPANYID { get; set; }
        //Applied Address
        public string ADDRA1 { get; set; }
        public string ADDRA2 { get; set; }
        public string ADDRA3 { get; set; }
        public string ADDRA4 { get; set; }
        public string PCODEA { get; set; }
        public string STATEA { get; set; }
        //Company Mailing Address
        public string ADDRC1 { get; set; }
        public string ADDRC2 { get; set; }
        public string ADDRC3 { get; set; }
        public string ADDRC4 { get; set; }
        public string PCODEC { get; set; }
        public string STATEC { get; set; }

        public string P_OWN { get; set; }
        public float? P_AREA { get; set; }
        public DateTime? START_RENT { get; set; }
        public DateTime? STOP_RENT { get; set; }
        public string FLOOR { get; set; }
        public int? PT_ID { get; set; }
        public string UPDATED_BY { get; set; }
        public float? TOTAL_FEE { get; set; }
        public int APPSTATUSID { get; set; }

        //Premise Application
        public int? SectorID { get; set; }
        public string BusinessCodeids { get; set; }
        public string BusinessCodeDesc { get; set; }
        public string SectorDesc { get; set; }
        public List<Select2ListItem> selectedbusinessCodeList = new List<Select2ListItem>();

        //Hotel Application
        public int? ROOM_QTY { get; set; }
        public float? OCC_RATE { get; set; }

        //Banner Application has Processing Fee
        public float? PRO_FEE { get; set; }
        public int? BC_ID { get; set; }
        public float? B_SIZE { get; set; }
        public float? B_QTY { get; set; }
        public int? B_O_TOTAL { get; set; }
        public int? totalBannerObjects { get; set; }

        //Hawker Application & Stall Application
        public int? StallType { get; set; }
        //Valid date & expire date
        public DateTime? V_START { get; set; }
        public DateTime? V_STOP { get; set; }        
        //Operation Hour start and stop
        public string H_START { get; set; }
        public string H_STOP { get; set; }        
        public int? HELPERA { get; set; }
        public string helperNameA { get; set; }
        public int? HELPERB { get; set; }
        public string helperNameB { get; set; }
        public int? HELPERC { get; set; }
        public string helperNameC { get; set; }
        //stall use PremiseNo, Hawker use this as Location
        public string PREMISNO { get; set; }
        public string GOODS_TYPE { get; set; }

        //Entertainment Application
        public int? E_P_FEEID { get; set; }
        public string E_P_DESC { get; set; }
        public string E_S_DESC { get; set; }
        public float? E_S_FEE { get; set; }
        public float? E_S_B_FEE { get; set; }
        public float? E_S_O_FEE { get; set; }
        public string E_S_O_NAME { get; set; }
        public int? E_S_PERIOD { get; set; }
        public int? E_S_P_QTY { get; set; }
        public List<Select2ListItem> selectedPremiseFeeList = new List<Select2ListItem>();

        //Banner Location Address
        public string ADDRB1 { get; set; }
        public string ADDRB2 { get; set; }
        public string ADDRB3 { get; set; }
        public string ADDRB4 { get; set; }

        //Liquor Application
        public int? EX_HOUR { get; set; }

        //Entertainment Application
        public int? OBJECT_QTY { get; set; }
        
        //User that creates the application, either Public user or Desk Officer
        public int USERSID { get; set; }
        public DateTime SUBMIT { get; set; }
        public DateTime? APPROVE { get; set; }
        public DateTime? PAID { get; set; }
        public string PRF_NO { get; set; }
        public string REF_NO { get; set; }
        public string L_STATUS { get; set; }
        public DateTime? EXPIRE { get; set; }

        
        public string Individualids { get; set; }
        public string LicenseDocIds { get; set; }
        public string RequiredDocIds { get; set; }
        public string AdditionalDocIds { get; set; }
        public int? ATT_ID { get; set; }


        public string LicenseTypeDesc { get; set; }
        public int UserRollTemplate { get; set; }
        public string BusinessTypeDesc { get; set; }
        public string FullName { get; set; }
        public string OwnerName { get; set; }
        public string CompanyName { get; set; }
        public string StatusDesc { get; set; }
        public string PremiseDesc { get; set; }
        public bool IsDraft { get; set; }
        public string UploadRequiredDocids { get; set; }
        public string UploadLicenseDocids { get; set; }
        public string newIndividualsList { get; set; }

        public int? routeID { get; set; }
        public string newCatatan { get; set; }
        public string newComment { get; set; }
        public string newQuestion { get; set; } = "Mohon semakan bagi permohonan berikut";
        public string newAnswer { get; set; }
        public string Supported { get; set; }
        public string SubmitType { get; set; }
        public bool HasPADepSupp { get; set; }

        public float? AmountDue { get; set; }

        
        public List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();

        public static string GetReferenceNo(int ApplicationId, DateTime submittedDateTime, string licCode)
        {
            return $"{submittedDateTime.Year}/{licCode}/NEW/{ApplicationId.ToString().PadLeft(6, '0')}";
        }

        public static string GetProfileNo(int ApplicationId, DateTime submittedDateTime, string licCode)
        {
            return $"{submittedDateTime.Year}/{licCode}/{ApplicationId.ToString().PadLeft(6, '0')}";
        }

    }
    public class NewIndividualModel
    {
        public string fullName { get; set; }
        public string passportNo { get; set; }
    }
}