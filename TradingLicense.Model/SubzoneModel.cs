using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class SubzoneModel
    {
        public int ZONEID { get; set; }
        [Display(Name = "Zone Code")]
        [Required(ErrorMessage = "Please enter Zone Code")]
        [StringLength(5)]
        public string ZONE_CODE { get; set; }
        public string SUBZONE_CODE { get; set; }
        [Display(Name = "Zone Description")]
        [Required(ErrorMessage = "Please enter Zone Description")]
        [StringLength(255)]
        public string SUBZONE_DESC { get; set; }
        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }
        public List<Select2ListItem> zoneCombineList = new List<Select2ListItem>();
    }
}
