using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingLicense.Entities;

namespace TradingLicense.Model
{
    public class PremiseApplicationModel
    {
        public int PremiseApplicationID { get; set; }

        [Required(ErrorMessage = "Please Select Business Type")]
        public int BusinessTypeID { get; set; }

        public int UsersID { get; set; }

        [Required(ErrorMessage = "Please Select Mykad/Passport No")]
        public int IndividualID { get; set; }

        [StringLength(255)]
        public string PremiseAddress { get; set; }

        [Required(ErrorMessage = "Please Select Premise Status")]
        public int PremiseStatus { get; set; }

        [Required(ErrorMessage = "Please Select Premise Type")]
        public int PremiseTypeID { get; set; }

        [Required(ErrorMessage = "Please Enter Premise Area")]
        public float PremiseArea { get; set; }

        [Required(ErrorMessage = "Please Select Premise Modification")]
        public int PremiseModification { get; set; }

        public DateTime DateSubmitted { get; set; }
        
        public string UpdatedBy { get; set; }

        public int AppStatusID { get; set; }

        public string BusinessCodeids { get; set; }

        public string Individualids { get; set; }

        public string RequiredDocIds { get; set; }

        public string AdditionalDocIds { get; set; }

        public int UserRollTemplate { get; set; }

        public string StatusDesc { get; set; }

        public string PremiseDesc { get; set; }

        public bool IsDraft { get; set; }

        public string UploadRequiredDocids { get; set; }

        public string UploadAdditionalDocids { get; set; }

        public List<SelectedBusinessCodeModel> selectedbusinessCodeList = new List<SelectedBusinessCodeModel>();

        public List<SelectedIndividualModel> selectedIndividualList = new List<SelectedIndividualModel>();
    }
}