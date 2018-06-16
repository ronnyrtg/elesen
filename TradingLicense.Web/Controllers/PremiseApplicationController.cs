﻿using DataTables.Mvc;
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
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Diagnostics;

namespace TradingLicense.Web.Controllers
{
    public class PremiseApplicationController : BaseController
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
        /// Get PremiseApplication Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PremiseApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string PremiseApplicationID, string IndividualMkNo)
        {
            List<TradingLicense.Model.PremiseApplicationModel> PremiseApplication = new List<Model.PremiseApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<PremiseApplication> query = (ProjectSession.User != null && ProjectSession.User.RoleTemplateID == (int)RollTemplate.Public) ? ctx.PremiseApplications.Where(p => p.UsersID == ProjectSession.User.UsersID) : ctx.PremiseApplications;

                if (!string.IsNullOrWhiteSpace(PremiseApplicationID))
                {
                    query = query.Where(q => q.PremiseApplicationID.ToString().Contains(PremiseApplicationID));
                }

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

                var result = Mapper.Map<List<PremiseApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "PremiseApplicationID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                PremiseApplication = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, PremiseApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get PremiseApplication Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PremiseApplicationsByIndividual([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? individualId)
        {
            List<TradingLicense.Model.PremiseApplicationModel> PremiseApplication = new List<Model.PremiseApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<PremiseApplication> query = ((ProjectSession.User != null && ProjectSession.User.RoleTemplateID == (int)RollTemplate.Public) ? ctx.PremiseApplications.Where(p => p.UsersID == ProjectSession.User.UsersID) : ctx.PremiseApplications);

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

                var result = Mapper.Map<List<PremiseApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "PremiseApplicationID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                PremiseApplication = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, PremiseApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get Required Document Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RequiredDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessTypeID, string premiseApplicationID)
        {
            List<TradingLicense.Model.BTLinkReqDocModel> RequiredDocument = new List<Model.BTLinkReqDocModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BTLinkReqDoc> query = ctx.PALinkReqDocs.Where(p => p.BusinessTypeID.ToString().Contains(businessTypeID));

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

                var result = Mapper.Map<List<BTLinkReqDocModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BTLinkReqDocID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                RequiredDocument = result;

                #region IsChecked

                if (!string.IsNullOrWhiteSpace(premiseApplicationID))
                {
                    int premiseAppId;
                    int.TryParse(premiseApplicationID, out premiseAppId);

                    var palinkReq = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == premiseAppId).ToList();
                    foreach (var item in RequiredDocument)
                    {
                        if (palinkReq != null && palinkReq.Count > 0)
                        {
                            var resultpalinkReq = palinkReq.Where(p => p.RequiredDocID == item.RequiredDocID && p.PremiseApplicationID == premiseAppId).FirstOrDefault();
                            if (resultpalinkReq != null)
                            {
                                item.IsChecked = "checked";
                                var attechmentdetails = ctx.Attachments.Where(a => a.AttachmentID == resultpalinkReq.AttachmentID).FirstOrDefault();
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
            return Json(new DataTablesResponse(requestModel.Draw, RequiredDocument, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="attechmentID"></param>
        /// /// <param name="premiseID"></param>
        /// <returns></returns>
        public FileResult Download(int? attechmentID, int? premiseID)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var attechment = ctx.Attachments.Where(a => a.AttachmentID == attechmentID).FirstOrDefault();
                var folder = Server.MapPath(Infrastructure.ProjectConfiguration.AttachmentDocument);
                try
                {
                    try
                    {
                        if (attechment != null && attechment.AttachmentID > 0)
                        {
                            var path = Path.Combine(folder, attechment.FileName);
                            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, attechment.FileName);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch
                    {

                    }
                }
                catch { }
                return null;
            }
        }

        /// <summary>
        /// Get PremiseApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManagePremiseApplication(int? Id)
        {
            PremiseApplicationModel premiseApplicationModel = new PremiseApplicationModel();
            premiseApplicationModel.StartRent = new DateTime(2012, 2, 2);
            premiseApplicationModel.StopRent = new DateTime(2012, 2, 2);
            ViewBag.SelectedMode = 0;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int PremiseApplicationID = Convert.ToInt32(Id);
                    var premiseApplication = ctx.PremiseApplications.Where(a => a.PremiseApplicationID == PremiseApplicationID).FirstOrDefault();
                    premiseApplicationModel = Mapper.Map<PremiseApplicationModel>(premiseApplication);

                    var paLinkBC = ctx.PALinkBC.Where(a => a.PremiseApplicationID == PremiseApplicationID).ToList();
                    if (paLinkBC != null && paLinkBC.Count > 0)
                    {
                        premiseApplicationModel.BusinessCodeids = (string.Join(",", paLinkBC.Select(x => x.BusinessCodeID.ToString()).ToArray()));

                        List<Select2ListItem> businessCodesList = new List<Select2ListItem>();
                        int selectedMode = 0;
                        foreach (var item in paLinkBC)
                        {
                            var buinesscode = ctx.BusinessCodes.Where(b => b.BusinessCodeID == item.BusinessCodeID).FirstOrDefault();
                            if (buinesscode != null && buinesscode.BusinessCodeID > 0)
                            {
                                selectedMode = buinesscode.Mode;
                                Select2ListItem selectedBusinessCodeModel = new Select2ListItem();
                                selectedBusinessCodeModel.id = buinesscode.BusinessCodeID;
                                selectedBusinessCodeModel.text = $"{buinesscode.CodeNumber}~{buinesscode.CodeDesc}";
                                businessCodesList.Add(selectedBusinessCodeModel);
                            }
                        }
                        premiseApplicationModel.selectedbusinessCodeList = businessCodesList;
                        ViewBag.SelectedMode = selectedMode;
                    }



                    var PALinkReqDocUmentList = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == PremiseApplicationID).ToList();
                    if (PALinkReqDocUmentList != null && PALinkReqDocUmentList.Count > 0)
                    {
                        premiseApplicationModel.UploadRequiredDocids = (string.Join(",", PALinkReqDocUmentList.Select(x => x.RequiredDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }

                    var paLinkAddDocumentlist = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == PremiseApplicationID).ToList();
                    if (paLinkAddDocumentlist != null && paLinkAddDocumentlist.Count > 0)
                    {
                        premiseApplicationModel.UploadAdditionalDocids = (string.Join(",", paLinkAddDocumentlist.Select(x => x.AdditionalDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                premiseApplicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
                premiseApplicationModel.UsersID = ProjectSession.User.UsersID;
            }
            premiseApplicationModel.IsDraft = false;
            return View(premiseApplicationModel);
        }

        /// <summary>
        /// Get PremiseApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ViewPremiseApplication(int Id)
        {
            ViewPremiseApplicationModel premiseApplicationModel = new ViewPremiseApplicationModel();
            if (Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int PremiseApplicationID = Convert.ToInt32(Id);
                    var premiseApplication = ctx.PremiseApplications.Where(a => a.PremiseApplicationID == PremiseApplicationID).FirstOrDefault();
                    premiseApplicationModel = Mapper.Map<ViewPremiseApplicationModel>(premiseApplication);
                    premiseApplicationModel.Individuals = new List<string>();

                    var paLinkBC = ctx.PALinkBC.Where(a => a.PremiseApplicationID == PremiseApplicationID).ToList();
                    if (paLinkBC != null && paLinkBC.Count > 0)
                    {
                        premiseApplicationModel.BusinessCodes = paLinkBC.Select(x => $"{x.BusinessCode.CodeNumber} | {x.BusinessCode.CodeDesc}").ToList();
                    }
                    else { premiseApplicationModel.BusinessCodes = new List<string>(); }

                    var PALinkReqDocUmentList = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == PremiseApplicationID).ToList();
                    if (PALinkReqDocUmentList != null && PALinkReqDocUmentList.Count > 0)
                    {
                        premiseApplicationModel.RequiredDocs = PALinkReqDocUmentList.Select(par => par.RequiredDoc.RequiredDocDesc).ToList();
                    }
                    else { premiseApplicationModel.RequiredDocs = new List<string>(); }

                    var paLinkAddDocumentlist = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == PremiseApplicationID).ToList();
                    if (paLinkAddDocumentlist != null && paLinkAddDocumentlist.Count > 0)
                    {
                        premiseApplicationModel.AdditionalDocs = paLinkAddDocumentlist.Select(pad => pad.AdditionalDoc.DocDesc).ToList();
                    }
                    else { premiseApplicationModel.AdditionalDocs = new List<string>(); }

                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                premiseApplicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
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
        public ActionResult ManagePremiseApplication(PremiseApplicationModel premiseApplicationModel, string btnSubmit)
        {
            try
            {
            if (ProjectSession.User != null && ProjectSession.UserID > 0 && ProjectSession.User.RoleTemplateID.HasValue)
            {
                if (ProjectSession.User.RoleTemplateID.Value == (int)RollTemplate.DeskOfficer)
                {
                    ModelState.Remove("PremiseArea");
                    ModelState.Remove("PremiseStatus");
                    ModelState.Remove("PremiseTypeID");
                    ModelState.Remove("PremiseModification");
                    ModelState.Remove("PremiseOwnership");
                }
            }
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    PremiseApplication premiseApplication;
                    premiseApplication = Mapper.Map<PremiseApplication>(premiseApplicationModel);

                    int UserroleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.UserID > 0)
                    {
                        premiseApplication.UpdatedBy = ProjectSession.User.Username;

                        #region Set PAStatus Value 

                        if (ProjectSession.User.RoleTemplateID != null)
                        {
                            UserroleTemplate = ProjectSession.User.RoleTemplateID.Value;
                        }

                        if (!premiseApplicationModel.IsDraft)
                        {
                            if (UserroleTemplate == (int)RollTemplate.Public || UserroleTemplate == (int)RollTemplate.DeskOfficer)
                            {
                                premiseApplication.AppStatusID = (int)PAStausenum.submittedtoclerk;
                            }
                        }
                        else
                        {
                            if (UserroleTemplate == (int)RollTemplate.Public || UserroleTemplate == (int)RollTemplate.DeskOfficer)
                            {
                                premiseApplication.AppStatusID = (int)PAStausenum.draftcreated;
                            }
                        }

                        if (UserroleTemplate == (int)RollTemplate.Clerk)
                        {
                            if (!string.IsNullOrWhiteSpace(premiseApplicationModel.BusinessCodeids))
                            {
                                var IslinkDept = false;
                                string[] ids = premiseApplicationModel.BusinessCodeids.Split(',');
                                foreach (var id in ids)
                                {
                                    int BusinessCodeID = Convert.ToInt32(id);
                                    var businesslinkDepartment = ctx.BCLinkDeps.Where(p => p.BusinessCodeID == BusinessCodeID).FirstOrDefault();
                                    if (businesslinkDepartment != null && businesslinkDepartment.BussCodLinkDepID > 0)
                                    {
                                        IslinkDept = true;
                                        break;
                                    }
                                }

                                if (IslinkDept)
                                {
                                    premiseApplication.AppStatusID = (int)PAStausenum.unitroute;
                                }
                                else
                                {
                                    premiseApplication.AppStatusID = (int)PAStausenum.supervisorcheck;
                                }
                            }
                            else
                            {
                                premiseApplication.AppStatusID = (int)PAStausenum.supervisorcheck;
                            }
                        }

                        #endregion
                    }

                    premiseApplication.DateSubmitted = DateTime.Now;

                    ctx.PremiseApplications.AddOrUpdate(premiseApplication);
                    ctx.SaveChanges();

                    int premiseApplicationID = premiseApplication.PremiseApplicationID;

                    int roleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
                    {
                        roleTemplate = ProjectSession.User.RoleTemplateID.Value;
                    }

                    if (UserroleTemplate == (int)RollTemplate.Public)
                    {
                        if (!string.IsNullOrWhiteSpace(premiseApplicationModel.UploadRequiredDocids))
                        {
                            string[] ids = premiseApplicationModel.UploadRequiredDocids.Split(',');
                            List<RequiredDocList> RequiredDoclist = new List<RequiredDocList>();

                            foreach (string id in ids)
                            {
                                string[] rId = id.Split(':');

                                RequiredDocList requiredDocList = new RequiredDocList();
                                requiredDocList.RequiredDocID = Convert.ToInt32(rId[0]);
                                requiredDocList.AttachmentID = Convert.ToInt32(rId[1]);
                                RequiredDoclist.Add(requiredDocList);
                            }

                            List<int> existingRecord = new List<int>();
                            var dbEntryRequiredDoc = ctx.PALinkReqDoc.Where(q => q.PremiseApplicationID == premiseApplicationID).ToList();
                            if (dbEntryRequiredDoc != null && dbEntryRequiredDoc.Count > 0)
                            {
                                foreach (var item in dbEntryRequiredDoc)
                                {
                                    if (RequiredDoclist.Where(q => q.RequiredDocID == item.RequiredDocID && q.AttachmentID == item.AttachmentID).Count() == 0)
                                    {
                                        if (roleTemplate == (int)RollTemplate.Public)
                                        {
                                            ctx.PALinkReqDoc.Remove(item);
                                        }
                                    }
                                    else
                                    {
                                        existingRecord.Add(item.RequiredDocID);
                                    }
                                }
                                ctx.SaveChanges();
                            }

                            foreach (var requiredDoc in RequiredDoclist)
                            {
                                if (existingRecord.Where(q => q == requiredDoc.RequiredDocID).Count() == 0)
                                {
                                    PALinkReqDoc pALinkReqDoc = new PALinkReqDoc();
                                    pALinkReqDoc.PremiseApplicationID = premiseApplicationID;
                                    pALinkReqDoc.RequiredDocID = requiredDoc.RequiredDocID;
                                    pALinkReqDoc.AttachmentID = requiredDoc.AttachmentID;
                                    ctx.PALinkReqDoc.AddOrUpdate(pALinkReqDoc);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            if (roleTemplate == (int)RollTemplate.Public)
                            {
                                var PALinkReqDocUmentList = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == premiseApplicationID).ToList();
                                if (PALinkReqDocUmentList != null && PALinkReqDocUmentList.Count > 0)
                                {
                                    ctx.PALinkReqDoc.RemoveRange(PALinkReqDocUmentList);
                                    ctx.SaveChanges();
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(premiseApplicationModel.UploadAdditionalDocids))
                        {
                            string[] ids = premiseApplicationModel.UploadAdditionalDocids.Split(',');
                            List<AdditionalDocList> AdditionalDoclistlist = new List<AdditionalDocList>();

                            foreach (string id in ids)
                            {
                                string[] aId = id.Split(':');
                                AdditionalDocList additionalDocList = new AdditionalDocList();
                                additionalDocList.AdditionalDocID = Convert.ToInt32(aId[0]);
                                additionalDocList.AttachmentID = Convert.ToInt32(aId[1]);
                                AdditionalDoclistlist.Add(additionalDocList);
                            }

                            List<int> existingRecord = new List<int>();
                            var dbEntryPALinkAddDoc = ctx.PALinkAddDocs.Where(q => q.PremiseApplicationID == premiseApplicationID).ToList();
                            if (dbEntryPALinkAddDoc != null && dbEntryPALinkAddDoc.Count > 0)
                            {
                                foreach (var item in dbEntryPALinkAddDoc)
                                {
                                    if (AdditionalDoclistlist.Where(q => q.AdditionalDocID == item.AdditionalDocID && q.AttachmentID == item.AttachmentID).Count() == 0)
                                    {
                                        if (roleTemplate == (int)RollTemplate.Public)
                                        {
                                            ctx.PALinkAddDocs.Remove(item);
                                        }
                                    }
                                    else
                                    {
                                        existingRecord.Add(item.AdditionalDocID);
                                    }
                                }
                                ctx.SaveChanges();
                            }

                            foreach (var AdditionalDoc in AdditionalDoclistlist)
                            {
                                if (existingRecord.Where(q => q == AdditionalDoc.AdditionalDocID).Count() == 0)
                                {
                                    PALinkAddDoc paLinkAddDoc = new PALinkAddDoc();
                                    paLinkAddDoc.PremiseApplicationID = premiseApplicationID;
                                    paLinkAddDoc.AdditionalDocID = AdditionalDoc.AdditionalDocID;
                                    paLinkAddDoc.AttachmentID = AdditionalDoc.AttachmentID;
                                    ctx.PALinkAddDocs.Add(paLinkAddDoc);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            if (roleTemplate == (int)RollTemplate.Public)
                            {
                                var paLinkAddDocumentlist = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == premiseApplicationID).ToList();
                                if (paLinkAddDocumentlist != null && paLinkAddDocumentlist.Count > 0)
                                {
                                    ctx.PALinkAddDocs.RemoveRange(paLinkAddDocumentlist);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                    }
                    else if (UserroleTemplate == (int)RollTemplate.DeskOfficer)
                    {
                        if (!string.IsNullOrWhiteSpace(premiseApplicationModel.RequiredDocIds))
                        {
                            string[] ids = premiseApplicationModel.RequiredDocIds.Split(',');
                            List<int> RequiredDoclist = new List<int>();

                            foreach (string id in ids)
                            {
                                int RequiredDocID = Convert.ToInt32(id);
                                RequiredDoclist.Add(RequiredDocID);
                            }

                            List<int> existingRecord = new List<int>();
                            var dbEntryRequiredDoc = ctx.PALinkReqDoc.Where(q => q.PremiseApplicationID == premiseApplicationID).ToList();
                            if (dbEntryRequiredDoc != null && dbEntryRequiredDoc.Count > 0)
                            {
                                foreach (var item in dbEntryRequiredDoc)
                                {
                                    if (RequiredDoclist.Where(q => q == item.RequiredDocID).Count() == 0)
                                    {
                                        if (roleTemplate == (int)RollTemplate.Public || roleTemplate == (int)RollTemplate.DeskOfficer)
                                        {
                                            ctx.PALinkReqDoc.Remove(item);
                                        }
                                    }
                                    else
                                    {
                                        existingRecord.Add(item.RequiredDocID);
                                    }
                                }
                                ctx.SaveChanges();
                            }

                            foreach (var requiredDoc in RequiredDoclist)
                            {
                                if (existingRecord.Where(q => q == requiredDoc).Count() == 0)
                                {
                                    PALinkReqDoc pALinkReqDoc = new PALinkReqDoc();
                                    pALinkReqDoc.PremiseApplicationID = premiseApplicationID;
                                    pALinkReqDoc.RequiredDocID = requiredDoc;
                                    ctx.PALinkReqDoc.AddOrUpdate(pALinkReqDoc);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            if (!premiseApplicationModel.IsDraft && roleTemplate == (int)RollTemplate.Public || roleTemplate == (int)RollTemplate.DeskOfficer)
                            {
                                var PALinkReqDocUmentList = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == premiseApplicationID).ToList();
                                if (PALinkReqDocUmentList != null && PALinkReqDocUmentList.Count > 0)
                                {
                                    ctx.PALinkReqDoc.RemoveRange(PALinkReqDocUmentList);
                                    ctx.SaveChanges();
                                }
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(premiseApplicationModel.AdditionalDocIds))
                        {
                            string[] ids = premiseApplicationModel.AdditionalDocIds.Split(',');
                            List<int> AdditionalDoclistlist = new List<int>();

                            foreach (string id in ids)
                            {
                                int IndividualID = Convert.ToInt32(id);
                                AdditionalDoclistlist.Add(IndividualID);
                            }

                            List<int> existingRecord = new List<int>();
                            var dbEntryPALinkAddDoc = ctx.PALinkAddDocs.Where(q => q.PremiseApplicationID == premiseApplicationID).ToList();
                            if (dbEntryPALinkAddDoc != null && dbEntryPALinkAddDoc.Count > 0)
                            {
                                foreach (var item in dbEntryPALinkAddDoc)
                                {
                                    if (AdditionalDoclistlist.Where(q => q == item.AdditionalDocID).Count() == 0)
                                    {
                                        if (roleTemplate == (int)RollTemplate.Public || roleTemplate == (int)RollTemplate.DeskOfficer)
                                        {
                                            ctx.PALinkAddDocs.Remove(item);
                                        }
                                    }
                                    else
                                    {
                                        existingRecord.Add(item.AdditionalDocID);
                                    }
                                }
                                ctx.SaveChanges();
                            }

                            foreach (var AdditionalDoc in AdditionalDoclistlist)
                            {
                                if (existingRecord.Where(q => q == AdditionalDoc).Count() == 0)
                                {
                                    PALinkAddDoc paLinkAddDoc = new PALinkAddDoc();
                                    paLinkAddDoc.PremiseApplicationID = premiseApplicationID;
                                    paLinkAddDoc.AdditionalDocID = AdditionalDoc;
                                    ctx.PALinkAddDocs.Add(paLinkAddDoc);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            if (!premiseApplicationModel.IsDraft && roleTemplate == (int)RollTemplate.Public || roleTemplate == (int)RollTemplate.DeskOfficer)
                            {
                                var paLinkAddDocumentlist = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == premiseApplicationID).ToList();
                                if (paLinkAddDocumentlist != null && paLinkAddDocumentlist.Count > 0)
                                {
                                    ctx.PALinkAddDocs.RemoveRange(paLinkAddDocumentlist);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(premiseApplicationModel.BusinessCodeids))
                    {
                        string[] ids = premiseApplicationModel.BusinessCodeids.Split(',');
                        List<int> BusinessCodelist = new List<int>();

                        foreach (string id in ids)
                        {
                            int BusinessCodeID = Convert.ToInt32(id);
                            BusinessCodelist.Add(BusinessCodeID);
                        }

                        List<int> existingRecord = new List<int>();
                        var dbEntryPALinkBAct = ctx.PALinkBC.Where(q => q.PremiseApplicationID == premiseApplicationID).ToList();
                        if (dbEntryPALinkBAct != null && dbEntryPALinkBAct.Count > 0)
                        {
                            foreach (var item in dbEntryPALinkBAct)
                            {
                                if (BusinessCodelist.Where(q => q == item.BusinessCodeID).Count() == 0)
                                {
                                    ctx.PALinkBC.Remove(item);
                                }
                                else
                                {
                                    existingRecord.Add(item.BusinessCodeID);
                                }
                            }
                            ctx.SaveChanges();
                        }

                        foreach (var businessCode in BusinessCodelist)
                        {
                            if (existingRecord.Where(q => q == businessCode).Count() == 0)
                            {
                                PALinkBC PALinkBC = new PALinkBC();
                                PALinkBC.PremiseApplicationID = premiseApplicationID;
                                PALinkBC.BusinessCodeID = businessCode;
                                ctx.PALinkBC.Add(PALinkBC);
                                ctx.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var dbEntryPALinkBActs = ctx.PALinkBC.Where(va => va.PremiseApplicationID == premiseApplicationID).ToList();
                        if (dbEntryPALinkBActs != null && dbEntryPALinkBActs.Count > 0)
                        {
                            ctx.PALinkBC.RemoveRange(dbEntryPALinkBActs);
                            ctx.SaveChanges();
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(premiseApplicationModel.Individualids))
                    {
                        string[] ids = premiseApplicationModel.Individualids.Split(',');
                        List<int> Individualidslist = new List<int>();

                        foreach (string id in ids)
                        {
                            int IndividualID = Convert.ToInt32(id);
                            Individualidslist.Add(IndividualID);
                        }

                        List<int> existingRecord = new List<int>();

                    }

                    if (!string.IsNullOrWhiteSpace(premiseApplicationModel.newIndividualsList))
                    {
                        try
                        {
                            List<NewIndividualModel> individuals = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<NewIndividualModel>>(premiseApplicationModel.newIndividualsList);
                            foreach (var indModel in individuals)
                            {
                                Individual ind = new Individual();
                                ind.FullName = indModel.fullName;
                                ind.MykadNo = indModel.passportNo;
                                ctx.Individuals.Add(ind);
                                ctx.SaveChanges();
                            }
                        }
                        catch
                        {

                        }
                    }
                    premiseApplicationModel.PremiseApplicationID = premiseApplicationID;
                }

                if (premiseApplicationModel.IsDraft)
                {
                    TempData["SuccessMessage"] = "Premise License Application draft saved successfully.";

                    return RedirectToAction("ManagePremiseApplication", new { Id = premiseApplicationModel.PremiseApplicationID });
                }
                else
                {
                    TempData["SuccessMessage"] = "Premise License Application saved successfully.";

                    return RedirectToAction("PremiseApplication");
                }
            }
            else
            {
                ViewBag.SelectedMode = 0;
                return View(premiseApplicationModel);
            }
            }
            catch (Exception)
            {
                ViewBag.SelectedMode = 0;
                return View(premiseApplicationModel);
            }
        }

        public ActionResult GenerateLetter(Int32? AppId)
        {
            PremiseApplicationModel premiseApplicationModel = new PremiseApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var PremiseApp = ctx.PremiseApplications
                                        .Include("Company").Where(x => x.PremiseApplicationID == AppId).ToList();
                    if (PremiseApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid Premise ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in PremiseApp)
                        {


                            int Lineheight = 10;
                            PdfDocument pdf = new PdfDocument();
                            pdf.Info.Title = "PDF Letter";
                            PdfPage pdfPage = pdf.AddPage();
                            XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                            XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                            XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                            XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                            XImage xImage1 = XImage.FromFile(Server.MapPath("~\\images\\logoPL.jpg"));
                            graph.DrawImage(xImage1, 180, 30, 75, 75);


                            graph.DrawString("PERBADANAN LABUAN", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("(LABUAN CORPORATION)", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("PETI SURAT 81245", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("87022 WILLAYAH PERSEKUTUAN LABUAN", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("Tel No 				:", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("087 408600, 408601", font, XBrushes.Black, new XRect(360, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("Faks No          :", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("087 428997, 419400, 426803", font, XBrushes.Black, new XRect(360, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            Lineheight = Lineheight + 12;
                            XPen lineRed = new XPen(XColors.Black, 2);
                            System.Drawing.Point pt1 = new System.Drawing.Point(0, Lineheight);
                            System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width), Lineheight);
                            graph.DrawLine(lineRed, pt1, pt2);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("Rujukan Kami :", nfont, XBrushes.Black, new XRect(360, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("PL/JP/" + DateTime.Now.Year.ToString() + "/T/00000" + AppId, nfont, XBrushes.Black, new XRect(435, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("Tarikh           :", nfont, XBrushes.Black, new XRect(360, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(DateTime.Now.ToString("dd/MM/yyyy"), nfont, XBrushes.Black, new XRect(435, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("Pengurus", nfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            string CompName = "";
                            if (item.Company.CompanyName == null)
                            {
                                CompName = "";
                            }
                            else
                            {
                                CompName = item.Company.CompanyName;
                            }
                            graph.DrawString(CompName, nfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;

                            string CompAdd = "";
                            if (item.Company.CompanyAddress == null)
                            {
                                CompAdd = "";
                            }
                            else
                            {
                                CompAdd = item.Company.CompanyAddress;
                            }

                            graph.DrawString(CompAdd.ToString(), nfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;

                            string CompPhone = "";
                            if (item.Company.CompanyPhone == null)
                            {
                                CompPhone = "";
                            }
                            else
                            {
                                CompPhone = item.Company.CompanyPhone;
                            }

                            graph.DrawString(CompPhone, nfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("Tuan/Puan,", nfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("PERMOHONAN LESEN PERNIAGAAN BARU,", lbfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 20;
                            graph.DrawString("NO. RUJUKAN", font, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(DateTime.Now.Year.ToString() + "/T/00000" + AppId, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 20;
                            graph.DrawString("NAMA PERNIAGAAN", font, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(CompName, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            Lineheight = Lineheight + 20;
                            graph.DrawString("ALAMAT PREMIS", font, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            string Add1 = "";
                            string Add2 = "";
                            string Add3 = "";
                            if (item.Addra1 != null)
                            {
                                Add1 = Add1 + item.Addra1 + ",";
                            }
                            if (item.Addra2 != null)
                            {
                                Add1 = Add1 + item.Addra2;
                            }

                            if (item.Addra3 != null)
                            {
                                Add2 = Add2 + item.Addra3 + ",";
                            }
                            if (item.Addra4 != null)
                            {
                                Add2 = Add2 + item.Addra4;
                            }
                            if (item.PcodeA != null)
                            {
                                Add3 = Add3 + item.PcodeA;

                            }
                            if (Add1 != "")
                            {
                                graph.DrawString(Add1, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            if (Add2 != "")
                            {
                                Lineheight = Lineheight + 15;
                                graph.DrawString(Add2, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            if (Add3 != "")
                            {
                                Lineheight = Lineheight + 15;
                                graph.DrawString(Add3, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            Lineheight = Lineheight + 20;
                            graph.DrawString("ACTIVITI", font, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            int cnt = 1;
                            foreach (var item1 in ctx.PALinkBC.Where(x => x.PremiseApplicationID == AppId))
                            {
                                foreach (var item2 in ctx.BusinessCodes.Where(x => x.BusinessCodeID == item1.BusinessCodeID))
                                {
                                    {
                                        if (item2.CodeDesc != null)
                                        {
                                            string itm = cnt.ToString() + ")    " + item2.CodeDesc;
                                            graph.DrawString(itm, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            cnt = cnt + 1;
                                            Lineheight = Lineheight + 15;
                                        }

                                    }

                                }
                            }
                            Lineheight = Lineheight + 20;
                            graph.DrawString("NAMA PEMILIK & NO. KP", font, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            cnt = 1;
                            foreach (var item3 in ctx.PALinkInd.Where(x => x.PremiseApplicationID == AppId))
                            {
                                foreach (var item4 in ctx.Individuals.Where(x => x.IndividualID == item3.IndividualID))
                                {
                                    {
                                        if (item4.FullName != null)
                                        {
                                            string FName = item4.FullName;
                                            if (item4.MykadNo != null)
                                            {
                                                FName = FName + "(" + item4.MykadNo + ")";
                                            }
                                            string itm = cnt.ToString() + ")    " + FName;
                                            graph.DrawString(itm, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            cnt = cnt + 1;
                                            Lineheight = Lineheight + 15;
                                        }
                                    }

                                }
                            }
                            Lineheight = Lineheight + 20;
                            graph.DrawString("KEPUTUSAN", font, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            string ModeValue = "";
                            if (item.Mode == 1)
                            {
                                ModeValue = "Ekspres";
                            }
                            else if (item.Mode == 2)
                            {
                                ModeValue = "Biasa";
                            }
                            else if (item.Mode == 3)
                            {
                                ModeValue = "Mesyuarat";
                            }
                            else
                            {
                                ModeValue = "Pengarah";
                            }
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(ModeValue, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 20;
                            graph.DrawString("CATATAN", font, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            cnt = 1;
                            foreach (var item4 in ctx.PAComments.Where(x => x.PremiseApplicationID == AppId))
                            {
                                {
                                    if (item4.Comment != null)
                                    {
                                        string itm = cnt.ToString() + ")    " + item4.Comment;
                                        graph.DrawString(itm, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        cnt = cnt + 1;
                                        Lineheight = Lineheight + 15;
                                    }
                                }

                            }
                            Lineheight = Lineheight + 20;
                            graph.DrawString("BAYARAN", font, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.ProcessingFee != null)
                            {
                                var mval = string.Format("{0:0.00}", item.ProcessingFee);
                                graph.DrawString("RM" + mval, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            else
                            {
                                graph.DrawString("RM0.00", font, XBrushes.Black, new XRect(300, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }


                            Lineheight = Lineheight + 20;
                            graph.DrawString("PERINGATAN:", font, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("i.   Sila buat bayaran Lesen Perniagaan select-lewatnya pada atau sebelum 28 FEBRUARI 2018", font, XBrushes.Black, new XRect(40, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("ii.  Surat kelulusan ini sah sehingga  " + DateTime.Now.AddMonths(6).ToString("dd/MM/yyyy"), font, XBrushes.Black, new XRect(40, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("iii. Sekiranya pihak tuan membuat kerja-kerja pengubahsuaian bangunan sila kemukakan", font, XBrushes.Black, new XRect(40, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("     kelulusan Permohonan Plan Mengubahsuai Bangunan di Jabatan Perancangan dan Kawalan", font, XBrushes.Black, new XRect(40, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph.DrawString("     Bangunan Perbadanan Labuan terlebih dahulu.", font, XBrushes.Black, new XRect(40, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 20;
                            graph.DrawString("Surat ini adalah cetakan komputer dan tandatangan tidak diperlukan", fontitalik, XBrushes.Black, new XRect(30, Lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            PdfPage pdfPage2 = pdf.AddPage();
                            XGraphics graph2 = XGraphics.FromPdfPage(pdfPage2);

                            graph2.DrawImage(xImage1, 180, 30, 75, 75);

                            Lineheight = 10;
                            graph2.DrawString("PERBADANAN LABUAN", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph2.DrawString("(LABUAN CORPORATION)", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph2.DrawString("PETI SURAT 81245", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph2.DrawString("87022 WILLAYAH PERSEKUTUAN LABUAN", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph2.DrawString("Tel No 				:", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString("087 408600, 408601", font, XBrushes.Black, new XRect(360, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph2.DrawString("Faks No          :", font, XBrushes.Black, new XRect(260, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString("087 428997, 419400, 426803", font, XBrushes.Black, new XRect(360, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            Lineheight = Lineheight + 12;
                            XPen line1 = new XPen(XColors.Black, 2);
                            System.Drawing.Point pt10 = new System.Drawing.Point(0, Lineheight);
                            System.Drawing.Point pt11 = new System.Drawing.Point(Convert.ToInt32(pdfPage2.Width), Lineheight);
                            graph2.DrawLine(lineRed, pt10, pt11);
                            Lineheight = Lineheight + 15;

                            graph2.DrawString("Pengakuan Setuju Terima:", nfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 25;
                            graph2.DrawString("1)  Saya bersetuju dengan keputusan permohonan ini dan segala maklumat yang  deberi adalah benar.", nfont, XBrushes.Black, new XRect(40, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph2.DrawString("2)  Saya bersetuju sekiranya maklumat deberi adalah palsu atau saya gagal mematuhi syarat-", nfont, XBrushes.Black, new XRect(40, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 15;
                            graph2.DrawString("    syarat pengeluaran lesen, Perbadanan Labuan berhak untuk membatalkan keputusan lesen ini.", nfont, XBrushes.Black, new XRect(40, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 50;
                            cnt = 1;
                            foreach (var item3 in ctx.PALinkInd.Where(x => x.PremiseApplicationID == AppId))
                            {
                                foreach (var item4 in ctx.Individuals.Where(x => x.IndividualID == item3.IndividualID))
                                {
                                    {
                                        if (item4.FullName != null)
                                        {
                                            XPen pen1 = new XPen(XColors.Black, 1);
                                            System.Drawing.Point pt6 = new System.Drawing.Point(20, Lineheight);
                                            System.Drawing.Point pt7 = new System.Drawing.Point(150, Lineheight);
                                            graph2.DrawLine(lineRed, pt6, pt7);
                                            Lineheight = Lineheight + 5;
                                            graph2.DrawString(item4.FullName, font, XBrushes.Black, new XRect(30, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                                            Lineheight = Lineheight + 35;
                                        }
                                    }

                                }
                            }

                            graph2.DrawString("s.k  Penolong Pengarah", nfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 20;
                            foreach (var item1 in ctx.PALinkBC.Where(x => x.PremiseApplicationID == AppId))
                            {
                                foreach (var item2 in ctx.BCLinkDeps.Where(x => x.BusinessCodeID == item1.BusinessCodeID))
                                {
                                    {
                                        foreach (var item3 in ctx.Departments.Where(x => x.DepartmentID == item2.DepartmentID))
                                        {

                                            if (item3.DepartmentDesc != null)
                                            {

                                                graph2.DrawString(" - " + item3.DepartmentDesc, font, XBrushes.Black, new XRect(40, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                                                cnt = cnt + 1;
                                                Lineheight = Lineheight + 15;
                                            }
                                        }
                                    }

                                }
                            }
                            Lineheight = Lineheight + 10;
                            XPen lineRed1 = new XPen(XColors.Black, 1);
                            System.Drawing.Point pt4 = new System.Drawing.Point(0, Lineheight);
                            System.Drawing.Point pt5 = new System.Drawing.Point(Convert.ToInt32(pdfPage2.Width), Lineheight);
                            graph2.DrawLine(lineRed1, pt4, pt5);
                            Lineheight = Lineheight + 5;
                            graph2.DrawString("UNTUK KEGUNAAN PEJABAT", lbfont, XBrushes.Black, new XRect(200, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 30;
                            graph2.DrawString("NO. RUJUKAN", nfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString(":", font, XBrushes.Black, new XRect(250, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString(DateTime.Now.Year.ToString() + "/P/00000" + AppId, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 20;
                            graph2.DrawString("NAMA PERNIAGAAN", nfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString(":", font, XBrushes.Black, new XRect(250, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            graph2.DrawString(CompName, font, XBrushes.Black, new XRect(300, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 40;
                            XPen pen = new XPen(XColors.Black, 1);
                            graph2.DrawRectangle(pen, 30, Lineheight, 10, 10);
                            graph2.DrawString("Telah disemak dan disahkan betul", nfont, XBrushes.Black, new XRect(100, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 20;
                            graph2.DrawRectangle(pen, 30, Lineheight, 10, 10);
                            graph2.DrawString("Pembetulan semula", nfont, XBrushes.Black, new XRect(100, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 40;
                            System.Drawing.Point pt8 = new System.Drawing.Point(30, Lineheight);
                            System.Drawing.Point pt9 = new System.Drawing.Point(200, Lineheight);
                            graph2.DrawLine(lineRed1, pt8, pt9);
                            Lineheight = Lineheight + 5;
                            graph2.DrawString("(PENYELIA)", font, XBrushes.Black, new XRect(80, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 30;
                            graph2.DrawString("Tarikh      :", nfont, XBrushes.Black, new XRect(30, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            Lineheight = Lineheight + 20;
                            graph2.DrawString("Surat ini adalah cetakan komputer dan tandatangan tidak diperlukan", fontitalik, XBrushes.Black, new XRect(30, Lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            string pdfFilename = "Letter.pdf";
                            pdf.Save(pdfFilename);
                            FileStream fs = new FileStream(pdfFilename, FileMode.Open, FileAccess.Read);
                            return File(fs, "application/pdf");

                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating Letter!');</script>");
        }


        private FileStreamResult GeneratePDF(Int32? AppId)  
        {
            try
            {
               
                return null;
            }
            
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Problem In Generating Letter.";
                return null;
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

        /// <summary>
        /// Get Business Code
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillBusinessCode(string query, int selectedMode, int selectedSector)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BusinessCode> primaryQuery = ctx.BusinessCodes;
                if (selectedMode > 0)
                {
                    primaryQuery = primaryQuery.Where(bc => bc.Mode == selectedMode);
                }
                if (selectedSector > 0)
                {
                    primaryQuery = primaryQuery.Where(bc => bc.SectorID == selectedSector);
                }
                if (!String.IsNullOrWhiteSpace(query))
                {
                    primaryQuery = primaryQuery.Where(bc => bc.CodeDesc.ToLower().Contains(query.ToLower()) || bc.CodeNumber.ToLower().Contains(query.ToLower()));
                }
                var businessCode = primaryQuery.Select(x => new { id = x.BusinessCodeID, text = x.CodeDesc + "~" + x.CodeNumber, mode = x.Mode }).ToList();
                return Json(businessCode, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get Individuale Code
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillIndividual(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var individual = ctx.Individuals.Where(t => t.MykadNo.ToLower().Contains(query.ToLower()) || t.FullName.ToLower().Contains(query.ToLower())).Select(x => new SelectedIndividualModel { id = x.IndividualID, text = x.FullName + " (" + x.MykadNo + ")", fullName = x.FullName, passportNo = x.MykadNo }).ToList();
                return Json(individual, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// get Mykad Data
        /// </summary>
        /// <param name="Individualids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Mykad([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string Individualids)
        {
            List<TradingLicense.Model.IndividualModel> Individual = new List<Model.IndividualModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {

                string[] ids = null;
                if (!string.IsNullOrWhiteSpace(Individualids))
                {
                    ids = Individualids.Split(',');
                }

                List<int> Individuallist = new List<int>();

                foreach (string id in ids)
                {
                    int BusinessCodeID = Convert.ToInt32(id);
                    Individuallist.Add(BusinessCodeID);
                }

                IQueryable<Individual> query = ctx.Individuals.Where(r => Individuallist.Contains(r.IndividualID));

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

                var result = Mapper.Map<List<IndividualModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "IndividualID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                Individual = result;

            }
            return Json(new DataTablesResponse(requestModel.Draw, Individual, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get Additional Document Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="BusinessCodeids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AdditionalDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string BusinessCodeids, string premiseApplicationID)
        {
            List<TradingLicense.Model.BCLinkADModel> RequiredDocument = new List<Model.BCLinkADModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                string[] ids = null;

                if (!string.IsNullOrWhiteSpace(BusinessCodeids))
                {
                    ids = BusinessCodeids.Split(',');
                }

                List<int> BusinessCodelist = new List<int>();

                if (ids != null)
                {
                    foreach (string id in ids)
                    {
                        int BusinessCodeID = Convert.ToInt32(id);
                        BusinessCodelist.Add(BusinessCodeID);
                    }
                }

                IQueryable<BCLinkAD> query = ctx.BCLinkAD.Where(p => BusinessCodelist.Contains(p.BusinessCodeID));

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

                var result = Mapper.Map<List<BCLinkADModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BCLinkADID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                RequiredDocument = result;


                #region IsChecked

                if (!string.IsNullOrWhiteSpace(premiseApplicationID))
                {
                    int premiseAppId;
                    int.TryParse(premiseApplicationID, out premiseAppId);

                    var palinkAdd = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == premiseAppId).ToList();
                    foreach (var item in RequiredDocument)
                    {
                        if (palinkAdd != null && palinkAdd.Count > 0)
                        {
                            var resultpalinkReq = palinkAdd.Where(p => p.AdditionalDocID == item.AdditionalDocID && p.PremiseApplicationID == premiseAppId).FirstOrDefault();
                            if (resultpalinkReq != null)
                            {
                                item.IsChecked = "checked";
                                var attechmentdetails = ctx.Attachments.Where(a => a.AttachmentID == resultpalinkReq.AttachmentID).FirstOrDefault();
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
            return Json(new DataTablesResponse(requestModel.Draw, RequiredDocument, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save Attachment Infomration
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadDocument(HttpPostedFileBase DocumentFile)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    if (DocumentFile != null)
                    {
                        var file = DocumentFile;
                        if (file != null && file.ContentLength > 0)
                        {
                            var premisevalue = Request["PremiseApplicationID"];
                            var reqDocvalue = Request["reqDocid"];
                            var addDocvalue = Request["addDocid"];
                            var isReqvalue = Request["isReqDoc"];

                            int premiseApplicationID;
                            if (int.TryParse(premisevalue, out premiseApplicationID) && premiseApplicationID > 0)
                            {
                                int requiredDocID;
                                int.TryParse(reqDocvalue, out requiredDocID);

                                int additionalDocID;
                                int.TryParse(addDocvalue, out additionalDocID);

                                if (requiredDocID > 0 || additionalDocID > 0)
                                {
                                    int isReq;
                                    int.TryParse(isReqvalue, out isReq);

                                    var fileName = Path.GetFileName(file.FileName);

                                    var folder = Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.PremiseAttachmentDocument + "\\" + premiseApplicationID.ToString());
                                    var path = Path.Combine(folder, fileName);
                                    if (!Directory.Exists(folder))
                                    {
                                        Directory.CreateDirectory(folder);
                                    }
                                    file.SaveAs(path);

                                    Attachment attachment = new Attachment();
                                    attachment.FileName = fileName;
                                    ctx.Attachments.AddOrUpdate(attachment);
                                    ctx.SaveChanges();

                                    if (attachment.AttachmentID > 0)
                                    {
                                        if (isReq > 0)
                                        {
                                            PALinkReqDoc paLinkReqDoc = new PALinkReqDoc();
                                            paLinkReqDoc = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == premiseApplicationID && p.RequiredDocID == requiredDocID).FirstOrDefault();
                                            if (paLinkReqDoc != null)
                                            {
                                                paLinkReqDoc.AttachmentID = attachment.AttachmentID;
                                                ctx.PALinkReqDoc.AddOrUpdate(paLinkReqDoc);
                                                ctx.SaveChanges();
                                            }
                                            else
                                            {
                                                PALinkReqDoc paLinkReqDocument = new PALinkReqDoc();
                                                paLinkReqDocument.PremiseApplicationID = premiseApplicationID;
                                                paLinkReqDocument.RequiredDocID = requiredDocID;
                                                paLinkReqDocument.AttachmentID = attachment.AttachmentID;
                                                ctx.PALinkReqDoc.AddOrUpdate(paLinkReqDocument);
                                                ctx.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            PALinkAddDoc paLinkAddDoc = new PALinkAddDoc();
                                            paLinkAddDoc = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == premiseApplicationID && p.AdditionalDocID == additionalDocID).FirstOrDefault();
                                            if (paLinkAddDoc != null)
                                            {
                                                paLinkAddDoc.AttachmentID = attachment.AttachmentID;
                                                ctx.PALinkAddDocs.AddOrUpdate(paLinkAddDoc);
                                                ctx.SaveChanges();
                                            }
                                            else
                                            {
                                                PALinkAddDoc paLinkAddDocument = new PALinkAddDoc();
                                                paLinkAddDocument.PremiseApplicationID = premiseApplicationID;
                                                paLinkAddDocument.AdditionalDocID = additionalDocID;
                                                paLinkAddDocument.AttachmentID = attachment.AttachmentID;
                                                ctx.PALinkAddDocs.AddOrUpdate(paLinkAddDocument);
                                                ctx.SaveChanges();
                                            }
                                        }

                                        return Json(new { status = "1", message = "Document Upload Successfully" }, JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
            {
                return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Save Attachment Infomration
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadAttechment(HttpPostedFileBase DocumentFile)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    if (DocumentFile != null)
                    {
                        var file = DocumentFile;
                        if (file != null && file.ContentLength > 0)
                        {
                            var reqDocvalue = Request["reqDocid"];
                            var addDocvalue = Request["addDocid"];
                            var isReqvalue = Request["isReqDoc"];

                            int requiredDocID;
                            int.TryParse(reqDocvalue, out requiredDocID);

                            int additionalDocID;
                            int.TryParse(addDocvalue, out additionalDocID);

                            if (requiredDocID > 0 || additionalDocID > 0)
                            {
                                int isReq;
                                int.TryParse(isReqvalue, out isReq);

                                var fileName = Path.GetFileName(file.FileName);

                                var folder = Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument);
                                var path = Path.Combine(folder, fileName);
                                if (!Directory.Exists(folder))
                                {
                                    Directory.CreateDirectory(folder);
                                }
                                file.SaveAs(path);

                                Attachment attachment = new Attachment();
                                attachment.FileName = fileName;
                                ctx.Attachments.AddOrUpdate(attachment);
                                ctx.SaveChanges();

                                if (attachment.AttachmentID > 0)
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

                                        return Json(new { status = "1", result = new { status = "1", RequiredDocID = requiredDocID, AttachmentID = attachment.AttachmentID, AttachmentName = attachment.FileName } }, JsonRequestBehavior.AllowGet);
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

                                        return Json(new { status = "1", result = new { status = "1", AdditionalDocID = additionalDocID, AttachmentID = attachment.AttachmentID, AttachmentName = attachment.FileName } }, JsonRequestBehavior.AllowGet);
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
            catch (Exception ex)
            {
                return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Save Attachment Infomration
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveComment()
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var premiseID = Request["PremiseApplicationID"];
                    var comment = Request["comment"];
                    var approveRejectType = Request["approveRejectType"];  /// 1) Approve  2) Reject 

                    int PremiseApplicationID;
                    int.TryParse(premiseID, out PremiseApplicationID);

                    int UsersID = 0;
                    int UserroleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
                    {
                        UsersID = ProjectSession.User.UsersID;
                        UserroleTemplate = ProjectSession.User.RoleTemplateID.Value;
                    }

                    if (PremiseApplicationID > 0 && UsersID > 0 && UserroleTemplate > 0)
                    {
                        PAComment paComment = new PAComment();
                        paComment.Comment = comment;
                        paComment.PremiseApplicationID = PremiseApplicationID;
                        paComment.UsersID = UsersID;
                        paComment.CommentDate = DateTime.Now;
                        ctx.PAComments.AddOrUpdate(paComment);
                        ctx.SaveChanges();

                        if (paComment.PACommentID > 0)
                        {
                            var premiseApplication = ctx.PremiseApplications.Where(p => p.PremiseApplicationID == PremiseApplicationID).FirstOrDefault();
                            var paLinkBC = ctx.PALinkBC.Where(t => t.PremiseApplicationID == PremiseApplicationID).ToList();

                            if (UserroleTemplate == (int)RollTemplate.Clerk)
                            {
                                if (approveRejectType == "Approve")
                                {
                                    var paLinkBusinessCode = ctx.PALinkBC.Where(t => t.PremiseApplicationID == PremiseApplicationID && t.BusinessCode != null).ToList();
                                    if (paLinkBusinessCode != null && paLinkBusinessCode.Count > 0)
                                    {
                                        if (premiseApplication != null && premiseApplication.PremiseApplicationID > 0)
                                        {
                                            premiseApplication.AppStatusID = (int)PAStausenum.unitroute;
                                            ctx.PremiseApplications.AddOrUpdate(premiseApplication);
                                            ctx.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        if (premiseApplication != null && premiseApplication.PremiseApplicationID > 0)
                                        {
                                            premiseApplication.AppStatusID = (int)PAStausenum.LetterofnotificationApproved;
                                            ctx.PremiseApplications.AddOrUpdate(premiseApplication);
                                            ctx.SaveChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    premiseApplication.AppStatusID = (int)PAStausenum.draftcreated;
                                    ctx.PremiseApplications.AddOrUpdate(premiseApplication);
                                    ctx.SaveChanges();
                                }
                            }

                            if (UserroleTemplate == (int)RollTemplate.RouteUnit)
                            {

                            }

                            if (UserroleTemplate == (int)RollTemplate.Supervisor)
                            {

                            }

                            if (UserroleTemplate == (int)RollTemplate.Director)
                            {

                            }

                            return Json(new { status = "1", message = "Comment Save Successfully" }, JsonRequestBehavior.AllowGet);
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
            }
            catch (Exception ex)
            {
                return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
            }
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
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string codeNumber, string codeDesc, string sectorID)
        {
            List<TradingLicense.Model.BusinessCodeModel> BusinessCode = new List<Model.BusinessCodeModel>();
            int totalRecord = 0;
            // int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BusinessCode> query = ctx.BusinessCodes;

                #region Filtering

                // Apply filters for comman Grid searching
                if (requestModel.Search.Value != string.Empty)
                {
                    var value = requestModel.Search.Value.ToLower().Trim();
                    query = query.Where(p => p.CodeNumber.ToLower().Contains(value) ||
                                             p.CodeDesc.ToLower().Contains(value) ||
                                             p.SectorID.ToString().Contains(value) ||
                                             p.DefaultRate.ToString().Contains(value) ||
                                             p.Sector.SectorDesc.ToLower().Contains(value)
                                       );
                }

                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(codeNumber))
                {
                    query = query.Where(p => p.CodeNumber.ToLower().Contains(codeNumber.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(codeDesc))
                {
                    query = query.Where(p => p.CodeDesc.ToLower().Contains(codeDesc.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(sectorID))
                {
                    query = query.Where(p => p.SectorID.ToString().Contains(sectorID));
                }

                // Filter End

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

                var result = Mapper.Map<List<BusinessCodeModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BusinessCodeID asc" : orderByString).ToList();

                totalRecord = result.Count();
                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();

                BusinessCode = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, BusinessCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get BusinessCode Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageBusinessCode(int? Id)
        {
            BusinessCodeModel businessCodeModel = new BusinessCodeModel();
            businessCodeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int businessCodeID = Convert.ToInt32(Id);
                    var businessCode = ctx.BusinessCodes.Where(a => a.BusinessCodeID == businessCodeID).FirstOrDefault();
                    businessCodeModel = Mapper.Map<BusinessCodeModel>(businessCode);

                    var additionalDocs = ctx.BCLinkAD.Where(blAD => blAD.BusinessCodeID == businessCodeID);
                    if (additionalDocs.Count() > 0)
                    {
                        businessCodeModel.AdditionalDocs = additionalDocs.Select(blAD => blAD.AdditionalDocID).ToList();
                    }
                    else
                    {
                        businessCodeModel.AdditionalDocs = new List<int>();
                    }

                    var departments = ctx.BCLinkDeps.Where(blD => blD.BusinessCodeID == businessCodeID);
                    if (departments.Count() > 0)
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
        public ActionResult ManageBusinessCode(BusinessCodeModel businessCodeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    BusinessCode businessCode;
                    if (IsBusinessCodeDuplicate(businessCodeModel.CodeNumber, businessCodeModel.BusinessCodeID))
                    {
                        TempData["ErrorMessage"] = "Business Code is already exist in the database.";
                        return View(businessCodeModel);
                    }
                    businessCode = Mapper.Map<BusinessCode>(businessCodeModel);
                    ctx.BusinessCodes.AddOrUpdate(businessCode);
                    ctx.SaveChanges();

                    if (!string.IsNullOrEmpty(businessCodeModel.DepartmentIDs))
                    {
                        List<BCLinkDep> selectedDepartments = new List<BCLinkDep>();
                        var selectedDeps = ctx.BCLinkDeps.Where(bd => bd.BusinessCodeID == businessCode.BusinessCodeID).ToList();
                        var deptIds = businessCodeModel.DepartmentIDs.Split(',');
                        foreach (var dep in deptIds)
                        {
                            var depId = Convert.ToInt32(dep);
                            if (selectedDeps == null || !selectedDeps.Any(sd => sd.DepartmentID == depId))
                            {
                                selectedDepartments.Add(new BCLinkDep { BusinessCodeID = businessCode.BusinessCodeID, DepartmentID = depId });
                            }
                        }
                        if (selectedDeps != null && selectedDeps.Count > 0)
                        {
                            foreach (var bcDep in selectedDeps)
                            {
                                if (!deptIds.Any(rd => rd == bcDep.DepartmentID.ToString()))
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
                        var selectedADocs = ctx.BCLinkAD.Where(bd => bd.BusinessCodeID == businessCode.BusinessCodeID).ToList();
                        var addDocIds = businessCodeModel.AdditionalDocs;
                        foreach (var addDocId in addDocIds)
                        {
                            if (selectedADocs == null || !selectedADocs.Any(sd => sd.AdditionalDocID == addDocId))
                            {
                                selectedAdditionalDocs.Add(new BCLinkAD { BusinessCodeID = businessCode.BusinessCodeID, AdditionalDocID = addDocId });
                            }
                        }
                        if (selectedADocs != null && selectedADocs.Count > 0)
                        {
                            foreach (var bcDep in selectedADocs)
                            {
                                if (!addDocIds.Any(rd => rd == bcDep.AdditionalDocID))
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
        public ActionResult DeleteBusinessCode(int id)
        {
            try
            {
                var BusinessCode = new TradingLicense.Entities.BusinessCode() { BusinessCodeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(BusinessCode).State = System.Data.Entity.EntityState.Deleted;
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
               ctx.BusinessCodes.FirstOrDefault(
                   c => c.BusinessCodeID != id && c.CodeNumber.ToLower() == name.ToLower())
               : ctx.BusinessCodes.FirstOrDefault(
                   c => c.CodeNumber.ToLower() == name.ToLower());
                return existObj != null;
            }
        }



        /// <summary>
        /// Get Departments List
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillDepartments(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<Department> primaryQuery = ctx.Departments;
                if (!String.IsNullOrWhiteSpace(query))
                {
                    primaryQuery = primaryQuery.Where(bc => bc.DepartmentDesc.ToLower().Contains(query.ToLower()) || bc.DepartmentCode.ToLower().Contains(query.ToLower()));
                }
                var businessCode = primaryQuery.Select(x => new { id = x.DepartmentID, text = x.DepartmentCode + "~" + x.DepartmentDesc }).ToList();
                return Json(businessCode, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}