using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public  class AccessPage
    {
        [Key]
        public int AccessPageID { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string PageDesc { get; set; }

        [Required]
        public int ScreenId { get; set; }

        [Required]
        public int CrudLevel { get; set; }

        public int RoleTemplateID { get; set; }

        public virtual RoleTemplate RoleTemplate { get; set; }
    }
}
