using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class RoadModel
    {
        public int RoadID { get; set; }
        [Display(Name = "Road Code")]
        [Required(ErrorMessage = "Please enter Road Code")]
        [StringLength(5)]
        public string RoadCode { get; set; }
        [Display(Name = "Road Description")]
        [Required(ErrorMessage = "Please enter Road Description")]
        [StringLength(255)]
        public string RoadDesc { get; set; }
        public int? UsersID { get; set; }
        public DateTime? LastUpdated { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
