namespace TradingLicense.Infrastructure
{
    public class Enums
    {
        public enum MessageType
        {
            /// <summary>
            /// for Success message Class
            /// </summary>
            success,

            /// <summary>
            /// for error message Class
            /// </summary>
            danger,

            /// <summary>
            /// for Warning message Class
            /// </summary>
            warning,

            /// <summary>
            /// for info message Class
            /// </summary>
            info
        }

        public enum ResetPasswordType
        {
            /// <summary>
            /// for set Password Type
            /// </summary>
            ResetPassword = 1,

            /// <summary>
            /// for forgot Password Type
            /// </summary>
            ForgotPassword = 2,

            /// <summary>
            /// for set password
            /// </summary>
            SetPassword = 3,

        }
    }
}
