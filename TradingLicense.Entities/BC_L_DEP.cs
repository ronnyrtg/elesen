using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class BC_L_DEP
    {
        [Key]
        public int BC_L_DEPID { get; set; }
        [Required]
        
        public int BC_ID { get; set; }
        [Required]
        
        public int DEP_ID { get; set; }

        public virtual BC BC { get; set; }
        public virtual DEPARTMENT DEPARTMENT { get; set; }

    }
}
