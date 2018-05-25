using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class ResetPasswordModel
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "{0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(TradingLicense.Model.RegularExpressions.HtmlTag, ErrorMessage = "New Password is not valid.")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(40, MinimumLength = 6, ErrorMessage = "{0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "New Password and Confirm Password does not match.")]
        [RegularExpression(TradingLicense.Model.RegularExpressions.HtmlTag, ErrorMessage = "Confirm Password is not valid.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the role type identifier.
        /// </summary>
        public int RoleTypeId { get; set; }
    }
}
