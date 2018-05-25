using DataTables.Mvc;
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
    public class LiquorApplicationController : BaseController
    {
        #region LiquorCode

        /// <summary>
        /// GET: LiquorCode
        /// </summary>
        /// <returns></returns>
        public ActionResult LiquorCode()
        {
            return View();
        }

        /// <summary>
        /// Save Liquor Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LiquorCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string liquorCodeDesc)
        {
            List<TradingLicense.Model.LiquorCodeModel> liquorCode = new List<Model.LiquorCodeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<LiquorCode> query = ctx.LiquorCodes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(liquorCodeDesc))
                {
                    query = query.Where(p =>
                                        p.LiquorCodeDesc.Contains(liquorCodeDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "LiquorCodeID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                liquorCode = Mapper.Map<List<LiquorCodeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, liquorCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get LiquorCode Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageLiquorCode(int? Id)
        {
            LiquorCodeModel liquorCodeModel = new LiquorCodeModel();
            liquorCodeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int liquorCodeID = Convert.ToInt32(Id);
                    var liquorCode = ctx.LiquorCodes.Where(a => a.LiquorCodeID == liquorCodeID).FirstOrDefault();
                    liquorCodeModel = Mapper.Map<LiquorCodeModel>(liquorCode);
                }
            }

            return View(liquorCodeModel);
        }

        /// <summary>
        /// Save Liquor Code Infomration
        /// </summary>
        /// <param name="liquorCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageLiquorCode(LiquorCodeModel liquorCodeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    LiquorCode liquorCode;
                    if (IsLiquorCodeDuplicate(liquorCodeModel.LiquorCodeDesc, liquorCodeModel.LiquorCodeID))
                    {
                        TempData["ErrorMessage"] = "Liquor Code already exists in the database.";
                        return View(liquorCodeModel);
                    }

                    liquorCode = Mapper.Map<LiquorCode>(liquorCodeModel);
                    ctx.LiquorCodes.AddOrUpdate(liquorCode);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Liquor Code saved successfully.";

                return RedirectToAction("LiquorCode");
            }
            else
            {
                return View(liquorCodeModel);
            }

        }

        /// <summary>
        /// Delete Liquor Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteLiquorCode(int id)
        {
            try
            {
                var liquorCode = new TradingLicense.Entities.LiquorCode() { LiquorCodeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(liquorCode).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsLiquorCodeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.LiquorCodes.FirstOrDefault(
                   c => c.LiquorCodeID != id && c.LiquorCodeDesc.ToLower() == name.ToLower())
               : ctx.LiquorCodes.FirstOrDefault(
                   c => c.LiquorCodeDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion
    }
}