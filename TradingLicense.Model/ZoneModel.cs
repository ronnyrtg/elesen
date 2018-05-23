using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class ZoneModel
    {
        public int ZoneID { get; set; }
        [Display(Name = "Zone Code")]
        [Required(ErrorMessage = "Please enter Zone Code")]
        [StringLength(5)]
        public string ZoneCode { get; set; }
        [Display(Name = "Zone Description")]
        [Required(ErrorMessage = "Please enter Zone Description")]
        [StringLength(255)]
        public string ZoneDesc { get; set; }
        public int? UsersID { get; set; }
        public DateTime? LastUpdated { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }
    }
}
