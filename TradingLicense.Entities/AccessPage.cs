using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public  class ACCESSPAGE
    {
        [Key]
        public int ACCESSPAGEID { get; set; }
        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string PAGEDESC { get; set; }
        [Required]
        public int SCREENID { get; set; }
        [Required]
        public int CRUDLEVEL { get; set; }
        public int ROLEID { get; set; }

        public virtual ROLE ROLE { get; set; }
    }
}
