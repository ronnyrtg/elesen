using System.Web.Mvc;
using System.Web.Routing;
using TradingLicense.Infrastructure;

namespace TradingLicense.Web.Classes
{
    /// <summary>
    /// Authorization Privilege Filter
    /// </summary>
    public class AuthorizationPrivilegeFilter : ActionFilterAttribute
    {

        public SystemEnum.Page AccessPage { get; set; }
        public SystemEnum.PageRight CrudLevel { get; set; }
        public AuthorizationPrivilegeFilter(SystemEnum.Page accessPage, SystemEnum.PageRight crudLevel)
        {
            AccessPage = accessPage;
            CrudLevel = crudLevel;
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!TradingLicenseCommon.HasAccess(AccessPage, CrudLevel))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", Pages.Controllers.Home }, { "action", Pages.Actions.AccessDenied } });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}