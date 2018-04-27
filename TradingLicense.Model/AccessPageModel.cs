﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class AccessPageModel
    {
        public int AccessPageID { get; set; }

        [Display(Name = "Page Description")]
        [Required(ErrorMessage = "Please enter Page Description")]
        [StringLength(100)]
        public string PageDesc { get; set; }

        public int ScreenId { get; set; }

        [Display(Name = "Crud Level")]
        [Required(ErrorMessage = "Please enter Crud Level")]
        public int CrudLevel { get; set; }

        public int RoleTemplateID { get; set; }

        public string RoleTemplateDesc { get; set; }
    }
}
