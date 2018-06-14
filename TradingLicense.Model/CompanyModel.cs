using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class CompanyModel
    {
        public int CompanyID { get; set; }
        [Required(ErrorMessage = "Please Enter Registration No")]
        [Display(Name = "Registration No")]
        [StringLength(50)]
        public string RegistrationNo { get; set; }

        [Required(ErrorMessage = "Please Enter Company Name")]
        [Display(Name = "Company Name")]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Sila masukkan nombor telefon pejabat")]
        [Display(Name = "No. Telefon Pejabat")]
        [StringLength(20)]
        public string CompanyPhone { get; set; }

        [Required(ErrorMessage = "Please Enter Company Address")]
        [Display(Name = "Company Address")]
        [StringLength(255)]
        public string CompanyAddress { get; set; }

        public bool Active { get; set; }
    }
}
