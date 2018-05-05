using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class PremiseApplicationModel
    {
        public int PremiseApplicationID { get; set; }

        [Display(Name = "Business Type")]
        [Required(ErrorMessage = "Please select Business Type")]
        public List<int> BusinessTypeID { get; set; }

        [Display(Name = "Applicant")]
        [Required(ErrorMessage = "Please select Individual")]
        public int IndividualID { get; set; }

        [Required(ErrorMessage = "Please enter Address")]
        [StringLength(255)]
        public string PremiseAddress { get; set; }

        public int PremiseStatus { get; set; }

        public int PremiseTypeID { get; set; }
        public string PremiseDesc { get; set; }

        public int PremiseModification { get; set; }
        public DateTime DateSubmitted { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }

        public int PAStatusID { get; set; }
        public string StatusDesc { get; set; }
    }
}