using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradingLicense.Data;
using TradingLicense.Infrastructure;

namespace TradingLicense.Web.Classes
{
    public class TradingLicenseCommon
    {
        /// <summary>
        /// Determines whether the specified page type has access.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>has access or not</returns>
        public static bool HasAccess(SystemEnum.Page page, SystemEnum.PageRight pageRight)
        {
            bool result = false;
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    if(ProjectSession.User != null)
                    {
                        int screenId = page.GetHashCode();
                        int crudLevel = pageRight.GetHashCode();
                        result = ctx.AccessPages.Where(o => o.RoleTemplateID == ProjectSession.User.RoleTemplateID && o.ScreenId == screenId && o.CrudLevel >= crudLevel).Count() > 0 ? true : false;
                    }
                }
            }
            catch(Exception ex)
            {
            }

            return result;
        }
    }
}