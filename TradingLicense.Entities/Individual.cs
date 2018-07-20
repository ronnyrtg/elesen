using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class INDIVIDUAL
    {
        [Key]
        public int IND_ID { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string FULLNAME { get; set; }

        [StringLength(30)]
        [Column(TypeName = "VARCHAR2")]
        public string MYKADNO { get; set; }

        public int? NAT_ID { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string ADD_IC { get; set; }

        [StringLength(20)]
        [Column(TypeName = "VARCHAR2")]
        public string PHONE { get; set; }

        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string IND_EMAIL { get; set; }

        //Profile Picture
        public int? ATT_ID { get; set; }

        public int? GENDER { get; set; }
        public bool ACTIVE { get; set; }
        public INDIVIDUAL()
        {
            ACTIVE = true;
        }

        public virtual ATTACHMENT ATTACHMENT { get; set; }
    }
}
