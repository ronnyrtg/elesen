using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using TradingLicense.Data;
using TradingLicense.Infrastructure;
using TradingLicense.Model;

namespace TradingLicense.Web.Classes
{
    public class TradingLicenseCommon
    {
        private static List<KeyValuePair<string, int>> _pagesCollection = null;
        private static List<KeyValuePair<string, int>> _crudCollection = null;

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
                    if (ProjectSession.User != null)
                    {
                        int screenId = page.GetHashCode();
                        int crudLevel = pageRight.GetHashCode();
                        result = ctx.AccessPages.Where(o => o.RoleTemplateID == ProjectSession.User.RoleTemplateID && o.ScreenId == screenId && o.CrudLevel >= crudLevel).Count() > 0 ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// Fetches all the pages in system
        /// </summary>
        /// <returns>List of list items with Text as the description and value as the enum value</returns>
        public static List<KeyValuePair<string, int>> GetPages()
        {
            if (_pagesCollection != null)
                return _pagesCollection;

            List<KeyValuePair<string, int>> items = new List<KeyValuePair<string, int>>();
            Array pagesArray = Enum.GetValues(typeof(SystemEnum.Page));

            foreach (SystemEnum.Page page in pagesArray)
            {
                items.Add(new KeyValuePair<string, int>(page.ToString(), page.GetHashCode()));
            }
            _pagesCollection = items;

            return _pagesCollection;
        }

        /// <summary>
        /// Fetches crud access per roletemplate for a Page
        /// </summary>
        /// <param name="page">Page for which we are finding Role template access</param>
        /// <returns>List of roles and its crud access level</returns>
        public static List<AccessPageModel> GetPageAccess(SystemEnum.Page page)
        {
            List<AccessPageModel> result = new List<AccessPageModel>();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var roles = ctx.RoleTemplates.ToList();
                    int screenId = page.GetHashCode();
                    var accessPages = ctx.AccessPages.Where(o => o.ScreenId == screenId).ToList();
                    foreach (var role in roles)
                    {
                        if (accessPages.Any(ap => ap.RoleTemplateID == role.RoleTemplateID))
                        {
                            var accessPage = accessPages.First(ap => ap.RoleTemplateID == role.RoleTemplateID);
                            result.Add(AutoMapper.Mapper.Map<AccessPageModel>(accessPage));
                        }
                        else
                        {
                            result.Add(new AccessPageModel()
                            {
                                RoleTemplateID = role.RoleTemplateID,
                                PageDesc = page.ToString(),
                                ScreenId = page.GetHashCode(),
                                RoleTemplateDesc = role.RoleTemplateDesc
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        /// <summary>
        /// Fetches Crud Access level
        /// </summary>
        /// <returns>List of Available Crud access levels</returns>
        public static List<KeyValuePair<string, int>> GetCRUDLevelList()
        {
            if (_crudCollection == null)
            {
                var crudColl = new List<KeyValuePair<string, int>>();
                crudColl.Add(new KeyValuePair<string, int>("Tiada Akses", 0));
                crudColl.Add(new KeyValuePair<string, int>("Baca Sahaja", 1));
                crudColl.Add(new KeyValuePair<string, int>("Baca & Cipta", 2));
                crudColl.Add(new KeyValuePair<string, int>("Baca, Cipta & Ubah", 3));
                crudColl.Add(new KeyValuePair<string, int>("Baca, Cipta, Ubah & Padam", 4));
                _crudCollection = crudColl;
            }

            return _crudCollection;
        }

        /// <summary>
        /// Fetches value of the requested CRUD Level
        /// </summary>
        /// <param name="crudLevel">CRUD level that needs description</param>
        /// <returns>Description of the CRUD level</returns>
        public static string GetCRUDLevelDesc(int crudLevel)
        {
            var lstCRUDList = GetCRUDLevelList();
            return lstCRUDList.FirstOrDefault(kv => kv.Value == crudLevel).Key;
        }
    }
}