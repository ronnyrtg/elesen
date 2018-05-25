using System.Web;
using TradingLicense.Model;

namespace TradingLicense.Infrastructure
{
    /// <summary>
    /// Project sessions
    /// </summary>
    /// <CreatedBy>  </CreatedBy>
    /// <CreatedDate>21-04-2018</CreatedDate>
    /// <ModifiedBy></ModifiedBy>
    /// <ModifiedDate></ModifiedDate>
    /// <ReviewBy></ReviewBy>
    /// <ReviewDate></ReviewDate>
    public class ProjectSession
    {

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public static int UserID
        {
            get
            {
                if (HttpContext.Current.Session["UserID"] == null)
                {
                    return 0;
                }
                else
                {
                    return ConvertTo.Integer(HttpContext.Current.Session["UserID"]);
                }
            }

            set
            {
                HttpContext.Current.Session["UserID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["UserName"] == null)
                {
                    return "User";
                }
                else
                {
                    return ConvertTo.String(HttpContext.Current.Session["UserName"]);
                }
            }

            set
            {
                HttpContext.Current.Session["UserName"] = value;
            }
        }


        /// <summary>
        /// Gets or sets Properties to store project session for User
        /// </summary>
        public static UsersModel User
        {
            get
            {
                return (UsersModel)HttpContext.Current.Session["User"];
            }

            set
            {
                HttpContext.Current.Session["User"] = value;
            }
        }
    }
}
