using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class RoleTemplateModel
    {
        public int RoleTemplateID { get; set; }
        
        [Required(ErrorMessage = "Please Enter Role Template")]
        [Display(Name = "Role Template Description")]
        public string RoleTemplateDesc { get; set; }

    }
}
