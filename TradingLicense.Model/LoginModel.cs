using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class LoginModel
    {
        /// <summary>
        /// Gets or sets Username
        /// </summary>
        [Required(ErrorMessage = "Please enter UserName")]
        [Display(Name = "UserName")]
        [RegularExpression(TradingLicense.Model.RegularExpressions.UserName, ErrorMessage = "Please enter valid UserName")]
        public string Username { get; set; }

        /// <summary> 
        /// Gets or sets Password
        /// </summary>
        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(TradingLicense.Model.RegularExpressions.HtmlTag, ErrorMessage = "Invalid password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether remember me true or false
        /// </summary>        
        public bool RememberMe { get; set; }

        /// <summary>
        /// Gets or sets ReturnUrl
        /// </summary>        
        public string ReturnUrl { get; set; }
    }
}
