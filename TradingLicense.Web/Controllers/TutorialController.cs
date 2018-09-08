using AutoMapper;
using DataTables.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using TradingLicense.Data;
using TradingLicense.Entities;
using TradingLicense.Model;
using TradingLicense.Web.Classes;

namespace TradingLicense.Web.Controllers
{
    public class TutorialController : BaseController
    {
        private Func<LIC_TYPE, Select2ListItem> fnSelectLicenseFormat = lic => new Select2ListItem { id = lic.LIC_TYPEID, text = $"{lic.LIC_TYPECODE} ({lic.LIC_TYPEDESC})" };

        #region ManageTextInput

        /// <summary>
        /// Get text input data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageTextInput(int? Id)
        {
            TutorialModel tutorialModel = new TutorialModel();
            tutorialModel.ACTIVE = true;
            if (Id.HasValue)
            {
                using (var ctx = new LicenseApplicationContext())
                {                  
                    var tutorialData = ctx.TUTORIALs.Where(a => a.TUTORIAL_ID == Id).FirstOrDefault();
                    tutorialModel = Mapper.Map<TutorialModel>(tutorialData);
                }
            }

            return View(tutorialModel);
        }
        
        /// <summary>
        /// Save Information
        /// </summary>
        /// <param name="tutorialModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageTextInput(TutorialModel tutorialModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    TUTORIAL tutorial;
                    if (IsDataDuplicate(tutorialModel.T_DESC, tutorialModel.TUTORIAL_ID))
                    {
                        TempData["ErrorMessage"] = "Data already exists in the database.";
                        return View(tutorialModel);
                    }

                    tutorial = Mapper.Map<TUTORIAL>(tutorialModel);
                    ctx.TUTORIALs.AddOrUpdate(tutorial);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Data saved successfully.";

                return RedirectToAction("ManageNormalTextBox");
            }
            else
            {
                return View(tutorialModel);
            }

        }

        /// <summary>
        /// Check Duplicate
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsDataDuplicate(string desc, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.TUTORIALs.FirstOrDefault(
                   c => c.TUTORIAL_ID != id && c.T_DESC.ToLower() == desc.ToLower())
               : ctx.TUTORIALs.FirstOrDefault(
                   c => c.T_DESC.ToLower() == desc.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region ManageCheckBox

        /// <summary>
        /// Empty Form
        /// </summary>
        /// <returns></returns>

        public ActionResult ManageCheckBox()
        {
            return View();
        }

        #endregion

        #region ManageDateSelect

        /// <summary>
        /// Empty Form
        /// </summary>
        /// <returns></returns>

        public ActionResult ManageDateSelect()
        {
            return View();
        }

        #endregion

        #region ManageDropdown

        /// <summary>
        /// Empty Form
        /// </summary>
        /// <returns></returns>

        public ActionResult ManageDropdown()
        {
            LicenseTypeModel licenseType = new LicenseTypeModel();
            List<TradingLicense.Model.LicenseTypeModel> licenseList = new List<TradingLicense.Model.LicenseTypeModel>();
            using (var ctx = new LicenseApplicationContext())
            {                                
                var license = ctx.LIC_TYPEs.ToList();                
                licenseType.licenseList = Mapper.Map<List<LicenseTypeModel>>(license);                
            }
            return View(licenseType);
        }

        #endregion

        #region ManageSelect2

        /// <summary>
        /// Empty Form
        /// </summary>
        /// <returns></returns>

        public ActionResult ManageSelect2()
        {
            LicenseTypeModel licenseType = new LicenseTypeModel();
            List<TradingLicense.Model.LicenseTypeModel> licenseList = new List<TradingLicense.Model.LicenseTypeModel>();
            using (var ctx = new LicenseApplicationContext())
            {
                var license = ctx.LIC_TYPEs.ToList();
                licenseType.licenseList = Mapper.Map<List<LicenseTypeModel>>(license);
            }
            return View(licenseType);
        }

        #endregion

        #region ManageMultipleSelect

        /// <summary>
        /// Empty Form
        /// </summary>
        /// <returns></returns>

        public ActionResult ManageMultipleSelect()
        {
            LicenseTypeModel licenseType = new LicenseTypeModel();
            List<TradingLicense.Model.LicenseTypeModel> licenseList = new List<TradingLicense.Model.LicenseTypeModel>();
            using (var ctx = new LicenseApplicationContext())
            {
                var license = ctx.LIC_TYPEs.ToList();
                licenseType.licenseList = Mapper.Map<List<LicenseTypeModel>>(license);
            }
            return View(licenseType);
        }

        #endregion

        #region FillLicense data
        /// <summary>
        /// Get License Types
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillLicense(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var license = ctx.LIC_TYPEs
                                    .Where(t => t.LIC_TYPEDESC.ToLower().Contains(query.ToLower()))
                                    .Select(fnSelectLicenseFormat).ToList();
                return Json(license, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}