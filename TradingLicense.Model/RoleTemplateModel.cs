using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
