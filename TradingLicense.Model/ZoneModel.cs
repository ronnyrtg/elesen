using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class ZoneModel
    {
        public int ZONEID { get; set; }
        [Display(Name = "Zone Code")]
        [Required(ErrorMessage = "Please enter Zone Code")]
        [StringLength(5)]
        public string ZONECODE { get; set; }
        [Display(Name = "Zone Description")]
        [Required(ErrorMessage = "Please enter Zone Description")]
        [StringLength(255)]
        public string ZONEDESC { get; set; }
        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }
    }
}
