using System;
using System.Web.Mvc;
using TradingLicense.Infrastructure;
using TradingLicense.Web.Pages;

namespace TradingLicense.Web.Classes
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                bool isAllow = false;
                if (ProjectSession.UserID == 0 && filterContext.ActionDescriptor.ActionName == Actions.LogIn)
                {
                    isAllow = true;
                }
                else if (ProjectSession.UserID == 0 && filterContext.ActionDescriptor.ActionName == Actions.ForgotPassword)
                {
                    isAllow = true;
                }
                else if (ProjectSession.UserID > 0)
                {
                    isAllow = true;
                }
                else
                {
                    isAllow = false;
                }

                if (!isAllow)
                {
                    filterContext.Result = new RedirectResult("~/Account/LogIn");
                    return;
                }

                base.OnActionExecuting(filterContext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}