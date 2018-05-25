using System.ComponentModel;

namespace TradingLicense.Infrastructure
{
    public class SystemEnum
    {

        /// <summary>
        /// Enumeration for Page
        /// </summary>
        public enum Page
        {
            /// <summary>
            /// AccessPages
            /// </summary>
            [Description("AccessPages")]
            AccessPages = 1,

            /// <summary>
            /// Additional Infos
            /// </summary>
            [Description("AdditionalInfos")]
            AdditionalInfos = 2,

            /// <summary>
            /// Attachment
            /// </summary>
            [Description("Attachment")]
            Attachment = 3,

            /// <summary>
            /// Administrator
            /// </summary>
            [Description("Administrator")]
            Administrator = 4,

            /// <summary>
            /// Master Setup
            /// </summary>
            [Description("MasterSetup")]
            MasterSetup = 5,

            /// <summary>
            /// Inquiry
            /// </summary>
            [Description("Inquiry")]
            Inquiry = 6,

            /// <summary>
            /// Reporting
            /// </summary>
            [Description("Reporting")]
            Reporting = 7,

            /// <summary>
            /// Individual
            /// </summary>
            [Description("Individual")]
            Individual = 8,

            /// <summary>
            /// DeskOfficer
            /// </summary>
            [Description("DeskOfficer")]
            DeskOfficer = 9,

            /// <summary>
            /// Profile
            /// </summary>
            [Description("Profile")]
            Profile = 10,

            /// <summary>
            /// Process
            /// </summary>
            [Description("Process")]
            Process = 11,

            /// <summary>
            /// Company
            /// </summary>
            [Description("Company")]
            Company = 12,
        }


        public enum PageRight
        {
            /// <summary>
            /// CrudLevel
            /// </summary>
            [Description("CrudLevel")]
            CrudLevel = 1,

            /// <summary>
            /// CrudLevel
            /// </summary>
            [Description("CrudLevel2")]
            CrudLevel2 = 2,

            /// <summary>
            /// CrudLevel
            /// </summary>
            [Description("CrudLevel3")]
            CrudLevel3 = 3,

            /// <summary>
            /// CrudLevel
            /// </summary>
            [Description("CrudLevel4")]
            CrudLevel4 = 4,
        }
        
        /// <summary>
        /// Enumeration for Role Type
        /// </summary>
        public enum RoleType
        {
            /// <summary>
            /// The User
            /// </summary>
            [Description("User")]
            User = 1,

        }

        /// <summary>
        /// Enumeration for EmailTemplateName
        /// </summary>
        public enum EmailTemplateFileName
        {
            /// <summary>
            /// Email Template Master.
            /// </summary>
            [Description("Master Email Template")]
            MasterEmailTemplate = 1,

            /// <summary>
            /// Email Template Forgot Password.
            /// </summary>
            [Description("Forgot Password Email Template")]
            ForgotPasswordEmailTemplate = 2,
            
        }
    }
}
