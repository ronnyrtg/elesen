using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class AdditionalDocModel
    {
        public int AdditionalDocID { get; set; }

        [Display(Name = "Required Document Description")]
        [Required]
        [StringLength(255)]
        public string DocDesc { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
