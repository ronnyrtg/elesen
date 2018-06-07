using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class PremiseApplicationModel
    {
        public int PremiseApplicationID { get; set; }
        [Required(ErrorMessage = "Sila pilih jenis perniagaan")]
        public int BusinessTypeID { get; set; }
        [Required(ErrorMessage = "Sila pilih Kumpulan Kod")]
        public int SectorID { get; set; }
        [Required(ErrorMessage = "Sila masukkan nama pemohon")]
        public string NamaPemohon { get; set; }
        [Required(ErrorMessage = "Sila masukkan IC atau No. Paspot")]
        public string ICPaspot { get; set; }
        [Required(ErrorMessage = "Sila masukkan nama syarikat")]
        public string NamaSyarikat { get; set; }

        public int? CompanyID { get; set; }
        public int? UsersID { get; set; }
        public int? IndividualID { get; set; }
        public int? PremiseAddressID { get; set; }
        [Required(ErrorMessage = "Sila pilih pemilikan premis")]
        public int? PremiseOwnership { get; set; }
        public DateTime? StartRent { get; set; }
        public DateTime? StopRent { get; set; }
        public string WhichFloor { get; set; }
        
        public int? PremiseTypeID { get; set; }
        public string OtherPremiseType { get; set; }
        
        public float? PremiseArea { get; set; }

        public DateTime DateSubmitted { get; set; }
        public string UpdatedBy { get; set; }
        public int AppStatusID { get; set; }

        public string BusinessCodeids { get; set; }
        public string Individualids { get; set; }
        public string RequiredDocIds { get; set; }
        public string AdditionalDocIds { get; set; }

        public int UserRollTemplate { get; set; }

        public string FullName { get; set; }

        public string CompanyName { get; set; }

        public string StatusDesc { get; set; }

        public string PremiseDesc { get; set; }

        public bool IsDraft { get; set; }

        public string UploadRequiredDocids { get; set; }

        public string UploadAdditionalDocids { get; set; }

        public string newIndividualsList { get; set; }

        public List<SelectedBusinessCodeModel> selectedbusinessCodeList = new List<SelectedBusinessCodeModel>();

        public List<SelectedIndividualModel> selectedIndividualList = new List<SelectedIndividualModel>();
    }

    public class NewIndividualModel
    {
        public string fullName { get; set; }
        public string passportNo { get; set; }
    }
}