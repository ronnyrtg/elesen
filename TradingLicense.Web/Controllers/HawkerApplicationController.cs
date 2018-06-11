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

        #region HawkerApplication

        /// <summary>
        /// GET: HawkerApplication
        /// </summary>
        /// <returns></returns>
        public ActionResult HawkerApplication()
        {
            return View();
        }

        /// <summary>
        /// Save Hawker Application Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HawkerApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string HawkerApplicationID)
        {
            List<TradingLicense.Model.HawkerApplicationModel> HawkerApplication = new List<Model.HawkerApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<HawkerApplication> query = ctx.HawkerApplications;
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

                query = query.OrderBy(orderByString == string.Empty ? "HawkerApplicationID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                HawkerApplication = Mapper.Map<List<HawkerApplicationModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, HawkerApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get HawkerApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageHawkerApplication(int? Id)
        {
            List<TradingLicense.Model.HAReqDocModel> HAReqDoc = new List<Model.HAReqDocModel>();
            HawkerApplicationModel HawkerApplicationModel = new HawkerApplicationModel();
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<HAReqDoc> query = ctx.HAReqDocs;
                HAReqDoc = Mapper.Map<List<HAReqDocModel>>(query.ToList());
                ViewBag.hawkerDocList = ctx.HAReqDocs.ToList();
                if (Id != null && Id > 0)
                {

                    int HawkerApplicationID = Convert.ToInt32(Id);
                    var HawkerApplication = ctx.HawkerApplications.Where(a => a.HawkerApplicationID == HawkerApplicationID).FirstOrDefault();
                    HawkerApplicationModel = Mapper.Map<HawkerApplicationModel>(HawkerApplication);
                }

            }

            return View(HawkerApplicationModel);
        }

        /// <summary>
        /// Save Hawker Application Infomration
        /// </summary>
        /// <param name="HawkerApplicationModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageHawkerApplication(HawkerApplicationModel HawkerApplicationModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    HawkerApplication HawkerApplication;


                    HawkerApplication = Mapper.Map<HawkerApplication>(HawkerApplicationModel);
                    ctx.HawkerApplications.AddOrUpdate(HawkerApplication);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Hawker Application saved successfully.";

                return RedirectToAction("HawkerApplication");
            }
            else
            {
                return View(HawkerApplicationModel);
            }

        }

        /// <summary>
        /// Delete Hawker Application Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteHawkerApplication(int id)
        {
            try
            {
                var HawkerApplication = new TradingLicense.Entities.HawkerApplication() { HawkerApplicationID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(HawkerApplication).State = System.Data.Entity.EntityState.Deleted;
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

        #region HAReqDoc

        /// <summary>
        /// GET: HAReqDoc
        /// </summary>
        /// <returns></returns>
        public ActionResult HAReqDoc()
        {
            return View();
        }

        /// <summary>
        /// Save Hawker Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HAReqDoc([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string HAReqDocDesc)
        {
            List<TradingLicense.Model.HAReqDocModel> HAReqDoc = new List<Model.HAReqDocModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<HAReqDoc> query = ctx.HAReqDocs;
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

                query = query.OrderBy(orderByString == string.Empty ? "HAReqDocID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                HAReqDoc = Mapper.Map<List<HAReqDocModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, HAReqDoc, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get HAReqDoc Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageHAReqDoc(int? Id)
        {
            HAReqDocModel HAReqDocModel = new HAReqDocModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int HAReqDocID = Convert.ToInt32(Id);
                    var HAReqDoc = ctx.HAReqDocs.Where(a => a.HAReqDocID == HAReqDocID).FirstOrDefault();
                    HAReqDocModel = Mapper.Map<HAReqDocModel>(HAReqDoc);
                }
            }

            return View(HAReqDocModel);
        }

        [HttpPost]
        public JsonResult SaveHAReqDoc(List<HAReqDoc> lstBarReqDoc)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    foreach (var item in lstBarReqDoc)
                    {
                        var DocCnt = ctx.HAReqDocs.Where(x => x.RequiredDocID == item.RequiredDocID).Count();
                        if (DocCnt == 0)
                        {
                            HAReqDoc HAReqDoc = new HAReqDoc();
                            HAReqDoc.HAReqDocID = 0;
                            HAReqDoc.RequiredDocID = item.RequiredDocID;
                            ctx.HAReqDocs.AddOrUpdate(HAReqDoc);
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
        /// Delete Hawker Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteHAReqDoc(int id)
        {
            try
            {
                var HAReqDoc = new TradingLicense.Entities.HAReqDoc() { HAReqDocID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(HAReqDoc).State = System.Data.Entity.EntityState.Deleted;
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