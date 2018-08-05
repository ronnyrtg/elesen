using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class APPLICATION
    {
        [Key]
        public int APP_ID { get; set; }
        //License Type
        public int LIC_TYPEID { get; set; }
        //Approval Type
        public int MODE { get; set; }
        //Sole Proprietorship or Sdn Bhd
        public int BT_ID { get; set; }
        //Foreign Key to Company Table
        public int COMPANYID { get; set; }
        //Address
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ADDRA1 { get; set; }
        [StringLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string ADDRA2 { get; set; }
        [StringLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string ADDRA3 { get; set; }
        [StringLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string ADDRA4 { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string PCODEA { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string STATEA { get; set; }
        //Rent or Own
        [StringLength(20)]
        [Column(TypeName = "VARCHAR2")]
        public string P_OWN { get; set; }
        //Premise Area in square meters
        public float P_AREA { get; set; }
        //Premise Rent period
        public DateTime? START_RENT { get; set; }
        public DateTime? STOP_RENT { get; set; }
        //Ground level or Numbered levels
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string FLOOR { get; set; }
        //Building type refer from PremiseType table
        public int PT_ID { get; set; }
        //Name of last user updating this application
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UPDATED_BY { get; set; }
        //Total fee calculated from selected Businesscodes x Premise Area
        public float? TOTAL_FEE { get; set; }
        //Foreign Key to AppStatus table
        public int APPSTATUSID { get; set; }

        //Banner Application has Processing Fee
        public float? PRO_FEE { get; set; }
        public int? BC_ID { get; set; }

        //Hawker Application & Stall Application
        public DateTime? V_START { get; set; }
        public DateTime? V_STOP { get; set; }
        //Type of goods sold
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string GOODS_TYPE { get; set; }
        //Location & Premise No
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string PREMISNO { get; set; }
        //Operation Hour start and stop
        public int H_START { get; set; }
        public int H_STOP { get; set; }
        //Assistant Workers foreign key to Individual table
        public int? HELPERA { get; set; }
        public int? HELPERB { get; set; }
        public int? HELPERC { get; set; }

        //User that creates the application, either Public user or Desk Officer
        public int USERSID { get; set; }
        public DateTime SUBMIT { get; set; }
        public DateTime? APPROVE { get; set; }
        public DateTime? PAID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string REF_NO { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string PRF_NO { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string L_STATUS { get; set; }
        public DateTime? EXPIRE { get; set; }

        public virtual LIC_TYPE LIC_TYPE { get; set; }
        public virtual BT BT { get; set; }
        public virtual BC BC { get; set; }
        public virtual COMPANY COMPANY { get; set; }
        public virtual PREMISETYPE PREMISETYPE { get; set; }
        public virtual APPSTATUS APPSTATUS { get; set; }
        public virtual USERS USERS { get; set; }
    }
}
