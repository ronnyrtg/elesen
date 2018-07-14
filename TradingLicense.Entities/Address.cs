using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class ADDRESS
    {
        [Key]
        public int ADDRESS_ID { get; set; }
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
    }
}
