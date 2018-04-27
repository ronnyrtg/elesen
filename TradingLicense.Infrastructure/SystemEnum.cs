﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CrudLeve3 = 3,

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
