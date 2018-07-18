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
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using TradingLicense.Web.Helpers;
using TradingLicense.Web.Services;
using PdfSharp.Drawing.Layout;

namespace TradingLicense.Web.Controllers
{
    public class ApplicationController : BaseController
    {

        public const string OnSubmit = "Submitted";
        public const string OnRouteSubmit = "SubmittedToRoute";
        public const string OnRejected = "Rejected";
        public const string OnKIV = "KIV";

        private Func<BC, Select2ListItem> fnSelectBusinessCode = bc => new Select2ListItem { id = bc.BC_ID, text = $"{bc.C_R_DESC}~{bc.C_R}" };
        private Func<Individual, Select2ListItem> fnSelectIndividualFormat = ind => new Select2ListItem { id = ind.IndividualID, text = $"{ind.FullName} ({ind.MykadNo})" };

        /// <summary>
        /// GET: Application
        /// </summary>
        /// <returns></returns>
        [AuthorizationPrivilegeFilter(SystemEnum.Page.Application, SystemEnum.PageRight.CrudLevel)]
        public ActionResult Application()
        {
            return View();
        }

        #region Application List Grid
        /// <summary>
        /// Get Application Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="ApplicationId">The premise application identifier.</param>
        /// <param name="individualMkNo">The individual mk no.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Application([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string ApplicationId, string individualMkNo)
        {
            List<ApplicationModel> Application;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                int? rollTemplateID = ProjectSession.User?.RoleTemplateID;
                IQueryable<APPLICATION> query = ctx.Applications;

                if (rollTemplateID.HasValue)
                {
                    switch (rollTemplateID)
                    {
                        case (int)RollTemplate.Public:
                            {
                                query = query.Where(q => q.USERSID == ProjectSession.UserID);
                            }
                            break;
                        case (int)RollTemplate.DeskOfficer:
                            {
                                query = query.Where(q => q.APPSTATUSID <= 3);
                            }
                            break;
                        case (int)RollTemplate.RouteUnit:
                            if (string.IsNullOrEmpty(ApplicationId))
                            {
                                var departmentID = ProjectSession.User?.DepartmentID;
                                if (departmentID.HasValue)
                                {
                                    var paIDs = ctx.RouteUnits
                                            .Where(pa => pa.DepartmentID == departmentID.Value && pa.ApplicationType == (int)Enums.ApplicationTypeID.Application && pa.Active)
                                            .Select(d => d.ApplicationID).Distinct()
                                            .ToList();
                                    query = query.Where(q => paIDs.Contains(q.APP_ID) && q.APPSTATUSID == 5);
                                }
                            }
                            break;
                        case (int)RollTemplate.CEO:
                            {
                                query = query.Where(q => q.APPSTATUSID == 8);
                            }
                            break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(ApplicationId))
                {
                    query = query.Where(q => q.APP_ID.ToString().Contains(ApplicationId));
                }

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<ApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "APP_ID desc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                Application = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, Application, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ManageApplication
        /// <summary>
        /// Get Application Data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ManageApplication(int? id)
        {
            ApplicationModel applicationModel = new ApplicationModel();
            applicationModel.START_RENT = DateTime.Today.AddMonths(-6);
            applicationModel.STOP_RENT = DateTime.Today.AddMonths(6);
            if (id != null && id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var application = ctx.Applications.FirstOrDefault(a => a.APP_ID == id);
                    applicationModel = Mapper.Map<ApplicationModel>(application);

                    var paLinkBc = ctx.PALinkBC.Where(a => a.PremiseApplicationID == id).ToList();
                    applicationModel.BusinessCodeids = string.Join(",", paLinkBc.Select(x => x.BusinessCodeID.ToString()).ToArray());

                    //TODO: replaced with this for avoid calling database in foreach loop
                    // TODO: Select2ListItem is just the same as build-in KeyValuePair class
                    List<Select2ListItem> businessCodesList = new List<Select2ListItem>();
                    var ids = paLinkBc.Select(b => b.BusinessCodeID).ToList();
                    businessCodesList = ctx.BCs
                        .Where(b => ids.Contains(b.BC_ID))
                        .Select(fnSelectBusinessCode)
                        .ToList();

                    applicationModel.selectedbusinessCodeList = businessCodesList;
                    applicationModel.HasPADepSupp = ctx.RouteUnits.Any(pa => pa.ApplicationID == id.Value);

                    var paLinkInd = ctx.PALinkInd.Where(a => a.PremiseApplicationID == id).ToList();
                    applicationModel.Individualids = string.Join(",", paLinkInd.Select(x => x.IndividualID.ToString()).ToArray());
                    List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
                    var iids = paLinkInd.Select(b => b.IndividualID).ToList();
                    selectedIndividualList = ctx.Individuals
                        .Where(b => iids.Contains(b.IndividualID))
                        .Select(fnSelectIndividualFormat)
                        .ToList();

                    applicationModel.selectedIndividualList = selectedIndividualList;

                    var paLinkReqDocumentList = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == id).ToList();
                    if (paLinkReqDocumentList.Count > 0)
                    {
                        applicationModel.UploadRequiredDocids = (string.Join(",", paLinkReqDocumentList.Select(x => x.RequiredDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }

                    var paLinkAddDocumentlist = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == id).ToList();
                    if (paLinkAddDocumentlist.Count > 0)
                    {
                        applicationModel.UploadAdditionalDocids = (string.Join(",", paLinkAddDocumentlist.Select(x => x.AdditionalDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }

                    if (application.APPSTATUSID == (int)PAStausenum.Pendingpayment)
                    {
                        var duePayment = ctx.PaymentDues.Where(pd => pd.PaymentFor == applicationModel.REF_NO).FirstOrDefault();
                        if (duePayment != null)
                        {
                            applicationModel.AmountDue = duePayment.AmountDue;
                        }
                    }
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                applicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
                applicationModel.USERSID = ProjectSession.User.UsersID;
            }


            applicationModel.IsDraft = false;
            return View(applicationModel);
        }
        #endregion

        #region Required Doc List Datatable
        /// <summary>
        /// Get Required Document Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="businessTypeId">The business type identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RequiredDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessTypeId)
        {
            List<BT_L_RDModel> requiredDocument;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BT_L_RD> query = ctx.BT_L_RD.Where(p => p.BT_ID.ToString().Contains(businessTypeId));

                #region Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                string orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<BT_L_RDModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BT_L_RDID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                requiredDocument = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, requiredDocument, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Business Code data for Datatable
        /// <summary>
        /// Get Business Code
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="selectedMode">The selected mode.</param>
        /// <param name="selectedSector">The selected sector.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillBusinessCode(string query, int selectedLic, int? selectedSector)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BC> primaryQuery = ctx.BCs;

                if (selectedLic == 1)
                {
                    if (selectedSector > 0) {
                        primaryQuery = primaryQuery.Where(bc => bc.LIC_TYPEID == selectedLic && bc.SECTORID == selectedSector);
                    }
                    else
                    {
                        primaryQuery = primaryQuery.Where(bc => bc.LIC_TYPEID == selectedLic);
                    }
                }
                else
                {
                    primaryQuery = primaryQuery.Where(bc => bc.LIC_TYPEID == selectedLic);
                }

                if (!String.IsNullOrWhiteSpace(query))
                {
                    primaryQuery = primaryQuery.Where(bc => bc.C_R_DESC.ToLower().Contains(query.ToLower()) || bc.C_R_DESC.ToLower().Contains(query.ToLower()));
                }
                var businessCode = primaryQuery.Select(fnSelectBusinessCode).ToList();
                return Json(businessCode, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Get Individual Name (MyKad) for Datatable
        /// <summary>
        /// Get Individual Code
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillIndividual(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var individual = ctx.Individuals
                                    .Where(t => t.MykadNo.ToLower().Contains(query.ToLower()) || t.FullName.ToLower().Contains(query.ToLower()))
                                    .Select(fnSelectIndividualFormat).ToList();
                return Json(individual, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Get Additional Doc data for Datatable
        /// <summary>
        /// get Additional Document Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="businessCodeids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AdditionalDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessCodeids, string premiseApplicationId)
        {
            List<BCLinkADModel> requiredDocument = new List<BCLinkADModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                string[] ids = null;

                if (!string.IsNullOrWhiteSpace(businessCodeids))
                {
                    ids = businessCodeids.Split(',');
                }

                List<int> businessCodelist = new List<int>();

                if (ids != null)
                {
                    foreach (string id in ids)
                    {
                        int businessCodeId = Convert.ToInt32(id);
                        businessCodelist.Add(businessCodeId);
                    }
                }

                IQueryable<BCLinkAD> query = ctx.BCLinkAD.Where(p => businessCodelist.Contains(p.BusinessCodeID));

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<BCLinkADModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BCLinkADID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                requiredDocument = result;

                #region IsChecked

                if (!string.IsNullOrWhiteSpace(premiseApplicationId))
                {
                    int premiseAppId;
                    int.TryParse(premiseApplicationId, out premiseAppId);

                    var palinkAdd = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == premiseAppId).ToList();
                    foreach (var item in requiredDocument)
                    {
                        if (palinkAdd.Count > 0)
                        {
                            var resultpalinkReq = palinkAdd.FirstOrDefault(p => p.AdditionalDocID == item.AdditionalDocID && p.PremiseApplicationID == premiseAppId);
                            if (resultpalinkReq != null)
                            {
                                item.IsChecked = "checked";
                                var attechmentdetails = ctx.Attachments.FirstOrDefault(a => a.AttachmentID == resultpalinkReq.AttachmentID);
                                if (attechmentdetails != null)
                                {
                                    item.AttachmentFileName = attechmentdetails.FileName;
                                    item.AttachmentId = attechmentdetails.AttachmentID;
                                    item.PremiseApplicationID = premiseAppId;
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            return Json(new DataTablesResponse(requestModel.Draw, requiredDocument, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BC

        /// <summary>
        /// GET: BusinessCode
        /// </summary>
        /// <returns></returns>
        public ActionResult BC()
        {
            return View();
        }

        /// <summary>
        /// Save BusinessCode Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="codeNumber">The code number.</param>
        /// <param name="codeDesc">The code desc.</param>
        /// <param name="sectorId">The sector identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BC([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string codeNumber, string codeDesc, string sectorId)
        {
            List<BCModel> businessCode;
            int totalRecord = 0;
            // int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BC> query = ctx.BCs;

                #region Filtering

                // Apply filters for comman Grid searching
                if (requestModel.Search.Value != string.Empty)
                {
                    var value = requestModel.Search.Value.ToLower().Trim();
                    query = query.Where(p => p.C_R_DESC.ToLower().Contains(value) ||
                                             p.SECTORID.ToString().Contains(value) ||
                                             p.DEF_RATE.ToString().Contains(value) ||
                                             p.Sector.SectorDesc.ToLower().Contains(value)
                                       );
                }

                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(codeDesc))
                {
                    query = query.Where(p => p.C_R_DESC.ToLower().Contains(codeDesc.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(sectorId))
                {
                    query = query.Where(p => p.SECTORID.ToString().Contains(sectorId));
                }

                // Filter End

                #endregion Filtering

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<BCModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BC_ID asc" : orderByString).ToList();

                totalRecord = result.Count;
                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();

                businessCode = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, businessCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get BusinessCode Data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ManageBC(int? id)
        {
            BCModel businessCodeModel = new BCModel
            {
                ACTIVE = true
            };
            if (id != null && id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int businessCodeId = Convert.ToInt32(id);
                    var businessCode = ctx.BCs.FirstOrDefault(a => a.BC_ID == businessCodeId);
                    businessCodeModel = Mapper.Map<BCModel>(businessCode);

                    var additionalDocs = ctx.BCLinkAD.Where(blAd => blAd.BusinessCodeID == businessCodeId);
                    businessCodeModel.AdditionalDocs = additionalDocs.Any()
                        ? additionalDocs.Select(blAd => blAd.AdditionalDocID).ToList()
                        : new List<int>();

                    var departments = ctx.BCLinkDeps.Where(blD => blD.BusinessCodeID == businessCodeId);
                    if (departments.Any())
                    {
                        foreach (var dep in departments)
                        {
                            if (dep.Department != null)
                            {
                                businessCodeModel.selectedDepartments.Add(new Select2ListItem() { id = dep.DepartmentID, text = $"{dep.Department.DepartmentCode} - {dep.Department.DepartmentDesc }" });
                            }
                        }
                        businessCodeModel.DepartmentIDs = String.Join(",", departments.Select(blD => blD.DepartmentID).ToArray());
                    }

                }
            }

            return View(businessCodeModel);
        }

        /// <summary>
        /// Save BusinessCode Infomration
        /// </summary>
        /// <param name="businessCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageBC(BCModel businessCodeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    BC businessCode;
                    if (IsBusinessCodeDuplicate(businessCodeModel.C_R_DESC, businessCodeModel.BC_ID))
                    {
                        TempData["ErrorMessage"] = "Business Code is already exist in the database.";
                        return View(businessCodeModel);
                    }
                    businessCode = Mapper.Map<BC>(businessCodeModel);
                    ctx.BCs.AddOrUpdate(businessCode);
                    ctx.SaveChanges();

                    if (!string.IsNullOrEmpty(businessCodeModel.DepartmentIDs))
                    {
                        List<BCLinkDep> selectedDepartments = new List<BCLinkDep>();
                        var selectedDeps = ctx.BCLinkDeps.Where(bd => bd.BusinessCodeID == businessCode.BC_ID).ToList();
                        var deptIds = businessCodeModel.DepartmentIDs.Split(',');
                        foreach (var dep in deptIds)
                        {
                            var depId = Convert.ToInt32(dep);
                            if (selectedDeps.All(sd => sd.DepartmentID != depId))
                            {
                                selectedDepartments.Add(new BCLinkDep { BusinessCodeID = businessCode.BC_ID, DepartmentID = depId });
                            }
                        }
                        if (selectedDeps.Count > 0)
                        {
                            foreach (var bcDep in selectedDeps)
                            {
                                if (deptIds.All(rd => rd != bcDep.DepartmentID.ToString()))
                                {
                                    ctx.Entry(bcDep).State = System.Data.Entity.EntityState.Deleted;
                                }
                            }
                        }
                        if (selectedDepartments.Count > 0)
                        {
                            ctx.BCLinkDeps.AddOrUpdate(selectedDepartments.ToArray());
                        }
                    }

                    if (businessCodeModel.AdditionalDocs.Count > 0)
                    {
                        List<BCLinkAD> selectedAdditionalDocs = new List<BCLinkAD>();
                        var selectedADocs = ctx.BCLinkAD.Where(bd => bd.BusinessCodeID == businessCode.BC_ID).ToList();
                        var addDocIds = businessCodeModel.AdditionalDocs;
                        foreach (var addDocId in addDocIds)
                        {
                            if (selectedADocs.All(sd => sd.AdditionalDocID != addDocId))
                            {
                                selectedAdditionalDocs.Add(new BCLinkAD { BusinessCodeID = businessCode.BC_ID, AdditionalDocID = addDocId });
                            }
                        }
                        if (selectedADocs.Count > 0)
                        {
                            foreach (var bcDep in selectedADocs)
                            {
                                if (addDocIds.All(rd => rd != bcDep.AdditionalDocID))
                                {
                                    ctx.Entry(bcDep).State = System.Data.Entity.EntityState.Deleted;
                                }
                            }
                        }
                        if (selectedAdditionalDocs.Count > 0)
                        {
                            ctx.BCLinkAD.AddOrUpdate(selectedAdditionalDocs.ToArray());
                        }

                    }
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Business Code saved successfully.";

                return RedirectToAction("BusinessCode");
            }
            else
            {
                return View(businessCodeModel);
            }

        }

        /// <summary>
        /// Delete BusinessCode Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBC(int id)
        {
            try
            {
                var businessCode = new BC() { BC_ID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(businessCode).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsBusinessCodeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.BCs.FirstOrDefault(
                   c => c.BC_ID != id && c.C_R_DESC.ToLower() == name.ToLower())
               : ctx.BCs.FirstOrDefault(
                   c => c.C_R_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region Save Banner Objects
        [HttpPost]
        public ActionResult AddBannerObject(int APP_ID, int BC_ID, string ADDRA1, string ADDRA2, string ADDRA3, string ADDRA4, float B_SIZE, int B_QTY)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                B_O ba = new B_O();
                var Fee = ctx.BCs.Where(p => p.BC_ID == BC_ID).Select(p => p.P_FEE).FirstOrDefault();
                var eFee = ctx.BCs.Where(p => p.BC_ID == BC_ID).Select(p => p.EX_FEE).FirstOrDefault();
                float? TotalFee = 0;

                if (ba != null)
                {
                    ba.APP_ID = APP_ID;
                    ba.BC_ID = BC_ID;
                    ba.ADDRA1 = ADDRA1;
                    ba.ADDRA2 = ADDRA2;
                    ba.ADDRA3 = ADDRA3;
                    ba.ADDRA4 = ADDRA4;
                    ba.B_SIZE = B_SIZE;
                    ba.B_QTY = B_QTY;
                    if (B_SIZE <= 8)
                    {
                        TotalFee = Fee * B_QTY;
                    }
                    else
                    {
                        TotalFee = (((float)Math.Floor(B_SIZE - 8) * eFee) + Fee) * B_QTY;
                    }
                    ba.FEE = TotalFee;

                    ctx.B_Os.Add(ba);
                    ctx.SaveChanges();
                    TempData["SuccessMessage"] = "Iklan berjaya ditambah.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Iklan tidak berjaya ditambah";
                }
            }

            return Redirect(Url.Action("ManageApplication", "Application") + "?id=" + APP_ID);
        }
        #endregion

        #region Banner Object List Datatable
        /// <summary>
        /// Get Banner Object Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="bannerApplicationId">The banner application identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BannerObject([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? bannerApplicationId)
        {
            List<B_O_Model> bannerObject = new List<B_O_Model>();
            int totalRecord = 0;
            if (bannerApplicationId.HasValue)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var bannerObj = ctx.B_Os.Where(bo => bo.APP_ID == bannerApplicationId).ToList();
                    bannerObject = Mapper.Map<List<B_O_Model>>(bannerObj);
                    totalRecord = bannerObject.Count;
                }
            }
            return Json(new DataTablesResponse(requestModel.Draw, bannerObject, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}