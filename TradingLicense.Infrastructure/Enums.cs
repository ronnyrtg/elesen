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
            unknown = 0,

            /// <summary>
            /// for set draft created
            /// </summary>
            draftcreated = 1,

            /// <summary>
            /// for set document incomplete
            /// </summary>
            documentIncomplete = 2,

            /// <summary>
            /// for set submitted to clerk
            /// </summary>
            submittedtoclerk = 3,

            /// <summary>
            /// for set processing by clerk
            /// </summary>
            processingByClerk = 4,

            /// <summary>
            /// for set unit route
            /// </summary>
            unitroute = 5,

            /// <summary>
            /// for set director check
            /// </summary>
            directorcheck = 6,

            /// <summary>
            /// for set meeting
            /// </summary>
            meeting = 7,

            /// <summary>
            /// for set CEO check
            /// </summary>
            CEOcheck = 8,

            /// <summary>
            /// for set Letter of notification (Approved)
            /// </summary>
            LetterofnotificationApproved = 9,

            /// <summary>
            /// for set Letter of notification (Rejected)
            /// </summary>
            LetterofnotificationRejected = 10,

            /// <summary>
            /// for set Letter of notification (Approved with Terms & Conditions)
            /// </summary>
            LetterofnotificationApprovedwithTermsConditions = 11,

            /// <summary>
            /// for set Pending payment
            /// </summary>
            Pendingpayment = 12,

            /// <summary>
            /// for set Pending payment
            /// </summary>
            Paid = 13,

            /// <summary>
            /// for set License Generated
            /// </summary>
            LicenseGenerated = 14,

            /// <summary>
            /// for set Complete Status
            /// </summary>
            Complete = 15,

            /// <summary>
            /// for set supervisor check
            /// </summary>
            supervisorcheck = 24,

            /// <summary>
            /// for set KIV at Meeting
            /// </summary>
            KIVatMeeting = 25,

            /// <summary>
            /// for set KIV at CEO
            /// </summary>
            KIVatCEO = 26,
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

        public enum ApplicationTypeID
        {
            /// <summary>
            /// Lesen Tred, Perniagaan & Perindustrian
            /// </summary>
            TradeApplication = 1,

            /// <summary>
            /// Lesen Petempatan Makanan
            /// </summary>
            FoodApplication = 2,

            /// <summary>
            /// Lesen Hotel & Rumah Tumpangan
            /// </summary>
            HotelApplication = 3,

            /// <summary>
            /// Lesen Pengurusan Skrap
            /// </summary>
            ScrapApplication = 4,

            /// <summary>
            /// lesen Iklan
            /// </summary>
            BannerApplication = 5,

            /// <summary>
            /// Lesen Penjaja
            /// </summary>
            HawkerApplication = 6,

            /// <summary>
            /// LEsen Pasar
            /// </summary>
            StallApplication = 7,

            /// <summary>
            /// Lesen Minuman Keras
            /// </summary>
            LiquorApplication = 8,

            /// <summary>
            /// Lesen Pemberi Pinjam Wang
            /// </summary>
            MLApplication = 9,

            /// <summary>
            /// Lesen Hiburan
            /// </summary>
            EntmtApplication = 10,
        }

        public enum Mode
        {
            /// <summary>
            /// for Express approval
            /// </summary>
            Express = 1,

            /// <summary>
            /// for Director approval
            /// </summary>
            Director = 2,

            /// <summary>
            /// for CEO approval
            /// </summary>
            CEO = 3,

            /// <summary>
            /// for Meeting approval
            /// </summary>
            Meeting = 4,
        }
    }
}
