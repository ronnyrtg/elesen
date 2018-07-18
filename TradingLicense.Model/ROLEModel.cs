using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class ROLEModel
    {
        public int ROLEID { get; set; }        
        [Required(ErrorMessage = "Sila masukkan nama peranan")]
        [Display(Name = "Nama Peranan")]
        public string ROLE_DESC { get; set; }
        //This is the maximum number of days allowed for each role to process an application
        [Display(Name = "Tempoh")]
        public int DURATION { get; set; }
    }
}
