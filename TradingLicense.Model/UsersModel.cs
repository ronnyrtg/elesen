using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class UsersModel
    {
        public int UsersID { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter UserName")]
        [RegularExpression(TradingLicense.Model.RegularExpressions.Email, ErrorMessage = "Please enter valid Email")]
        public string Email { get; set; }

        [Display(Name = "RoleTemplate")]
        public int? RoleTemplateID { get; set; }

        [Display(Name = "Department Code")]
        public int? DepartmentID { get; set; }

        public int Locked { get; set; }

        public bool Active { get; set; }

        public string Password { get; set; }

    }
}
