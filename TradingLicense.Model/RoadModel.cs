using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class RoadModel
    {
        public int ROADID { get; set; }
        [Display(Name = "Road Code")]
        [Required(ErrorMessage = "Please enter Road Code")]
        [StringLength(5)]
        public string ROADCODE { get; set; }
        [Display(Name = "Road Description")]
        [Required(ErrorMessage = "Please enter Road Description")]
        [StringLength(255)]
        public string ROADDESC { get; set; }
        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }
    }
}
