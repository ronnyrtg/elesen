using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class LicenseTypeModel
    {
        public int LIC_TYPEID { get; set; }

        [Display(Name = "Jenis Lesen")]
        [Required(ErrorMessage = "Sila masukkan nama jenis lesen")]
        [StringLength(255)]
        public string LIC_TYPEDESC { get; set; }
        [Display(Name = "Kod Lesen")]
        [Required(ErrorMessage = "Sila masukkan nama kod lesen")]
        [StringLength(10)]
        public string LIC_TYPECODE { get; set; }
        [Display(Name = "Aktif?")]
        public bool ACTIVE { get; set; }

        public List<int> RequiredDocs { get; set; }
    }
}
