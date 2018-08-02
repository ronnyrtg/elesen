using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class APP_LOG
    {
        [Key]
        public int APP_LOGID { get; set; }
        public int APP_ID { get; set; }
        public int APPSTATUSID { get; set; }
        public DateTime TIME_STAMP { get; set; }
        public int USERSID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string ACTIVITY { get; set; }

        public virtual APPSTATUS APPSTATUS { get; set; }
        public virtual APPLICATION APPLICATION { get; set; }
        public virtual USERS USERS { get; set; }
    }
}
