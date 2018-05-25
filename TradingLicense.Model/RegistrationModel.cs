using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class RegistrationModel
    {
        public int UsersID { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter UserName")]
        [RegularExpression(TradingLicense.Model.RegularExpressions.Email, ErrorMessage = "Please enter valid Email")]
        public string Email { get; set; }
        
        public int Locked { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "{0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(TradingLicense.Model.RegularExpressions.HtmlTag, ErrorMessage = "Password is not valid.")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "{0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password does not match.")]
        [RegularExpression(TradingLicense.Model.RegularExpressions.HtmlTag, ErrorMessage = "Confirm Password is not valid.")]
        public string ConfirmPassword { get; set; }
    }
}
