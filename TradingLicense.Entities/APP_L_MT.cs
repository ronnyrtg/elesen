using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class APP_L_MT
    {
        [Key]
        public int APP_L_MTID { get; set; }
        public int APP_ID { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string MT_REF { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string MT_DESC { get; set; }
        public DateTime? MT_DATE { get; set; }
        public int USERSID { get; set; }
        public DateTime CREATED { get; set; }
        public bool ACTIVE { get; set; }
        public APP_L_MT()
        {
            ACTIVE = true;
        }

        public virtual APPLICATION APPLICATION { get; set; }
        public virtual USERS USERS { get; set; }
    }
}
