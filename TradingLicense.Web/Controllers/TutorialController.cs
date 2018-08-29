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
        #region NormalTextBox

        /// <summary>
        /// Get PremiseType Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult NormalTextBox(int? Id)
        {
            PremiseTypeModel premiseTypeModel = new PremiseTypeModel();
            premiseTypeModel.ACTIVE = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int premiseTypeID = Convert.ToInt32(Id);
                    var premiseType = ctx.PREMISETYPEs.Where(a => a.PT_ID == premiseTypeID).FirstOrDefault();
                    premiseTypeModel = Mapper.Map<PremiseTypeModel>(premiseType);
                }
            }

            return View(premiseTypeModel);
        }
        
        /// <summary>
        /// Save Premise Type Infomration
        /// </summary>
        /// <param name="premiseTypeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult NormalTextBox(PremiseTypeModel premiseTypeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    PREMISETYPE premiseType;
                    if (IsPremiseTypeDuplicate(premiseTypeModel.PT_DESC, premiseTypeModel.PT_ID))
                    {
                        TempData["ErrorMessage"] = "Premise Type is already exist in the database.";
                        return View(premiseTypeModel);
                    }

                    premiseType = Mapper.Map<PREMISETYPE>(premiseTypeModel);
                    ctx.PREMISETYPEs.AddOrUpdate(premiseType);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Premise Type saved successfully.";

                return RedirectToAction("PremiseType");
            }
            else
            {
                return View(premiseTypeModel);
            }

        }

        /// <summary>
        /// Check Duplicate
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsPremiseTypeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.PREMISETYPEs.FirstOrDefault(
                   c => c.PT_ID != id && c.PT_DESC.ToLower() == name.ToLower())
               : ctx.PREMISETYPEs.FirstOrDefault(
                   c => c.PT_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion
    }
}