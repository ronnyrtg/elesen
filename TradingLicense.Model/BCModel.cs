using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class BCModel
    {

        public int BC_ID { get; set; }
        public int LIC_TYPEID { get; set; }

        [Required(ErrorMessage = "Please Enter Code Description")]
        [Display(Name = "Code Description")]
        public string C_R_DESC { get; set; }
        public int? SECTORID { get; set; }
        [Display(Name = "Default Rate")]
        public float? DEF_RATE { get; set; }       
        [Display(Name = "Base Fee")]
        public float? BASE_FEE { get; set; }
        public string O_NAME { get; set; }
        public float? O_FEE { get; set; }
        public string DEF_HOUR { get; set; }
        public float? EX_HOUR_FEE { get; set; }
        public float? EX_FEE { get; set; }
        public float? DEPOSIT { get; set; }
        public float? P_FEE { get; set; }
        [Display(Name = "Period")]
        public int? PERIOD { get; set; }
        [Required(ErrorMessage = "Please Enter Period Quantity")]
        [Display(Name = "Period Quantity")]
        public int? PERIOD_Q { get; set; }

        [Display(Name = "Is Active")]
        public bool ACTIVE { get; set; }

        public string LIC_TYPEDESC { get; set; }
        public string SectorDesc { get; set; }
        public string DepartmentIDs { get; set; }
        public List<int> AdditionalDocs { get; set; }

        public List<Select2ListItem> selectedDepartments = new List<Select2ListItem>();
    }
}
