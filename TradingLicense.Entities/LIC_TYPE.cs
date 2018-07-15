using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TradingLicense.Entities
{
    public class LIC_TYPE
    {
        [Key]
        public int LIC_TYPEID { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string LIC_TYPEDESC { get; set; }
        [Required]
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string LIC_TYPECODE { get; set; }
        public bool ACTIVE { get; set; }
        public LIC_TYPE()
        {
            ACTIVE = true;
        }
    }
}
