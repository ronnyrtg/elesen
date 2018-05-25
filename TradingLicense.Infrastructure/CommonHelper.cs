namespace TradingLicense.Infrastructure
{
    public class CommonHelper
    {
        /// <summary>
        /// Sets alert message and displays on page.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="IsConfirmation"></param>
        /// <param name="YesResponseMethod"></param>
        /// <param name="NoResponseMethod"></param>
        /// <returns></returns>
        public static string ShowAlertMessageToastr(string type, string message, bool IsConfirmation = false, string YesResponseMethod = "", string NoResponseMethod = "")
        {
            message = message.Replace("'", " ");
            var strString = @"<script type='text/javascript' language='javascript'>$(function() { ShowMessageToastr('" + type + "','" + message + "','" + IsConfirmation.ToString().ToLower() + "','" + YesResponseMethod + "','" + NoResponseMethod + "') ; })</script>";
            return strString;
        }
    }
}
