using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class BANK_M
    {
        [Key]
        public int BANKID { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string BANK_CODE { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string BANK_NAME { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string REMARKS { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ID_STAMP { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string TIME_STAMP { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string REC_STATUS { get; set; }
    }
}
