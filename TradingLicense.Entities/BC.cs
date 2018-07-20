using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class BC
    {
        [Key]
        public int BC_ID { get; set; }
        //Type of License
        public int LIC_TYPEID { get; set; }
        //Code Reference
        [StringLength(20)]
        [Column(TypeName = "VARCHAR2")]
        public string C_R { get; set; }
        //Code Reference Description
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string C_R_DESC { get; set; }
        //For Tred License
        public int? SECTORID { get; set; }
        //Rate per Square meter
        public float? DEF_RATE { get; set; }
        //Rate per License
        public float? BASE_FEE { get; set; }
        //Object name
        [StringLength(20)]
        [Column(TypeName = "VARCHAR2")]
        public string O_NAME { get; set; }
        //Rate per Object
        public float? O_FEE { get; set; }
        //Start and stop time
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string DEF_HOUR { get; set; }
        //Extra fee if exceed default stop time
        public float? EX_HOUR_FEE { get; set; }
        //For Banner License, extra charges if size is more than 8 square meters
        public float? EX_FEE { get; set; }
        //For Banner License, extra refundable charge if banner is Bunting type
        public float? DEPOSIT { get; set; }
        //Fee per Period
        public float? P_FEE { get; set; }
        //year, month, week or day
        public int? PERIOD { get; set; }
        //Quantity of year,month,week or day
        public int? PERIOD_Q { get; set; }
        public bool ACTIVE { get; set; }
        public BC()
        {
            ACTIVE = true;
        }

        public virtual SECTOR SECTOR { get; set; }
        public virtual LIC_TYPE LIC_TYPE { get; set; }
    }
}
