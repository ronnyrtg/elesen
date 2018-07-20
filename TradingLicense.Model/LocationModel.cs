using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class LocationModel
    {
        public int LOCATIONID { get; set; }
        [Display(Name = "Location Code")]
        [Required(ErrorMessage = "Please enter Location Code")]
        [StringLength(5)]
        public string L_CODE { get; set; }
        [Display(Name = "Location Description")]
        [Required(ErrorMessage = "Please enter Location Description")]
        [StringLength(255)]
        public string L_DESC { get; set; }
        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }
    }
}
