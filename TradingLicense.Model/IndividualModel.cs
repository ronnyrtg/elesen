using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class IndividualModel
    {
        public int IND_ID { get; set; }

        [Required(ErrorMessage = "Sila masukkan nama penuh")]
        [Display(Name = "Full Name")]
        [StringLength(50)]
        public string FULLNAME { get; set; }

        [Required(ErrorMessage = "Masukkan nombor MyKad atau Paspot")]
        [Display(Name = "Mykad No")]
        [StringLength(30)]
        public string MYKADNO { get; set; }

        [Display(Name = "Kewarganegaraan")]
        public int? CITIZENID { get; set; }
        [Display(Name = "Kaum")]
        public int? RACEID { get; set; }
        [Display(Name = "Lain-lain bangsa")]
        [StringLength(100)]
        public string LAIN_DESC { get; set; }

        [Required(ErrorMessage = "Sila masukkan alamat mengikut IC")]
        [Display(Name = "Address")]
        [StringLength(200)]
        public string ADD_IC { get; set; }

        [Required(ErrorMessage = "Sila masukkan nombor telefon")]
        [Display(Name = "Phone No")]
        [StringLength(200)]
        public string PHONE { get; set; }

        [Display(Name = "Email")]
        [StringLength(200)]
        public string IND_EMAIL { get; set; }
        [Display(Name = "Gambar")]
        public int? ATT_ID { get; set; }
        public string FileName { get; set; }
        [Display(Name = "Gender")]
        public int? GENDER { get; set; }

        [Display(Name = "Active")]
        public bool ACTIVE { get; set; }
    }
}
