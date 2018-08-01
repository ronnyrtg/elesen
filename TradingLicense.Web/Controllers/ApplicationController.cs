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

        private Func<BC, Select2ListItem> fnSelectBusinessCode = bc => new Select2ListItem { id = bc.BC_ID, text = $"{bc.C_R}~{bc.C_R_DESC}" };
        private Func<DEPARTMENT, Select2ListItem> fnSelectDepartment = de => new Select2ListItem { id = de.DEP_ID, text = $"{de.DEP_CODE}~{de.DEP_DESC}" };
        private Func<E_P_FEE, Select2ListItem> fnSelectPremiseFee = ep => new Select2ListItem { id = ep.E_P_FEEID, text = $"{ep.E_P_DESC}~{ep.E_S_DESC}" };
        private Func<INDIVIDUAL, Select2ListItem> fnSelectIndividualFormat = ind => new Select2ListItem { id = ind.IND_ID, text = $"{ind.FULLNAME} ({ind.MYKADNO})" };

        #region Application List Grid
        /// <summary>
        /// GET: Application
        /// </summary>
        /// <returns></returns>
        [AuthorizationPrivilegeFilter(SystemEnum.Page.Application, SystemEnum.PageRight.CrudLevel)]
        public ActionResult Application()
        {
            return View();
        }
       
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
                int? rollTemplateID = ProjectSession.User?.ROLEID;
                IQueryable<APPLICATION> query = ctx.APPLICATIONs;

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
                                var departmentID = ProjectSession.User?.DEP_ID;
                                if (departmentID.HasValue)
                                {
                                    var paIDs = ctx.ROUTEUNITs
                                            .Where(pa => pa.DEP_ID == departmentID.Value && pa.SUPPORT == 0)
                                            .Select(d => d.APP_ID).Distinct()
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

        #region Delete Application from Datatable List
        /// <summary>
        /// Delete Application Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteApplication(int id)
        {
            try
            {
                var premiseApplication = new APPLICATION() { APP_ID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(premiseApplication).State = System.Data.Entity.EntityState.Deleted;
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

        #region ManageApplication
        /// <summary>
        /// Get Application Data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ManageApplication(int? id)
        {
            ApplicationModel applicationModel = new ApplicationModel();
            
            if (id != null && id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var application = ctx.APPLICATIONs.FirstOrDefault(a => a.APP_ID == id);
                    applicationModel = Mapper.Map<ApplicationModel>(application);

                    var paLinkBc = ctx.APP_L_BCs.Where(a => a.APP_ID == id).ToList();
                    applicationModel.BusinessCodeids = string.Join(",", paLinkBc.Select(x => x.BC_ID.ToString()).ToArray());

                    var routeSettled = ctx.ROUTEUNITs.Where(r => r.APP_ID == id);

                    var bannerObjects = ctx.B_Os.Where(a => a.APP_ID == id).ToList();
                    applicationModel.totalBannerObjects = bannerObjects.Count;

                    //TODO: replaced with this for avoid calling database in foreach loop
                    // TODO: Select2ListItem is just the same as build-in KeyValuePair class
                    List<Select2ListItem> businessCodesList = new List<Select2ListItem>();
                    var ids = paLinkBc.Select(b => b.BC_ID).ToList();
                    businessCodesList = ctx.BCs
                        .Where(b => ids.Contains(b.BC_ID))
                        .Select(fnSelectBusinessCode)
                        .ToList();

                    applicationModel.selectedbusinessCodeList = businessCodesList;

                    List<Select2ListItem> premiseFeeList = new List<Select2ListItem>();
                    premiseFeeList = ctx.E_P_FEEs
                        .Select(fnSelectPremiseFee)
                        .ToList();

                    applicationModel.selectedPremiseFeeList = premiseFeeList;

                    applicationModel.HasPADepSupp = ctx.ROUTEUNITs.Any(pa => pa.APP_ID == id.Value);

                    var paLinkInd = ctx.APP_L_INDs.Where(a => a.APP_ID == id).ToList();
                    applicationModel.Individualids = string.Join(",", paLinkInd.Select(x => x.IND_ID.ToString()).ToArray());
                    List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
                    var iids = paLinkInd.Select(b => b.IND_ID).ToList();
                    selectedIndividualList = ctx.INDIVIDUALs
                        .Where(b => iids.Contains(b.IND_ID))
                        .Select(fnSelectIndividualFormat)
                        .ToList();

                    applicationModel.selectedIndividualList = selectedIndividualList;

                    var paLinkReqDocumentList = ctx.APP_L_RDs.Where(p => p.APP_ID == id && p.RD_TYPE == 1).ToList();
                    if (paLinkReqDocumentList.Count > 0)
                    {
                        applicationModel.UploadRequiredDocids = (string.Join(",", paLinkReqDocumentList.Select(x => x.RD_ID.ToString() + ":" + x.ATT_ID.ToString()).ToArray()));
                    }
                    var paLinkLicDocumentList = ctx.APP_L_RDs.Where(p => p.APP_ID == id && p.RD_TYPE == 3).ToList();
                    if (paLinkLicDocumentList.Count > 0)
                    {
                        applicationModel.UploadLicenseDocids = (string.Join(",", paLinkLicDocumentList.Select(x => x.RD_ID.ToString() + ":" + x.ATT_ID.ToString()).ToArray()));
                    }

                    if (application.APPSTATUSID == (int)PAStausenum.Pendingpayment)
                    {
                        
                    }
                }
            }
            else
            {
                
                applicationModel.START_RENT = DateTime.Today.AddMonths(-6);
                applicationModel.STOP_RENT = DateTime.Today.AddMonths(6);
                using (var ctx = new LicenseApplicationContext())
                {
                    

                    List<Select2ListItem> premiseFeeList = new List<Select2ListItem>();
                    premiseFeeList = ctx.E_P_FEEs
                        .Select(fnSelectPremiseFee)
                        .ToList();

                    applicationModel.selectedPremiseFeeList = premiseFeeList;

                    var bannerObjects = ctx.B_Os.Where(a => a.APP_ID == id).ToList();
                    applicationModel.totalBannerObjects = bannerObjects.Count;
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.ROLEID > 0)
            {
                applicationModel.UserRollTemplate = ProjectSession.User.ROLEID.Value;
                applicationModel.USERSID = ProjectSession.User.USERSID;
            }


            applicationModel.IsDraft = false;
            return View(applicationModel);
        }
        #endregion

        #region Check ManageApplication data isValid
        /// <summary>
        /// Check Application Information
        /// </summary>
        /// <param name="ApplicationModel">The application model.</param>
        /// <param name="btnSubmit">The BTN submit.</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageApplication(ApplicationModel ApplicationModel, string btnSubmit)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    bool saveSuccess = false;
                    using (var ctx = new LicenseApplicationContext())
                    {
                        saveSuccess = SaveApplication(ApplicationModel, ctx);
                    }
                    if (saveSuccess && ApplicationModel.IsDraft)
                    {
                        TempData["SuccessMessage"] = "License Application draft saved successfully.";

                        return Redirect(Url.Action("ManageApplication", "Application") + "?id=" + ApplicationModel.APP_ID);
                    }
                    if (saveSuccess)
                    {
                        TempData["SuccessMessage"] = "License Application saved successfully.";
                        return RedirectToAction("Application");
                    }
                    return Redirect(Url.Action("ManageApplication", "Application") + "?id=" + ApplicationModel.APP_ID);
                }


                return View(ApplicationModel);
            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }
        #endregion

        #region Get Roletemplate from ProjectSession

        private static int GetUserRoleTemplate(ApplicationModel applicationModel,
            APPLICATION application, LicenseApplicationContext ctx)
        {
            int userroleTemplate = 0;
            application.UPDATED_BY = ProjectSession.User.USERNAME;

            if (ProjectSession.User.ROLEID != null)
            {
                userroleTemplate = ProjectSession.User.ROLEID.Value;
            }

            return userroleTemplate;
        }
        #endregion

        #region Get APPSTATUSID Upon Submit Button
        private int GetStatusOnSubmit(ApplicationModel applicationModel, LicenseApplicationContext ctx, APPLICATION application, int roleTemplate)
        {
            PAStausenum finalStatus = 0;
            if (!applicationModel.IsDraft)
            {
                switch (roleTemplate)
                {
                    case (int)RollTemplate.DeskOfficer:
                        finalStatus = PAStausenum.submittedtoclerk;
                        break;
                    case (int)RollTemplate.Clerk:
                        if (applicationModel.SubmitType == OnSubmit)
                        {                            
                            finalStatus = PAStausenum.directorcheck;                                    
                        }
                        else if (applicationModel.SubmitType == OnRouteSubmit)
                        {
                            finalStatus = PAStausenum.unitroute;
                        }
                        else if (applicationModel.SubmitType == OnRejected)
                        {
                            finalStatus = PAStausenum.documentIncomplete;
                        }
                        break;
                    case (int)RollTemplate.Director:
                        if (applicationModel.SubmitType == OnSubmit)
                        {
                            switch (applicationModel.MODE)
                            {
                                case 1:
                                    finalStatus = PAStausenum.CEOcheck;
                                    break;
                                case 2:
                                    finalStatus = PAStausenum.LetterofnotificationApproved;
                                    break;
                                case 3:
                                    finalStatus = PAStausenum.CEOcheck;
                                    break;
                                case 4:
                                    finalStatus = PAStausenum.meeting;
                                    break;
                            }
                        }
                        else if (applicationModel.SubmitType == OnRejected)
                        {
                            switch (applicationModel.MODE)
                            {
                                case 1:
                                    finalStatus = PAStausenum.meeting;
                                    break;
                                case 2:
                                    finalStatus = PAStausenum.submittedtoclerk;
                                    break;
                                case 3:
                                    finalStatus = PAStausenum.submittedtoclerk;
                                    break;
                                case 4:
                                    finalStatus = PAStausenum.submittedtoclerk;
                                    break;
                            }
                        }
                        break;
                    case (int)RollTemplate.CEO:
                        if (applicationModel.SubmitType == OnSubmit)
                        {
                            switch (applicationModel.MODE)
                            {
                                case 1:
                                    finalStatus = PAStausenum.Complete;
                                    break;
                                case 3:
                                    finalStatus = PAStausenum.LetterofnotificationApproved;
                                    break;
                            }
                        }
                        else if (applicationModel.SubmitType == OnRejected)
                        {
                            switch (applicationModel.MODE)
                            {
                                case 1:
                                    finalStatus = PAStausenum.meeting;
                                    break;
                                case 3:
                                    finalStatus = PAStausenum.LetterofnotificationRejected;
                                    break;
                            }
                        }
                        break;
                }
            }
            else
            {
                finalStatus = PAStausenum.draftcreated;
            }
            return (int)finalStatus;
        }
        #endregion

        #region Save data from ManageApplication
        private bool SaveApplication(ApplicationModel applicationModel, LicenseApplicationContext ctx)
        {

            var application = Mapper.Map<APPLICATION>(applicationModel);
            int userroleTemplate = 0;
            if (ProjectSession.User != null && ProjectSession.UserID > 0)
            {
                userroleTemplate = GetUserRoleTemplate(applicationModel, application, ctx);
            }
            var finalStatus = GetStatusOnSubmit(applicationModel, ctx, application, userroleTemplate);
            if (finalStatus != 0)
            {
                application.APPSTATUSID = finalStatus;
            }

            if (applicationModel.APPSTATUSID <= (int)Enums.PAStausenum.draftcreated && applicationModel.MODE == 1 || applicationModel.APPSTATUSID == (int)Enums.PAStausenum.LetterofnotificationApprovedwithTermsConditions && applicationModel.MODE > 1)
            {
                application.APPROVE = DateTime.Now;
                application.EXPIRE = DateTime.Now.AddMonths(6);
                application.L_STATUS = "LULUS BERSYARAT";
            }
            else if (applicationModel.APPSTATUSID == (int)Enums.PAStausenum.LetterofnotificationApproved && applicationModel.MODE > 1)
            {
                application.APPROVE = DateTime.Now;
                application.EXPIRE = DateTime.Now.AddMonths(12);
                application.L_STATUS = "LULUS";
            }
            else if (applicationModel.APPSTATUSID == (int)Enums.PAStausenum.LetterofnotificationRejected && applicationModel.MODE > 1)
            {
                application.APPROVE = DateTime.Now;
                application.EXPIRE = DateTime.Now;
                application.L_STATUS = "TIDAK DILULUSKAN";
            }
            ctx.APPLICATIONs.AddOrUpdate(application);
            ctx.SaveChanges();


            int applicationId = application.APP_ID;
            if (applicationModel.APP_ID == 0)
            {
                var licCode = ctx.LIC_TYPEs.Where(m => m.LIC_TYPEID == application.LIC_TYPEID).Select(m => m.LIC_TYPECODE).SingleOrDefault().ToString();
                application.SUBMIT = DateTime.Now;
                applicationModel.APP_ID = applicationId;
                application.REF_NO = ApplicationModel.GetReferenceNo(applicationId, application.SUBMIT, licCode);
                ctx.APPLICATIONs.AddOrUpdate(application);
                ctx.SaveChanges();
            }

            int roleTemplate = 0;
            if (ProjectSession.User != null && ProjectSession.User.ROLEID > 0)
            {
                roleTemplate = ProjectSession.User.ROLEID.Value;
            }

            if (userroleTemplate == (int)RollTemplate.Public)
            {
                if (!string.IsNullOrWhiteSpace(applicationModel.UploadRequiredDocids))
                {
                    DocumentService.UpdateDocs(applicationModel, ctx, applicationId, roleTemplate);
                }
                else
                {
                    if (roleTemplate == (int)RollTemplate.Public)
                    {
                        var paLinkReqDocUmentList = ctx.APP_L_RDs
                            .Where(p => p.APP_ID == applicationId).ToList();
                        if (paLinkReqDocUmentList.Count > 0)
                        {
                            ctx.APP_L_RDs.RemoveRange(paLinkReqDocUmentList);
                            ctx.SaveChanges();
                        }
                    }
                }
            }

            else if (userroleTemplate == (int)RollTemplate.DeskOfficer)
            {
                if (!string.IsNullOrWhiteSpace(applicationModel.RequiredDocIds))
                {
                    DocumentService.UpdateRequiredDocs(applicationModel, ctx, applicationId, roleTemplate);
                }
                if (!string.IsNullOrWhiteSpace(applicationModel.AdditionalDocIds))
                {
                    DocumentService.SaveAdditionalDocInfo(applicationModel, ctx, applicationId, roleTemplate);
                }
                if (!string.IsNullOrWhiteSpace(applicationModel.LicenseDocIds))
                {
                    DocumentService.SaveLicenseDocInfo(applicationModel, ctx, applicationId, roleTemplate);
                }
            }

            if (!string.IsNullOrWhiteSpace(applicationModel.BusinessCodeids))
            {
                var businessCodelist = applicationModel.BusinessCodeids.ToIntList();

                List<int> existingRecord = new List<int>();
                var dbEntryPaLinkBAct = ctx.APP_L_BCs.Where(q => q.APP_ID == applicationId).ToList();
                if (dbEntryPaLinkBAct.Count > 0)
                {
                    float? Totalfee = 0;
                    foreach (var item in dbEntryPaLinkBAct)
                    {
                        if (businessCodelist.All(q => q != item.BC_ID))
                        {
                            ctx.APP_L_BCs.Remove(item);
                        }
                        else
                        {
                            existingRecord.Add(item.BC_ID);
                        }
                        var fee = ctx.BCs.Where(b => b.BC_ID == item.BC_ID).Select(b => b.DEF_RATE).FirstOrDefault();
                        if (fee == 0)
                        {
                            fee = ctx.BCs.Where(b => b.BC_ID == item.BC_ID).Select(b => b.BASE_FEE).FirstOrDefault();
                        }
                        if (applicationModel.P_AREA != 0)
                        {
                            Totalfee = Totalfee + fee * applicationModel.P_AREA;
                            application.TOTAL_FEE = (float)Math.Round((float)Totalfee, 1);
                            //application.TOTAL_FEE = Totalfee;
                        }
                        ctx.APPLICATIONs.AddOrUpdate(application);
                    }
                    ctx.SaveChanges();
                }


                foreach (var businessCode in businessCodelist)
                {
                    if (existingRecord.All(q => q != businessCode))
                    {
                        APP_L_BC paLinkBc = new APP_L_BC();
                        paLinkBc.APP_ID = applicationId;
                        paLinkBc.BC_ID = businessCode;
                        ctx.APP_L_BCs.Add(paLinkBc);

                    }
                }
                ctx.SaveChanges();
            }
            else
            {
                var dbEntryPaLinkBActs = ctx.APP_L_BCs.Where(va => va.APP_ID == applicationId).ToList();
                if (dbEntryPaLinkBActs.Count > 0)
                {
                    ctx.APP_L_BCs.RemoveRange(dbEntryPaLinkBActs);
                    ctx.SaveChanges();
                }
            }

            if (!string.IsNullOrWhiteSpace(applicationModel.Individualids))
            {
                //todo: I guess it's a draft for new logic
                var individualidslist = applicationModel.Individualids.ToIntList();
                List<int> existingRecord = new List<int>();
                var dbEntryPaLinkInd = ctx.APP_L_INDs.Where(q => q.APP_ID == applicationId).ToList();
                if (dbEntryPaLinkInd.Count > 0)
                {
                    foreach (var item in dbEntryPaLinkInd)
                    {
                        if (individualidslist.All(q => q != item.IND_ID))
                        {
                            ctx.APP_L_INDs.Remove(item);
                        }
                        else
                        {
                            existingRecord.Add(item.IND_ID);
                        }
                    }
                    ctx.SaveChanges();
                }

                foreach (var individual in individualidslist)
                {
                    if (existingRecord.All(q => q != individual))
                    {
                        APP_L_IND paLinkInd = new APP_L_IND();
                        paLinkInd.APP_ID = applicationId;
                        paLinkInd.IND_ID = individual;
                        ctx.APP_L_INDs.Add(paLinkInd);

                    }
                }
                ctx.SaveChanges();
            }

            if (!string.IsNullOrWhiteSpace(applicationModel.newIndividualsList))
            {
                List<NewIndividualModel> individuals = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<NewIndividualModel>>(applicationModel.newIndividualsList);
                foreach (var indModel in individuals)
                {
                    INDIVIDUAL ind = new INDIVIDUAL();
                    ind.FULLNAME = indModel.fullName;
                    ind.MYKADNO = indModel.passportNo;
                    ctx.INDIVIDUALs.Add(ind);

                }
                ctx.SaveChanges();
            }


            if (applicationModel.B_QTY > 0)
            {
                var fee = ctx.BCs.Where(b => b.BC_ID == applicationModel.BC_ID).Select(b => b.P_FEE).FirstOrDefault();
                var exfee = ctx.BCs.Where(b => b.BC_ID == applicationModel.BC_ID).Select(b => b.EX_FEE).FirstOrDefault();

                B_O banner = new B_O();
                banner.APP_ID = applicationModel.APP_ID;
                banner.ADDRB1 = applicationModel.ADDRB1;
                banner.ADDRB2 = applicationModel.ADDRB2;
                banner.ADDRB3 = applicationModel.ADDRB3;
                banner.ADDRB4 = applicationModel.ADDRB4;
                banner.BC_ID = (int)applicationModel.BC_ID;
                banner.B_QTY = (int)applicationModel.B_QTY;
                banner.B_SIZE = (float)applicationModel.B_SIZE;
                if (applicationModel.B_SIZE < 8)
                {
                    banner.FEE = applicationModel.B_SIZE * applicationModel.B_QTY * fee;
                }
                else
                {
                    banner.FEE = applicationModel.B_SIZE * applicationModel.B_QTY * fee + (float)Math.Floor((float)applicationModel.B_SIZE - 8) * applicationModel.B_QTY * exfee;
                }
                applicationModel.TOTAL_FEE = banner.FEE + applicationModel.PRO_FEE;

                ctx.B_Os.Add(banner);
                ctx.SaveChanges();
            }
            
            applicationModel.APP_ID = applicationId;
            return true;

        }
        #endregion

        #region Get Business Code data for Datatable (FillBusinessCode)
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

        #region Get Routeable Departments data for Datatable (FillDepartments)
        /// <summary>
        /// Get Department Aray String
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillDepartments(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<DEPARTMENT> primaryQuery = ctx.DEPARTMENTs.Where(de => de.ROUTE == 1);               
               
                if (!String.IsNullOrWhiteSpace(query))
                {
                    primaryQuery = primaryQuery.Where(bc => bc.DEP_DESC.ToLower().Contains(query.ToLower()) || bc.DEP_DESC.ToLower().Contains(query.ToLower()));
                }
                var businessCode = primaryQuery.Select(fnSelectDepartment).ToList();
                return Json(businessCode, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Get List of Route Departments linked to selected BusinessCodes in Datatable (FillRouteDepartments)
        /// <summary>
        /// Gets List of department details to which this premise application will be routed.
        /// </summary>
        /// <param name="businessCodeIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FillRouteDepartments([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessCodeIds)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var businessCodelist = businessCodeIds.ToIntList();
                var departmentIds = ctx.BC_L_DEPs.Where(bc => businessCodelist.Contains(bc.BC_ID)).Select(bc => bc.DEP_ID).Distinct().ToList();
                var departmentList = ctx.DEPARTMENTs.Where(dep => departmentIds.Contains(dep.DEP_ID)).ToList();
                int totalRecord = departmentList.Count;
                if(totalRecord == 0)
                {
                    departmentList = ctx.DEPARTMENTs.Where(dep => dep.DEP_ID == 3).ToList();
                }
                return Json(new DataTablesResponse(requestModel.Draw, Mapper.Map<List<DepartmentModel>>(departmentList), totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Add new Route Departments entry to ROUTEUNIT table
        /// <summary>
        /// Add Departments to Route
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRoute(int id, int[] DeptId, string newQuestion)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                int i = 0;
                foreach (var item in DeptId)
                {

                    ROUTEUNIT pad = new ROUTEUNIT();
                    pad.APP_ID = id;
                    pad.DEP_ID = DeptId[i];
                    pad.QUESTION = newQuestion;
                    pad.SENDER = ProjectSession.User.FULLNAME;
                    pad.SUBMITTED = DateTime.Now;
                    pad.SUPPORT = 0;
                    ctx.ROUTEUNITs.Add(pad);
                    i++;
                }
                ctx.SaveChanges();
                TempData["SuccessMessage"] = "Routing ke unit berjaya ditambah.";

            }

            return Redirect(Url.Action("ManageApplication", "Application") + "?id=" + id);
        }
        #endregion

        #region Route Comments Datatable
        /// <summary>
        /// Get Department support/non-supported Comments for the premise applicaiton
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="premiseApplicationID">The premise application identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RouteComments([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? APP_ID)
        {
            List<RouteUnitModel> premiseRouteComments = new List<RouteUnitModel>();
            int totalRecord = 0;
            if (APP_ID.HasValue)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    if (ProjectSession.User.ROLEID == (int)Enums.RollTemplate.RouteUnit)
                    {
                        IQueryable<ROUTEUNIT> query = ctx.ROUTEUNITs
                                                            .Include("Department")
                                                            .Where(pac => pac.APP_ID == APP_ID.Value && pac.DEP_ID == ProjectSession.User.DEP_ID);


                        #region Sorting
                        // Sorting
                        var sortedColumns = requestModel.Columns.GetSortedColumns();
                        var orderByString = sortedColumns.GetOrderByString();

                        var result = Mapper.Map<List<RouteUnitModel>>(query.ToList());
                        result = result.OrderBy(orderByString == string.Empty ? "SUBMITTED desc" : orderByString).ToList();

                        totalRecord = result.Count;

                        #endregion Sorting


                        // Paging
                        //result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                        premiseRouteComments = result;
                    }
                    else
                    {
                        IQueryable<ROUTEUNIT> query = ctx.ROUTEUNITs
                                                            .Include("Department")
                                                            .Where(pac => pac.APP_ID == APP_ID.Value);


                        #region Sorting
                        // Sorting
                        var sortedColumns = requestModel.Columns.GetSortedColumns();
                        var orderByString = sortedColumns.GetOrderByString();

                        var result = Mapper.Map<List<RouteUnitModel>>(query.ToList());
                        result = result.OrderBy(orderByString == string.Empty ? "SUBMITTED desc" : orderByString).ToList();

                        totalRecord = result.Count;

                        #endregion Sorting


                        // Paging
                        //result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                        premiseRouteComments = result;
                    }
                }
            }
            return Json(new DataTablesResponse(requestModel.Draw, premiseRouteComments, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update Route Unit Answers
        /// <summary>
        /// Update answers to RouteUnit table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveRouteComment(int APP_ID, int routeID, string newComment, int Supported)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var route = ctx.ROUTEUNITs.SingleOrDefault(r => r.ROUTEUNITID == routeID);
                route.ANSWER = newComment;
                route.SUPPORT = Supported;
                route.RECEIVER = ProjectSession.User.FULLNAME;                
                route.REPLIED = DateTime.Now;
                ctx.SaveChanges();
                TempData["SuccessMessage"] = "Maklumbalas berjaya dihantar.";
            }

            return Redirect(Url.Action("ManageApplication", "Application") + "?id=" + APP_ID);
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
                var individual = ctx.INDIVIDUALs
                                    .Where(t => t.MYKADNO.ToLower().Contains(query.ToLower()) || t.FULLNAME.ToLower().Contains(query.ToLower()))
                                    .Select(fnSelectIndividualFormat).ToList();
                return Json(individual, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Get Required Doc List Datatable
        /// <summary>
        /// Get Required Document Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="businessTypeId">The business type identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RequiredDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessTypeID, string ApplicationID)
        {
            List<RD_L_BTModel> requiredDocument;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<RD_L_BT> query = ctx.RD_L_BTs.Where(p => p.BT_ID.ToString().Contains(businessTypeID));

                #region Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                string orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<RD_L_BTModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "RD_L_BTID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                requiredDocument = result;

                #region IsChecked

                if (!string.IsNullOrWhiteSpace(ApplicationID))
                {
                    int premiseAppId;
                    int.TryParse(ApplicationID, out premiseAppId);

                    var palinkAdd = ctx.APP_L_RDs.Where(p => p.APP_ID == premiseAppId && p.RD_TYPE == 1).ToList();
                    foreach (var item in requiredDocument)
                    {
                        if (palinkAdd.Count > 0)
                        {
                            var resultpalinkReq = palinkAdd.FirstOrDefault(p => p.RD_ID == item.RD_ID && p.APP_ID == premiseAppId);
                            if (resultpalinkReq != null)
                            {
                                item.IsChecked = "checked";
                                var attechmentdetails = ctx.ATTACHMENTs.FirstOrDefault(a => a.ATT_ID == resultpalinkReq.ATT_ID);
                                if (attechmentdetails != null)
                                {
                                    item.AttachmentFileName = attechmentdetails.FILENAME;
                                    item.AttachmentId = attechmentdetails.ATT_ID;

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
            List<RD_L_BCModel> requiredDocument = new List<RD_L_BCModel>();
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

                IQueryable<RD_L_BC> query = ctx.RD_L_BCs.Where(p => businessCodelist.Contains(p.BC_ID));

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<RD_L_BCModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "RD_L_BCID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                requiredDocument = result;

                #region IsChecked

                if (!string.IsNullOrWhiteSpace(premiseApplicationId))
                {
                    int premiseAppId;
                    int.TryParse(premiseApplicationId, out premiseAppId);

                    var palinkAdd = ctx.APP_L_RDs.Where(p => p.APP_ID == premiseAppId && p.RD_TYPE == 2).ToList();
                    foreach (var item in requiredDocument)
                    {
                        if (palinkAdd.Count > 0)
                        {
                            var resultpalinkReq = palinkAdd.FirstOrDefault(p => p.RD_ID == item.RD_ID && p.APP_ID == premiseAppId);
                            if (resultpalinkReq != null)
                            {
                                item.IsChecked = "checked";
                                var attechmentdetails = ctx.ATTACHMENTs.FirstOrDefault(a => a.ATT_ID == resultpalinkReq.ATT_ID);
                                if (attechmentdetails != null)
                                {
                                    item.AttachmentFileName = attechmentdetails.FILENAME;
                                    item.AttachmentId = attechmentdetails.ATT_ID;
                                    
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

        #region Get License Doc data for Datatable
        /// <summary>
        /// get Additional Document Data for Selected License Type
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LicenseDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int licTypeID, string ApplicationID)
        {
            List<RD_L_LTModel> licenseDocument = new List<RD_L_LTModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<RD_L_LT> query = ctx.RD_L_LTs.Where(rd => rd.LIC_TYPEID == licTypeID);

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<RD_L_LTModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "RD_L_LTID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                licenseDocument = result;

                #region IsChecked

                if (!string.IsNullOrWhiteSpace(ApplicationID))
                {
                    int premiseAppId;
                    int.TryParse(ApplicationID, out premiseAppId);

                    var palinkAdd = ctx.APP_L_RDs.Where(p => p.APP_ID == premiseAppId && p.RD_TYPE == 3).ToList();
                    foreach (var item in licenseDocument)
                    {
                        if (palinkAdd.Count > 0)
                        {
                            var resultpalinkReq = palinkAdd.FirstOrDefault(p => p.RD_ID == item.RD_ID && p.APP_ID == premiseAppId);
                            if (resultpalinkReq != null)
                            {
                                item.IsChecked = "checked";
                                var attechmentdetails = ctx.ATTACHMENTs.FirstOrDefault(a => a.ATT_ID == resultpalinkReq.ATT_ID);
                                if (attechmentdetails != null)
                                {
                                    item.AttachmentFileName = attechmentdetails.FILENAME;
                                    item.AttachmentId = attechmentdetails.ATT_ID;
                                    
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            return Json(new DataTablesResponse(requestModel.Draw, licenseDocument, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BusinessCode

        /// <summary>
        /// GET: BusinessCode
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessCode()
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
        public JsonResult BusinessCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string codeNumber, string codeDesc, string sectorId)
        {
            List<BusinessCodeModel> businessCode;
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
                                             p.SECTOR.SECTORDESC.ToLower().Contains(value)
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

                var result = Mapper.Map<List<BusinessCodeModel>>(query.ToList());
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
        public ActionResult ManageBusinessCode(int? id)
        {
            BusinessCodeModel businessCodeModel = new BusinessCodeModel();

            if (id != null && id > 0)

            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int businessCodeId = Convert.ToInt32(id);

                    var businessCode = ctx.BCs.FirstOrDefault(a => a.BC_ID == businessCodeId);
                    businessCodeModel = Mapper.Map<BusinessCodeModel>(businessCode);

                    var sector = ctx.SECTORs.ToList();
                    businessCodeModel.sectorList = Mapper.Map<List<SectorModel>>(sector);

                    var additionaDocs = ctx.RDs.ToList();
                    businessCodeModel.additionalDocsList = Mapper.Map<List<RequiredDocModel>>(additionaDocs);

                    var additionalDocs = ctx.RD_L_BCs.Where(blAd => blAd.BC_ID == businessCodeId);
                    businessCodeModel.AdditionalDocs = additionalDocs.Any()
                        ? additionalDocs.Select(blAd => blAd.RD_ID).ToList()
                        : new List<int>();

                    var departments = ctx.BC_L_DEPs.Where(blD => blD.BC_ID == businessCodeId);
                    if (departments.Any())
                    {
                        foreach (var dep in departments)
                        {
                            if (dep.DEPARTMENT != null)
                            {
                                businessCodeModel.selectedDepartments.Add(new Select2ListItem() { id = dep.DEP_ID, text = $"{dep.DEPARTMENT.DEP_CODE} - {dep.DEPARTMENT.DEP_DESC }" });
                            }
                        }
                        businessCodeModel.DepartmentIDs = String.Join(",", departments.Select(blD => blD.DEP_ID).ToArray());
                    }      
                }
            }
            else
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var license = ctx.LIC_TYPEs.ToList();
                    businessCodeModel.licenseList = Mapper.Map<List<LicenseTypeModel>>(license);

                    var sector = ctx.SECTORs.ToList();
                    businessCodeModel.sectorList = Mapper.Map<List<SectorModel>>(sector);

                    var additionaDocs = ctx.RDs.ToList();
                    businessCodeModel.additionalDocsList = Mapper.Map<List<RequiredDocModel>>(additionaDocs);
                }
            }

            businessCodeModel.periodList.Add(new Select2ListItem() { id = 1, text = "Tahun" });
            businessCodeModel.periodList.Add(new Select2ListItem() { id = 2, text = "Bulan" });
            businessCodeModel.periodList.Add(new Select2ListItem() { id = 3, text = "Minggu" });
            businessCodeModel.periodList.Add(new Select2ListItem() { id = 4, text = "Hari" });
            return View(businessCodeModel);
        }

        /// <summary>
        /// Save BusinessCode Infomration
        /// </summary>
        /// <param name="businessCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageBusinessCode(BusinessCodeModel businessCodeModel)
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
                        List<BC_L_DEP> selectedDepartments = new List<BC_L_DEP>();
                        var selectedDeps = ctx.BC_L_DEPs.Where(bd => bd.BC_ID == businessCode.BC_ID).ToList();
                        var deptIds = businessCodeModel.DepartmentIDs.Split(',');
                        foreach (var dep in deptIds)
                        {
                            var depId = Convert.ToInt32(dep);
                            if (selectedDeps.All(sd => sd.DEP_ID != depId))
                            {
                                selectedDepartments.Add(new BC_L_DEP { BC_ID = businessCodeModel.BC_ID, DEP_ID = depId });
                            }
                        }
                        if (selectedDeps.Count > 0)
                        {
                            foreach (var bcDep in selectedDeps)
                            {
                                if (deptIds.All(rd => rd != bcDep.DEP_ID.ToString()))
                                {
                                    ctx.Entry(bcDep).State = System.Data.Entity.EntityState.Deleted;
                                }
                            }
                        }
                        if (selectedDepartments.Count > 0)
                        {
                            ctx.BC_L_DEPs.AddOrUpdate(selectedDepartments.ToArray());
                        }
                    }

                    if (businessCodeModel.AdditionalDocs != null)
                    {
                        List<RD_L_BC> selectedAdditionalDocs = new List<RD_L_BC>();
                        var selectedADocs = ctx.RD_L_BCs.Where(bd => bd.BC_ID == businessCode.BC_ID).ToList();
                        var addDocIds = businessCodeModel.AdditionalDocs;
                        foreach (var addDocId in addDocIds)
                        {
                            if (selectedADocs.All(sd => sd.RD_ID != addDocId))
                            {
                                selectedAdditionalDocs.Add(new RD_L_BC { BC_ID = businessCode.BC_ID, RD_ID = addDocId });
                            }
                        }
                        if (selectedADocs.Count > 0)
                        {
                            foreach (var bcDep in selectedADocs)
                            {
                                if (addDocIds.All(rd => rd != bcDep.RD_ID))
                                {
                                    ctx.Entry(bcDep).State = System.Data.Entity.EntityState.Deleted;
                                }
                            }
                        }
                        if (selectedAdditionalDocs.Count > 0)
                        {
                            ctx.RD_L_BCs.AddOrUpdate(selectedAdditionalDocs.ToArray());
                        }

                    }
                    else
                    {                       
                        var paLinkReqDocUmentList = ctx.RD_L_BCs
                        .Where(p => p.BC_ID == businessCodeModel.BC_ID).ToList();
                        if (paLinkReqDocUmentList.Count > 0)
                        {
                            ctx.RD_L_BCs.RemoveRange(paLinkReqDocUmentList);
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
        public ActionResult DeleteBusinessCode(int id)
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

        #region EntmtPremiseFee

        /// <summary>
        /// GET: EntmtPremiseFee
        /// </summary>
        /// <returns></returns>
        public ActionResult EntmtPremiseFee()
        {
            return View();
        }

        /// <summary>
        /// Save EntmtPremiseFee Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="codeNumber">The code number.</param>
        /// <param name="codeDesc">The code desc.</param>
        /// <param name="sectorId">The sector identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EntmtPremiseFee([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string codeNumber, string codeDesc, string sectorId)
        {
            List<EntmtPremiseFeeModel> entmtPremiseFee;
            int totalRecord = 0;
            // int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<E_P_FEE> query = ctx.E_P_FEEs;

                #region Filtering

                // Apply filters for comman Grid searching
                if (requestModel.Search.Value != string.Empty)
                {
                    var value = requestModel.Search.Value.ToLower().Trim();
                    query = query.Where(p => p.E_P_DESC.ToLower().Contains(value) ||
                                             p.E_P_FEEID.ToString().Contains(value) ||
                                             p.E_S_FEE.ToString().Contains(value) ||
                                             p.E_S_DESC.ToLower().Contains(value)
                                       );
                }

                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(codeDesc))
                {
                    query = query.Where(p => p.E_P_DESC.ToLower().Contains(codeDesc.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(sectorId))
                {
                    query = query.Where(p => p.E_P_FEEID.ToString().Contains(sectorId));
                }

                // Filter End

                #endregion Filtering

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<EntmtPremiseFeeModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "E_P_FEEID asc" : orderByString).ToList();

                totalRecord = result.Count;
                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();

                entmtPremiseFee = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, entmtPremiseFee, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get EntmtPremiseFee Data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ManageEntmtPremiseFee(int? id)
        {
            EntmtPremiseFeeModel entmtPremiseModel = new EntmtPremiseFeeModel();

            if (id != null && id > 0)

            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int businessCodeId = Convert.ToInt32(id);

                    var entmtPremiseFee = ctx.E_P_FEEs.FirstOrDefault(a => a.E_P_FEEID == businessCodeId);
                    entmtPremiseModel = Mapper.Map<EntmtPremiseFeeModel>(entmtPremiseFee);

                    

                    
                }
            }
            else
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    
                }
            }

            entmtPremiseModel.periodList.Add(new Select2ListItem() { id = 1, text = "Tahun" });
            entmtPremiseModel.periodList.Add(new Select2ListItem() { id = 2, text = "Bulan" });
            entmtPremiseModel.periodList.Add(new Select2ListItem() { id = 3, text = "Minggu" });
            entmtPremiseModel.periodList.Add(new Select2ListItem() { id = 4, text = "Hari" });
            return View(entmtPremiseModel);
        }

        /// <summary>
        /// Save EntmtPremiseFee Infomration
        /// </summary>
        /// <param name="entmtPremiseModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageEntmtPremiseFee(EntmtPremiseFeeModel entmtPremiseModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    E_P_FEE entmtPremiseFee;
                    if (IsEntmtPremiseFeeDuplicate(entmtPremiseModel.E_P_DESC, entmtPremiseModel.E_P_FEEID))
                    {
                        TempData["ErrorMessage"] = "Premise Fee detail already exists in the database.";
                        return View(entmtPremiseModel);
                    }
                    entmtPremiseFee = Mapper.Map<E_P_FEE>(entmtPremiseModel);
                    ctx.E_P_FEEs.AddOrUpdate(entmtPremiseFee);
                    ctx.SaveChanges();                   
                }

                TempData["SuccessMessage"] = "Business Code saved successfully.";

                return RedirectToAction("EntmtPremiseFee");
            }
            else
            {
                return View(entmtPremiseModel);
            }

        }

        /// <summary>
        /// Delete EntmtPremiseFee Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEntmtPremiseFee(int id)
        {
            try
            {
                var entmtPremiseFee = new E_P_FEE() { E_P_FEEID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(entmtPremiseFee).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsEntmtPremiseFeeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.E_P_FEEs.FirstOrDefault(
                   c => c.E_P_FEEID != id && c.E_P_DESC.ToLower() == name.ToLower())
               : ctx.E_P_FEEs.FirstOrDefault(
                   c => c.E_P_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region Save Banner Objects to Model
        [HttpPost]
        public ActionResult AddBannerObject(int APP_ID, int BC_ID, string ADDRB1, string ADDRB2, string ADDRB3, string ADDRB4, float B_SIZE, int B_QTY)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var ba = ctx.B_Os.Where(p => p.B_O_ID == APP_ID).FirstOrDefault();
                var Fee = ctx.BCs.Where(p => p.BC_ID == BC_ID).Select(p => p.P_FEE).FirstOrDefault();
                var eFee = ctx.BCs.Where(p => p.BC_ID == BC_ID).Select(p => p.EX_FEE).FirstOrDefault();
                float? TOTAL_FEE = 0;

                if (ba != null)
                {
                    ba.APP_ID = APP_ID;
                    ba.BC_ID = BC_ID;
                    ba.ADDRB1 = ADDRB1;
                    ba.ADDRB2 = ADDRB2;
                    ba.ADDRB3 = ADDRB3;
                    ba.ADDRB4 = ADDRB4;
                    ba.B_SIZE = B_SIZE;
                    ba.B_QTY = B_QTY;
                    if (B_SIZE <= 8)
                    {
                        TOTAL_FEE = Fee * B_QTY;
                    }
                    else
                    {
                        TOTAL_FEE = (((float)Math.Floor(B_SIZE - 8) * eFee) + Fee) * B_QTY;
                    }
                    ba.FEE = TOTAL_FEE;

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
        public JsonResult BannerObject([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? ApplicationId)
        {
            List<BannerObjectModel> bannerObject = new List<BannerObjectModel>();
            int totalRecord = 0;
            if (ApplicationId.HasValue)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var bannerObj = ctx.B_Os.Where(bo => bo.APP_ID == ApplicationId).ToList();
                    bannerObject = Mapper.Map<List<BannerObjectModel>>(bannerObj);
                    totalRecord = bannerObject.Count;
                }
            }
            return Json(new DataTablesResponse(requestModel.Draw, bannerObject, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Comments List Datatable
        /// <summary>
        /// Get Comments for the premise applicaiton
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="APP_ID">The premise application identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Comments([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? applicationID)
        {
            List<CommentModel> premiseComments = new List<CommentModel>();
            int totalRecord = 0;
            if (applicationID.HasValue)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    IQueryable<COMMENT> query = ctx.COMMENTs.Include("Users").Where(pac => pac.APP_ID == applicationID.Value);

                    #region Sorting
                    // Sorting
                    var sortedColumns = requestModel.Columns.GetSortedColumns();
                    var orderByString = sortedColumns.GetOrderByString();

                    var result = Mapper.Map<List<CommentModel>>(query.ToList());
                    result = result.OrderBy(orderByString == string.Empty ? "COMMENTDATE desc" : orderByString).ToList();

                    totalRecord = result.Count;

                    #endregion Sorting

                    // Paging
                    result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                    premiseComments = result;
                }
            }
            return Json(new DataTablesResponse(requestModel.Draw, premiseComments, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Comments
        /// <summary>
        /// Save Comment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveComment()
        {
            var premiseId = Request["APP_ID"];
            var comment = Request["newComment"];
            try
            {

                using (var ctx = new LicenseApplicationContext())
                {
                                        

                    int premiseApplicationId;
                    int.TryParse(premiseId, out premiseApplicationId);

                    int usersId = 0;
                    int userroleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.User.ROLEID > 0)
                    {
                        usersId = ProjectSession.User.USERSID;
                        userroleTemplate = ProjectSession.User.ROLEID.Value;
                    }

                    if (premiseApplicationId > 0 && usersId > 0 && userroleTemplate > 0)
                    {
                        COMMENT paComment = new COMMENT();
                        paComment.CONTENT = comment;
                        paComment.APP_ID = premiseApplicationId;
                        paComment.USERSID = usersId;
                        paComment.COMMENTDATE = DateTime.Now;
                        ctx.COMMENTs.AddOrUpdate(paComment);
                        ctx.SaveChanges();                        
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Data Missing.";
                        return Redirect(Url.Action("ManageApplication", "Application") + "?id=" + premiseId);
                        //return Json(new { status = "2", message = "Data Missing" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Something went wrong, Please try again.";
                return Redirect(Url.Action("Application", "Application"));
                //return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
            }
            return Redirect(Url.Action("ManageApplication", "Application") + "?id=" + premiseId);
        }
        #endregion

        #region Save Catatan
        /// <summary>
        /// Save Comment
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveCatatan()
        {
            var premiseId = Request["APP_ID"];
            var comment = Request["newCatatan"];
            try
            {

                using (var ctx = new LicenseApplicationContext())
                {


                    int premiseApplicationId;
                    int.TryParse(premiseId, out premiseApplicationId);

                    int usersId = 0;
                    int userroleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.User.ROLEID > 0)
                    {
                        usersId = ProjectSession.User.USERSID;
                        userroleTemplate = ProjectSession.User.ROLEID.Value;
                    }

                    if (premiseApplicationId > 0 && usersId > 0 && userroleTemplate > 0)
                    {
                        CATATAN paComment = new CATATAN();
                        paComment.CONTENT = comment;
                        paComment.APP_ID = premiseApplicationId;                        
                        ctx.CATATANs.AddOrUpdate(paComment);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Data Missing.";
                        return Redirect(Url.Action("ManageApplication", "Application") + "?id=" + premiseId);
                        //return Json(new { status = "2", message = "Data Missing" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Something went wrong, Please try again.";
                return Redirect(Url.Action("Application", "Application"));
                //return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
            }
            return Redirect(Url.Action("ManageApplication", "Application") + "?id=" + premiseId);
        }
        #endregion

        #region Delete Comments
        /// <summary>
        /// Delete Comment from PAComment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteComment(int id)
        {
            try
            {
                var paComment = new COMMENT() { COMMENTID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(paComment).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
                //return RedirectToAction("ManageApplication");
                return Json(new { success = true, message = "Delete Comment Successful" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Error While Deleting Comment" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Generate Letter PDF

        public ActionResult GenerateLetter(Int32? appId)
        {
            ApplicationModel ApplicationModel = new ApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var qry = ctx.APPLICATIONs.Include("COMPANY").Where(x => x.APP_ID == appId);
                    var appDetails = ctx.APPLICATIONs.Where(x => x.APP_ID == appId).ToList();
                    var compID = ctx.APPLICATIONs.Where(a => a.APP_ID == appId).Select(a => a.COMPANYID).SingleOrDefault();
                    var compDetails = ctx.COMPANIES.Where(c => c.COMPANYID == compID).SingleOrDefault();
                    if (appDetails.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in appDetails)
                        {
                            int lineheight = 10;
                            PdfDocument pdf = new PdfDocument();
                            pdf.Info.Title = "PDF Letter";
                            PdfPage pdfPage = pdf.AddPage();
                            XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                            XFont font = new XFont("Verdana", 8, XFontStyle.Bold);
                            XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                            XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                            XImage xImage1 = XImage.FromFile(Server.MapPath("~\\images\\logoPL.png"));
                            graph.DrawImage(xImage1, 180, 30, 100, 75);


                            graph.DrawString("PERBADANAN LABUAN", font, XBrushes.Black, new XRect(285, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("(LABUAN CORPORATION)", font, XBrushes.Black, new XRect(285, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("PETI SURAT 81245", font, XBrushes.Black, new XRect(285, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("87022 WILLAYAH PERSEKUTUAN LABUAN", font, XBrushes.Black, new XRect(285, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Tel No 				:", font, XBrushes.Black, new XRect(285, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("087 408600, 408601", font, XBrushes.Black, new XRect(385, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Faks No          :", font, XBrushes.Black, new XRect(285, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("087 428997, 419400, 426803", font, XBrushes.Black, new XRect(385, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            lineheight = lineheight + 12;
                            XPen lineRed = new XPen(XColors.Black, 2);
                            System.Drawing.Point pt1 = new System.Drawing.Point(0, lineheight);
                            System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width), lineheight);
                            graph.DrawLine(lineRed, pt1, pt2);
                            lineheight = lineheight + 15;
                            graph.DrawString("Rujukan Kami :", nfont, XBrushes.Black, new XRect(360, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("PL/JP/" + DateTime.Now.Year.ToString() + "/T/00000" + appId, nfont, XBrushes.Black, new XRect(435, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Tarikh           :", nfont, XBrushes.Black, new XRect(360, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(DateTime.Now.ToString("dd/MM/yyyy"), nfont, XBrushes.Black, new XRect(435, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Pengurus", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            string compName = "";
                           
                            if (!string.IsNullOrEmpty(compDetails.C_NAME))
                            {
                                compName = compDetails.C_NAME.ToString();

                            }
                            else
                            {
                                compName = "";
                            }
                                                        
                                                            
                            graph.DrawString(compName, nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;

                            string compAdd = "";
                            if (!string.IsNullOrEmpty(item.COMPANY.ADDRA1))
                            {
                                compAdd = item.COMPANY.ADDRA1;
                            }
                            else
                            {

                                compAdd = "";
                            }

                            graph.DrawString(compAdd.ToString(), nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;

                            string compPhone = "";
                            if (!string.IsNullOrEmpty(item.COMPANY.C_PHONE))
                            {
                                compPhone = item.COMPANY.C_PHONE;
                            }
                            else
                            {

                                compPhone = "";
                            }

                            graph.DrawString(compPhone, nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Tuan/Puan,", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("PERMOHONAN LESEN PERNIAGAAN BARU,", lbfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph.DrawString("NO. RUJUKAN", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(DateTime.Now.Year.ToString() + "/T/00000" + appId, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph.DrawString("NAMA PERNIAGAAN", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(compDetails.C_NAME, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            lineheight = lineheight + 20;
                            graph.DrawString("ALAMAT PREMIS", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);                            
                            
                            graph.DrawString(item.ADDRA1, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);                            
                            lineheight = lineheight + 15;
                            if (!string.IsNullOrEmpty(item.ADDRA2))
                            {
                                graph.DrawString(item.ADDRA2, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                lineheight = lineheight + 15;
                            }
                            if (!string.IsNullOrEmpty(item.ADDRA3))
                            {
                                graph.DrawString(item.ADDRA3, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                lineheight = lineheight + 15;
                            }
                            if (!string.IsNullOrEmpty(item.ADDRA4))
                            {
                                graph.DrawString(item.ADDRA4, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                lineheight = lineheight + 15;
                            }
                            string add1 = "";
                            add1 = item.PCODEA + ',' + item.STATEA;
                            graph.DrawString(add1, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("AKTIVITI", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            int cnt = 1;
                            if (item.LIC_TYPEID == (int)Enums.ApplicationTypeID.BannerApplication)
                            {
                                foreach (var item1 in ctx.B_Os.Where(x => x.APP_ID == appId))
                                {
                                    if (Convert.ToInt32(item1.B_O_ID) > 0)
                                    {
                                        foreach (var item2 in ctx.BCs.Where(x => x.BC_ID == item1.BC_ID))
                                        {
                                            {
                                                if (item2.C_R_DESC != null)
                                                {
                                                    string itm = cnt.ToString() + ")    " + item2.C_R_DESC;
                                                    graph.DrawString(itm, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                                    cnt = cnt + 1;
                                                    lineheight = lineheight + 15;
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach (var item1 in ctx.APP_L_BCs.Where(x => x.APP_ID == appId))
                                {
                                    if (Convert.ToInt32(item1.BC_ID) > 0)
                                    {
                                        foreach (var item2 in ctx.BCs.Where(x => x.BC_ID == item1.BC_ID))
                                        {
                                            {
                                                if (item2.C_R_DESC != null)
                                                {
                                                    string itm = cnt.ToString() + ")    " + item2.C_R_DESC;
                                                    graph.DrawString(itm, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                                    cnt = cnt + 1;
                                                    lineheight = lineheight + 15;
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            lineheight = lineheight + 20;
                            graph.DrawString("NAMA PEMILIK & NO. KP", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            cnt = 1;
                            foreach (var item3 in ctx.APP_L_INDs.Where(x => x.APP_ID == appId))
                            {
                                foreach (var item4 in ctx.INDIVIDUALs.Where(x => x.IND_ID == item3.IND_ID))
                                {
                                    if (item4.FULLNAME != null)
                                    {
                                        string fName = item4.FULLNAME;
                                        if (item4.MYKADNO != null)
                                        {
                                            fName = fName + "(" + item4.MYKADNO + ")";
                                        }
                                        string itm = cnt.ToString() + ")    " + fName;
                                        graph.DrawString(itm, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        cnt = cnt + 1;
                                        lineheight = lineheight + 15;
                                    }
                                }
                            }

                            lineheight = lineheight + 20;
                            graph.DrawString("KEPUTUSAN", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            string modeValue = "";
                            if (item.APPSTATUSID == 11)
                            {
                                modeValue = "LULUS BERSYARAT";
                            }
                            else if (item.APPSTATUSID == 9)
                            {
                                modeValue = "LULUS";
                            }
                            else if (item.APPSTATUSID == 10)
                            {
                                modeValue = "GAGAL";

                            }
                            else
                            {
                                modeValue = "LULUS BERSYARAT";
                            }

                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(modeValue, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph.DrawString("CATATAN", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            cnt = 1;

                            foreach (var item5 in ctx.CATATANs.Where(x => x.APP_ID == appId))
                            {
                                if (item5.CONTENT != null)
                                {
                                    string itm = cnt.ToString() + ") " + item5.CONTENT;
                                    XTextFormatter tf = new XTextFormatter(graph);
                                    XRect rect = new XRect(300, lineheight, 290, 50);
                                    graph.DrawRectangle(XBrushes.White, rect);
                                    tf.DrawString(itm, font, XBrushes.Black, rect, XStringFormats.TopLeft);
                                    //graph.DrawString(itm, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    cnt = cnt + 1;
                                    if (itm.Length < 60)
                                    {
                                        lineheight = lineheight + 16;
                                    }
                                    else if (itm.Length < 100)
                                    {
                                        lineheight = lineheight + 25;
                                    }
                                    else
                                    {
                                        lineheight = lineheight + 45;
                                    }
                                }
                            }
                            lineheight = lineheight + 20;
                            graph.DrawString("BAYARAN", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.TOTAL_FEE != null)
                            {
                                var mval = string.Format("{0:0.00}", item.TOTAL_FEE);
                                graph.DrawString("RM" + mval, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            else
                            {
                                graph.DrawString("RM0.00", font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }


                            lineheight = lineheight + 20;
                            graph.DrawString("PERINGATAN:", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("i.   Pihak tuan/puan adalah tidak dibenarkan menjalankan perniagaan selagi lesen perniagaan belum dikeluarkan.", font, XBrushes.Black, new XRect(40, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("ii.  Surat kelulusan ini sah sehingga  " + DateTime.Now.AddMonths(6).ToString("dd/MM/yyyy"), font, XBrushes.Black, new XRect(40, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("iii. Sekiranya pihak tuan membuat kerja-kerja pengubahsuaian bangunan sila kemukakan", font, XBrushes.Black, new XRect(40, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("     kelulusan Permohonan Plan Mengubahsuai Bangunan di Jabatan Perancangan dan Kawalan", font, XBrushes.Black, new XRect(40, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("     Bangunan Perbadanan Labuan terlebih dahulu.", font, XBrushes.Black, new XRect(40, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph.DrawString("Surat ini adalah cetakan komputer dan tandatangan tidak diperlukan", fontitalik, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            PdfPage pdfPage2 = pdf.AddPage();
                            XGraphics graph2 = XGraphics.FromPdfPage(pdfPage2);

                            graph2.DrawImage(xImage1, 180, 30, 75, 75);

                            lineheight = 10;
                            graph2.DrawString("PERBADANAN LABUAN", font, XBrushes.Black, new XRect(260, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph2.DrawString("(LABUAN CORPORATION)", font, XBrushes.Black, new XRect(260, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph2.DrawString("PETI SURAT 81245", font, XBrushes.Black, new XRect(260, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph2.DrawString("87022 WILLAYAH PERSEKUTUAN LABUAN", font, XBrushes.Black, new XRect(260, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph2.DrawString("Tel No 				:", font, XBrushes.Black, new XRect(260, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString("087 408600, 408601", font, XBrushes.Black, new XRect(360, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph2.DrawString("Faks No          :", font, XBrushes.Black, new XRect(260, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString("087 428997, 419400, 426803", font, XBrushes.Black, new XRect(360, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            lineheight = lineheight + 12;
                            XPen line1 = new XPen(XColors.Black, 2);
                            System.Drawing.Point pt10 = new System.Drawing.Point(0, lineheight);
                            System.Drawing.Point pt11 = new System.Drawing.Point(Convert.ToInt32(pdfPage2.Width), lineheight);
                            graph2.DrawLine(lineRed, pt10, pt11);
                            lineheight = lineheight + 15;

                            graph2.DrawString("Pengakuan Setuju Terima:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 25;
                            graph2.DrawString("1)  Saya bersetuju dengan keputusan permohonan ini dan segala maklumat yang  deberi adalah benar.", nfont, XBrushes.Black, new XRect(40, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph2.DrawString("2)  Saya bersetuju sekiranya maklumat deberi adalah palsu atau saya gagal mematuhi syarat-", nfont, XBrushes.Black, new XRect(40, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph2.DrawString("    syarat pengeluaran lesen, Perbadanan Labuan berhak untuk membatalkan keputusan lesen ini.", nfont, XBrushes.Black, new XRect(40, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 50;
                            cnt = 1;
                            foreach (var item6 in ctx.APP_L_INDs.Where(x => x.APP_ID == appId))
                            {
                                if (Convert.ToInt32(item6.IND_ID) > 0)
                                {
                                    foreach (var item7 in ctx.INDIVIDUALs.Where(x => x.IND_ID == item6.IND_ID))
                                    {
                                        if (item7.FULLNAME != null)
                                        {
                                            XPen pen1 = new XPen(XColors.Black, 1);
                                            System.Drawing.Point pt6 = new System.Drawing.Point(20, lineheight);
                                            System.Drawing.Point pt7 = new System.Drawing.Point(150, lineheight);
                                            graph2.DrawLine(lineRed, pt6, pt7);
                                            lineheight = lineheight + 5;
                                            graph2.DrawString(item7.FULLNAME, font, XBrushes.Black, new XRect(30, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                                            lineheight = lineheight + 35;
                                        }
                                    }
                                }
                            }


                            graph2.DrawString("s.k  Penolong Pengarah", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;




                            lineheight = lineheight + 10;
                            XPen lineRed1 = new XPen(XColors.Black, 1);
                            System.Drawing.Point pt4 = new System.Drawing.Point(0, lineheight);
                            System.Drawing.Point pt5 = new System.Drawing.Point(Convert.ToInt32(pdfPage2.Width), lineheight);
                            graph2.DrawLine(lineRed1, pt4, pt5);
                            lineheight = lineheight + 5;
                            graph2.DrawString("UNTUK KEGUNAAN PEJABAT", lbfont, XBrushes.Black, new XRect(200, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 30;
                            graph2.DrawString("NO. RUJUKAN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            if (item.REF_NO != null)
                            {
                                graph2.DrawString(item.REF_NO, font, XBrushes.Black, new XRect(300, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            }
                            //graph2.DrawString(DateTime.Now.Year.ToString() + "/BA/NEW/00000" + appId, font, XBrushes.Black, new XRect(300, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph2.DrawString("NAMA PERNIAGAAN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString(compName, font, XBrushes.Black, new XRect(300, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 40;
                            XPen pen = new XPen(XColors.Black, 1);
                            graph2.DrawRectangle(pen, 30, lineheight, 10, 10);
                            graph2.DrawString("Telah disemak dan disahkan betul", nfont, XBrushes.Black, new XRect(100, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph2.DrawRectangle(pen, 30, lineheight, 10, 10);
                            graph2.DrawString("Pembetulan semula", nfont, XBrushes.Black, new XRect(100, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 40;
                            System.Drawing.Point pt8 = new System.Drawing.Point(30, lineheight);
                            System.Drawing.Point pt9 = new System.Drawing.Point(200, lineheight);
                            graph2.DrawLine(lineRed1, pt8, pt9);
                            lineheight = lineheight + 5;
                            graph2.DrawString("(PENYELIA)", font, XBrushes.Black, new XRect(80, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 30;
                            graph2.DrawString("Tarikh      :", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph2.DrawString("Surat ini adalah cetakan komputer dan tandatangan tidak diperlukan", fontitalik, XBrushes.Black, new XRect(30, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            MemoryStream strm = new MemoryStream();
                            pdf.Save(strm, false);
                            return File(strm, "application/pdf");

                        }
                    }
                }
            }
            catch
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating Letter!');</script>");
        }

        private FileStreamResult GeneratePdf(Int32? appId)
        {
            try
            {

                return null;
            }

            catch
            {
                TempData["ErrorMessage"] = "Problem In Generating Letter.";
                return null;
            }
        }
        #endregion

        #region Generate Trade License PDF
        public ActionResult GeneratLicense(Int32? appId)
        {
            ApplicationModel applicationModel = new ApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var premiseApp = ctx.APPLICATIONs
                                        .Include("COMPANY").Where(x => x.APP_ID == appId).ToList();
                    if (premiseApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid Premise ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in premiseApp)
                        {
                            int lineheight = 50;
                            PdfDocument pdf = new PdfDocument();
                            pdf.Info.Title = "PDF License";
                            PdfPage pdfPage = pdf.AddPage();
                            XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                            XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                            XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                            XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                            XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);

                            XImage xImage = XImage.FromFile(Server.MapPath("~\\images\\wm1.png"));
                            graph.DrawImage(xImage, 0, 0, pdfPage.Width, pdfPage.Height);

                            XImage xImage1 = XImage.FromFile(Server.MapPath("~\\images\\logoPL.png"));
                            graph.DrawImage(xImage1, 10, 70, 100, 75);

                            graph.DrawString("TARIKH CETAKAN :", font, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(string.Format("{0:dd MMMM yyyy}", DateTime.Now), nfont, XBrushes.Black, new XRect(500, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            graph.DrawString("PERBADANAN LABUAN", lbfont, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Wisma Perbadanan Labuan", font, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Peti Surat 81245", font, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("87022 Willayah Persekutuan Labuan", font, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Tel No 		:", font, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("087-408692/600", font, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Faks No    :", font, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("087-408348", font, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph.DrawString("LESEN", lbfont, XBrushes.Black, new XRect(260, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("(Lesen ini dikeluarkan dibawah Undang-Undang Kecil Tred,Perniagaan dan Perindustrian Wilayah Persekutuan Labuan 2016)", sfont, XBrushes.Black, new XRect(50, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            lineheight = lineheight + 30;
                            string compName = "";
                            if (item.COMPANY.C_NAME == null)
                            {
                                compName = "";
                            }
                            else
                            {
                                compName = item.COMPANY.C_NAME;
                            }
                            graph.DrawString(compName, lbfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("SSM No", font, XBrushes.Black, new XRect(310, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.COMPANY.REG_NO != null)
                            {
                                graph.DrawString(item.COMPANY.REG_NO, nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }

                            lineheight = lineheight + 15;
                            graph.DrawString("No Rujukan", font, XBrushes.Black, new XRect(310, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(DateTime.Now.Year + "/PA/LB/" + string.Format("{0:0000000}", appId), nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            //if (item.REF_NO != null)
                            //{
                            //    graph.DrawString(DateTime.Now.Year + "/PA/LB" + string.Format("{0:000000}",appId) , nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            //}
                            string compAdd = "";
                            if (item.COMPANY.ADDRA1 == null)
                            {
                                compAdd = "";
                            }
                            else
                            {
                                compAdd = item.COMPANY.ADDRA1;
                            }
                            XTextFormatter tf = new XTextFormatter(graph);
                            XRect rect = new XRect(30, lineheight, 250, 30);
                            graph.DrawRectangle(XBrushes.Transparent, rect);
                            tf.DrawString(compAdd.ToString(), nfont, XBrushes.Black, rect, XStringFormats.TopLeft);

                            //graph.DrawString(compAdd.ToString(), nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;

                            graph.DrawString("Taraf Lesen", font, XBrushes.Black, new XRect(310, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.L_STATUS != null)
                            {
                                graph.DrawString(item.L_STATUS, nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }

                            string compPhone = "";
                            if (item.COMPANY.C_PHONE == null)
                            {
                                compPhone = "";
                            }
                            else
                            {
                                compPhone = item.COMPANY.C_PHONE;
                            }

                            //graph.DrawString(compPhone, nfont, XBrushes.Black, new XRect(30, lineheight+15, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Tempoh Sah", font, XBrushes.Black, new XRect(310, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.PAID != null && item.PAID.ToString() != "")
                            {
                                DateTime dt = new DateTime(item.PAID.Value.Year, item.PAID.Value.Month, item.PAID.Value.Day);
                                var mDate = string.Format("{0:dd MMMM yyyy}", dt);
                                graph.DrawString(mDate, nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                                graph.DrawString("Hingga", font, XBrushes.Black, new XRect(453, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                DateTime? hDate;
                                hDate = item.PAID;
                                if (Convert.ToInt32(item.MODE) == 1)
                                {
                                    hDate = hDate.Value.AddMonths(6);
                                }
                                else
                                {
                                    hDate = hDate.Value.AddMonths(12);
                                }
                                graph.DrawString(string.Format("{0:dd MMMM yyyy}", hDate), nfont, XBrushes.Black, new XRect(490, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 30;
                            graph.DrawRectangle(XBrushes.LightSalmon, 10, lineheight, pdfPage.Width - 30, 20);
                            graph.DrawString("MAKLUMAT LESEN", font, XBrushes.Black, new XRect(15, lineheight + 5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 30;

                            graph.DrawString("KOD AKTIVITI", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("KETERANGAN", font, XBrushes.Black, new XRect(((pdfPage.Width) / 2) - 70, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("AMAUN", font, XBrushes.Black, new XRect(510, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            XPen lineRed1 = new XPen(XColors.Black, 0.5);
                            System.Drawing.Point pt4 = new System.Drawing.Point(10, lineheight);
                            System.Drawing.Point pt5 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 20, lineheight);
                            graph.DrawLine(lineRed1, pt4, pt5);
                            lineheight = lineheight + 3;
                            float TotAmount = 0;
                            int TotHeight = 0;
                            foreach (var item1 in ctx.APP_L_BCs.Where(x => x.APP_ID == appId))
                            {
                                if (Convert.ToInt32(item1.BC_ID) > 0)
                                {
                                    foreach (var item2 in ctx.BCs.Where(x => x.BC_ID == item1.BC_ID))
                                    {
                                        if (item2.C_R != null)
                                        {
                                            graph.DrawString(item2.C_R, nfont, XBrushes.Black, new XRect(40, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                                        }
                                        if (item2.C_R_DESC != null)
                                        {
                                            XTextFormatter tf1 = new XTextFormatter(graph);
                                            XRect rect1 = new XRect(((pdfPage.Width) / 2) - 100, lineheight, 300, 30);
                                            graph.DrawRectangle(XBrushes.Transparent, rect1);
                                            tf1.DrawString(item2.C_R_DESC, nfont, XBrushes.Black, rect1, XStringFormats.TopLeft);
                                            //graph.DrawString(item2.CodeDesc, nfont, XBrushes.Black, new XRect(((pdfPage.Width) / 2) - 100, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                        float? Amount = 0;
                                        if (item2.BASE_FEE == 0)
                                        {
                                            Amount = (item2.DEF_RATE) * (item.P_AREA);
                                            Amount = (float)Math.Round((float)Amount, 1);
                                        }
                                        else
                                        {
                                            Amount = item2.BASE_FEE;
                                            Amount = (float)Math.Round((float)Amount, 1);
                                        }
                                        graph.DrawString("RM " + string.Format("{0:0.00}", Amount), nfont, XBrushes.Black, new XRect(510, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        lineheight = lineheight + 20;
                                        TotHeight = TotHeight + lineheight;
                                        XPen lineRed2 = new XPen(XColors.Black, 0.5);
                                        System.Drawing.Point pt6 = new System.Drawing.Point(10, lineheight);
                                        System.Drawing.Point pt7 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 20, lineheight);
                                        graph.DrawLine(lineRed1, pt6, pt7);
                                    }
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("JUMLAH", font, XBrushes.Black, new XRect(450, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("RM " + string.Format("{0:0.00}", TotAmount), nfont, XBrushes.Black, new XRect(510, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (TotAmount == 0)
                            {
                                lineheight = lineheight + 100;
                            }
                            else { lineheight = lineheight + 30; }

                            graph.DrawString("PEMILIK / PEKONGSI", font, XBrushes.Black, new XRect(20, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            Int32 cnt = 1;
                            foreach (var item3 in ctx.APP_L_INDs.Where(x => x.APP_ID == appId))
                            {
                                if (Convert.ToInt32(item3.IND_ID) > 0)
                                {
                                    foreach (var item4 in ctx.INDIVIDUALs.Where(x => x.IND_ID == item3.IND_ID))
                                    {
                                        {
                                            if (item4.FULLNAME != null)
                                            {
                                                string fName = item4.FULLNAME;
                                                string itm = cnt.ToString() + " .    " + fName;
                                                graph.DrawString(itm, nfont, XBrushes.Black, new XRect(20, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                                cnt = cnt + 1;
                                            }
                                            if (item4.MYKADNO != null)
                                            {
                                                graph.DrawString(item4.MYKADNO, nfont, XBrushes.Black, new XRect(480, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            }
                                            lineheight = lineheight + 15;
                                        }
                                    }
                                }
                            }
                            lineheight = lineheight + 50;
                            string str = "LESEN TAHUN  ";
                            if (item.APPROVE != null)
                            {
                                str = str + item.APPROVE.Value.Year;
                            }
                            graph.DrawString(str, font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph.DrawString("LESEN INI HENDAKLAH DIPAMERKAN", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            XPen lineRed3 = new XPen(XColors.Black, 0.5);
                            System.Drawing.Point pt8 = new System.Drawing.Point(400, lineheight - 20);
                            System.Drawing.Point pt9 = new System.Drawing.Point(570, lineheight - 20);
                            graph.DrawLine(lineRed1, pt8, pt9);
                            graph.DrawString("KETUA PEGAWAI EKSEKUTIF", font, XBrushes.Black, new XRect(420, lineheight - 15, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("PERBADANAN LABUAN ", font, XBrushes.Black, new XRect(430, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("Pihak Berkuasa Pelesenan", nfont, XBrushes.Black, new XRect(434, lineheight + 15, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            MemoryStream strm = new MemoryStream();
                            pdf.Save(strm, false);
                            return File(strm, "application/pdf");

                        }
                    }
                }
            }
            catch
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion

        #region Generate Banner License PDF

        public ActionResult GenerateLicense(Int32? appId)
        {
            ApplicationModel applicationModel = new ApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var qry = ctx.APP_L_INDs
                                    .Include("INDIVIDUAL").Where(x => x.APP_ID == appId);
                    var BannerApp = ctx.APPLICATIONs.Where(x => x.APP_ID == appId).ToList();
                    if (BannerApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid Banner ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in BannerApp)
                        {
                            int lineheight = 30;
                            PdfDocument pdf = new PdfDocument();
                            pdf.Info.Title = "PDF Letter";
                            PdfPage pdfPage = pdf.AddPage();
                            XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                            XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                            XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                            XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                            XFont nnfont = new XFont("Verdana", 8, XFontStyle.Regular);
                            XFont nlfont = new XFont("Verdana", 10, XFontStyle.Regular);
                            XImage xImage1 = XImage.FromFile(Server.MapPath("~\\images\\logoPL.png"));
                            graph.DrawImage(xImage1, 50, 50, 100, 75);


                            graph.DrawString("BORANG C", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 20;
                            graph.DrawString("JADUAL PERTAMA", nlfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 15;
                            graph.DrawString("ORDINAN KERAJAAN TEMPATAN 1961", nlfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 15;
                            graph.DrawString("AKTA WILAYAH PERSEKUTUAN LABUAN", nnfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 15;
                            graph.DrawString("(PINDAAN ORDINAN KERAJAAN TEMPATAN) 1993", nnfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 15;
                            graph.DrawString("PERINTAH WILAYAH PERSEKUTUAN LABUAN", nnfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 15;
                            graph.DrawString("(PENGUBAHSUAIAN ORDINAN KERAJAAN TEMPATAN) 1984", nnfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 25;
                            graph.DrawString("UNDANG-UNDANG KECIL PERBADANAN LABUAN (IKLAN) 1996", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 15;
                            graph.DrawString("[ Perenggan 8(1) ]", font, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 20;
                            graph.DrawString("LESEN IKLAN", font, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 20;
                            graph.DrawString("Nombor Resit:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            XPen lineRed1 = new XPen(XColors.Black, 0.5);
                            System.Drawing.Point pt1 = new System.Drawing.Point(97, lineheight + 13);
                            System.Drawing.Point pt2 = new System.Drawing.Point(290, lineheight + 13);
                            graph.DrawLine(lineRed1, pt1, pt2);
                           
                                    graph.DrawString(string.Format("{0:000000}", 1234), nfont, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                
                            graph.DrawString("Rujukan Fail:", nfont, XBrushes.Black, new XRect(291, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            System.Drawing.Point pt3 = new System.Drawing.Point(354, lineheight + 13);
                            System.Drawing.Point pt4 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                            graph.DrawLine(lineRed1, pt3, pt4);
                            if (item.REF_NO != null)
                            {
                                graph.DrawString(item.REF_NO, nfont, XBrushes.Black, new XRect(354, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("Nama:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            System.Drawing.Point pt5 = new System.Drawing.Point(61, lineheight + 13);
                            System.Drawing.Point pt6 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                            graph.DrawLine(lineRed1, pt5, pt6);
                            foreach (var item1 in qry)
                            {
                                if (item1.INDIVIDUAL.IND_ID > 0)
                                {
                                    graph.DrawString(item1.INDIVIDUAL.FULLNAME, nfont, XBrushes.Black, new XRect(63, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("Alamat:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            System.Drawing.Point pt7 = new System.Drawing.Point(68, lineheight + 13);
                            System.Drawing.Point pt8 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                            graph.DrawLine(lineRed1, pt7, pt8);
                            foreach (var item1 in qry)
                            {
                                if (item1.INDIVIDUAL.ADD_IC != null)
                                {
                                    graph.DrawString(item1.INDIVIDUAL.ADD_IC, nfont, XBrushes.Black, new XRect(70, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("adalah dilesenkan oleh Perbadanan Labuan untuk mempamerkan iklan/iklan-iklan berikut:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Int32 cnt = 1;

                            foreach (var item1 in ctx.B_Os.Where(x => x.APP_ID == item.APP_ID))
                            {
                                foreach (var item2 in ctx.BCs.Where(x => x.BC_ID == item1.BC_ID))
                                {
                                    var str = "";
                                    lineheight = lineheight + 25;
                                    if (item2.C_R_DESC != null)
                                    {
                                        str = str + item2.C_R_DESC + ",";
                                    }
                                    str = str + string.Format("{0:0.00}", item1.B_SIZE) + " meter persegi ";
                                    str = str + " x " + item1.B_QTY + " unit";
                                    graph.DrawString("(" + cnt.ToString() + ")", nfont, XBrushes.Black, new XRect(45, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    System.Drawing.Point pt9 = new System.Drawing.Point(61, lineheight + 13);
                                    System.Drawing.Point pt10 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                                    graph.DrawLine(lineRed1, pt9, pt10);
                                    graph.DrawString(str, nfont, XBrushes.Black, new XRect(61, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    cnt = cnt + 1;
                                }
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("di:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            string strLocDesc = "";
                            foreach (var item1 in ctx.B_Os.Where(x => x.APP_ID == item.APP_ID))
                            {
                                
                                        strLocDesc = strLocDesc + item1.ADDRB1 + ",";
                                
                            }
                            if (strLocDesc != "")
                            {
                                var mLen = (strLocDesc.Length) / 108;
                                Int32 TLen = 0;
                                if (mLen.ToString().Contains("."))
                                {
                                    mLen = Convert.ToInt32(mLen.ToString().Split('.')[0]) + 1;
                                }
                                for (int i = 0; i <= mLen; i++)
                                {
                                    if (i == 0)
                                    {
                                        TLen = 0;
                                    }
                                    else
                                    {
                                        TLen = (i * 108) + 1;
                                    }

                                    if (i == mLen)
                                    {
                                        Int32 sIndex = 0;
                                        Int32 EIndex = 0;
                                        sIndex = 0;
                                        EIndex = strLocDesc.Length;
                                        if (TLen > 0)
                                        {
                                            sIndex = TLen - 1;
                                        }
                                        if (TLen > 0)
                                        {
                                            EIndex = (strLocDesc.Length - TLen) + 1;
                                        }
                                        graph.DrawString(strLocDesc.Substring(sIndex, EIndex), nfont, XBrushes.Black, new XRect(44, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        System.Drawing.Point pt9 = new System.Drawing.Point(44, lineheight + 13);
                                        System.Drawing.Point pt10 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                                        graph.DrawLine(lineRed1, pt9, pt10);
                                    }
                                    else
                                    {
                                        graph.DrawString(strLocDesc.Substring(TLen, 108), nfont, XBrushes.Black, new XRect(44, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        System.Drawing.Point pt9 = new System.Drawing.Point(44, lineheight + 13);
                                        System.Drawing.Point pt10 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                                        graph.DrawLine(lineRed1, pt9, pt10);
                                    }
                                    lineheight = lineheight + 25;

                                }
                            }

                            if (strLocDesc != "")
                            {
                                lineheight = lineheight + 5;
                            }
                            else
                            { lineheight = lineheight + 25; }

                            graph.DrawString("mulai dari:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            System.Drawing.Point pt11 = new System.Drawing.Point(82, lineheight + 13);
                            System.Drawing.Point pt12 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                            graph.DrawLine(lineRed1, pt11, pt12);
                            if (item.APPROVE != null)
                            {
                                graph.DrawString(string.Format("{0:dd MMMM yyyy}", item.APPROVE), nfont, XBrushes.Black, new XRect(82, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("sehingga:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            System.Drawing.Point pt13 = new System.Drawing.Point(77, lineheight + 13);
                            System.Drawing.Point pt14 = new System.Drawing.Point(300, lineheight + 13);
                            graph.DrawLine(lineRed1, pt13, pt14);
                            if (item.APPROVE != null)
                            {
                                graph.DrawString(string.Format("{0:dd MMMM yyyy}", item.APPROVE.Value.AddYears(1)), nfont, XBrushes.Black, new XRect(77, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            //graph.DrawString("200", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            //pt13 = new System.Drawing.Point(322, lineheight + 13);
                            //pt14 = new System.Drawing.Point(360, lineheight + 13);
                            //graph.DrawLine(lineRed1, pt13, pt14);
                            graph.DrawString("tertakluk kepada syarat-syarat berikut:", nfont, XBrushes.Black, new XRect(335, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            cnt = 1;
                            foreach (var item3 in ctx.CATATANs.Where(x => x.APP_ID == item.APP_ID))
                            {
                                if (item3.CONTENT != null)
                                {
                                    if (lineheight >= 785)
                                    {
                                        lineheight = 20;
                                        pdfPage = pdf.AddPage();
                                        graph = XGraphics.FromPdfPage(pdfPage);
                                    }
                                    lineheight = lineheight + 25;
                                    graph.DrawString(("(" + cnt.ToString() + ") ") + item3.CONTENT, nfont, XBrushes.Black, new XRect(27, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    pt13 = new System.Drawing.Point(43, lineheight + 13);
                                    pt14 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                                    graph.DrawLine(lineRed1, pt13, pt14);
                                    cnt = cnt + 1;
                                }
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("Fee Lesen: RM", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt13 = new System.Drawing.Point(97, lineheight + 13);
                            pt14 = new System.Drawing.Point(250, lineheight + 13);
                            graph.DrawLine(lineRed1, pt13, pt14);
                            if (item.TOTAL_FEE != null)
                            {
                                graph.DrawString(string.Format("{0:0.00}", item.TOTAL_FEE), nfont, XBrushes.Black, new XRect(98, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 30;
                            graph.DrawString("Tarikh:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt13 = new System.Drawing.Point(62, lineheight + 13);
                            pt14 = new System.Drawing.Point(230, lineheight + 13);
                            graph.DrawLine(lineRed1, pt13, pt14);
                            if (item.PAID != null)
                            {
                                graph.DrawString(string.Format("{0:dd MMMM yyyy}", item.PAID), nfont, XBrushes.Black, new XRect(63, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            pt13 = new System.Drawing.Point(400, lineheight + 13);
                            pt14 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                            graph.DrawLine(lineRed1, pt13, pt14);
                            MemoryStream strm = new MemoryStream();
                            pdf.Save(strm, false);
                            return File(strm, "application/pdf");

                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating Letter!');</script>");
        }
        #endregion

        #region Generate Entertainment License PDF
        public ActionResult GeneratLicense_Entertainment(int? appId)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {

                    int lineheight = 30;
                    PdfDocument pdf = new PdfDocument();
                    pdf.Info.Title = "PDF License";
                    PdfPage pdfPage = pdf.AddPage();
                    XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                    XFont smaaFont = new XFont("Verdana", 8, XFontStyle.Regular);
                    XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                    XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                    XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                    XFont fontN10 = new XFont("Verdana", 10, XFontStyle.Regular);
                    XFont Italikfont = new XFont("Verdana", 11, XFontStyle.Italic);
                    XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                    XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                    XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);


                    graph.DrawString("Borang B", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("(Kaedah 5)", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 18;
                    graph.DrawString("AKTA HIBURAN (WILAYAH PERSEKUTUAN KUALA LUMPUR) 1992", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 22;
                    graph.DrawString("KAEDAN-KAEDAN HIBURAN (WILAYAH PERSEKUTUAN KUALA LUMPUR)", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 12;
                    graph.DrawString("1993", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("*LESEN UNTUK MEMBUKA TEMPAT HIBURAN/LESEN HIBURAN", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    XPen lineRed1 = new XPen(XColors.Black, 0.5);
                    System.Drawing.Point pt1;
                    System.Drawing.Point pt2;
                    lineheight = lineheight + 18;
                    graph.DrawString("Dengan ini  lesen dikeluarkan kepada", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(261, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("yang tinggal di", nfont, XBrushes.Black, new XRect(312, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(380, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 17;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    graph.DrawString("*untuk membuka suatu tempat bagi maksud mengadakan suatu", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(382, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) / 2, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("(ayatakan jenis hiburan/untuk menjalankan)", nfont, XBrushes.Black, new XRect((Convert.ToInt32(pdfPage.Width) / 2) + 2, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 18;
                    graph.DrawString("suatu", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(118, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) / 2, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("(nyatakan jenis hiburan) di ", nfont, XBrushes.Black, new XRect((Convert.ToInt32(pdfPage.Width) / 2) + 2, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point((Convert.ToInt32(pdfPage.Width) / 2) + 127, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(140, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("selama ", nfont, XBrushes.Black, new XRect(142, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(175, lineheight + 13);
                    pt2 = new System.Drawing.Point(215, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("*hari/tahun mulai dari", nfont, XBrushes.Black, new XRect(217, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(318, lineheight + 13);
                    pt2 = new System.Drawing.Point(370, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("hingga", nfont, XBrushes.Black, new XRect(372, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(405, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 25;
                    graph.DrawString("Lesen ini tertakluk kepada syarat-syarat yang berikut:", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;
                    graph.DrawString("(i)  Bahawa  tiada  pertunjukan boleh dimulakan sebelum", nfont, XBrushes.Black, new XRect(130, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(393, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 20;
                    graph.DrawString("(ii)  Bahawa  tiada  pertunjukan boleh diteruskan selepas", nfont, XBrushes.Black, new XRect(130, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(392, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 20;
                    graph.DrawString("(iii)  Bahawa  tidak lebih daripada", nfont, XBrushes.Black, new XRect(130, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(287, lineheight + 13);
                    pt2 = new System.Drawing.Point(455, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("orang. boleh", nfont, XBrushes.Black, new XRect(457, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("       dibenarkan masuk pada bila-bila masa ke tempat hiburan.", nfont, XBrushes.Black, new XRect(130, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;
                    graph.DrawString("(iv)  Bahawa ", nfont, XBrushes.Black, new XRect(130, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(185, lineheight + 13);
                    pt2 = new System.Drawing.Point(245, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("pintu dan", nfont, XBrushes.Black, new XRect(247, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(293, lineheight + 13);
                    pt2 = new System.Drawing.Point(465, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("jalan keluar", nfont, XBrushes.Black, new XRect(467, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("      disediakan.", nfont, XBrushes.Black, new XRect(130, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 18;
                    graph.DrawString("(v)  Bahawa pengawasan-pengawasan berikut dipatuhi:", nfont, XBrushes.Black, new XRect(130, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 18;
                    graph.DrawString("(a)", nfont, XBrushes.Black, new XRect(155, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(170, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    graph.DrawString("(b)", nfont, XBrushes.Black, new XRect(155, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(170, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    graph.DrawString("(vi)  +", nfont, XBrushes.Black, new XRect(130, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(159, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 27;
                    graph.DrawString("Fee bagi *lesen untuk membuka tempat hiburan/lesen hiburan ialah RM", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(422, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 40;
                    pt1 = new System.Drawing.Point(370, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("Dato Bandar Kuala Lumpur", nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;
                    graph.DrawString("Bertarikh", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(131, lineheight + 13);
                    pt2 = new System.Drawing.Point(250, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("* Potong mana-mana yang tidak berkenaan", smaaFont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 13;
                    graph.DrawString("* Masukkan syarat-syarat lanjut, jika ada. ", smaaFont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    MemoryStream strm = new MemoryStream();
                    pdf.Save(strm, false);
                    return File(strm, "application/pdf");

                }
            }
            catch (Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion

        #region Generate Stall License PDF
        public ActionResult GenerateStallLicense(Int32? appId)
        {
            ApplicationModel premiseApplicationModel = new ApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var StallApp = ctx.APPLICATIONs.Where(x => x.APP_ID == appId).ToList();
                    var indApp = ctx.APP_L_INDs.Where(y => y.APP_ID == appId).ToList();
                    if (StallApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid Stall ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in StallApp)
                        {
                            int lineheight = 50;
                            PdfDocument pdf = new PdfDocument();
                            pdf.Info.Title = "PDF License";
                            PdfPage pdfPage = pdf.AddPage();
                            XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                            XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                            XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                            XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                            XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                            XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);

                            XImage xImage1 = XImage.FromFile(Server.MapPath("~\\images\\logoPL.png"));
                            graph.DrawImage(xImage1, 30, 50, 100, 75);

                            graph.DrawString("PERBADANAN LABUAN", lbfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Wisma Perbadanan Labuan", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Peti Surat 81245", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("87022 Willayah Persekutuan Labuan", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Tel No 		", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(": 087-408692/596", nfont, XBrushes.Black, new XRect(205, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Faks No    ", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(": 087-408348", nfont, XBrushes.Black, new XRect(205, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("WEBSITE  ", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(": www.pl.gov.my", nfont, XBrushes.Black, new XRect(205, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 25;
                            XPen lineRed1 = new XPen(XColors.Black, 0.5);
                            System.Drawing.Point pt1 = new System.Drawing.Point(30, lineheight);
                            System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 10;
                            graph.DrawString("LESEN", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 15;
                            graph.DrawString("UNDANG-UNDANG KECIL PASAR(WP LABUAN) 2016", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 15;
                            graph.DrawString("PEMILIK", nUfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            XTextFormatter tf = new XTextFormatter(graph);
                            XRect rect = new XRect(30, lineheight, 100, 100);
                            graph.DrawRectangle(lineRed1, rect);

                            var individualUploadPath = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument), "Individual");
                            foreach (var item2 in indApp)
                            {
                                var individualActualPath = Path.Combine(individualUploadPath, item2.IND_ID.ToString("D6"));
                                var IndItm = ctx.INDIVIDUALs.Where(x => x.IND_ID == item2.IND_ID).ToList();
                                foreach (var item3 in IndItm)
                                {
                                    var indAtt = ctx.ATTACHMENTs.Where(y => y.ATT_ID == item3.ATT_ID).Select(y => y.FILENAME).FirstOrDefault();

                                    if (indAtt != null && indAtt.Count() > 0)
                                    {
                                        var individualActualPath1 = Path.Combine(individualActualPath, indAtt);
                                        if (System.IO.File.Exists(individualActualPath1))
                                        {
                                            xImage1 = XImage.FromFile(individualActualPath1);
                                            graph.DrawImage(xImage1, 30, lineheight, 100, 100);
                                        }
                                    }
                                }

                                graph.DrawString("NO.LESEN", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (item.REF_NO != null)
                                {
                                    graph.DrawString(item.REF_NO, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                graph.DrawString("Bayaran Lesen:", nfont, XBrushes.Black, new XRect(410, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (item.TOTAL_FEE != null)
                                {
                                    graph.DrawString("RM" + string.Format("{0:0.00}", item.TOTAL_FEE), nUfont, XBrushes.Black, new XRect(485, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("Nama Pemilik", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                foreach (var item3 in IndItm)
                                {
                                    if (item3.FULLNAME != null)
                                    {
                                        graph.DrawString(item3.FULLNAME, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }

                                    lineheight = lineheight + 15;
                                    graph.DrawString("NO.K/P", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    if (item3.MYKADNO != null)
                                    {
                                        graph.DrawString(item3.MYKADNO, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                    lineheight = lineheight + 15;
                                    graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                                    if (item3.ADD_IC != null)
                                    {
                                        if (item3.ADD_IC.ToString().Length > 55)
                                        {
                                            graph.DrawString(item3.ADD_IC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            lineheight = lineheight + 15;
                                            graph.DrawString(item3.ADD_IC.Substring(55, item3.ADD_IC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                        else
                                        {
                                            graph.DrawString(item3.ADD_IC, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                    }
                                }
                                lineheight = lineheight + 25;
                                graph.DrawString("NO.PREMIS", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (item.ADDRA1 != null)
                                {
                                    graph.DrawString(item.ADDRA1, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("MASA PERNIAGAAN", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (item.H_START > 0)
                                {
                                    graph.DrawString(item.H_START.ToString(), nUfont, XBrushes.Black, new XRect(307, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                System.Drawing.Point pt3 = new System.Drawing.Point(305, lineheight + 13);
                                System.Drawing.Point pt4 = new System.Drawing.Point(410, lineheight + 13);

                                lineheight = lineheight + 15;
                                graph.DrawString("JENIS JUALAN", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                var bcLink = ctx.APP_L_BCs.Where(a => a.APP_ID == item.APP_ID).Select(a => a.BC_ID).FirstOrDefault();

                                var scode = ctx.BCs.Where(b => b.BC_ID == bcLink).Select(b => b.C_R_DESC).FirstOrDefault();
                                
                                if (scode != null)
                                {
                                    graph.DrawString(scode, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("TEMPOH SAH LESEN", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (item.V_START != null && item.V_STOP != null)
                                {
                                    var strDate = string.Format("{0:dd MMMM yyyy}", item.V_START) + " - " + string.Format("{0:dd MMMM yyyy}", item.V_STOP);
                                    graph.DrawString(strDate, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                lineheight = lineheight + 30;
                                graph.DrawString("PEMBANTU", nUfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                lineheight = lineheight + 20;
                                tf = new XTextFormatter(graph);
                                rect = new XRect(30, lineheight, 100, 100);
                                graph.DrawRectangle(lineRed1, rect);
                                IndItm = ctx.INDIVIDUALs.Where(x => x.IND_ID == item.HELPERA).ToList();
                                foreach (var item3 in IndItm)
                                {
                                    var indAtt = ctx.ATTACHMENTs.Where(y => y.ATT_ID == item3.ATT_ID).Select(y => y.FILENAME).FirstOrDefault();

                                    if (indAtt != null && indAtt.Count() > 0)
                                    {
                                        individualActualPath = Path.Combine(individualUploadPath, IndItm[0].IND_ID.ToString("D6"));
                                        var individualActualPath2 = Path.Combine(individualActualPath, indAtt);
                                        if (System.IO.File.Exists(individualActualPath2))
                                        {
                                            xImage1 = XImage.FromFile(individualActualPath2);
                                            graph.DrawImage(xImage1, 30, lineheight, 100, 100);
                                        }
                                    }
                                }
                                lineheight = lineheight + 30;
                                graph.DrawString("NAMA PEMBANTU", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].FULLNAME != null)
                                    {
                                        graph.DrawString(IndItm[0].FULLNAME, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("NO.KP", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].MYKADNO != null)
                                    {
                                        graph.DrawString(IndItm[0].MYKADNO, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].ADD_IC != null)
                                    {
                                        if (IndItm[0].ADD_IC.ToString().Length > 55)
                                        {
                                            graph.DrawString(IndItm[0].ADD_IC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            lineheight = lineheight + 15;
                                            graph.DrawString(IndItm[0].ADD_IC.Substring(55, IndItm[0].ADD_IC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                        else
                                        {
                                            graph.DrawString(IndItm[0].ADD_IC, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                    }
                                }

                                lineheight = lineheight + 90;
                                tf = new XTextFormatter(graph);
                                rect = new XRect(30, lineheight, 100, 100);
                                graph.DrawRectangle(lineRed1, rect);
                                if (item.HELPERB == null)
                                {
                                    IndItm = ctx.INDIVIDUALs.Where(x => x.IND_ID == 0).ToList();
                                }
                                else
                                {
                                    IndItm = ctx.INDIVIDUALs.Where(x => x.IND_ID == item.HELPERB).ToList();
                                }
                                foreach (var item3 in IndItm)
                                {
                                    var indAtt = ctx.ATTACHMENTs.Where(y => y.ATT_ID == item3.ATT_ID).Select(y => y.FILENAME).FirstOrDefault();

                                    if (indAtt != null && indAtt.Count() > 0)
                                    {
                                        individualActualPath = Path.Combine(individualUploadPath, IndItm[0].IND_ID.ToString("D6"));
                                        var individualActualPath3 = Path.Combine(individualActualPath, indAtt);
                                        if (System.IO.File.Exists(individualActualPath3))
                                        {
                                            xImage1 = XImage.FromFile(individualActualPath3);
                                            graph.DrawImage(xImage1, 30, lineheight, 100, 100);
                                        }
                                    }
                                }

                                lineheight = lineheight + 30;
                                graph.DrawString("NAMA PEMBANTU", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].FULLNAME != null)
                                    {
                                        graph.DrawString(IndItm[0].FULLNAME, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("NO.KP", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].MYKADNO != null)
                                    {
                                        graph.DrawString(IndItm[0].MYKADNO, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].ADD_IC != null)
                                    {
                                        if (IndItm[0].ADD_IC.ToString().Length > 55)
                                        {
                                            graph.DrawString(IndItm[0].ADD_IC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            lineheight = lineheight + 15;
                                            graph.DrawString(IndItm[0].ADD_IC.Substring(55, IndItm[0].ADD_IC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                        else
                                        {
                                            graph.DrawString(IndItm[0].ADD_IC, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                    }
                                }
                                lineheight = lineheight + 75;
                                pt3 = new System.Drawing.Point(30, lineheight);
                                pt4 = new System.Drawing.Point(170, lineheight);
                                graph.DrawLine(lineRed1, pt3, pt4);
                                lineheight = lineheight + 5;
                                graph.DrawString("b.p.", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString("KETUA PEGAWAI EKSEKUTIF", nfont, XBrushes.Black, new XRect(50, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                lineheight = lineheight + 12;
                                graph.DrawString("PERBADANAN LABUAN", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                lineheight = lineheight + 20;
                                graph.DrawString("Tarikh:", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (item.APPROVE != null)
                                {
                                    graph.DrawString(string.Format("{0:dd MMMM yyyy}", item.APPROVE), nfont, XBrushes.Black, new XRect(70, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                pt3 = new System.Drawing.Point(65, lineheight + 13);
                                pt4 = new System.Drawing.Point(150, lineheight + 13);
                                graph.DrawLine(lineRed1, pt3, pt4);
                                lineheight = lineheight + 20;
                                graph.DrawString("***LESEN INI HENDAKLAH DIPAMERKAN", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);


                                MemoryStream strm = new MemoryStream();
                                pdf.Save(strm, false);
                                return File(strm, "application/pdf");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion

        #region Generate Money Lender Premise License PDF
        public ActionResult GeneratLicense_PremiseApp(int? appId)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {

                    int lineheight = 30;
                    PdfDocument pdf = new PdfDocument();
                    pdf.Info.Title = "PDF License";
                    PdfPage pdfPage = pdf.AddPage();
                    XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                    XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                    XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                    XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                    XFont fontN10 = new XFont("Verdana", 10, XFontStyle.Regular);
                    XFont Italikfont = new XFont("Verdana", 11, XFontStyle.Italic);
                    XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                    XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                    XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);


                    graph.DrawString("JADUAL B", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 18;
                    graph.DrawString("AKTA PEMBERI PINJAM  WANG 1951", Italikfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 25;
                    graph.DrawString("PERATURAN-PERATURAN PEMBERI PINJAM WANG", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 11;
                    graph.DrawString("(KAWALAN DAN PELESENAN) 2003", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 17;
                    graph.DrawString("(Perenggan 3(6))", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 25;
                    graph.DrawString("LESEN  PEMBERI PINJAM WANG", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 25;
                    graph.DrawString("Nama Pemberi Pinjam Wang", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    XPen lineRed1 = new XPen(XColors.Black, 0.5);
                    System.Drawing.Point pt1 = new System.Drawing.Point(219, lineheight + 13);
                    System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    graph.DrawString("alamat berdaftar", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(174, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("dengan ini diberikan lesen di bawah seksyen 5B Akta Pemberi Pinjam Wang 1951 untuk", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("menjalankan perniagaan meminjamkan wang di premis dinyatakan di bawah:", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 25;
                    graph.DrawString("Alamat perniagaan", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(179, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("Tertakluk kepada syarat-syarat yang berikut:", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 30;
                    XTextFormatter tf = new XTextFormatter(graph);
                    XRect rect = new XRect(90, lineheight, 230, 110);
                    graph.DrawRectangle(lineRed1, rect);
                    lineheight = lineheight + 5;
                    graph.DrawString("Butir-butir lesen", fontN10, XBrushes.Black, new XRect(160, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(90, lineheight + 15);
                    pt2 = new System.Drawing.Point(320, lineheight + 15);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 25;
                    graph.DrawString("No.Lesen:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(150, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    pt1 = new System.Drawing.Point(360, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("Pendaftar  Pemberi Pinjam Wang", fontN10, XBrushes.Black, new XRect(362, (lineheight + 15), pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    lineheight = lineheight + 23;
                    graph.DrawString("Sah dari:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(145, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 23;
                    graph.DrawString("hingga:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(138, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    graph.DrawString("Tarikh:", fontN10, XBrushes.Black, new XRect(360, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(395, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    pt1 = new System.Drawing.Point(90, lineheight + 50);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 50);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 60;
                    graph.DrawString("*Potong yang tidak berkenaan", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    MemoryStream strm = new MemoryStream();
                    pdf.Save(strm, false);
                    return File(strm, "application/pdf");

                }

            }
            catch (Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion

        #region Generate Money Lender Advertisement Permit License PDF
        public ActionResult GeneratLicense_PermitApp(int? appId)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {

                    int lineheight = 30;
                    PdfDocument pdf = new PdfDocument();
                    pdf.Info.Title = "PDF License";
                    PdfPage pdfPage = pdf.AddPage();
                    XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                    XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                    XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                    XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                    XFont fontN10 = new XFont("Verdana", 10, XFontStyle.Regular);
                    XFont Italikfont = new XFont("Verdana", 11, XFontStyle.Italic);
                    XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                    XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                    XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);


                    graph.DrawString("JADUAL G", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 18;
                    graph.DrawString("AKTA PEMBERI PINJAM  WANG 1951", Italikfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 25;
                    graph.DrawString("PERATURAN-PERATURAN PEMBERI PINJAM WANG", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 11;
                    graph.DrawString("(KAWALAN DAN PELESENAN) 2003", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 17;
                    graph.DrawString("(Subperturan 3(6))", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 25;
                    graph.DrawString("PERMIT IKLAN", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 25;
                    graph.DrawString("Nama Pemberi Pinjam Wang", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    XPen lineRed1 = new XPen(XColors.Black, 0.5);
                    System.Drawing.Point pt1 = new System.Drawing.Point(219, lineheight + 13);
                    System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    graph.DrawString("alamat berdaftar", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(174, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("dengan ini diberikan permit iklan bagi perniagaan meminjamkan wang di bawah", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("subperaturan 6(8) Peraturan-Peraturan Pemberi Pinjam Wang(Kawalan dan Pelesenan) ", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("2003 seperti yang dinyatakan di bawah-", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 25;
                    graph.DrawString("Alamat perniagaan", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(179, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 25;
                    graph.DrawString("Tertakluk kepada syarat-syarat yang berikut:", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;

                    XTextFormatter tf = new XTextFormatter(graph);
                    XRect rect = new XRect(90, lineheight, 438, 207);
                    graph.DrawRectangle(lineRed1, rect);

                    pt1 = new System.Drawing.Point(310, 284);
                    pt2 = new System.Drawing.Point(310, 491);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 5;
                    graph.DrawString("Jenis iklan", nfont, XBrushes.Black, new XRect(175, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    graph.DrawString("Tarikh kelulusan", nfont, XBrushes.Black, new XRect(365, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Akhbar", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Papan tanda", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Radio", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Internet", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Televisyen", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Cakera padat-video", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Panggung wayang", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Lain-lain(sila nyatakan)", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);


                    lineheight = lineheight + 30;
                    graph.DrawString("Syarat-Syarat lain:", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 30;
                    tf = new XTextFormatter(graph);
                    rect = new XRect(90, lineheight, 230, 110);
                    graph.DrawRectangle(lineRed1, rect);
                    lineheight = lineheight + 5;
                    graph.DrawString("Butir-butir permit", fontN10, XBrushes.Black, new XRect(160, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(90, lineheight + 15);
                    pt2 = new System.Drawing.Point(320, lineheight + 15);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 25;
                    graph.DrawString("No.Permit:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(150, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    pt1 = new System.Drawing.Point(360, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("Pendaftar  Pemberi Pinjam Wang", fontN10, XBrushes.Black, new XRect(362, (lineheight + 15), pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    lineheight = lineheight + 23;
                    graph.DrawString("Sah dari:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(145, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 23;
                    graph.DrawString("hingga:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(138, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    graph.DrawString("Tarikh:", fontN10, XBrushes.Black, new XRect(360, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(395, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    pt1 = new System.Drawing.Point(90, lineheight + 50);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 50);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 60;
                    graph.DrawString("*Potong yang tidak berkenaan", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    MemoryStream strm = new MemoryStream();
                    pdf.Save(strm, false);
                    return File(strm, "application/pdf");

                }
            }
            catch (Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion

        #region Generate Liquor License PDF
        public ActionResult GeneratLicense_RetailShop(Int32? appId)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    
                    int lineheight = 30;
                    PdfDocument pdf = new PdfDocument();
                    pdf.Info.Title = "PDF License";
                    PdfPage pdfPage = pdf.AddPage();
                    XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                    XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                    XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                    XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                    XFont lbnfont = new XFont("Verdana", 11, XFontStyle.Regular);
                    XFont fontN10 = new XFont("Verdana", 10, XFontStyle.Regular);
                    XFont Italikfont = new XFont("Verdana", 11, XFontStyle.Italic);
                    XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                    XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                    XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);


                    graph.DrawString("MALAYSIA", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("JADUAL KEDUA/SECOND SCHEDULE", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 18;
                    graph.DrawString("AKTA EKSAIS 1976", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 13;
                    graph.DrawString("(Seksyen 35 (1) (c))", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("CMS No.", fontN10, XBrushes.Black, new XRect(420, lineheight - 10, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    graph.DrawString("EXCISE ACT 1976", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 13;
                    graph.DrawString("(Section 35 (1) (C))", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("LESEN KEDAI RUNCIT", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 15;
                    graph.DrawString("RETAIL SHOP LICENCE", lbnfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 35;
                    graph.DrawString("Kuasa adalah dengan ini diberi kepada", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    XPen lineRed1 = new XPen(XColors.Black, 0.5);
                    System.Drawing.Point pt1 = new System.Drawing.Point(260, lineheight + 13);
                    System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    graph.DrawString("Authority is hereby granted to", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(300, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("untuk menjual secara runcit liquor yang", nfont, XBrushes.Black, new XRect(302, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 18;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(300, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("to sell by retail intoxicating liquors in the", nfont, XBrushes.Black, new XRect(302, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("memabukkan di premis yang diperihalkan di bawah ini dan terakluk kepada syarat-syarat yang", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("dinyatakan di bawah ini.", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("premises described below, and subject to the conditions also entered below,", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("Lesen ini adalah sah mulai dari hari pertama bulan ", nfont, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(355, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("This licence is valid from the first day of ", nfont, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(102, lineheight + 13);
                    pt2 = new System.Drawing.Point(120, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("hingga hari terakhir bulan", nfont, XBrushes.Black, new XRect(122, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(241, lineheight + 13);
                    pt2 = new System.Drawing.Point(330, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(332, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(344, lineheight + 13);
                    pt2 = new System.Drawing.Point(364, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("melainkan jika", nfont, XBrushes.Black, new XRect(366, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(102, lineheight + 13);
                    //pt2 = new System.Drawing.Point(120, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("until the last day of ", nfont, XBrushes.Black, new XRect(122, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(216, lineheight + 13);
                    //pt2 = new System.Drawing.Point(310, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(312, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(327, lineheight + 13);
                    //pt2 = new System.Drawing.Point(350, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("unless previously", nfont, XBrushes.Black, new XRect(352, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("digantung atau dibataiklan terdahulu daripada itu.", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("suspended or cancelled.", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;
                    graph.DrawString("Bayaran diterima RM", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(184, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("Fee received", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(153, lineheight + 13);
                    //pt2 = new System.Drawing.Point(310, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("Tarikh", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(121, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("Date", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 25;
                    pt1 = new System.Drawing.Point(380, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("Pegawai Daerah", nfont, XBrushes.Black, new XRect(408, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("District Officer", nfont, XBrushes.Black, new XRect(408, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 25;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("PERIHAL PREMIS / DESCRIPTION OF PREMISES", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 50;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("SYARAT-SYARAT LESEN /  CONDITIONS OF LICENCE", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 50;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    MemoryStream strm = new MemoryStream();
                    pdf.Save(strm, false);
                    return File(strm, "application/pdf");

                }
                //}
                //}
            }
            catch (Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }

        public ActionResult GeneratLicense_Wholesale(Int32? appId)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    //var MLApp = ctx.MLPremiseApplications
                    //                    .Where(x => x.MLPremiseApplicationID == appId).ToList();
                    //if (MLApp.Count == 0)
                    //{
                    //    return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid MLPremise ApplicationID!');</script>");
                    //}
                    //else
                    //{
                    //    foreach (var item in MLApp)
                    //    {
                    int lineheight = 30;
                    PdfDocument pdf = new PdfDocument();
                    pdf.Info.Title = "PDF License";
                    PdfPage pdfPage = pdf.AddPage();
                    XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                    XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                    XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                    XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                    XFont lbnfont = new XFont("Verdana", 11, XFontStyle.Regular);
                    XFont fontN10 = new XFont("Verdana", 10, XFontStyle.Regular);
                    XFont Italikfont = new XFont("Verdana", 11, XFontStyle.Italic);
                    XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                    XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                    XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);


                    graph.DrawString("MALAYSIA", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("JADUAL PERTAMA/FIRST SCHEDULE", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 18;
                    graph.DrawString("AKTA EKSAIS 1976", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 13;
                    graph.DrawString("(Seksyen 35 (1) (d))", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("No.", fontN10, XBrushes.Black, new XRect(420, lineheight - 10, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    graph.DrawString("EXCISE ACT 1976", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 13;
                    graph.DrawString("(Section 35 (1) (d))", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("LESEN PENIAGA BORONG", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 15;
                    graph.DrawString("WHOLESALE DEALERS'S LICENCE", lbnfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 35;
                    graph.DrawString("Kuasa adalah dengan ini diberi kepada", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    XPen lineRed1 = new XPen(XColors.Black, 0.5);
                    System.Drawing.Point pt1 = new System.Drawing.Point(260, lineheight + 13);
                    System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    graph.DrawString("Authority is hereby granted to", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(300, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("untuk menjual secara borong liquor yang", nfont, XBrushes.Black, new XRect(302, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 18;
                    //pt1 = new System.Drawing.Point(90, lineheight + 13);
                    //pt2 = new System.Drawing.Point(300, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("to sell by wholesale intoxicating liquors", nfont, XBrushes.Black, new XRect(302, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("memabukkan di premis yang diperihalkan di bawah ini dan terakluk kepada syarat-syarat yang", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("dinyatakan di bawah ini.", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("in the premises described below, and subject to the conditions also entered below,", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("Lesen ini adalah sah mulai dari hari pertama bulan ", nfont, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(355, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("This licence is valid from the first day of ", nfont, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(102, lineheight + 13);
                    pt2 = new System.Drawing.Point(120, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("hingga hari akhir bulan", nfont, XBrushes.Black, new XRect(122, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(235, lineheight + 13);
                    pt2 = new System.Drawing.Point(330, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(332, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(344, lineheight + 13);
                    pt2 = new System.Drawing.Point(364, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("melainkan jika", nfont, XBrushes.Black, new XRect(366, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(102, lineheight + 13);
                    //pt2 = new System.Drawing.Point(120, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("until the last day of ", nfont, XBrushes.Black, new XRect(122, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(216, lineheight + 13);
                    //pt2 = new System.Drawing.Point(310, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(312, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(327, lineheight + 13);
                    //pt2 = new System.Drawing.Point(350, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("unless previously", nfont, XBrushes.Black, new XRect(352, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("digantung atau dibataiklan terdahulu daripada itu.", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("suspended or cancelled.", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;
                    graph.DrawString("Bayaran diterima RM", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(184, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("Fee received", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(153, lineheight + 13);
                    //pt2 = new System.Drawing.Point(310, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("Tarikh", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(121, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("Date", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 25;
                    pt1 = new System.Drawing.Point(370, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("Pengerusi Lembaga Pelesenan", nfont, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("Wilayah Persekutuan Labuan", nfont, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 25;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("PERIHAL PREMIS / DESCRIPTION OF PREMISES", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 50;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("SYARAT-SYARAT LESEN /  CONDITIONS OF LICENCE", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 50;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    MemoryStream strm = new MemoryStream();
                    pdf.Save(strm, false);
                    return File(strm, "application/pdf");

                }
                //}
                //}
            }
            catch (Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }

        public ActionResult GeneratLicense_House(Int32? appId)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    //var MLApp = ctx.MLPremiseApplications
                    //                    .Where(x => x.MLPremiseApplicationID == appId).ToList();
                    //if (MLApp.Count == 0)
                    //{
                    //    return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid MLPremise ApplicationID!');</script>");
                    //}
                    //else
                    //{
                    //    foreach (var item in MLApp)
                    //    {
                    int lineheight = 30;
                    PdfDocument pdf = new PdfDocument();
                    pdf.Info.Title = "PDF License";
                    PdfPage pdfPage = pdf.AddPage();
                    XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                    XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                    XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                    XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                    XFont lbnfont = new XFont("Verdana", 11, XFontStyle.Regular);
                    XFont fontN10 = new XFont("Verdana", 10, XFontStyle.Regular);
                    XFont Italikfont = new XFont("Verdana", 11, XFontStyle.Italic);
                    XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                    XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                    XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);


                    graph.DrawString("MALAYSIA", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("JADUAL PERTAMA/THIRD SCHEDULE", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 18;
                    graph.DrawString("AKTA EKSAIS 1976", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 13;
                    graph.DrawString("(Seksyen 35 (1) (a) * / 35 (1) (b) *)", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("No.", fontN10, XBrushes.Black, new XRect(420, lineheight - 10, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    graph.DrawString("EXCISE ACT 1976", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 13;
                    graph.DrawString("(Section 35 (1) (a) * /35 (1)(b) *)", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("LESEN RUMAH AWAM * / LESEN RUMAH BIR *", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 15;
                    graph.DrawString("PUBLIC HOUSE LICENCE * / BEER HOUSE LICENCE *", lbnfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 35;
                    graph.DrawString("Lesen kelas", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    XPen lineRed1 = new XPen(XColors.Black, 0.5);
                    System.Drawing.Point pt1 = new System.Drawing.Point(143, lineheight + 13);
                    System.Drawing.Point pt2 = new System.Drawing.Point(260, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("ini adalah membenarkan", nfont, XBrushes.Black, new XRect(262, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(378, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("This", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    graph.DrawString("class licence authotities", nfont, XBrushes.Black, new XRect(262, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    lineheight = lineheight + 15;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(300, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("untuk menjual secara runcit liquor yang", nfont, XBrushes.Black, new XRect(302, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 18;
                    //pt1 = new System.Drawing.Point(90, lineheight + 13);
                    //pt2 = new System.Drawing.Point(300, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("to sell by retail intoxicating liquor", nfont, XBrushes.Black, new XRect(302, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("memabukkan untuk diminum hanya di premis diperihalkan di bawah ini dan bukan di tempat-", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("tempat lain.", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 18;
                    graph.DrawString("for consumption only on the premises described below, and not otherwise.", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("Lesen ini adalah sah mulai hari pertama bulan ", nfont, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(328, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("This licence is valid from the first day of ", nfont, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(102, lineheight + 13);
                    pt2 = new System.Drawing.Point(120, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("hingga hari akhir bulan", nfont, XBrushes.Black, new XRect(122, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(235, lineheight + 13);
                    pt2 = new System.Drawing.Point(330, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(332, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(344, lineheight + 13);
                    pt2 = new System.Drawing.Point(364, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString(",melainkan jika", nfont, XBrushes.Black, new XRect(366, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(102, lineheight + 13);
                    //pt2 = new System.Drawing.Point(120, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("until the last day of ", nfont, XBrushes.Black, new XRect(122, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(216, lineheight + 13);
                    //pt2 = new System.Drawing.Point(310, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("20", nfont, XBrushes.Black, new XRect(312, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(327, lineheight + 13);
                    //pt2 = new System.Drawing.Point(350, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString(",unless previously", nfont, XBrushes.Black, new XRect(352, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("digantung atau dibataiklan terdahulu daripada itu.", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("suspended or cancelled.", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;
                    graph.DrawString("Bayaran diterima RM", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(184, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("Fee received", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    //pt1 = new System.Drawing.Point(153, lineheight + 13);
                    //pt2 = new System.Drawing.Point(310, lineheight + 13);
                    //graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("Tarikh", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(121, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("Date", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 25;
                    pt1 = new System.Drawing.Point(370, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("Pengerusi Lembaga Pelesenan", nfont, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("Wilayah Persekutuan Labuan", nfont, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 25;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("* Potong yang mana tidak berkenaan", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 13;
                    graph.DrawString("Delete where inapplicable", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 20;
                    graph.DrawString("PERIHAL PREMIS/DESCRIPTION OF PREMISES", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 50;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 20;
                    graph.DrawString("SYARAT-STARAT LESEN /  CONDITIONS OF LICENCE", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 50;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 80, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    MemoryStream strm = new MemoryStream();
                    pdf.Save(strm, false);
                    return File(strm, "application/pdf");

                }
                //}
                //}
            }
            catch (Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion

        #region Generate Hawker License PDF
        public ActionResult GenerateHawkerLicense(Int32? appId)
        {
            ApplicationModel premiseApplicationModel = new ApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var StallApp = ctx.APPLICATIONs.Where(x => x.APP_ID == appId).ToList();
                    var indApp = ctx.APP_L_INDs.Where(y => y.APP_ID == appId).ToList();
                    if (StallApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid Hawker ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in StallApp)
                        {
                            int lineheight = 20;
                            PdfDocument pdf = new PdfDocument();
                            pdf.Info.Title = "PDF License";
                            PdfPage pdfPage = pdf.AddPage();
                            XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                            XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                            XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                            XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                            XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                            XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);

                            XImage xImage1 = XImage.FromFile(Server.MapPath("~\\images\\logoPL.png"));
                            graph.DrawImage(xImage1, 30, 50, 100, 75);

                            graph.DrawString("PERBADANAN LABUAN", lbfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Wisma Perbadanan Labuan", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Peti Surat 81245", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("87022 Willayah Persekutuan Labuan", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Tel No 		", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(": 087-408692/596", nfont, XBrushes.Black, new XRect(205, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Faks No    ", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(": 087-408348", nfont, XBrushes.Black, new XRect(205, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("WEBSITE  ", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(": www.pl.gov.my", nfont, XBrushes.Black, new XRect(205, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 25;
                            XPen lineRed1 = new XPen(XColors.Black, 0.5);
                            System.Drawing.Point pt1 = new System.Drawing.Point(30, lineheight);
                            System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 7;
                            graph.DrawString("LESEN", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 13;
                            graph.DrawString("UNDANG-UNDANG KECIL PELESENAN PENJAJA(WILAYAH PERSEKUTUAN LABUAN) 2016", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 20;


                            XTextFormatter tf = new XTextFormatter(graph);
                            XRect rect = new XRect(450, lineheight, 100, 100);
                            graph.DrawRectangle(lineRed1, rect);
                            foreach (var item2 in indApp)
                            {
                                var individualUploadPath = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument), "Individual");
                                var individualActualPath = Path.Combine(individualUploadPath, item2.IND_ID.ToString("D6"));
                                var IndItm = ctx.INDIVIDUALs.Where(x => x.IND_ID == item2.IND_ID).ToList();
                                foreach (var item3 in IndItm)
                                {
                                    if (IndItm != null && IndItm.Count() > 0)
                                    {
                                        if (IndItm[0].ATT_ID != null)
                                        {
                                            var indAtt = ctx.ATTACHMENTs.Where(y => y.ATT_ID == item3.ATT_ID).Select(y => y.FILENAME).FirstOrDefault();
                                            var individualActualPath1 = Path.Combine(individualActualPath, indAtt);
                                            if (System.IO.File.Exists(individualActualPath1))
                                            {
                                                xImage1 = XImage.FromFile(individualActualPath1);
                                                graph.DrawImage(xImage1, 450, lineheight, 100, 100);
                                            }
                                        }
                                    }

                                    graph.DrawString("NO.LESEN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    if (item.REF_NO != null)
                                    {
                                        graph.DrawString(item.REF_NO, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }

                                    lineheight = lineheight + 15;
                                    graph.DrawString("Nama Pemilik", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    if (item3.FULLNAME != null)
                                    {
                                        graph.DrawString(item3.FULLNAME, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }

                                    lineheight = lineheight + 15;
                                    graph.DrawString("NO.K/P", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    if (item3.MYKADNO != null)
                                    {
                                        graph.DrawString(item3.MYKADNO, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                    lineheight = lineheight + 15;
                                    graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                                    if (item3.ADD_IC != null)
                                    {
                                        if (item3.ADD_IC.ToString().Length > 55)
                                        {
                                            graph.DrawString(item3.ADD_IC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            lineheight = lineheight + 15;
                                            graph.DrawString(item3.ADD_IC.Substring(55, item3.ADD_IC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                        else
                                        {
                                            graph.DrawString(item3.ADD_IC, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                    }
                                }
                                lineheight = lineheight + 25;
                                graph.DrawString("JENIS LESEN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                var bcLink = ctx.APP_L_BCs.Where(a => a.APP_ID == item.APP_ID).Select(a => a.BC_ID).FirstOrDefault();
                                var hcode = ctx.BCs.Where(b => b.BC_ID == bcLink);
                                foreach (var item3 in hcode)
                                {
                                    if (hcode != null)
                                    {
                                        graph.DrawString(item3.C_R_DESC, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                    lineheight = lineheight + 15;
                                    graph.DrawString("TEMPOH SAH LESEN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    string HW = item3.PERIOD_Q.ToString();
                                    if (item3.PERIOD == 1)
                                    {
                                        HW = HW + " " + "Tahun";
                                    }
                                    else if (item3.PERIOD == 2)
                                    {
                                        HW = HW + " " + "Bulan";
                                    }
                                    else if (item3.PERIOD == 3)
                                    {
                                        HW = HW + " " + "Minggu";
                                    }
                                    else if (item3.PERIOD == 4)
                                    {
                                        HW = HW + " " + "Hari";
                                    }

                                    graph.DrawString(HW, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("JENIS BARANG DIJAJA", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (item.GOODS_TYPE != null)
                                {
                                    graph.DrawString(item.GOODS_TYPE, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("MASA PERNIAGAAN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (item.H_START > 0)
                                {
                                    graph.DrawString(item.H_START.ToString(), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("LOKASI PERNIAGAAN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (item.ADDRA1 != null)
                                {
                                    graph.DrawString(item.ADDRA1, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }

                                lineheight = lineheight + 20;

                                tf = new XTextFormatter(graph);
                                rect = new XRect(450, lineheight, 100, 100);
                                graph.DrawRectangle(lineRed1, rect);
                                IndItm = ctx.INDIVIDUALs.Where(x => x.IND_ID == item.HELPERA).ToList();
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    var indAtt = ctx.ATTACHMENTs.Where(y => y.ATT_ID == IndItm[0].ATT_ID).Select(y => y.FILENAME).FirstOrDefault();
                                    if (indAtt != null)
                                    {
                                        individualActualPath = Path.Combine(individualUploadPath, IndItm[0].IND_ID.ToString("D6"));
                                        var individualActualPath2 = Path.Combine(individualActualPath, indAtt);
                                        if (System.IO.File.Exists(individualActualPath2))
                                        {
                                            xImage1 = XImage.FromFile(individualActualPath2);
                                            graph.DrawImage(xImage1, 450, lineheight, 100, 100);
                                        }
                                    }
                                }
                                lineheight = lineheight + 20;
                                graph.DrawString("NAMA PEMBANTU", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].FULLNAME != null)
                                    {
                                        graph.DrawString(IndItm[0].FULLNAME, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("NO.KP", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].MYKADNO != null)
                                    {
                                        graph.DrawString(IndItm[0].MYKADNO, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].ADD_IC != null)
                                    {
                                        if (IndItm[0].ADD_IC.ToString().Length > 55)
                                        {
                                            graph.DrawString(IndItm[0].ADD_IC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            lineheight = lineheight + 15;
                                            graph.DrawString(IndItm[0].ADD_IC.Substring(55, IndItm[0].ADD_IC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                        else
                                        {
                                            graph.DrawString(IndItm[0].ADD_IC, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                    }
                                }

                                lineheight = lineheight + 70;
                                tf = new XTextFormatter(graph);
                                rect = new XRect(450, lineheight, 100, 100);
                                graph.DrawRectangle(lineRed1, rect);
                                if (item.HELPERB == null)
                                {
                                    IndItm = ctx.INDIVIDUALs.Where(x => x.IND_ID == 0).ToList();
                                }
                                else
                                {
                                    IndItm = ctx.INDIVIDUALs.Where(x => x.IND_ID == item.HELPERB).ToList();
                                }
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    var indAtt = ctx.ATTACHMENTs.Where(y => y.ATT_ID == IndItm[0].ATT_ID).Select(y => y.FILENAME).FirstOrDefault();
                                    if (IndItm[0].ATT_ID != null && indAtt != null)
                                    {
                                        individualActualPath = Path.Combine(individualUploadPath, IndItm[0].IND_ID.ToString("D6"));
                                        var individualActualPath3 = Path.Combine(individualActualPath, indAtt);
                                        if (System.IO.File.Exists(individualActualPath3))
                                        {
                                            xImage1 = XImage.FromFile(individualActualPath3);
                                            graph.DrawImage(xImage1, 450, lineheight, 100, 100);
                                        }
                                    }
                                }

                                lineheight = lineheight + 15;
                                graph.DrawString("NAMA PEMBANTU", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].FULLNAME != null)
                                    {
                                        graph.DrawString(IndItm[0].FULLNAME, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("NO.KP", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].MYKADNO != null)
                                    {
                                        graph.DrawString(IndItm[0].MYKADNO, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].ADD_IC != null)
                                    {
                                        if (IndItm[0].ADD_IC.ToString().Length > 55)
                                        {
                                            graph.DrawString(IndItm[0].ADD_IC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            lineheight = lineheight + 15;
                                            graph.DrawString(IndItm[0].ADD_IC.Substring(55, IndItm[0].ADD_IC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                        else
                                        {
                                            graph.DrawString(IndItm[0].ADD_IC, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                    }
                                }

                                lineheight = lineheight + 70;
                                tf = new XTextFormatter(graph);
                                rect = new XRect(450, lineheight, 100, 100);
                                graph.DrawRectangle(lineRed1, rect);
                                if (item.HELPERC == null)
                                {
                                    IndItm = ctx.INDIVIDUALs.Where(x => x.IND_ID == 0).ToList();
                                }
                                else
                                {
                                    IndItm = ctx.INDIVIDUALs.Where(x => x.IND_ID == item.HELPERC).ToList();
                                }
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    var indAtt = ctx.ATTACHMENTs.Where(y => y.ATT_ID == IndItm[0].ATT_ID).Select(y => y.FILENAME).FirstOrDefault();
                                    if (IndItm[0].ATT_ID != null && indAtt != null)
                                    {
                                        individualActualPath = Path.Combine(individualUploadPath, IndItm[0].IND_ID.ToString("D6"));
                                        var individualActualPath3 = Path.Combine(individualActualPath, indAtt);
                                        if (System.IO.File.Exists(individualActualPath3))
                                        {
                                            xImage1 = XImage.FromFile(individualActualPath3);
                                            graph.DrawImage(xImage1, 450, lineheight, 100, 100);
                                        }
                                    }
                                }

                                lineheight = lineheight + 15;
                                graph.DrawString("NAMA PEMBANTU", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].FULLNAME != null)
                                    {
                                        graph.DrawString(IndItm[0].FULLNAME, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("NO.KP", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].MYKADNO != null)
                                    {
                                        graph.DrawString(IndItm[0].MYKADNO, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                                lineheight = lineheight + 15;
                                graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].ADD_IC != null)
                                    {
                                        if (IndItm[0].ADD_IC.ToString().Length > 55)
                                        {
                                            graph.DrawString(IndItm[0].ADD_IC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            lineheight = lineheight + 15;
                                            graph.DrawString(IndItm[0].ADD_IC.Substring(55, IndItm[0].ADD_IC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                        else
                                        {
                                            graph.DrawString(IndItm[0].ADD_IC, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                    }
                                }

                                lineheight = lineheight + 65;
                                System.Drawing.Point pt3 = new System.Drawing.Point(30, lineheight);
                                System.Drawing.Point pt4 = new System.Drawing.Point(170, lineheight);
                                graph.DrawLine(lineRed1, pt3, pt4);
                                lineheight = lineheight + 5;
                                graph.DrawString("b.p.", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                graph.DrawString("KETUA PEGAWAI EKSEKUTIF", nfont, XBrushes.Black, new XRect(50, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                lineheight = lineheight + 12;
                                graph.DrawString("PERBADANAN LABUAN", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                lineheight = lineheight + 20;
                                graph.DrawString("Tarikh:", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                if (item.APPROVE != null)
                                {
                                    graph.DrawString(string.Format("{0:dd MMMM yyyy}", item.APPROVE), nfont, XBrushes.Black, new XRect(67, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                pt3 = new System.Drawing.Point(65, lineheight + 13);
                                pt4 = new System.Drawing.Point(160, lineheight + 13);
                                graph.DrawLine(lineRed1, pt3, pt4);
                                lineheight = lineheight + 20;
                                graph.DrawString("***LESEN INI HENDAKLAH DIPAMERKAN", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);


                                MemoryStream strm = new MemoryStream();
                                pdf.Save(strm, false);
                                return File(strm, "application/pdf");

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion

        #region Upload Document
        /// <summary>
        /// Save Attachment Information
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadDocument(HttpPostedFileBase documentFile)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    if (documentFile != null)
                    {
                        var file = documentFile;
                        if (file.ContentLength > 0)
                        {
                            var premisevalue = Request["APP_ID"];
                            var licDocvalue = Request["licDocid"];
                            var reqDocvalue = Request["reqDocid"];
                            var addDocvalue = Request["addDocid"];
                            var isReqvalue = Request["isReqDoc"];

                            int premiseApplicationId;
                            if (int.TryParse(premisevalue, out premiseApplicationId) && premiseApplicationId > 0)
                            {
                                int requiredDocId;
                                if (reqDocvalue != null)
                                {
                                    int.TryParse(reqDocvalue, out requiredDocId);
                                    if (requiredDocId > 0)
                                    {
                                        int isReq;
                                        int.TryParse(isReqvalue, out isReq);

                                        var fileName = Path.GetFileName(file.FileName);

                                        var folder = Server.MapPath("~/Documents/Attachment/Application/" + premiseApplicationId.ToString());
                                        var path = Path.Combine(folder, fileName);
                                        if (!Directory.Exists(folder))
                                        {
                                            Directory.CreateDirectory(folder);
                                        }
                                        file.SaveAs(path);

                                        ATTACHMENT attachment = new ATTACHMENT();
                                        attachment.FILENAME = fileName;
                                        ctx.ATTACHMENTs.AddOrUpdate(attachment);
                                        ctx.SaveChanges();

                                        if (attachment.ATT_ID > 0)
                                        {

                                            APP_L_RD paLinkReqDoc;
                                            paLinkReqDoc = ctx.APP_L_RDs.FirstOrDefault(p => p.APP_ID == premiseApplicationId && p.RD_ID == requiredDocId);
                                            if (paLinkReqDoc != null)
                                            {
                                                paLinkReqDoc.APP_ID = premiseApplicationId;
                                                paLinkReqDoc.RD_ID = requiredDocId;
                                                paLinkReqDoc.RD_TYPE = 1;
                                                paLinkReqDoc.ATT_ID = attachment.ATT_ID;
                                                ctx.APP_L_RDs.AddOrUpdate(paLinkReqDoc);
                                                ctx.SaveChanges();
                                            }
                                           

                                            return Json(new { status = "1", message = "Document Upload Successfully" }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    if (licDocvalue != null)
                                    {
                                        int.TryParse(licDocvalue, out requiredDocId);
                                        if (requiredDocId > 0)
                                        {
                                            int isReq;
                                            int.TryParse(isReqvalue, out isReq);

                                            var fileName = Path.GetFileName(file.FileName);

                                            var folder = Server.MapPath("~/Documents/Attachment/Application/" + premiseApplicationId.ToString());
                                            var path = Path.Combine(folder, fileName);
                                            if (!Directory.Exists(folder))
                                            {
                                                Directory.CreateDirectory(folder);
                                            }
                                            file.SaveAs(path);

                                            ATTACHMENT attachment = new ATTACHMENT();
                                            attachment.FILENAME = fileName;
                                            ctx.ATTACHMENTs.AddOrUpdate(attachment);
                                            ctx.SaveChanges();

                                            if (attachment.ATT_ID > 0)
                                            {

                                                APP_L_RD paLinkReqDoc;
                                                paLinkReqDoc = ctx.APP_L_RDs.FirstOrDefault(p => p.APP_ID == premiseApplicationId && p.RD_ID == requiredDocId);
                                                if (paLinkReqDoc != null)
                                                {
                                                    paLinkReqDoc.APP_ID = premiseApplicationId;
                                                    paLinkReqDoc.RD_ID = requiredDocId;
                                                    paLinkReqDoc.RD_TYPE = 3;
                                                    paLinkReqDoc.ATT_ID = attachment.ATT_ID;
                                                    ctx.APP_L_RDs.AddOrUpdate(paLinkReqDoc);
                                                    ctx.SaveChanges();
                                                }

                                                return Json(new { status = "1", message = "Document Upload Successfully" }, JsonRequestBehavior.AllowGet);
                                            }
                                        }
                                        if (addDocvalue != null)
                                        {
                                            int.TryParse(addDocvalue, out requiredDocId);
                                            if (requiredDocId > 0)
                                            {
                                                int isReq;
                                                int.TryParse(isReqvalue, out isReq);

                                                var fileName = Path.GetFileName(file.FileName);

                                                var folder = Server.MapPath("~/Documents/Attachment/Application/" + premiseApplicationId.ToString());
                                                var path = Path.Combine(folder, fileName);
                                                if (!Directory.Exists(folder))
                                                {
                                                    Directory.CreateDirectory(folder);
                                                }
                                                file.SaveAs(path);

                                                ATTACHMENT attachment = new ATTACHMENT();
                                                attachment.FILENAME = fileName;
                                                ctx.ATTACHMENTs.AddOrUpdate(attachment);
                                                ctx.SaveChanges();

                                                if (attachment.ATT_ID > 0)
                                                {

                                                    APP_L_RD paLinkReqDoc;
                                                    paLinkReqDoc = ctx.APP_L_RDs.FirstOrDefault(p => p.APP_ID == premiseApplicationId && p.RD_ID == requiredDocId);
                                                    if (paLinkReqDoc != null)
                                                    {
                                                        paLinkReqDoc.APP_ID = premiseApplicationId;
                                                        paLinkReqDoc.RD_ID = requiredDocId;
                                                        paLinkReqDoc.RD_TYPE = 2;
                                                        paLinkReqDoc.ATT_ID = attachment.ATT_ID;
                                                        ctx.APP_L_RDs.AddOrUpdate(paLinkReqDoc);
                                                        ctx.SaveChanges();
                                                    }

                                                    return Json(new { status = "1", message = "Document Upload Successfully" }, JsonRequestBehavior.AllowGet);
                                                }
                                            }


                                            return Json(new { status = "2", message = "Error While Saving Record" }, JsonRequestBehavior.AllowGet);
                                        }

                                        return Json(new { status = "2", message = "Data Missing" }, JsonRequestBehavior.AllowGet);
                                    }

                                    return Json(new { status = "2", message = "Data Missing" }, JsonRequestBehavior.AllowGet);
                                }

                                return Json(new { status = "2", message = "Please select File" }, JsonRequestBehavior.AllowGet);
                            }
                            return Json(new { status = "2", message = "Please select File" }, JsonRequestBehavior.AllowGet);
                        }
                        return Json(new { status = "2", message = "Please select File" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = "2", message = "Please select File" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
                //return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Save Attachment/ UploadAttechment
        /// <summary>
        /// Save Attachment Information
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadAttechment(HttpPostedFileBase documentFile)
        {
            //todo: this method is hard to understasnd, a lot of commented code
            //todo and too similar with method UploadDocument
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    if (documentFile != null)
                    {
                        var file = documentFile;
                        if (file.ContentLength > 0)
                        {
                            var licDocvalue = Request["licDocid"];
                            var reqDocvalue = Request["reqDocid"];
                            var addDocvalue = Request["addDocid"];
                            var isReqvalue = Request["isReqDoc"];

                            int licenseDocId;
                            int.TryParse(licDocvalue, out licenseDocId);

                            int requiredDocId;
                            int.TryParse(reqDocvalue, out requiredDocId);

                            int additionalDocId;
                            int.TryParse(addDocvalue, out additionalDocId);

                            if (requiredDocId > 0 || additionalDocId > 0 || licenseDocId > 0)
                            {
                                int isReq;
                                int.TryParse(isReqvalue, out isReq);

                                var fileName = Path.GetFileName(file.FileName);

                                var folder = Server.MapPath(ProjectConfiguration.AttachmentDocument);
                                var path = Path.Combine(folder, fileName);
                                if (!Directory.Exists(folder))
                                {
                                    Directory.CreateDirectory(folder);
                                }
                                file.SaveAs(path);

                                ATTACHMENT attachment = new ATTACHMENT();
                                attachment.FILENAME = fileName;
                                ctx.ATTACHMENTs.AddOrUpdate(attachment);
                                ctx.SaveChanges();

                                if (attachment.ATT_ID > 0)
                                {
                                    if (licenseDocId > 0)
                                    {


                                        return Json(new { status = "1", result = new { status = "1", LicenseDocID = licenseDocId, AttachmentID = attachment.ATT_ID, AttachmentName = attachment.FILENAME } }, JsonRequestBehavior.AllowGet);
                                    }
                                    if (requiredDocId > 0)
                                    {
                                        

                                        return Json(new { status = "1", result = new { status = "1", RequiredDocID = requiredDocId, AttachmentID = attachment.ATT_ID, AttachmentName = attachment.FILENAME } }, JsonRequestBehavior.AllowGet);
                                    }
                                    if(additionalDocId > 0)
                                    {

                                        return Json(new { status = "1", result = new { status = "1", AdditionalDocID = additionalDocId, AttachmentID = attachment.ATT_ID, AttachmentName = attachment.FILENAME } }, JsonRequestBehavior.AllowGet);
                                    }

                                    //return Json(new { status = "1", message = "Document Upload Successfully" }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { status = "2", message = "Error While Saving Record" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json(new { status = "2", message = "Data Missing" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { status = "2", message = "Please select File" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    return Json(new { status = "2", message = "Please select File" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Download Attachment Files
        /// <summary>
        /// Download
        /// </summary>
        /// <param name="attechmentId"></param>
        /// /// <param name="premiseId"></param>
        /// <returns></returns>
        public FileResult Download(int? attechmentId, int? premiseId)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var attechment = ctx.ATTACHMENTs.FirstOrDefault(a => a.ATT_ID == attechmentId);
                var folder = Server.MapPath("~/Documents/Attachment/Application/" + premiseId.ToString());
                try
                {
                    try
                    {
                        if (attechment != null && attechment.ATT_ID > 0)
                        {
                            var path = Path.Combine(folder, attechment.FILENAME);
                            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, attechment.FILENAME);
                        }

                        return null;
                    }
                    catch
                    {
                        // todo: this is very bad code with empty catch. Log or write or do anything to notify  about error

                    }
                }
                catch
                {
                    // todo: this is very bad code with empty catch. Log or write or do anything to notify  about error

                }
                return null;
            }
        }
        #endregion

        #region Meeting List Grid
        /// <summary>
        /// GET: Meeting
        /// </summary>
        /// <returns></returns>
        [AuthorizationPrivilegeFilter(SystemEnum.Page.Application, SystemEnum.PageRight.CrudLevel)]
        public ActionResult Meeting()
        {
            return View();
        }

        /// <summary>
        /// Get Meeting Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>

        /// <returns></returns>
        [HttpPost]
        public JsonResult Meeting([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            List<APP_L_MTModel> Meeting;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                int? rollTemplateID = ProjectSession.User?.ROLEID;
                IQueryable<APP_L_MT> query = ctx.APP_L_MTs;

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<APP_L_MTModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "MT_DATE desc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                Meeting = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, Meeting, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ManageMeeting
        /// <summary>
        /// Get Application Data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ManageMeeting(int? id)
        {
            APP_L_MTModel meetingModel = new APP_L_MTModel();
            using (var ctx = new LicenseApplicationContext())
            {
                if (id != null && id > 0)
                {

                    var meeting = ctx.APP_L_MTs.Where(a => a.APP_L_MTID == id);
                    meetingModel = Mapper.Map<APP_L_MTModel>(meeting);

                }
                else
                {
                    meetingModel.MT_DATE = DateTime.Today;
                    meetingModel.USERSID = ProjectSession.UserID;
                    meetingModel.CREATED = DateTime.Now;
                }
            }
            return View(meetingModel);
        }

        /// <summary>
        /// Get Application Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="ApplicationId">The premise application identifier.</param>
        /// <param name="individualMkNo">The individual mk no.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ManageMeeting([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            List<ApplicationModel> Application;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                int? rollTemplateID = ProjectSession.User?.ROLEID;
                IQueryable<APPLICATION> query = ctx.APPLICATIONs.Where(a => a.APPSTATUSID == (int)Enums.PAStausenum.directorcheck);

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

        #region Save data from ManageMeeting
        private bool SaveMeeting(APP_L_MTModel meetingModel, LicenseApplicationContext ctx)
        {

            var meeting = Mapper.Map<APP_L_MT>(meetingModel);           
            ctx.APP_L_MTs.AddOrUpdate(meeting);
            ctx.SaveChanges();

            return true;

        }
        #endregion

    }
}