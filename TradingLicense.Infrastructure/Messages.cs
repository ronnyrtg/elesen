
namespace TradingLicense.Infrastructure
{
    public class Messages
    {
        #region General
        public static string AccessDenied = "You are not authorized to access this page.";
        public static string AccessDeniedContactAdmin = "If you require to access this page, Please contact the system administrator.";
        public static string CommonErrorMessage = "Something went wrong, Please try again";
        public static string ConfirmDelete = "Are you sure you want to delete this Record?";
        public static string ContactToAdmin = "An error occurred on the system. Please contact the administrator.";
        public static string RecordSavedSuccessfully = "Record saved successfully.";
        public static string RecordDeletedSuccessfully = "Record deleted successfully.";
        public static string RecordNotDeleted = "Record is not deleted successfully because it refers to another entity.";
        public static string ConfirmActivate = "Are you sure you want to activate this Record?";
        public static string ConfirmDectivate = "Are you sure you want to deactivate this Record?";
        public static string RecordActivatedSuccessfully = "Record activated successfully.";
        public static string RecordDeactivatedSuccessfully = "Record deactivated successfully.";
        public static string RecordActivationFailed = "Record activation/deactivation failed.";
      
        #endregion

        #region LogIn
        public static string UserName = "Please Enter valid UserName.";
        public static string Password = "Please Enter valid Password.";
        public static string InValidCredential = "Please Enter valid UserName or Password.";
        public static string InActiveAccount = "Your Account is not active. Please contact to administrator.";
        public static string ClientConfigInvalid = "Client is not Configured properly. Please contact to administrator.";
        public static string Mailsend = "Instructions on how to reset your password have been sent to your email account.";
        public static string InvalidEmail = "We couldn't find a Trading License account associated with email address provided.";
        public static string PasswordReSet = "Password change successfully.";
        public static string ResetPasswordMessage = "The URL is no longer valid.";
        public static string ResetPasswordRequest = "An error occurred while processing your request.";
        public static string UrlNotExist = "User does not exist.";
        public static string ResetPasswordMatch = "New Password and Confirm Password does not match.";
        public static string UrlNotvalid = "URL is not valid.";
        public static string AccountLock = "Your Account has been locked. Please Contact your administrator.";
        #endregion


        #region Registration
        public static string DataMissing = "Data Missing.";
        public static string UserNameAvailable= "UserName Allready Available.";
        public static string EmailAvailable = "Email Allready Available.";
        public static string UserRegistration = "User Registered Successfully.";
        #endregion

    }
}
