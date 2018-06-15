using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class IndividualModel
    {
        public int IndividualID { get; set; }

        [Required(ErrorMessage = "SIla masukkan nama penuh")]
        [Display(Name = "Full Name")]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Masukkan nombor MyKad atau Paspot")]
        [Display(Name = "Mykad No")]
        [StringLength(30)]
        public string MykadNo { get; set; }

        [Display(Name = "Kewarganegaraan")]
        public int NationalityID { get; set; }

        [Required(ErrorMessage = "Sila masukkan alamat mengikut IC")]
        [Display(Name = "Address")]
        [StringLength(200)]
        public string AddressIC { get; set; }

        [Required(ErrorMessage = "Sila masukkan nombor telefon")]
        [Display(Name = "Phone No")]
        [StringLength(200)]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Sila masukkan Email")]
        [Display(Name = "Email")]
        [StringLength(200)]
        public string IndividualEmail { get; set; }
        [Display(Name = "Gambar")]
        public int AttachmentID { get; set; }
        public string FileName { get; set; }
        [Display(Name = "Gender")]
        public int Gender { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
