using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class APP_L_MTModel
    {
        public int APP_L_MTID { get; set; }
        public int APP_ID { get; set; }
        [Required(ErrorMessage = "Sila masukkan nombor rujukan mesyuarat")]
        [Display(Name = "Nombor Rujukan")]
        public string MT_REF { get; set; }
        [Required(ErrorMessage = "Sila masukkan tajuk mesyuarat")]
        [Display(Name = "Tajuk Mesyuarat")]
        public string MT_DESC { get; set; }
        public DateTime? MT_DATE { get; set; }
        public int USERSID { get; set; }
        public DateTime CREATED { get; set; }
        public bool ACTIVE { get; set; }

        public string ReferenceNo { get; set; }
        public string CompanyName { get; set; }
        public string LicenseTypeDesc { get; set; }
        public string Result { get; set; }
        public string FullName { get; set; }
    }
}
