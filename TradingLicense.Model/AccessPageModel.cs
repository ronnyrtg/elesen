using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class AccessPageModel
    {
        public int ACCESSPAGEID { get; set; }

        [Display(Name = "Page Description")]
        [Required(ErrorMessage = "Please enter Page Description")]
        [StringLength(100)]
        public string PAGEDESC { get; set; }
        public int SCREENID { get; set; }
        [Display(Name = "Crud Level")]
        [Required(ErrorMessage = "Please enter Crud Level")]
        public int CRUDLEVEL { get; set; }
        public int ROLEID { get; set; }

        public string ROLE_DESC { get; set; }
    }
}
