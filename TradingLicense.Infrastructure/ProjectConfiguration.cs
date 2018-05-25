using System;
using System.Configuration;
using System.Web;

namespace TradingLicense.Infrastructure
{
    public class ProjectConfiguration
    {
        /// <summary>
        /// Gets a page value 
        /// </summary>
        public static int PageSize
        {
            get
            {
                return ConvertTo.Integer(ConfigurationManager.AppSettings.Get("PageSize"));
            }
        }


        /// <summary>
        /// Gets a value indicating whether [is log error].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is log error]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsLogError
        {
            get
            {
                return ConvertTo.Boolean(System.Configuration.ConfigurationManager.AppSettings["IsLogError"]);
            }
        }

        public static string AttachmentDocument
        {
            get
            {
                return ConvertTo.String(ConfigurationManager.AppSettings.Get("Attachment"));
            }
        }

        public static string PremiseAttachmentDocument
        {
            get
            {
                return ConvertTo.String(ConfigurationManager.AppSettings.Get("PremiseAttachment"));
            }
        }


        /// <summary>
        /// Gets the upload file format.
        /// </summary>
        /// <value>The upload file format.</value>
        public static string UploadFileFormat
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("UploadFileFormat");
            }
        }

        /// <summary>
        /// Gets current Url 
        /// </summary>
        public static string CurrentUrl
        {
            get
            {
                return HttpContext.Current.Request.Url.ToString() + (HttpContext.Current.Request.Url.Port > 0 ? (":" + HttpContext.Current.Request.Url.Port) : string.Empty);
            }
        }

        #region Configuration Settings

        /// <summary>
        /// Gets the Configuration From Email Address
        /// </summary>
        public static string FromEmailAddress
        {
            get
            {
                return ConvertTo.String(System.Configuration.ConfigurationManager.AppSettings["FromEmailAddress"]);
            }
        }

        /// <summary>
        /// Gets the Configuration Test To Email Address
        /// </summary>
        public static string TestEmailToAddress
        {
            get
            {
                return ConvertTo.String(System.Configuration.ConfigurationManager.AppSettings["TestEmailToAddress"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether is Send mail on Test account or not.
        /// </summary>
        public static bool IsEmailTest
        {
            get
            {
                return ConvertTo.Boolean(System.Configuration.ConfigurationManager.AppSettings["IsEmailTest"]);
            }
        }


        /// <summary>
        /// Gets the Configuration Reset Password Expire Time in Minutes
        /// </summary>
        public static int ResetPasswordExpireTime
        {
            get
            {
                return ConvertTo.Integer(System.Configuration.ConfigurationManager.AppSettings["ResetPasswordExpireTime"]);
            }
        }

        #endregion

        #region Format

        /// <summary>
        /// Gets Url Suffix
        /// </summary>
        private static string UrlSuffix
        {
            get
            {
                if (HttpContext.Current.Request.ApplicationPath == "/")
                {
                    return HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port > 0 ? (":" + HttpContext.Current.Request.Url.Port) : string.Empty) + HttpContext.Current.Request.ApplicationPath;
                }
                else
                {
                    return HttpContext.Current.Request.Url.Host + (HttpContext.Current.Request.Url.Port > 0 ? (":" + HttpContext.Current.Request.Url.Port) : string.Empty) + HttpContext.Current.Request.ApplicationPath + "/";
                }
            }
        }

        /// <summary>
        /// Gets the Date Format
        /// </summary>
        public static string EmailDateTimeFormat
        {
            get
            {
                return "yyyyMMddHHmm";
                ////return "dd MMM yyyy HH:mm";
                ////return "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Gets Secure User Base
        /// </summary>
        public static string SecureUrlBase
        {
            get
            {
                return "https://" + UrlSuffix;
            }
        }

        /// <summary>
        /// Gets Url Base
        /// </summary>
        public static string UrlBase
        {
            get
            {
                return "http://" + UrlSuffix;
            }
        }

        /// <summary>
        /// Gets Site Url Base
        /// </summary>
        public static string SiteUrlBase
        {
            get
            {
                if (HttpContext.Current.Request.IsSecureConnection)
                {
                    return SecureUrlBase;
                }
                else
                {
                    return UrlBase;
                }
            }
        }

        /// <summary>
        /// Gets Email Template Path
        /// </summary>
        public static string EmailTemplatePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/EmailTemplates/");
            }
        }

        public static string EmailTemplateFloder
        {
            get
            {
                return "/EmailTemplates/";
            }
        }

        #endregion

        #region System Path

        /// <summary>
        /// Gets the admin path.
        /// </summary>
        public static string AdminPath
        {
            get
            {
                return ConvertTo.String(System.Configuration.ConfigurationManager.AppSettings["AdminPath"]);
            }
        }

        /// <summary>
        /// Gets the Root Path of the Project
        /// </summary>
        public static string ApplicationRootPath
        {
            get
            {
                string rootPath = AdminPath;
                if (rootPath.EndsWith("\\", StringComparison.CurrentCulture))
                {
                    return rootPath;
                }
                else
                {
                    return rootPath + "\\";
                }
            }
        }

        #endregion
    }
}
