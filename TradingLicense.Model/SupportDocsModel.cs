using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class SupportDocsModel
    {
        public int? SupportDocsID { get; set; }

        [Required(ErrorMessage = "Please Select Business Code")]
        public int BusinessCodeID { get; set; }

        [Display(Name ="Description")]
        [Required(ErrorMessage ="Please Enter Description")]
        [StringLength(255)]
        public string SuppDocDesc { get; set; }

        public string CodeNumber { get; set; }
    }
}
