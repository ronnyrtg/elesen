using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class IndividualModel
    {
        public int IndividualID { get; set; }

        [Required(ErrorMessage = "Please Enter Full Name")]
        [Display(Name = "Full Name")]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please Enter Mykad No")]
        [Display(Name = "Mykad No")]
        [StringLength(30)]
        public string MykadNo { get; set; }

       // [Required(ErrorMessage = "Please Select Nationality")]
        [Display(Name = "Nationality")]
        //[StringLength(30)]
        public int NationalityID { get; set; }

        [Required(ErrorMessage = "Please Select Address")]
        [Display(Name = "Address")]
        [StringLength(200)]
        public string AddressIC { get; set; }

        [Required(ErrorMessage = "Please Select Phone No")]
        [Display(Name = "Phone No")]
        [StringLength(200)]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Please Select Individual Email")]
        [Display(Name = "Individual Email")]
        [StringLength(200)]
        public string IndividualEmail { get; set; }

        [Display(Name = "Gender")]
        public int Gender { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
