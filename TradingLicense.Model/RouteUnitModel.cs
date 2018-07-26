using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class RouteUnitModel
    {
        public int ROUTEUNITID { get; set; }
        public int LIC_TYPEID { get; set; }
        public int APP_ID { get; set; }
        public int DEP_ID { get; set; }
        public bool SUPPORT { get; set; }
        public string COMMENT { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int USERSID { get; set; }
        public DateTime? SUBMITTED { get; set; }
        public bool ACTIVE { get; set; }

        public string DepartmentDesc { get; set; }
        public string FullName { get; set; }
    }
}
