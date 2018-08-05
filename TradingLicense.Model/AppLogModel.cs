using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class AppLogModel
    {
        public int APP_LOGID { get; set; }
        public int APP_ID { get; set; }
        public int APPSTATUSID { get; set; }
        public DateTime TIME_STAMP { get; set; }
        public int USERSID { get; set; }
        public string ACTIVITY { get; set; }

        public string ReferenceNo { get; set; }
        public string FullName { get; set; }
        public string StatusDesc { get; set; }
    }
}
