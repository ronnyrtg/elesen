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

namespace TradingLicense.Web.Controllers
{
    public class PremiseApplicationController : Controller
    {
        #region PremiseApplication

        /// <summary>
        /// GET: PremiseApplication
        /// </summary>
        /// <returns></returns>

        public ActionResult PremiseApplication()
        {
            return View();
        }

        /// <summary>
        /// Save PremiseApplication Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PremiseApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int premiseApplicationID, int individualID)
        {
            List<TradingLicense.Model.PremiseApplicationModel> PremiseApplication = new List<Model.PremiseApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<PremiseApplication> query = ctx.PremiseApplications;
                totalRecord = query.Count();

                

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

                query = query.OrderBy(orderByString == string.Empty ? "PremiseApplicationID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                PremiseApplication = Mapper.Map<List<PremiseApplicationModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, PremiseApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get PremiseApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManagePremiseApplication(int? Id)
        {
            PremiseApplicationModel premiseApplicationModel = new PremiseApplicationModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int PremiseApplicationID = Convert.ToInt32(Id);
                    var premiseApplication = ctx.PremiseApplications.Where(a => a.PremiseApplicationID == PremiseApplicationID).FirstOrDefault();
                    premiseApplicationModel = Mapper.Map<PremiseApplicationModel>(premiseApplication);
                }
            }

            return View(premiseApplicationModel);
        }

        /// <summary>
        /// Save PremiseApplication Information
        /// </summary>
        /// <param name="premiseApplicationModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManagePremiseApplication(PremiseApplicationModel premiseApplicationModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    PremiseApplication premiseApplication;
                    premiseApplication = Mapper.Map<PremiseApplication>(premiseApplicationModel);
                    ctx.PremiseApplications.AddOrUpdate(premiseApplication);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Premise License Application saved successfully.";

                return RedirectToAction("PremiseApplication");
            }
            else
            {
                return View(premiseApplicationModel);
            }

        }

        /// <summary>
        /// Delete PremiseApplication Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePremiseApplication(int id)
        {
            try
            {
                var PremiseApplication = new TradingLicense.Entities.PremiseApplication() { PremiseApplicationID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(PremiseApplication).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
                return Json(new { success = true, message = " Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Error While Delete Record" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}