namespace TradingLicense.Infrastructure
{
    public class UserMail
    {
        /// <summary>
        /// Send forgot password mail
        /// </summary>
        /// <param name="emailAddress">email Address</param>
        /// <param name="userName">user Name</param>
        /// <param name="resetUrl">Reset Url</param>
        /// <returns>return boolean</returns>
        public static bool SendForgotPassword(string emailAddress, string userName, string resetUrl)
        {
            string emailbody = Email.GetEmailTemplate(SystemEnum.EmailTemplateFileName.ForgotPasswordEmailTemplate.ToString());
            emailbody = emailbody.Replace("[@UserName]", userName);
            emailbody = emailbody.Replace("[@LoginUrl]", ProjectConfiguration.SiteUrlBase);
            emailbody = emailbody.Replace("[@ResetUrl]", resetUrl);
            return Email.Send(Infrastructure.ProjectConfiguration.FromEmailAddress, emailAddress, null, null, "Trading License Password Recovery", emailbody);
        }

    }
}
