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
    public class StallApplicationController : BaseController
    {
        #region StallCode

        /// <summary>
        /// GET: StallCode
        /// </summary>
        /// <returns></returns>
        public ActionResult StallCode()
        {
            return View();
        }

        /// <summary>
        /// Save Stall Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StallCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string stallCodeDesc)
        {
            List<TradingLicense.Model.StallCodeModel> stallCode = new List<Model.StallCodeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<StallCode> query = ctx.StallCodes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(stallCodeDesc))
                {
                    query = query.Where(p =>
                                        p.StallCodeDesc.Contains(stallCodeDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "StallCodeID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                stallCode = Mapper.Map<List<StallCodeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, stallCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get StallCode Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageStallCode(int? Id)
        {
            StallCodeModel stallCodeModel = new StallCodeModel();
            stallCodeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int stallCodeID = Convert.ToInt32(Id);
                    var stallCode = ctx.StallCodes.Where(a => a.StallCodeID == stallCodeID).FirstOrDefault();
                    stallCodeModel = Mapper.Map<StallCodeModel>(stallCode);
                }
            }

            return View(stallCodeModel);
        }

        /// <summary>
        /// Save Stall Code Infomration
        /// </summary>
        /// <param name="stallCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageStallCode(StallCodeModel stallCodeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    StallCode stallCode;
                    if (IsStallCodeDuplicate(stallCodeModel.StallCodeDesc, stallCodeModel.StallCodeID))
                    {
                        TempData["ErrorMessage"] = "Stall Code already exists in the database.";
                        return View(stallCodeModel);
                    }

                    stallCode = Mapper.Map<StallCode>(stallCodeModel);
                    ctx.StallCodes.AddOrUpdate(stallCode);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Stall Code saved successfully.";

                return RedirectToAction("StallCode");
            }
            else
            {
                return View(stallCodeModel);
            }

        }

        /// <summary>
        /// Delete Stall Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteStallCode(int id)
        {
            try
            {
                var stallCode = new TradingLicense.Entities.StallCode() { StallCodeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(stallCode).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsStallCodeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.StallCodes.FirstOrDefault(
                   c => c.StallCodeID != id && c.StallCodeDesc.ToLower() == name.ToLower())
               : ctx.StallCodes.FirstOrDefault(
                   c => c.StallCodeDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region StallApplication

        /// <summary>
        /// GET: StallApplication
        /// </summary>
        /// <returns></returns>
        public ActionResult StallApplication()
        {
            return View();
        }

        /// <summary>
        /// Save Stall Application Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StallApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string stallApplicationID)
        {
            List<TradingLicense.Model.StallApplicationModel> stallApplication = new List<Model.StallApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<StallApplication> query = ctx.StallApplications;
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

                query = query.OrderBy(orderByString == string.Empty ? "StallApplicationID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                stallApplication = Mapper.Map<List<StallApplicationModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, stallApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get StallApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageStallApplication(int? Id)
        {
            List<TradingLicense.Model.SAReqDocModel> SAReqDoc = new List<Model.SAReqDocModel>();
            StallApplicationModel stallApplicationModel = new StallApplicationModel();
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<SAReqDoc> query = ctx.SAReqDocs;
                SAReqDoc = Mapper.Map<List<SAReqDocModel>>(query.ToList());
                ViewBag.stallDocList = ctx.SAReqDocs.ToList();
                if (Id != null && Id > 0)
                {

                    int stallApplicationID = Convert.ToInt32(Id);
                    var stallApplication = ctx.StallApplications.Where(a => a.StallApplicationID == stallApplicationID).FirstOrDefault();
                    stallApplicationModel = Mapper.Map<StallApplicationModel>(stallApplication);
                }

            }

            return View(stallApplicationModel);
        }

        /// <summary>
        /// Save Stall Application Infomration
        /// </summary>
        /// <param name="stallApplicationModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageStallApplication(StallApplicationModel stallApplicationModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    StallApplication stallApplication;


                    stallApplication = Mapper.Map<StallApplication>(stallApplicationModel);
                    ctx.StallApplications.AddOrUpdate(stallApplication);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Stall Application saved successfully.";

                return RedirectToAction("StallApplication");
            }
            else
            {
                return View(stallApplicationModel);
            }

        }

        /// <summary>
        /// Delete Stall Application Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteStallApplication(int id)
        {
            try
            {
                var stallApplication = new TradingLicense.Entities.StallApplication() { StallApplicationID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(stallApplication).State = System.Data.Entity.EntityState.Deleted;
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

        #region SAReqDoc

        /// <summary>
        /// GET: SAReqDoc
        /// </summary>
        /// <returns></returns>
        public ActionResult SAReqDoc()
        {
            return View();
        }

        /// <summary>
        /// Save Stall Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SAReqDoc([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string SAReqDocDesc)
        {
            List<TradingLicense.Model.SAReqDocModel> SAReqDoc = new List<Model.SAReqDocModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<SAReqDoc> query = ctx.SAReqDocs;
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

                query = query.OrderBy(orderByString == string.Empty ? "SAReqDocID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                SAReqDoc = Mapper.Map<List<SAReqDocModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, SAReqDoc, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get SAReqDoc Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageSAReqDoc(int? Id)
        {
            SAReqDocModel SAReqDocModel = new SAReqDocModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int SAReqDocID = Convert.ToInt32(Id);
                    var SAReqDoc = ctx.SAReqDocs.Where(a => a.SAReqDocID == SAReqDocID).FirstOrDefault();
                    SAReqDocModel = Mapper.Map<SAReqDocModel>(SAReqDoc);
                }
            }

            return View(SAReqDocModel);
        }

        [HttpPost]
        public JsonResult SaveSAReqDoc(List<SAReqDoc> lstBarReqDoc)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    foreach (var item in lstBarReqDoc)
                    {
                        var DocCnt = ctx.SAReqDocs.Where(x => x.RequiredDocID == item.RequiredDocID).Count();
                        if (DocCnt == 0)
                        {
                            SAReqDoc SAReqDoc = new SAReqDoc();
                            SAReqDoc.SAReqDocID = 0;
                            SAReqDoc.RequiredDocID = item.RequiredDocID;
                            ctx.SAReqDocs.AddOrUpdate(SAReqDoc);
                            ctx.SaveChanges();
                        }
                    }
                }

                TempData["SuccessMessage"] = "Banner Required Documents successfully.";
                return Json(Convert.ToString(1));
            }
            catch (Exception)
            {
                return Json(Convert.ToString(0));
            }
        }

        /// <summary>
        /// Delete Stall Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSAReqDoc(int id)
        {
            try
            {
                var SAReqDoc = new TradingLicense.Entities.SAReqDoc() { SAReqDocID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(SAReqDoc).State = System.Data.Entity.EntityState.Deleted;
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