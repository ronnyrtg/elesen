using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class LocationModel
    {
        public int LocationID { get; set; }
        [Display(Name = "Location Code")]
        [Required(ErrorMessage = "Please enter Location Code")]
        [StringLength(5)]
        public string LocationCode { get; set; }
        [Display(Name = "Location Description")]
        [Required(ErrorMessage = "Please enter Location Description")]
        [StringLength(255)]
        public string LocationDesc { get; set; }
        public int? UsersID { get; set; }
        public DateTime? LastUpdated { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
