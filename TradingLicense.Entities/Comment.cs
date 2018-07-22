using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class COMMENT
    {
        [Key]
        public int COMMENTID { get; set; }
        public int APP_ID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string CONTENT { get; set; }
        public int USERSID { get; set; }
        public DateTime COMMENTDATE { get; set; }

        public virtual APPLICATION APPLICATION { get; set; }
        public virtual USERS USERS { get; set; }
    }
}
