using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class ROLE
    {
        [Key]
        public int ROLEID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ROLE_DESC { get; set; }
        //This is the maximum number of days allowed for each role to process an application
        public int DURATION { get; set; }
        
    }
}
