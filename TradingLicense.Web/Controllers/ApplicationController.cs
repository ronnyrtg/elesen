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
        private Func<DEPARTMENT, Select2ListItem> fnSelectDepartment = de => new Select2ListItem { id = de.DEP_ID, text = $"{de.DEP_CODE}~{de.DEP_DESC}" };
        private Func<E_P_FEE, Select2ListItem> fnSelectPremiseFee = ep => new Select2ListItem { id = ep.E_P_FEEID, text = $"{ep.E_P_DESC}~{ep.E_S_DESC}" };
        private Func<INDIVIDUAL, Select2ListItem> fnSelectIndividualFormat = ind => new Select2ListItem { id = ind.IND_ID, text = $"{ind.FULLNAME} ({ind.MYKADNO})" };

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
                                            .Where(pa => pa.DEP_ID == departmentID.Value && pa.APP_TYPE == (int)Enums.ApplicationTypeID.TradeApplication && pa.ACTIVE)
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

                    var paLinkReqDocumentList = ctx.APP_L_RDs.Where(p => p.APP_ID == id).ToList();
                    if (paLinkReqDocumentList.Count > 0)
                    {
                        applicationModel.UploadRequiredDocids = (string.Join(",", paLinkReqDocumentList.Select(x => x.RD_ID.ToString() + ":" + x.ATT_ID.ToString()).ToArray()));
                    }

                    var paLinkAddDocumentlist = ctx.APP_L_RDs.Where(p => p.APP_ID == id).ToList();
                    if (paLinkAddDocumentlist.Count > 0)
                    {
                        applicationModel.UploadAdditionalDocids = (string.Join(",", paLinkAddDocumentlist.Select(x => x.RD_ID.ToString() + ":" + x.ATT_ID.ToString()).ToArray()));
                    }

                    if (application.APPSTATUSID == (int)PAStausenum.Pendingpayment)
                    {
                        var duePayment = ctx.PAY_DUEs.Where(pd => pd.PAY_FOR == applicationModel.REF_NO).FirstOrDefault();
                        if (duePayment != null)
                        {
                            applicationModel.AmountDue = duePayment.AMT_DUE;
                        }
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
                            switch (applicationModel.MODE)
                            {
                                case 1:
                                    finalStatus = PAStausenum.unitroute;
                                    break;
                                case 2:
                                    finalStatus = PAStausenum.directorcheck;
                                    break;
                                case 3:
                                    finalStatus = PAStausenum.unitroute;
                                    break;
                                case 4:
                                    finalStatus = PAStausenum.unitroute;
                                    break;
                            }
                        }
                        else if (applicationModel.SubmitType == OnRejected)
                        {
                            finalStatus = PAStausenum.documentIncomplete;
                        }
                        else
                        {
                            finalStatus = PAStausenum.unitroute;
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
                application.SUBMIT = DateTime.Now;
                applicationModel.APP_ID = applicationId;
                application.REF_NO = ApplicationModel.GetReferenceNo(applicationId, application.SUBMIT);
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
                else
                {
                    if (!applicationModel.IsDraft && roleTemplate == (int)RollTemplate.Public || roleTemplate == (int)RollTemplate.DeskOfficer)
                    {
                        var paLinkReqDocUmentList = ctx.APP_L_RDs.Where(p => p.APP_ID == applicationId).ToList();
                        if (paLinkReqDocUmentList.Count > 0)
                        {
                            ctx.APP_L_RDs.RemoveRange(paLinkReqDocUmentList);
                            ctx.SaveChanges();
                        }
                    }
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

            if (!string.IsNullOrWhiteSpace(applicationModel.newComment))
            {
                COMMENT comment = new COMMENT();
                comment.CONTENT = applicationModel.newComment;
                comment.COMMENTDATE = DateTime.Now;
                comment.APP_ID = applicationId;
                comment.USERSID = ProjectSession.UserID;
                ctx.COMMENTs.Add(comment);
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

                    var palinkAdd = ctx.APP_L_RDs.Where(p => p.APP_ID == premiseAppId).ToList();
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

                    var palinkAdd = ctx.APP_L_RDs.Where(p => p.APP_ID == premiseAppId).ToList();
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
            List<RD_L_LTModel> requiredDocument = new List<RD_L_LTModel>();
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

                requiredDocument = result;

                #region IsChecked

                if (!string.IsNullOrWhiteSpace(ApplicationID))
                {
                    int premiseAppId;
                    int.TryParse(ApplicationID, out premiseAppId);

                    var palinkAdd = ctx.APP_L_RDs.Where(p => p.APP_ID == premiseAppId).ToList();
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
        public ActionResult AddBannerObject(int APP_ID, int BC_ID, string ADDRA1, string ADDRA2, string ADDRA3, string ADDRA4, float B_SIZE, int B_QTY)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var ba = ctx.B_Os.Where(p => p.B_O_ID == APP_ID).FirstOrDefault();
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
            List<BannerObjectModel> bannerObject = new List<BannerObjectModel>();
            int totalRecord = 0;
            if (bannerApplicationId.HasValue)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var bannerObj = ctx.B_Os.Where(bo => bo.APP_ID == bannerApplicationId).ToList();
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

        #region Generate Letter PDF

        public ActionResult GenerateLetter(Int32? appId)
        {
            ApplicationModel ApplicationModel = new ApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var qry = ctx.APPLICATIONs
                                        .Include("COMPANY").Where(x => x.APP_ID == appId);
                    var bannerApp = ctx.APPLICATIONs
                                        .Include("COMPANY").Where(x => x.APP_ID == appId).ToList();
                    if (bannerApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in bannerApp)
                        {
                            int lineheight = 10;
                            PdfDocument pdf = new PdfDocument();
                            pdf.Info.Title = "PDF Letter";
                            PdfPage pdfPage = pdf.AddPage();
                            XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                            XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
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
                            if (!string.IsNullOrEmpty(item.COMPANY.C_NAME))
                            {
                                compName = item.COMPANY.C_NAME;

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

                                compName = "";
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

                                compName = "";
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
                            graph.DrawString(compName, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            lineheight = lineheight + 20;
                            graph.DrawString("ALAMAT PREMIS", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            string add1 = "";
                            if (add1 != "")
                            {
                                graph.DrawString(add1, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }

                            lineheight = lineheight + 20;
                            graph.DrawString("AKTIVITI", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            int cnt = 1;
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
                            //if (item.ReferenceNo != null)
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
                            foreach (var item1 in qry)
                            {
                                var Payment = ctx.PAY_RECs.Where(x => x.IND_ID == item1.IND_ID).ToList();

                                if (Payment != null && Payment.Count() > 0)
                                {
                                    graph.DrawString(string.Format("{0:000000}", Payment[0].PAY_RECID), nfont, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
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
                                
                                        strLocDesc = strLocDesc + item1.ADDRA1 + ",";
                                
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
                            var reqDocvalue = Request["reqDocid"];
                            var breqDocvalue = Request["breqDocid"];
                            var addDocvalue = Request["addDocid"];
                            var isReqvalue = Request["isReqDoc"];

                            int premiseApplicationId;
                            if (int.TryParse(premisevalue, out premiseApplicationId) && premiseApplicationId > 0)
                            {
                                int requiredDocId;
                                int.TryParse(reqDocvalue, out requiredDocId);

                                int brequiredDocId;
                                int.TryParse(breqDocvalue, out brequiredDocId);

                                int additionalDocId;
                                int.TryParse(addDocvalue, out additionalDocId);

                                if (requiredDocId > 0 || additionalDocId > 0 || brequiredDocId > 0)
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
                                        if (isReq > 0)
                                        {
                                            APP_L_RD paLinkReqDoc;
                                            paLinkReqDoc = ctx.APP_L_RDs.FirstOrDefault(p => p.APP_ID == premiseApplicationId && p.RD_ID == requiredDocId);
                                            if (paLinkReqDoc != null)
                                            {
                                                paLinkReqDoc.ATT_ID = attachment.ATT_ID;
                                                ctx.APP_L_RDs.AddOrUpdate(paLinkReqDoc);
                                                ctx.SaveChanges();
                                            }
                                            else
                                            {
                                                APP_L_RD paLinkReqDocument = new APP_L_RD();
                                                paLinkReqDocument.APP_ID = premiseApplicationId;
                                                paLinkReqDocument.RD_ID = requiredDocId;
                                                paLinkReqDocument.ATT_ID = attachment.ATT_ID;
                                                ctx.APP_L_RDs.AddOrUpdate(paLinkReqDocument);
                                                ctx.SaveChanges();
                                            }

                                            APP_L_RD paLinkBReqDoc;
                                            paLinkBReqDoc = ctx.APP_L_RDs.FirstOrDefault(p => p.APP_ID == premiseApplicationId && p.RD_ID == brequiredDocId);
                                            if (paLinkBReqDoc != null)
                                            {
                                                paLinkBReqDoc.ATT_ID = attachment.ATT_ID;
                                                ctx.APP_L_RDs.AddOrUpdate(paLinkBReqDoc);
                                                ctx.SaveChanges();
                                            }
                                            else
                                            {
                                                APP_L_RD paLinkReqBDocument = new APP_L_RD();
                                                paLinkReqBDocument.APP_ID = premiseApplicationId;
                                                paLinkReqBDocument.RD_ID = brequiredDocId;
                                                paLinkReqBDocument.ATT_ID = attachment.ATT_ID;
                                                ctx.APP_L_RDs.AddOrUpdate(paLinkReqBDocument);
                                                ctx.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            APP_L_RD paLinkAddDoc;
                                            paLinkAddDoc = ctx.APP_L_RDs.FirstOrDefault(p => p.APP_ID == premiseApplicationId && p.RD_ID == additionalDocId);
                                            if (paLinkAddDoc != null)
                                            {
                                                paLinkAddDoc.ATT_ID = attachment.ATT_ID;
                                                ctx.APP_L_RDs.AddOrUpdate(paLinkAddDoc);
                                                ctx.SaveChanges();
                                            }
                                            else
                                            {
                                                APP_L_RD paLinkAddDocument = new APP_L_RD();
                                                paLinkAddDocument.APP_ID = premiseApplicationId;
                                                paLinkAddDocument.RD_ID = additionalDocId;
                                                paLinkAddDocument.ATT_ID = attachment.ATT_ID;
                                                ctx.APP_L_RDs.AddOrUpdate(paLinkAddDocument);
                                                ctx.SaveChanges();
                                            }
                                        }

                                        return Json(new { status = "1", message = "Document Upload Successfully" }, JsonRequestBehavior.AllowGet);
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
            }
            catch
            {
                return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
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
                            var reqDocvalue = Request["reqDocid"];
                            var addDocvalue = Request["addDocid"];
                            var isReqvalue = Request["isReqDoc"];

                            int requiredDocId;
                            int.TryParse(reqDocvalue, out requiredDocId);

                            int additionalDocId;
                            int.TryParse(addDocvalue, out additionalDocId);

                            if (requiredDocId > 0 || additionalDocId > 0)
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
                                    if (isReq > 0)
                                    {
                                        //PALinkReqDoc paLinkReqDoc = new PALinkReqDoc();
                                        //paLinkReqDoc = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == premiseApplicationID && p.RequiredDocID == requiredDocID).FirstOrDefault();
                                        //if (paLinkReqDoc != null)
                                        //{
                                        //    paLinkReqDoc.AttachmentID = attachment.AttachmentID;
                                        //    ctx.PALinkReqDoc.AddOrUpdate(paLinkReqDoc);
                                        //    ctx.SaveChanges();
                                        //}
                                        //else
                                        //{
                                        //    PALinkReqDoc paLinkReqDocument = new PALinkReqDoc();
                                        //    paLinkReqDocument.PremiseApplicationID = premiseApplicationID;
                                        //    paLinkReqDocument.RequiredDocID = requiredDocID;
                                        //    paLinkReqDocument.AttachmentID = attachment.AttachmentID;
                                        //    ctx.PALinkReqDoc.AddOrUpdate(paLinkReqDocument);
                                        //    ctx.SaveChanges();
                                        //}

                                        return Json(new { status = "1", result = new { status = "1", RequiredDocID = requiredDocId, AttachmentID = attachment.ATT_ID, AttachmentName = attachment.FILENAME } }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        //PALinkAddDoc paLinkAddDoc = new PALinkAddDoc();
                                        //paLinkAddDoc = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == premiseApplicationID && p.AdditionalDocID == additionalDocID).FirstOrDefault();
                                        //if (paLinkAddDoc != null)
                                        //{
                                        //    paLinkAddDoc.AttachmentID = attachment.AttachmentID;
                                        //    ctx.PALinkAddDocs.AddOrUpdate(paLinkAddDoc);
                                        //    ctx.SaveChanges();
                                        //}
                                        //else
                                        //{
                                        //    PALinkAddDoc paLinkAddDocument = new PALinkAddDoc();
                                        //    paLinkAddDocument.PremiseApplicationID = premiseApplicationID;
                                        //    paLinkAddDocument.AdditionalDocID = additionalDocID;
                                        //    paLinkAddDocument.AttachmentID = attachment.AttachmentID;
                                        //    ctx.PALinkAddDocs.AddOrUpdate(paLinkAddDocument);
                                        //    ctx.SaveChanges();
                                        //}

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

    }
}