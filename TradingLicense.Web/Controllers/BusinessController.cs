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

namespace TradingLicense.Web.Controllers
{
    public class BusinessController : BaseController
    {

        #region "Support Document"

        /// <summary>
        /// GET: Support Document
        /// </summary>
        /// <returns></returns>
        public ActionResult SupportDocument()
        {
            return View();
        }

        /// <summary>
        /// Save Support Document Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SupportDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessCode)
        {
            List<TradingLicense.Model.SupportDocsModel> supportDocs = new List<Model.SupportDocsModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<SupportDocs> query = ctx.SupportDocs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(businessCode))
                {
                    query = query.Where(p =>p.BusinessCode.CodeNumber.Contains(businessCode));
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

                var result= Mapper.Map<List<SupportDocsModel>>(query.ToList());

                result = result.OrderBy(orderByString == string.Empty ? "SupportDocsID asc" : orderByString).ToList();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();

                supportDocs = result;

            }
            return Json(new DataTablesResponse(requestModel.Draw, supportDocs, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Support Document Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageSupportDocument(int? Id)
        {
            SupportDocsModel supportDocsModel = new SupportDocsModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int SupportDocsID = Convert.ToInt32(Id);
                    var SupportDocs = ctx.SupportDocs.Where(a => a.SupportDocsID == SupportDocsID).FirstOrDefault();
                    supportDocsModel = Mapper.Map<SupportDocsModel>(SupportDocs);
                }
            }

            return View(supportDocsModel);
        }

        /// <summary>
        /// Save Support Document Infomration
        /// </summary>
        /// <param name="supportDocsModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageSupportDocument(SupportDocsModel supportDocsModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    SupportDocs supportDocs;
                    if (IsSupportDocsDuplicate(supportDocsModel.BusinessCodeID, supportDocsModel.SupportDocsID))
                    {
                        TempData["ErrorMessage"] = "Support Document is already exist in the database.";
                        return View(supportDocsModel);
                    }
                    supportDocs = Mapper.Map<SupportDocs>(supportDocsModel);
                    ctx.SupportDocs.AddOrUpdate(supportDocs);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Support Document saved successfully.";

                return RedirectToAction("SupportDocument");
            }
            else
            {
                return View(supportDocsModel);
            }
        }

        /// <summary>
        /// Delete Support Document
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSupportDocs(int id)
        {
            try
            {
                var supportDocs = new TradingLicense.Entities.SupportDocs() { SupportDocsID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(supportDocs).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsSupportDocsDuplicate(int businesscodeId, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.SupportDocs.FirstOrDefault(
                   c => c.SupportDocsID != id && c.BusinessCodeID == businesscodeId)
               : ctx.SupportDocs.FirstOrDefault(
                   c => c.BusinessCodeID == businesscodeId);
                return existObj != null;
            }
        }

        #endregion

        #region "Business Code Link Department"

        /// <summary>
        /// GET: Business Code Link Department
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessCodeLinkDepartment()
        {
            return View();
        }

        /// <summary>
        /// Save Business Code Link Department Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessCodeLinkDepartment([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessCode,string departmentCode)
        {
            List<TradingLicense.Model.BussCodLinkDepModel> bussCodLinkDepModel = new List<Model.BussCodLinkDepModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BussCodLinkDep> query = ctx.BussCodLinkDeps;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(businessCode) || !string.IsNullOrWhiteSpace(departmentCode))
                {
                    query = query.Where(p => p.BusinessCode.CodeNumber.Contains(businessCode) && p.Department.DepartmentCode.Contains(departmentCode));
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

                var result = Mapper.Map<List<BussCodLinkDepModel>>(query.ToList());

                result = result.OrderBy(orderByString == string.Empty ? "BussCodLinkDepID asc" : orderByString).ToList();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();

                bussCodLinkDepModel = result;

            }
            return Json(new DataTablesResponse(requestModel.Draw, bussCodLinkDepModel, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Business Code Link Department Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageBussCodLinkDep(int? Id)
        {
            BussCodLinkDepModel bussCodLinkDepModel = new BussCodLinkDepModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int bussCodLinkDepID = Convert.ToInt32(Id);
                    var bussCodLinkDeps = ctx.BussCodLinkDeps.Where(a => a.BussCodLinkDepID == bussCodLinkDepID).FirstOrDefault();
                    bussCodLinkDepModel = Mapper.Map<BussCodLinkDepModel>(bussCodLinkDeps);
                }
            }

            return View(bussCodLinkDepModel);
        }

        /// <summary>
        /// Save Business Code Link Department Infomration
        /// </summary>
        /// <param name="bussCodLinkDepModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageBussCodLinkDep(BussCodLinkDepModel bussCodLinkDepModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    BussCodLinkDep bussCodLinkDep;

                    if (IsBussCodLinkDepDuplicate(bussCodLinkDepModel.BusinessCodeID,bussCodLinkDepModel.DepartmentID,bussCodLinkDepModel.BussCodLinkDepID))
                    {
                        TempData["ErrorMessage"] = "Business Code Link Department is already exist in the database.";
                        return View(bussCodLinkDepModel);
                    }

                    bussCodLinkDep = Mapper.Map<BussCodLinkDep>(bussCodLinkDepModel);
                    ctx.BussCodLinkDeps.AddOrUpdate(bussCodLinkDep);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Business Code Link Department saved successfully.";

                return RedirectToAction("BusinessCodeLinkDepartment");
            }
            else
            {
                return View(bussCodLinkDepModel);
            }
        }

        /// <summary>
        /// Delete Business Code Link Department
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBussCodLinkDep(int id)
        {
            try
            {
                var bussCodLinkDep = new TradingLicense.Entities.BussCodLinkDep() { BussCodLinkDepID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(bussCodLinkDep).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Error While Delete Record" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Check Duplicate
        /// </summary>
        /// <param name="businesscodeId"></param>
        /// <param name="departmentID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsBussCodLinkDepDuplicate(int businesscodeId,int departmentID, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.BussCodLinkDeps.FirstOrDefault(
                   c => c.BussCodLinkDepID != id && c.BusinessCodeID == businesscodeId && c.DepartmentID== departmentID)
               : ctx.BussCodLinkDeps.FirstOrDefault(
                   c => c.BusinessCodeID == businesscodeId && c.DepartmentID == departmentID);
                return existObj != null;
            }
        }

        #endregion
    }
}