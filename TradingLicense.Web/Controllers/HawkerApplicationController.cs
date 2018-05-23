﻿using DataTables.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using TradingLicense.Data;
using TradingLicense.Entities;
using System.Linq.Dynamic;
using TradingLicense.Model;
using AutoMapper;
using TradingLicense.Web.Classes;

namespace TradingLicense.Web.Controllers
{
    public class HawkerApplicationController : BaseController
    {
        #region HawkerCode

        /// <summary>
        /// GET: HawkerCode
        /// </summary>
        /// <returns></returns>
        public ActionResult HawkerCode()
        {
            return View();
        }

        /// <summary>
        /// Save Hawker Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HawkerCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string hawkerCodeDesc)
        {
            List<TradingLicense.Model.HawkerCodeModel> hawkerCode = new List<Model.HawkerCodeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<HawkerCode> query = ctx.HawkerCodes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(hawkerCodeDesc))
                {
                    query = query.Where(p =>
                                        p.HawkerCodeDesc.Contains(hawkerCodeDesc)
                                    );
                }

                filteredRecord = query.Count();

                #endregion Filtering

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = String.Empty;

                foreach (var column in sortedColumns)
                {
                    orderByString += orderByString != String.Empty ? "," : "";
                    orderByString += (column.Data) +
                      (column.SortDirection ==
                      Column.OrderDirection.Ascendant ? " asc" : " desc");
                }

                query = query.OrderBy(orderByString == string.Empty ? "HawkerCodeID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                hawkerCode = Mapper.Map<List<HawkerCodeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, hawkerCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get HawkerCode Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageHawkerCode(int? Id)
        {
            HawkerCodeModel hawkerCodeModel = new HawkerCodeModel();
            hawkerCodeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int hawkerCodeID = Convert.ToInt32(Id);
                    var hawkerCode = ctx.HawkerCodes.Where(a => a.HawkerCodeID == hawkerCodeID).FirstOrDefault();
                    hawkerCodeModel = Mapper.Map<HawkerCodeModel>(hawkerCode);
                }
            }

            return View(hawkerCodeModel);
        }

        /// <summary>
        /// Save Hawker Code Infomration
        /// </summary>
        /// <param name="hawkerCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageHawkerCode(HawkerCodeModel hawkerCodeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    HawkerCode hawkerCode;
                    if (IsHawkerCodeDuplicate(hawkerCodeModel.HawkerCodeDesc, hawkerCodeModel.HawkerCodeID))
                    {
                        TempData["ErrorMessage"] = "Hawker Code already exists in the database.";
                        return View(hawkerCodeModel);
                    }

                    hawkerCode = Mapper.Map<HawkerCode>(hawkerCodeModel);
                    ctx.HawkerCodes.AddOrUpdate(hawkerCode);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Hawker Code saved successfully.";

                return RedirectToAction("HawkerCode");
            }
            else
            {
                return View(hawkerCodeModel);
            }

        }

        /// <summary>
        /// Delete Hawker Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteHawkerCode(int id)
        {
            try
            {
                var hawkerCode = new TradingLicense.Entities.HawkerCode() { HawkerCodeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(hawkerCode).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
                return Json(new { success = true, message = " Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Error While Delete Record" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Check Duplicate
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsHawkerCodeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.HawkerCodes.FirstOrDefault(
                   c => c.HawkerCodeID != id && c.HawkerCodeDesc.ToLower() == name.ToLower())
               : ctx.HawkerCodes.FirstOrDefault(
                   c => c.HawkerCodeDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion
    }
}