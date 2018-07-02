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
using System.Web.Script.Serialization;
using System.Web;
using System.IO;
using TradingLicense.Infrastructure;
using System.Data.Entity;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Drawing.Layout;
using static TradingLicense.Infrastructure.Enums;
using TradingLicense.Web.Services;

namespace TradingLicense.Web.Controllers
{
    public class BannerApplicationController : BaseController
    {
        public const string OnSubmit = "Submitted";
        public const string OnRouteSubmit = "SubmittedToRoute";
        public const string OnRejected = "Rejected";
        public const string OnKIV = "KIV";

        LicenseApplicationContext db = new LicenseApplicationContext();
        
        #region BannerCode

        #region Display BannerCode page
        /// <summary>
        /// GET: BannerCode
        /// </summary>
        /// <returns></returns>
        public ActionResult BannerCode()
        {
            return View();
        }
        #endregion

        #region BannerCode list for Datatable
        /// <summary>
        /// BannerCode List page
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BannerCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string bannerCodeDesc)
        {
            List<BannerCodeModel> bannerCode = new List<BannerCodeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BannerCode> query = ctx.BannerCodes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(bannerCodeDesc))
                {
                    query = query.Where(p =>
                                        p.BannerCodeDesc.Contains(bannerCodeDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "BannerCodeID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                bannerCode = Mapper.Map<List<BannerCodeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, bannerCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ManageBannerCode page
        /// <summary>
        /// Get BannerCode Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageBannerCode(int? Id)
        {
            BannerCodeModel bannerCodeModel = new BannerCodeModel();
            bannerCodeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int bannerCodeID = Convert.ToInt32(Id);
                    var bannerCode = ctx.BannerCodes.Where(a => a.BannerCodeID == bannerCodeID).FirstOrDefault();
                    bannerCodeModel = Mapper.Map<BannerCodeModel>(bannerCode);
                }
            }

            return View(bannerCodeModel);
        }
        #endregion

        #region Save BannerCode
        /// <summary>
        /// Save Banner Code data
        /// </summary>
        /// <param name="bannerCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageBannerCode(BannerCodeModel bannerCodeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    BannerCode bannerCode;
                    if (IsBannerCodeDuplicate(bannerCodeModel.BannerCodeDesc, bannerCodeModel.BannerCodeID))
                    {
                        TempData["ErrorMessage"] = "Banner Code already exists in the database.";
                        return View(bannerCodeModel);
                    }

                    bannerCode = Mapper.Map<BannerCode>(bannerCodeModel);
                    ctx.BannerCodes.AddOrUpdate(bannerCode);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Banner Code saved successfully.";

                return RedirectToAction("BannerCode");
            }
            else
            {
                return View(bannerCodeModel);
            }

        }
        #endregion

        #region Delete Banner Code data
        /// <summary>
        /// Delete Banner Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBannerCode(int id)
        {
            try
            {
                var bannerCode = new TradingLicense.Entities.BannerCode() { BannerCodeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(bannerCode).State = System.Data.Entity.EntityState.Deleted;
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

        #region Check Duplicate Banner Code
        /// <summary>
        /// Check Duplicate
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsBannerCodeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.BannerCodes.FirstOrDefault(
                   c => c.BannerCodeID != id && c.BannerCodeDesc.ToLower() == name.ToLower())
               : ctx.BannerCodes.FirstOrDefault(
                   c => c.BannerCodeDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }
        #endregion

        #endregion

        #region BannerApplication

        /// <summary>
        /// GET: BannerApplication
        /// </summary>
        /// <returns></returns>
        public ActionResult BannerApplication()
        {
            return View();
        }

        #region Get BannerApplication List Information for Datatable
        /// <summary>
        /// Get BannerApplication List Information for Datatable
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BannerApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string bannerApplicationId, string individualMkNo)
        {
            List<BannerApplicationModel> bannerApplication;
            int totalRecord = 0;          
            using (var ctx = new LicenseApplicationContext())
            {
                int? rollTemplateID = ProjectSession.User?.RoleTemplateID;
                IQueryable<BannerApplication> query = ctx.BannerApplications;

                if (!string.IsNullOrWhiteSpace(bannerApplicationId))
                {
                    query = query.Where(q => q.BannerApplicationID.ToString().Contains(bannerApplicationId));
                }

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = String.Empty;

                var result = Mapper.Map<List<BannerApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BannerApplicationID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion

                //Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                bannerApplication = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, bannerApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get BannerApplication Data By Individual for Datatable
        /// <summary>
        /// Get BannerApplication Data By Individual for Datatable
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BannerApplicationsByIndividual([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? individualId)
        {
            List<Model.BannerApplicationModel> bannerApplication = new List<Model.BannerApplicationModel>();
            int totalRecord = 0;

            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    IQueryable<BannerApplication> query = ctx.BannerApplications
                                                                .Include("AppStatus")
                                                                .Include("Company")
                                                                .Include("Individual")
                                                                .Where(ba => ba.IndividualID == individualId);
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

                    query = query.OrderBy(orderByString == string.Empty ? "BannerApplicationID asc" : orderByString);

                    #endregion Sorting
                    // Paging
                    query = query.Skip(requestModel.Start).Take(requestModel.Length);
                    var Dtls = db.BannerApplications
                                        .Include("AppStatus")
                                        .Include("Company")
                                        .Include("Individual")
                                        .Where(ba => ba.IndividualID == individualId)
                                        .OrderBy(m => m.BannerApplicationID).ToList();


                    /*var Dtls = query.ToList();*/
                    return Json(new DataTablesResponse(requestModel.Draw, Dtls.ToList(), totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return null;

            }
        }
        #endregion

        #region ManageBannerApplication
        /// <summary>
        /// Get BannerApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageBannerApplication(int? Id)
        {
            BannerApplicationModel bannerApplicationModel = new BannerApplicationModel();
            List<BannerObject> BannerObjectModel = new List<BannerObject>();
            List<BAReqDocModel> BAReqDoc = new List<BAReqDocModel>();
            List<BALinkReqDoc> BALinkReqDoc = new List<BALinkReqDoc>();
            List<Attchments> Attchments = new List<Attchments>();

            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BAReqDoc> query = ctx.BAReqDocs;
                BAReqDoc = Mapper.Map<List<BAReqDocModel>>(query.ToList());
                ViewBag.bannerDocList = ctx.BAReqDocs.ToList();

                if (Id != null && Id > 0)
                {
                    var bannerApplication = ctx.BannerApplications.FirstOrDefault(a => a.BannerApplicationID == Id);
                    bannerApplicationModel = Mapper.Map<BannerApplicationModel>(bannerApplication);

                    var baLinkBO = ctx.BannerObjects.Where(a => a.BannerApplicationID == Id).ToList();
                    bannerApplicationModel.BannerObjectids = string.Join(",", baLinkBO.Select(x => x.BannerCodeID.ToString()).ToArray());

                    var Docs = (from d in db.BALinkReqDocs
                                join f in db.Attachments
                                on d.AttachmentID equals f.AttachmentID
                                where d.BannerApplicationID == Id
                                select new
                                {
                                    AttachmentID = d.AttachmentID,
                                    RequiredDocID = d.RequiredDocID,
                                    FileName = f.FileName
                                }).ToList();
                    foreach (var item in Docs)
                    {
                        Attchments Atch = new Attchments();
                        Atch.Id = Convert.ToInt32(item.AttachmentID);
                        Atch.RequiredDocID = item.RequiredDocID;
                        Atch.filename = item.FileName;
                        Attchments.Add(Atch);
                    }
                }
            }
            ViewBag.BALinkReqDoc = Attchments;
            ViewBag.BannerObjects = BannerObjectModel;
            ViewBag.UserRole = ProjectSession.User.RoleTemplateID;
            return View(bannerApplicationModel);
        }
        #endregion

        #region Get AppStatusID Upon Submit Button
        private int GetStatusOnSubmit(BannerApplicationModel bannerApplicationModel, LicenseApplicationContext ctx, BannerApplication bannerApplication, int roleTemplate)
        {
            PAStausenum finalStatus = 0;
            if (!bannerApplicationModel.IsDraft)
            {
                switch (roleTemplate)
                {
                    case (int)RollTemplate.DeskOfficer:
                        finalStatus = PAStausenum.submittedtoclerk;
                        if (bannerApplicationModel.AppStatusID == (int)PAStausenum.meeting)
                        {
                            if (bannerApplicationModel.SubmitType == OnSubmit)
                            {
                                finalStatus = PAStausenum.LetterofnotificationApproved;
                            }
                            else if (bannerApplicationModel.SubmitType == OnRejected)
                            {
                                finalStatus = PAStausenum.LetterofnotificationRejected;
                            }
                        }
                        break;
                    case (int)RollTemplate.Clerk:
                        if (bannerApplicationModel.AppStatusID == (int)PAStausenum.meeting)
                        {
                            if (bannerApplicationModel.SubmitType == OnSubmit)
                            {
                                finalStatus = PAStausenum.LetterofnotificationApproved;
                            }
                            else if (bannerApplicationModel.SubmitType == OnRejected)
                            {
                                finalStatus = PAStausenum.LetterofnotificationRejected;
                            }
                        }
                        else if (bannerApplicationModel.SubmitType == OnSubmit)
                        {
                            finalStatus = PAStausenum.directorcheck;
                        }
                        break;
                        case (int)RollTemplate.Director:
                        if (bannerApplicationModel.AppStatusID == (int)PAStausenum.meeting)
                        {
                            if (bannerApplicationModel.SubmitType == OnSubmit)
                            {
                                finalStatus = PAStausenum.LetterofnotificationApproved;
                            }
                            else if (bannerApplicationModel.SubmitType == OnRejected)
                            {
                                finalStatus = PAStausenum.LetterofnotificationRejected;
                            }
                        }
                        else if (bannerApplicationModel.SubmitType == OnRejected)
                        {
                            finalStatus = PAStausenum.LetterofnotificationRejected;
                        }
                        break;
                    case (int)RollTemplate.CEO:
                        if (bannerApplicationModel.SubmitType == OnSubmit)
                        {
                            finalStatus = PAStausenum.LetterofnotificationApproved;
                        }
                        else if (bannerApplicationModel.SubmitType == OnRejected)
                        {
                            finalStatus = PAStausenum.LetterofnotificationRejected;
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

        #region Save ManageBannerApplication
        /// <summary>
        /// Save Banner Application Information
        /// </summary>
        /// <param name="bannerApplicationModel"></param>
        /// <returns></returns>
        [HttpPost]
        private bool SaveManageBannerApplication(BannerApplicationModel bannerApplicationModel, LicenseApplicationContext ctx)
        {
            var bannerApplication = Mapper.Map<BannerApplication>(bannerApplicationModel);

            int userroleTemplate = 0;
            if (ProjectSession.User != null && ProjectSession.UserID > 0)
            {
                userroleTemplate = GetUserRoleTemplate(bannerApplicationModel, bannerApplication, ctx);
            }
            var finalStatus = GetStatusOnSubmit(bannerApplicationModel, ctx, bannerApplication, userroleTemplate);
            if (finalStatus != 0)
            {
                bannerApplication.AppStatusID = finalStatus;
            }
            bannerApplication.DateSubmitted = DateTime.Now;

            ctx.BannerApplications.AddOrUpdate(bannerApplication);
            ctx.SaveChanges();

            int bannerApplicationId = bannerApplication.BannerApplicationID;
            if (bannerApplicationModel.BannerApplicationID == 0)
            {
                bannerApplicationModel.BannerApplicationID = bannerApplicationId;
                bannerApplication.ReferenceNo = BannerApplicationModel.GetReferenceNo(bannerApplicationId, bannerApplication.DateSubmitted);
                ctx.BannerApplications.AddOrUpdate(bannerApplication);
                ctx.SaveChanges();
            }

            int roleTemplate = 0;
            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                roleTemplate = ProjectSession.User.RoleTemplateID.Value;
            }

            if (userroleTemplate == (int)RollTemplate.Public)
            {
                if (!string.IsNullOrWhiteSpace(bannerApplicationModel.UploadRequiredDocids))
                {
                    BADocumentService.UpdateDocs(bannerApplicationModel, ctx, bannerApplicationId, roleTemplate);
                }
                else
                {
                    if (roleTemplate == (int)RollTemplate.Public)
                    {
                        var baLinkReqDocumentList = ctx.BALinkReqDocs
                            .Where(p => p.BannerApplicationID == bannerApplicationId).ToList();
                        if (baLinkReqDocumentList.Count > 0)
                        {
                            ctx.BALinkReqDocs.RemoveRange(baLinkReqDocumentList);
                            ctx.SaveChanges();
                        }
                    }
                }
            }
            else if (userroleTemplate == (int)RollTemplate.DeskOfficer)
            {
                if (!string.IsNullOrWhiteSpace(bannerApplicationModel.RequiredDocIds))
                {
                    BADocumentService.UpdateRequiredDocs(bannerApplicationModel, ctx, bannerApplicationId, roleTemplate);
                }
                else
                {
                    if (!bannerApplicationModel.IsDraft && roleTemplate == (int)RollTemplate.Public || roleTemplate == (int)RollTemplate.DeskOfficer)
                    {
                        var baLinkReqDocumentList = ctx.BALinkReqDocs.Where(p => p.BannerApplicationID == bannerApplicationId).ToList();
                        if (baLinkReqDocumentList.Count > 0)
                        {
                            ctx.BALinkReqDocs.RemoveRange(baLinkReqDocumentList);
                            ctx.SaveChanges();
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(bannerApplicationModel.newComment))
            {
                BAComment comment = new BAComment();
                comment.Comment = bannerApplicationModel.newComment;
                comment.CommentDate = DateTime.Now;
                comment.BannerApplicationID = bannerApplicationId;
                comment.UsersID = ProjectSession.UserID;
                ctx.BAComments.Add(comment);
                ctx.SaveChanges();
            }
            bannerApplicationModel.BannerApplicationID = bannerApplicationId;
            return true;
        }
        #endregion

        #region Get Roletemplate from ProjectSession

        private static int GetUserRoleTemplate(BannerApplicationModel bannerApplicationModel,
            BannerApplication bannerApplication, LicenseApplicationContext ctx)
        {
            int userroleTemplate = 0;
            bannerApplication.UpdatedBy = ProjectSession.User.Username;

            if (ProjectSession.User.RoleTemplateID != null)
            {
                userroleTemplate = ProjectSession.User.RoleTemplateID.Value;
            }

            return userroleTemplate;
        }
        #endregion

        #region Save Banner Objects
        [HttpPost]
        public ActionResult AddBannerObject(int bannerApplicationID, int BannerCode, int Location, float SaizIklan, int BilanganIklan )
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var ba = ctx.BannerObjects.Where(p => p.BannerApplicationID == bannerApplicationID).FirstOrDefault();
                var Fee = ctx.BannerCodes.Where(p => p.BannerCodeID == BannerCode).Select(p => p.PeriodFee).FirstOrDefault();
                var eFee = ctx.BannerCodes.Where(p => p.BannerCodeID == BannerCode).Select(p => p.ExtraFee).FirstOrDefault();
                float TotalFee = 0;

                if (ba != null)
                {
                    ba.BannerApplicationID = bannerApplicationID;
                    ba.BannerCodeID = BannerCode;
                    ba.LocationID = Location;
                    ba.BSize = SaizIklan;
                    ba.BQuantity = BilanganIklan;
                    if(SaizIklan <= 8)
                    {
                        TotalFee = Fee * BilanganIklan;
                    }
                    else
                    {
                        TotalFee = (((float)Math.Floor(SaizIklan - 8)*eFee)+ Fee)*BilanganIklan;
                    }
                    ba.Fee = TotalFee;

                    ctx.BannerObjects.AddOrUpdate(ba);
                    ctx.SaveChanges();
                    TempData["SuccessMessage"] = "Premise License Application payments saved successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to find matching banner application ID";
                }
            }

            return Redirect(Url.Action("ManageBannerApplication", "BannerApplication") + "?id=" + bannerApplicationID);
        }
        #endregion

        #region Delete BannerApplication from Datatable List
        /// <summary>
        /// Delete Banner Application Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBannerApplication(int id)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Int32[] AtchId = new int[db.BALinkReqDocs.Where(x => x.BannerApplicationID == id).Count()];
                    var bannerApplication = new TradingLicense.Entities.BannerApplication() { BannerApplicationID = id };
                    int cnt = 0;
                    foreach (var item in db.BALinkReqDocs.Where(x => x.BannerApplicationID == id))
                    {
                        AtchId[cnt] = Convert.ToInt32(item.AttachmentID);
                        cnt = cnt + 1;
                    }
                    foreach (var item in AtchId)
                    {
                        db.BALinkReqDocs.RemoveRange(db.BALinkReqDocs.Where(x => x.AttachmentID == item));
                        db.SaveChanges();
                        db.Attachments.RemoveRange(db.Attachments.Where(x => x.AttachmentID == item));
                        db.SaveChanges();
                    }

                    db.Entry(bannerApplication).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    transaction.Commit();
                    return Json(new { success = true, message = " Deleted Successfully" }, JsonRequestBehavior.AllowGet);

                }
                catch
                {
                    transaction.Rollback();
                    return Json(new { success = false, message = "Error While Delete Record" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        #endregion

        #endregion

        #region BAReqDoc

        /// <summary>
        /// GET: BAReqDoc
        /// </summary>
        /// <returns></returns>
        public ActionResult BAReqDoc()
        {
            return View();
        }

        /// <summary>
        /// Save Banner Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BAReqDoc([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string BAReqDocDesc)
        {
            List<TradingLicense.Model.BAReqDocModel> BAReqDoc = new List<Model.BAReqDocModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BAReqDoc> query = ctx.BAReqDocs;
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

                query = query.OrderBy(orderByString == string.Empty ? "BAReqDocID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                BAReqDoc = Mapper.Map<List<BAReqDocModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, BAReqDoc, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get BAReqDoc Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageBAReqDoc(int? Id)
        {
            BAReqDocModel BAReqDocModel = new BAReqDocModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int BAReqDocID = Convert.ToInt32(Id);
                    var BAReqDoc = ctx.BAReqDocs.Where(a => a.BAReqDocID == BAReqDocID).FirstOrDefault();
                    BAReqDocModel = Mapper.Map<BAReqDocModel>(BAReqDoc);
                }
            }
            
            IList<BAReqDoc> list = db.BAReqDocs.ToList();
            ViewData["BAReqDocs"] = list;

            return View(BAReqDocModel);
        }

        [HttpPost]
        public JsonResult SaveBAReqDoc(List<BAReqDoc> lstBarReqDoc)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    
                    List<BAReqDoc> lstToDelete = ctx.BAReqDocs.ToList().Except(lstBarReqDoc).ToList();
                    foreach (var v in lstToDelete)
                    {
                        ctx.BAReqDocs.Remove(v);
                        ctx.SaveChanges();
                    }
                    foreach (var item in lstBarReqDoc)
                    {
                        var DocCnt = ctx.BAReqDocs.Where(x => x.RequiredDocID == item.RequiredDocID).Count();
                        if (DocCnt == 0)
                        {
                            BAReqDoc BAReqDoc = new BAReqDoc();
                            BAReqDoc.BAReqDocID = 0;
                            BAReqDoc.RequiredDocID = item.RequiredDocID;
                            ctx.BAReqDocs.AddOrUpdate(BAReqDoc);
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
        [HttpPost]
        public JsonResult DeleteFile(int? id, string FileName, int BannerAppId)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if ((id != null && id > 0) && FileName != "")
                    {
                        db.BALinkReqDocs.RemoveRange(db.BALinkReqDocs.Where(x => x.AttachmentID == id));
                        db.Attachments.RemoveRange(db.Attachments.Where(x => x.AttachmentID == id));
                        db.SaveChanges();
                        string fPath = Path.Combine(Server.MapPath("~/Documents/Attachment/BannerApplication/0000000" + BannerAppId), FileName);
                        if (System.IO.File.Exists(fPath))
                        {
                            System.IO.File.Delete(fPath);
                        }
                        transaction.Commit();
                        return Json(new { Result = "1" });
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return Json(new { Result = "0" });
        }

        /// <summary>
        /// Delete Banner Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBAReqDoc(int id)
        {
            try
            {
                var BAReqDoc = new TradingLicense.Entities.BAReqDoc() { BAReqDocID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(BAReqDoc).State = System.Data.Entity.EntityState.Deleted;
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

        #region Generate Letter PDF

        public ActionResult GenerateLetter(Int32? appId)
        {
            BannerApplicationModel bannerApplicationModel = new BannerApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var qry = ctx.BannerApplications
                                        .Include("Company").Where(x => x.BannerApplicationID == appId);
                    var bannerApp = ctx.BannerApplications
                                        .Include("Company").Where(x => x.BannerApplicationID == appId).ToList();
                    if (bannerApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid Banner ApplicationID!');</script>");
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
                            if (item.Company.CompanyName == null)
                            {
                                compName = "";
                            }
                            else
                            {
                                compName = item.Company.CompanyName;
                            }
                            graph.DrawString(compName, nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;

                            string compAdd = "";
                            if (item.Company.CompanyAddress == null)
                            {
                                compAdd = "";
                            }
                            else
                            {
                                compAdd = item.Company.CompanyAddress;
                            }

                            graph.DrawString(compAdd.ToString(), nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;

                            string compPhone = "";
                            if (item.Company.CompanyPhone == null)
                            {
                                compPhone = "";
                            }
                            else
                            {
                                compPhone = item.Company.CompanyPhone;
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
                            graph.DrawString("ACTIVITI", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            int cnt = 1;
                            foreach (var item1 in ctx.BannerObjects.Where(x => x.BannerApplicationID == appId))
                            {
                                if (Convert.ToInt32(item1.BannerCodeID) > 0)
                                {
                                    foreach (var item2 in ctx.BannerCodes.Where(x => x.BannerCodeID == item1.BannerCodeID))
                                    {
                                        {
                                            if (item2.BannerCodeDesc != null)
                                            {
                                                string itm = cnt.ToString() + ")    " + item2.BannerCodeDesc;
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
                            foreach (var item3 in ctx.BannerApplications.Where(x => x.BannerApplicationID == appId))
                            {
                                foreach (var item4 in ctx.Individuals.Where(x => x.IndividualID == item3.IndividualID))
                                {
                                    if (item4.FullName != null)
                                    {
                                        string fName = item4.FullName;
                                        if (item4.MykadNo != null)
                                        {
                                            fName = fName + "(" + item4.MykadNo + ")";
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
                            if (item.AppStatusID == 11)
                            {
                                modeValue = "LULUS BERSYARAT";
                            }
                            else if (item.AppStatusID == 9)
                            {
                                modeValue = "LULUS";
                            }
                            else if (item.AppStatusID == 10)
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

                            foreach (var item5 in ctx.BAComments.Where(x => x.BannerApplicationID == appId))
                            {                                
                                    if (item5.Comment != null)
                                    {
                                        string itm = cnt.ToString() + ") " + item5.Comment;
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
                            if (item.TotalFee != null)
                            {
                                var mval = string.Format("{0:0.00}", item.TotalFee);
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
                            foreach (var item6 in ctx.BannerApplications.Where(x => x.BannerApplicationID == appId))
                            {
                                if (Convert.ToInt32(item6.IndividualID) > 0)
                                {
                                    foreach (var item7 in ctx.Individuals.Where(x => x.IndividualID == item6.IndividualID))
                                    {
                                        if (item7.FullName != null)
                                        {
                                            XPen pen1 = new XPen(XColors.Black, 1);
                                            System.Drawing.Point pt6 = new System.Drawing.Point(20, lineheight);
                                            System.Drawing.Point pt7 = new System.Drawing.Point(150, lineheight);
                                            graph2.DrawLine(lineRed, pt6, pt7);
                                            lineheight = lineheight + 5;
                                            graph2.DrawString(item7.FullName, font, XBrushes.Black, new XRect(30, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
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
                            if (item.ReferenceNo != null)
                            {
                                graph2.DrawString(item.ReferenceNo, font, XBrushes.Black, new XRect(300, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
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

        #region Generate License PDF

        public ActionResult GenerateLicense(Int32? appId)
        {
            PremiseApplicationModel premiseApplicationModel = new PremiseApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var qry = ctx.BannerApplications
                                    .Include("Individual").Where(x => x.BannerApplicationID  == appId);
                    var BannerApp = ctx.BannerApplications.Where(x => x.BannerApplicationID == appId).ToList();
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


                            graph.DrawString("BORANG C", lbfont, XBrushes.Black, new XRect( 0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
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
                            System.Drawing.Point pt1 = new System.Drawing.Point(97, lineheight+13);
                            System.Drawing.Point pt2 = new System.Drawing.Point(290, lineheight+13);
                            graph.DrawLine(lineRed1, pt1, pt2);

                            var Payment = ctx.PaymentReceiveds.Where(x => x.IndividualID == item.IndividualID).ToList();
                            if(Payment != null && Payment.Count() > 0)
                            {
                                graph.DrawString(string.Format("{0:000000}",Payment[0].PaymentReceivedID), nfont, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }

                            graph.DrawString("Rujukan Fail:", nfont, XBrushes.Black, new XRect(291, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            System.Drawing.Point pt3 = new System.Drawing.Point(354, lineheight + 13);
                            System.Drawing.Point pt4 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                            graph.DrawLine(lineRed1, pt3, pt4);
                            if(item.ReferenceNo != null)
                            {
                                graph.DrawString(item.ReferenceNo, nfont, XBrushes.Black, new XRect(354, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("Nama:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            System.Drawing.Point pt5 = new System.Drawing.Point(61, lineheight + 13);
                            System.Drawing.Point pt6 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width)-30, lineheight + 13);
                            graph.DrawLine(lineRed1, pt5, pt6);
                            if(item.Individual.FullName != null)
                            {
                                graph.DrawString(item.Individual.FullName, nfont, XBrushes.Black, new XRect(63, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("Alamat:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            System.Drawing.Point pt7 = new System.Drawing.Point(68, lineheight + 13);
                            System.Drawing.Point pt8 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight + 13);
                            graph.DrawLine(lineRed1, pt7, pt8);
                            if (item.Individual.AddressIC != null)
                            {
                                graph.DrawString(item.Individual.AddressIC, nfont, XBrushes.Black, new XRect(70, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("adalah dilesenkan oleh Perbadanan Labuan untuk mempamerkan iklan/iklan-iklan berikut:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            Int32 cnt = 1;
                            
                            foreach (var item1 in ctx.BannerObjects.Where(x=> x.BannerApplicationID == item.BannerApplicationID))
                            {
                                foreach (var item2 in ctx.BannerCodes.Where(x=> x.BannerCodeID == item1.BannerCodeID) )
                                {
                                    var str = "";
                                    lineheight = lineheight + 25;
                                    if (item2.BannerCodeDesc != null)
                                    {
                                        str = str + item2.BannerCodeDesc + ",";
                                    }
                                        str = str + string.Format("{0:0.00}",item1.BSize) + " meter persegi ";
                                        str = str + " x " +  item1.BQuantity  + " unit";
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
                            foreach (var item1 in ctx.BannerObjects.Where(x => x.BannerApplicationID == item.BannerApplicationID))
                            {
                                foreach (var item2 in ctx.Locations.Where(x => x.LocationID == item1.LocationID))
                                {
                                    if(item2.LocationDesc != null)
                                    {
                                        strLocDesc = strLocDesc + item2.LocationDesc + ",";
                                    }
                                }
                            }
                            if(strLocDesc != "")
                            {
                                var mLen = (strLocDesc.Length) / 108;
                                Int32 TLen = 0;
                                if(mLen.ToString().Contains(".") )
                                {
                                    mLen =Convert.ToInt32( mLen.ToString().Split('.')[0]) +1;
                                }
                                for (int i = 0; i <= mLen; i ++)
                                {
                                    if(i==0)
                                    {
                                        TLen = 0;
                                    }
                                    else
                                    {
                                        TLen = (i * 108) + 1;
                                    }
                                    
                                    if(i==mLen)
                                    {
                                        Int32 sIndex = 0;
                                        Int32 EIndex = 0;
                                        sIndex = 0;
                                        EIndex =strLocDesc.Length;
                                        if (TLen >0)
                                        {
                                            sIndex = TLen - 1;
                                        }
                                        if(TLen > 0)
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
                            if(item.DateApproved != null)
                            {
                                graph.DrawString(string.Format("{0:dd MMMM yyyy}", item.DateApproved), nfont, XBrushes.Black, new XRect(82, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("sehingga:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            System.Drawing.Point pt13 = new System.Drawing.Point(77, lineheight + 13);
                            System.Drawing.Point pt14 = new System.Drawing.Point(300, lineheight + 13);
                            graph.DrawLine(lineRed1, pt13, pt14);
                            if (item.DateApproved != null)
                            {
                                graph.DrawString(string.Format("{0:dd MMMM yyyy}", item.DateApproved.Value.AddYears(1)), nfont, XBrushes.Black, new XRect(77, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            //graph.DrawString("200", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            //pt13 = new System.Drawing.Point(322, lineheight + 13);
                            //pt14 = new System.Drawing.Point(360, lineheight + 13);
                            //graph.DrawLine(lineRed1, pt13, pt14);
                            graph.DrawString("tertakluk kepada syarat-syarat berikut:", nfont, XBrushes.Black, new XRect(335, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            cnt = 1;
                            foreach (var item3 in ctx.BAComments.Where(x=> x.BannerApplicationID ==item.BannerApplicationID ) )
                            {
                                if(item3.Comment != null)
                                {
                                    if(lineheight >= 785)
                                    {
                                        lineheight = 20;
                                        pdfPage = pdf.AddPage();
                                        graph = XGraphics.FromPdfPage(pdfPage);
                                    }
                                    lineheight = lineheight + 25;
                                    graph.DrawString(("(" + cnt.ToString() + ") ") + item3.Comment, nfont, XBrushes.Black, new XRect(27, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    pt13 = new System.Drawing.Point(43, lineheight + 13);
                                    pt14 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width)-30, lineheight + 13);
                                    graph.DrawLine(lineRed1, pt13, pt14);
                                    cnt = cnt + 1;
                                }
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("Fee Lesen: RM", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt13 = new System.Drawing.Point(97, lineheight + 13);
                            pt14 = new System.Drawing.Point(250, lineheight + 13);
                            graph.DrawLine(lineRed1, pt13, pt14);
                            if (item.TotalFee != null)
                            {
                                graph.DrawString(string.Format("{0:0.00}", item.TotalFee), nfont, XBrushes.Black, new XRect(98, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 30;
                            graph.DrawString("Tarikh:", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt13 = new System.Drawing.Point(62, lineheight + 13);
                            pt14 = new System.Drawing.Point(230, lineheight + 13);
                            graph.DrawLine(lineRed1, pt13, pt14);
                            if (item.DatePaid != null)
                            {
                                graph.DrawString(string.Format("{0:dd MMMM yyyy}", item.DatePaid), nfont, XBrushes.Black, new XRect(63, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
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
            catch(Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating Letter!');</script>");
        }
        #endregion

        public class Attchments
        {
            public int RequiredDocID { get; set; }
            public int Id { get; set; }
            public string filename { get; set; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}