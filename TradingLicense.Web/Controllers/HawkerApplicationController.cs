using DataTables.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TradingLicense.Data;
using TradingLicense.Entities;
using System.Linq.Dynamic;
using TradingLicense.Model;
using AutoMapper;
using TradingLicense.Web.Classes;
using TradingLicense.Infrastructure;
using static TradingLicense.Infrastructure.Enums;

namespace TradingLicense.Web.Controllers
{
    public class HawkerApplicationController : BaseController
    {
        #region HawkerType

        /// <summary>
        /// GET: HawkerType
        /// </summary>
        /// <returns></returns>

        public ActionResult HawkerType()
        {
            return View();
        }

        /// <summary>
        /// Save HawkerType Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HawkerType([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string hawkerTypeDesc)
        {
            List<TradingLicense.Model.HawkerTypeModel> HawkerType = new List<Model.HawkerTypeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<HawkerType> query = ctx.HawkerTypes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                query = query.Where(p =>p.HawkerTypeDesc.ToString().Contains(hawkerTypeDesc));

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

                query = query.OrderBy(orderByString == string.Empty ? "HawkerTypeID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                HawkerType = Mapper.Map<List<HawkerTypeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, HawkerType, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get HawkerType Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageHawkerType(int? Id)
        {
            HawkerTypeModel hawkerTypeModel = new HawkerTypeModel();
            hawkerTypeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int HawkerTypeID = Convert.ToInt32(Id);
                    var hawkerType = ctx.HawkerTypes.Where(a => a.HawkerTypeID == HawkerTypeID).FirstOrDefault();
                    hawkerTypeModel = Mapper.Map<HawkerTypeModel>(hawkerType);
                }
            }

            return View(hawkerTypeModel);
        }

        /// <summary>
        /// Save HawkerType Infomration
        /// </summary>
        /// <param name="hawkerTypeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageHawkerType(HawkerTypeModel hawkerTypeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    HawkerType hawkerType;
                    if (IsHawkerTypeDuplicate(hawkerTypeModel.HawkerTypeDesc, hawkerTypeModel.HawkerTypeID))
                    {
                        TempData["ErrorMessage"] = "Hawker Type already exists in the database.";
                        return View(hawkerTypeModel);
                    }
                    hawkerType = Mapper.Map<HawkerType>(hawkerTypeModel);
                    ctx.HawkerTypes.AddOrUpdate(hawkerType);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "HawkerType saved successfully.";

                return RedirectToAction("HawkerType");
            }
            else
            {
                return View(hawkerTypeModel);
            }

        }

        /// <summary>
        /// Delete HawkerType Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteHawkerType(int id)
        {
            try
            {
                var HawkerType = new TradingLicense.Entities.HawkerType() { HawkerTypeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(HawkerType).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsHawkerTypeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.HawkerTypes.FirstOrDefault(
                   c => c.HawkerTypeID != id && c.HawkerTypeDesc.ToLower() == name.ToLower())
               : ctx.HawkerTypes.FirstOrDefault(
                   c => c.HawkerTypeDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion
    }
}