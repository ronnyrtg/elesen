using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TradingLicense.Web.Controllers
{
    public class HomeController : Controller
    {

        /// <summary>
        /// Access Denied
        /// </summary>
        /// <returns>Access Denied</returns>
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}