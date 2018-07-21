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
            applicationModel.START_RENT = DateTime.Today.AddMonths(-6);
            applicationModel.STOP_RENT = DateTime.Today.AddMonths(6);
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
                        applicationModel.UploadRequiredDocids = (string.Join(",", paLinkReqDocumentList.Select(x => x.RD_ID.ToString() + ":" + x.ATTACHMENTID.ToString()).ToArray()));
                    }

                    var paLinkAddDocumentlist = ctx.APP_L_RDs.Where(p => p.APP_ID == id).ToList();
                    if (paLinkAddDocumentlist.Count > 0)
                    {
                        applicationModel.UploadAdditionalDocids = (string.Join(",", paLinkAddDocumentlist.Select(x => x.RD_ID.ToString() + ":" + x.ATTACHMENTID.ToString()).ToArray()));
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
            List<RD_L_BTModel> requiredDocument;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<RD_L_BT> query = ctx.RD_L_BTs.Where(p => p.BT_ID.ToString().Contains(businessTypeId));

                #region Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                string orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<RD_L_BTModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "RD_L_BTID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                requiredDocument = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, requiredDocument, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
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
                                var attechmentdetails = ctx.ATTACHMENTs.FirstOrDefault(a => a.ATT_ID == resultpalinkReq.ATTACHMENTID);
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
        /// <param name="businessCodeids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LicenseDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string APP_TYPE_ID)
        {
            List<RD_L_LTModel> requiredDocument = new List<RD_L_LTModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<RD_L_LT> query = ctx.RD_L_LTs;

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

                if (!string.IsNullOrWhiteSpace(APP_TYPE_ID))
                {
                    int premiseAppId;
                    int.TryParse(APP_TYPE_ID, out premiseAppId);

                    var palinkAdd = ctx.APP_L_RDs.Where(p => p.APP_ID == premiseAppId).ToList();
                    foreach (var item in requiredDocument)
                    {
                        if (palinkAdd.Count > 0)
                        {
                            var resultpalinkReq = palinkAdd.FirstOrDefault(p => p.RD_ID == item.RD_ID && p.APP_ID == premiseAppId);
                            if (resultpalinkReq != null)
                            {
                                item.IsChecked = "checked";
                                var attechmentdetails = ctx.ATTACHMENTs.FirstOrDefault(a => a.ATT_ID == resultpalinkReq.ATTACHMENTID);
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
        public ActionResult ManageBC(int? id)
        {
            BusinessCodeModel businessCodeModel = new BusinessCodeModel
            {
                ACTIVE = true
            };
            if (id != null && id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int businessCodeId = Convert.ToInt32(id);
                    var businessCode = ctx.BCs.FirstOrDefault(a => a.BC_ID == businessCodeId);
                    businessCodeModel = Mapper.Map<BusinessCodeModel>(businessCode);

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

            return View(businessCodeModel);
        }

        /// <summary>
        /// Save BusinessCode Infomration
        /// </summary>
        /// <param name="businessCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageBC(BusinessCodeModel businessCodeModel)
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
                                selectedDepartments.Add(new BC_L_DEP { BC_ID = businessCode.BC_ID, DEP_ID = depId });
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

                    if (businessCodeModel.AdditionalDocs.Count > 0)
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

    }
}