using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class UsersModel
    {
        public int USERSID { get; set; }

        public string FULLNAME { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string USERNAME { get; set; }

        [Required(ErrorMessage = "Please enter UserName")]
        [RegularExpression(TradingLicense.Model.RegularExpressions.Email, ErrorMessage = "Please enter valid Email")]
        public string EMAIL { get; set; }

        [Display(Name = "Peranan Pengguna")]
        public int? ROLEID { get; set; }

        [Display(Name = "Department Code")]
        public int? DEP_ID { get; set; }

        public int LOCKED { get; set; }

        public bool ACTIVE { get; set; }

        public string Password { get; set; }

    }
}
