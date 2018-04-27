﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradingLicense.Web.Pages
{
    public class Actions
    {
        #region Common
        public const string Index = "Index";
        public const string Detail = "Detail";

        #endregion Common

        #region Account Controller
        public const string LogIn = "LogIn";
        public const string ForgotPassword = "ForgotPassword";
        public const string Logout = "Logout";
        public const string Error = "Error";
        public const string ResetPassword = "ResetPassword";
        #endregion

        #region Master
        public const string Department = "Department";
        #endregion

        #region Eroro

        public const string AccessDenied = "AccessDenied";
        #endregion
    }
}