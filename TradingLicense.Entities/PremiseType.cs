using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TradingLicense.Entities
{
    public class PremiseType
    {
        [Key]
        public int PremiseTypeID { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string PremiseDesc { get; set; }
        public bool Active { get; set; }
        public PremiseType()
        {
            Active = true;
        }
    }
}
