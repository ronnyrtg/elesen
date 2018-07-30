using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class RouteUnitModel
    {
        public int ROUTEUNITID { get; set; }
        public int APP_ID { get; set; }
        public int DEP_ID { get; set; }
        public int? SUPPORT { get; set; }
        public string SENDER { get; set; }
        public string QUESTION { get; set; }
        public string RECEIVER { get; set; }
        public string ANSWER { get; set; }
        public DateTime? SUBMITTED { get; set; }
        public DateTime? REPLIED { get; set; }        

        public string DepartmentDesc { get; set; }
    }
}
