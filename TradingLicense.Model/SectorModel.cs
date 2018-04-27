using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class SectorModel
    {
        public int SectorID { get; set; }

        [Display(Name = "Sector Description")]
        [Required(ErrorMessage = "Please enter Sector Description")]
        [StringLength(30)]
        public string SectorDesc { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
