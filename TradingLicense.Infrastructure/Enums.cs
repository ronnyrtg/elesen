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

        public enum PAStausenum
        {
            /// <summary>
            /// for set draft created
            /// </summary>
            draftcreated = 1,

            /// <summary>
            /// for set submitted to clerk
            /// </summary>
            submittedtoclerk = 2,

            /// <summary>
            /// for set unit route
            /// </summary>
            unitroute = 3,

            /// <summary>
            /// for set supervisor check
            /// </summary>
            supervisorcheck = 4,

            /// <summary>
            /// for set director check
            /// </summary>
            directorcheck = 5,

            /// <summary>
            /// for set meeting
            /// </summary>
            meeting = 6,

            /// <summary>
            /// for set KIV at Meeting
            /// </summary>
            KIVatMeeting = 7,

            /// <summary>
            /// for set CEO check
            /// </summary>
            CEOcheck = 8,

            /// <summary>
            /// for set KIV at CEO
            /// </summary>
            KIVatCEO = 9,

            /// <summary>
            /// for set Letter of notification (Approved)
            /// </summary>
            LetterofnotificationApproved = 10,

            /// <summary>
            /// for set Letter of notification (Rejected)
            /// </summary>
            LetterofnotificationRejected = 11,

            /// <summary>
            /// for set Letter of notification (Approved with Terms & Conditions)
            /// </summary>
            LetterofnotificationApprovedwithTermsConditions = 12,

            /// <summary>
            /// for set Pending payment
            /// </summary>
            Pendingpayment = 13,

            /// <summary>
            /// for set License Generated
            /// </summary>
            LicenseGenerated = 14,
        }

        public enum RollTemplate
        {
            /// <summary>
            /// for set Public
            /// </summary>
            Public = 1,

            /// <summary>
            /// for set Desk Officer
            /// </summary>
            DeskOfficer = 2,

            /// <summary>
            /// for set Clerk
            /// </summary>
            Clerk = 3,

            /// <summary>
            /// for set Supervisor
            /// </summary>
            Supervisor = 4,

            /// <summary>
            /// for set Route Unit
            /// </summary>
            RouteUnit = 5,

            /// <summary>
            /// for set Director
            /// </summary>
            Director = 6,

            /// <summary>
            /// for set CEO
            /// </summary>
            CEO = 7,

            /// <summary>
            /// for set Public
            /// </summary>
            Administrator = 8,
        }

    }
}
