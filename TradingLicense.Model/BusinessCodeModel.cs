using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class BusinessCodeModel
    {

        public int BusinessCodeID { get; set; }

        [Required(ErrorMessage = "Please Enter Code Number")]
        [Display(Name = "Code Number")]
        [StringLength(5)]
        public string CodeNumber { get; set; }

        [Required(ErrorMessage = "Please Enter Code Description")]
        [Display(Name = "Code Description")]
        [StringLength(255)]
        public string CodeDesc { get; set; }

        [Required(ErrorMessage = "Please Enter Default Rate")]
        [Display(Name = "Default Rate")]
        public float DefaultRate { get; set; }

        [Required(ErrorMessage = "Please Enter Base Fee")]
        [Display(Name = "Base Fee")]
        public float BaseFee { get; set; }

        [Required(ErrorMessage = "Please Enter Extra Fee")]
        [Display(Name = "Extra Fee")]
        public float ExtraFee { get; set; }

        [Required(ErrorMessage = "Please Enter Extra Unit")]
        [Display(Name = "Extra Unit")]
        public int ExtraUnit { get; set; }

        [Required(ErrorMessage = "Please Enter Period")]
        [Display(Name = "Period")]
        [StringLength(1)]
        public string Period { get; set; }

        [Required(ErrorMessage = "Please Enter Period Quantity")]
        [Display(Name = "Period Quantity")]
        public int PQuantity { get; set; }

        [Required(ErrorMessage = "Please Enter Express")]
        [Display(Name = "Express")]
        public bool Express { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        [Required(ErrorMessage = "Please Select Sector")]
        public int SectorID { get; set; }
        public string SectorDesc { get; set; }
    }

    public class SelectedBusinessCodeModel
    {
       public int id { get; set; }
       public string text { get; set; }
    }

    public class SelectedIndividualModel
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
