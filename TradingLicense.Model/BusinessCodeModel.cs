using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Please Enter Period")]
        [Display(Name = "Period")]
        public int Period { get; set; }

        [Required(ErrorMessage = "Please Enter Period Quantity")]
        [Display(Name = "Period Quantity")]
        public int PeriodQuantity { get; set; }

        [Required(ErrorMessage = "Please Select Mode")]
        [Display(Name = "Mode")]
        public int Mode { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; }

        [Required(ErrorMessage = "Please Select Sector")]
        public int SectorID { get; set; }
        public string SectorDesc { get; set; }

        public string DepartmentIDs { get; set; }
        public List<int> AdditionalDocs { get; set; }

        public List<Select2ListItem> selectedDepartments = new List<Select2ListItem>();
    }

    public class Select2ListItem
    {
       public int id { get; set; }
       public string text { get; set; }
    }

    public class SelectedIndividualModel
    {
        public int id { get; set; }
        public string text { get; set; }
        public string fullName { get; set; }
        public string passportNo { get; set; }
    }
}
