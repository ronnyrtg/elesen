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
using System.Web;
using TradingLicense.Infrastructure;
using static TradingLicense.Infrastructure.Enums;
using System.IO;

namespace TradingLicense.Web.Controllers
{
    public class LiquorApplicationController : BaseController
    {
        #region LiquorCode

        /// <summary>
        /// GET: LiquorCode
        /// </summary>
        /// <returns></returns>
        public ActionResult LiquorCode()
        {
            return View();
        }

        /// <summary>
        /// Save Liquor Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LiquorCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string liquorCodeDesc)
        {
            List<TradingLicense.Model.LiquorCodeModel> liquorCode = new List<Model.LiquorCodeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<LiquorCode> query = ctx.LiquorCodes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(liquorCodeDesc))
                {
                    query = query.Where(p =>
                                        p.LiquorCodeDesc.Contains(liquorCodeDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "LiquorCodeID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                liquorCode = Mapper.Map<List<LiquorCodeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, liquorCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get LiquorCode Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageLiquorCode(int? Id)
        {
            LiquorCodeModel liquorCodeModel = new LiquorCodeModel();
            liquorCodeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int liquorCodeID = Convert.ToInt32(Id);
                    var liquorCode = ctx.LiquorCodes.Where(a => a.LiquorCodeID == liquorCodeID).FirstOrDefault();
                    liquorCodeModel = Mapper.Map<LiquorCodeModel>(liquorCode);
                }
            }

            return View(liquorCodeModel);
        }

        /// <summary>
        /// Save Liquor Code Infomration
        /// </summary>
        /// <param name="liquorCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageLiquorCode(LiquorCodeModel liquorCodeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    LiquorCode liquorCode;
                    if (IsLiquorCodeDuplicate(liquorCodeModel.LiquorCodeDesc, liquorCodeModel.LiquorCodeID))
                    {
                        TempData["ErrorMessage"] = "Liquor Code already exists in the database.";
                        return View(liquorCodeModel);
                    }

                    liquorCode = Mapper.Map<LiquorCode>(liquorCodeModel);
                    ctx.LiquorCodes.AddOrUpdate(liquorCode);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Liquor Code saved successfully.";

                return RedirectToAction("LiquorCode");
            }
            else
            {
                return View(liquorCodeModel);
            }

        }

        /// <summary>
        /// Delete Liquor Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteLiquorCode(int id)
        {
            try
            {
                var liquorCode = new TradingLicense.Entities.LiquorCode() { LiquorCodeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(liquorCode).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsLiquorCodeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.LiquorCodes.FirstOrDefault(
                   c => c.LiquorCodeID != id && c.LiquorCodeDesc.ToLower() == name.ToLower())
               : ctx.LiquorCodes.FirstOrDefault(
                   c => c.LiquorCodeDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region LiquorApplication

        /// <summary>
        /// GET: LiquorApplication
        /// </summary>
        /// <returns></returns>

        public ActionResult LiquorApplication()
        {
            return View();
        }

        /// <summary>
        /// Get LiquorApplication Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LiquorApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string LiquorApplicationID, string IndividualMkNo)
        {
            List<TradingLicense.Model.LiquorApplicationModel> LiquorApplication = new List<Model.LiquorApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<LiquorApplication> query = (ProjectSession.User != null && ProjectSession.User.RoleTemplateID == (int)RollTemplate.Public) ? ctx.LiquorApplications.Where(p => p.UsersID == ProjectSession.User.UsersID) : ctx.LiquorApplications;

                if (!string.IsNullOrWhiteSpace(LiquorApplicationID))
                {
                    query = query.Where(q => q.LiquorApplicationID.ToString().Contains(LiquorApplicationID));
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

                var result = Mapper.Map<List<LiquorApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "LiquorApplicationID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                LiquorApplication = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, LiquorApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get Required Document Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RequiredDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessTypeID, string liquorApplicationID)
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

                if (!string.IsNullOrWhiteSpace(liquorApplicationID))
                {
                    int liquorAppId;
                    int.TryParse(liquorApplicationID, out liquorAppId);

                    var lalinkReq = ctx.LAReqDocs.Where(p => p.LiquorApplicationID == liquorAppId).ToList();
                    foreach (var item in RequiredDocument)
                    {
                        if (lalinkReq != null && lalinkReq.Count > 0)
                        {
                            var resultlalinkReq = lalinkReq.Where(p => p.RequiredDocID == item.RequiredDocID && p.LiquorApplicationID == liquorAppId).FirstOrDefault();
                            if (resultlalinkReq != null)
                            {
                                item.IsChecked = "checked";
                                var attechmentdetails = ctx.Attachments.Where(a => a.AttachmentID == resultlalinkReq.AttachmentID).FirstOrDefault();
                                if (attechmentdetails != null)
                                {
                                    item.AttachmentFileName = attechmentdetails.FileName;
                                    item.AttachmentId = attechmentdetails.AttachmentID;
                                    item.LiquorApplicationID = liquorAppId;
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
        /// /// <param name="liquorID"></param>
        /// <returns></returns>
        public FileResult Download(int? attechmentID, int? liquorID)
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
        /// Get LiquorApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageLiquorApplication(int? Id)
        {
            LiquorApplicationModel liquorApplicationModel = new LiquorApplicationModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int LiquorApplicationID = Convert.ToInt32(Id);
                    var liquorApplication = ctx.LiquorApplications.Where(a => a.LiquorApplicationID == LiquorApplicationID).FirstOrDefault();
                    liquorApplicationModel = Mapper.Map<LiquorApplicationModel>(liquorApplication);
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                liquorApplicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
                liquorApplicationModel.UsersID = ProjectSession.User.UsersID;
            }
            liquorApplicationModel.IsDraft = false;
            return View(liquorApplicationModel);
        }

        /// <summary>
        /// Save LiquorApplication Information
        /// </summary>
        /// <param name="liquorApplicationModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageLiquorApplication(LiquorApplicationModel liquorApplicationModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    LiquorApplication liquorApplication;
                    liquorApplication = Mapper.Map<LiquorApplication>(liquorApplicationModel);

                    int UserroleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.UserID > 0)
                    {
                        liquorApplication.UpdatedBy = ProjectSession.User.Username;

                        #region Set PAStatus Value 

                        if (ProjectSession.User.RoleTemplateID != null)
                        {
                            UserroleTemplate = ProjectSession.User.RoleTemplateID.Value;
                        }

                        if (liquorApplicationModel.IsDraft)
                        {
                            if (UserroleTemplate == (int)RollTemplate.Public || UserroleTemplate == (int)RollTemplate.DeskOfficer)
                            {
                                liquorApplication.AppStatusID = (int)PAStausenum.submittedtoclerk;
                            }
                        }
                        else
                        {
                            if (UserroleTemplate == (int)RollTemplate.Public || UserroleTemplate == (int)RollTemplate.DeskOfficer)
                            {
                                liquorApplication.AppStatusID = (int)PAStausenum.draftcreated;
                            }
                        }

                        

                        #endregion
                    }

                    liquorApplication.DateSubmitted = DateTime.Now;

                    ctx.LiquorApplications.AddOrUpdate(liquorApplication);
                    ctx.SaveChanges();

                    int liquorApplicationID = liquorApplication.LiquorApplicationID;

                    int roleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
                    {
                        roleTemplate = ProjectSession.User.RoleTemplateID.Value;
                    }

                    if (UserroleTemplate == (int)RollTemplate.Public)
                    {
                        if (!string.IsNullOrWhiteSpace(liquorApplicationModel.UploadRequiredDocids))
                        {
                            string[] ids = liquorApplicationModel.UploadRequiredDocids.Split(',');
                            List<RequiredDocList> RequiredDoclist = new List<RequiredDocList>();

                            foreach (string id in ids)
                            {
                                string[] rId = id.Split(':');

                                RequiredDocList requiredDocList = new RequiredDocList();
                                requiredDocList.RequiredDocID = Convert.ToInt32(rId[0]);
                                requiredDocList.AttachmentID = Convert.ToInt32(rId[1]);
                                RequiredDoclist.Add(requiredDocList);
                            }
                        }
                    }
                    else if (UserroleTemplate == (int)RollTemplate.DeskOfficer)
                    {
                        if (!string.IsNullOrWhiteSpace(liquorApplicationModel.RequiredDocIds))
                        {
                            string[] ids = liquorApplicationModel.RequiredDocIds.Split(',');
                            List<int> RequiredDoclist = new List<int>();

                            foreach (string id in ids)
                            {
                                int RequiredDocID = Convert.ToInt32(id);
                                RequiredDoclist.Add(RequiredDocID);
                            }
                        }
                    }
                }

                TempData["SuccessMessage"] = "Liquor License Application saved successfully.";

                return RedirectToAction("LiquorApplication");
            }
            else
            {
                return View(liquorApplicationModel);
            }
        }

        /// <summary>
        /// Delete LiquorApplication Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteLiquorApplication(int id)
        {
            try
            {
                var LiquorApplication = new TradingLicense.Entities.LiquorApplication() { LiquorApplicationID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(LiquorApplication).State = System.Data.Entity.EntityState.Deleted;
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
        /// Get Liquor Code
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillLiquorCode(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var liquorCode = ctx.LiquorCodes.Where(t => t.LiquorCodeDesc.ToLower().Contains(query.ToLower())).Select(x => new { id = x.LiquorCodeID, text = x.LiquorCodeDesc }).ToList();
                return Json(liquorCode, JsonRequestBehavior.AllowGet);
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
                var individual = ctx.Individuals.Where(t => t.MykadNo.ToLower().Contains(query.ToLower())).Select(x => new { id = x.IndividualID, text = x.MykadNo }).ToList();
                return Json(individual, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// get Liquor Code Data
        /// </summary>
        /// <param name="LiquorCodeids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult liquorCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string LiquorCodeids)
        {
            List<TradingLicense.Model.LiquorCodeModel> LiquorCode = new List<Model.LiquorCodeModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {

                string[] ids = null;

                if (!string.IsNullOrWhiteSpace(LiquorCodeids))
                {
                    ids = LiquorCodeids.Split(',');
                }

                List<int> LiquorCodelist = new List<int>();

                foreach (string id in ids)
                {
                    int LiquorCodeID = Convert.ToInt32(id);
                    LiquorCodelist.Add(LiquorCodeID);
                }

                IQueryable<LiquorCode> query = ctx.LiquorCodes.Where(r => LiquorCodelist.Contains(r.LiquorCodeID));

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

                var result = Mapper.Map<List<LiquorCodeModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "LiquorCodeID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                LiquorCode = result;

            }
            return Json(new DataTablesResponse(requestModel.Draw, LiquorCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
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
                            var liquorvalue = Request["LiquorApplicationID"];
                            var reqDocvalue = Request["reqDocid"];
                            var addDocvalue = Request["addDocid"];
                            var isReqvalue = Request["isReqDoc"];

                            int liquorApplicationID;
                            if (int.TryParse(liquorvalue, out liquorApplicationID) && liquorApplicationID > 0)
                            {
                                int requiredDocID;
                                int.TryParse(reqDocvalue, out requiredDocID);

                                int additionalDocID;
                                int.TryParse(addDocvalue, out additionalDocID);
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
                                        //LALinkReqDoc laLinkReqDoc = new LALinkReqDoc();
                                        //laLinkReqDoc = ctx.LALinkReqDoc.Where(p => p.LiquorApplicationID == liquorApplicationID && p.RequiredDocID == requiredDocID).FirstOrDefault();
                                        //if (laLinkReqDoc != null)
                                        //{
                                        //    laLinkReqDoc.AttachmentID = attachment.AttachmentID;
                                        //    ctx.LALinkReqDoc.AddOrUpdate(laLinkReqDoc);
                                        //    ctx.SaveChanges();
                                        //}
                                        //else
                                        //{
                                        //    LALinkReqDoc laLinkReqDocument = new LALinkReqDoc();
                                        //    laLinkReqDocument.LiquorApplicationID = liquorApplicationID;
                                        //    laLinkReqDocument.RequiredDocID = requiredDocID;
                                        //    laLinkReqDocument.AttachmentID = attachment.AttachmentID;
                                        //    ctx.LALinkReqDoc.AddOrUpdate(laLinkReqDocument);
                                        //    ctx.SaveChanges();
                                        //}

                                        return Json(new { status = "1", result = new { status = "1", RequiredDocID = requiredDocID, AttachmentID = attachment.AttachmentID, AttachmentName = attachment.FileName } }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        //PALinkAddDoc paLinkAddDoc = new PALinkAddDoc();
                                        //paLinkAddDoc = ctx.PALinkAddDocs.Where(p => p.LiquorApplicationID == liquorApplicationID && p.AdditionalDocID == additionalDocID).FirstOrDefault();
                                        //if (paLinkAddDoc != null)
                                        //{
                                        //    paLinkAddDoc.AttachmentID = attachment.AttachmentID;
                                        //    ctx.PALinkAddDocs.AddOrUpdate(paLinkAddDoc);
                                        //    ctx.SaveChanges();
                                        //}
                                        //else
                                        //{
                                        //    PALinkAddDoc paLinkAddDocument = new PALinkAddDoc();
                                        //    paLinkAddDocument.LiquorApplicationID = liquorApplicationID;
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
        #endregion

        #region LAReqDoc

        /// <summary>
        /// GET: LAReqDoc
        /// </summary>
        /// <returns></returns>
        public ActionResult LAReqDoc()
        {
            return View();
        }

        
        

        /// <summary>
        /// Get LAReqDoc Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageLAReqDoc(int? Id)
        {
            LAReqDocModel LAReqDocModel = new LAReqDocModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int LAReqDocID = Convert.ToInt32(Id);
                    var LAReqDoc = ctx.LAReqDocs.Where(a => a.LAReqDocID == LAReqDocID).FirstOrDefault();
                    LAReqDocModel = Mapper.Map<LAReqDocModel>(LAReqDoc);
                }
            }

            return View(LAReqDocModel);
        }

        /// <summary>
        /// Save Liquor Required Document List
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveLAReqDoc(List<SALinkReqDoc> lstBarReqDoc)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    foreach (var item in lstBarReqDoc)
                    {
                        var DocCnt = ctx.LAReqDocs.Where(x => x.RequiredDocID == item.RequiredDocID).Count();
                        if (DocCnt == 0)
                        {
                            SALinkReqDoc LAReqDoc = new SALinkReqDoc();
                            LAReqDoc.LAReqDocID = 0;
                            LAReqDoc.RequiredDocID = item.RequiredDocID;
                            ctx.LAReqDocs.AddOrUpdate(LAReqDoc);
                            ctx.SaveChanges();
                        }
                    }
                }

                TempData["SuccessMessage"] = "Pilihan dokumen wajib bagi Lesen Minuman Keras telah berjaya disimpan";
                return Json(Convert.ToString(1));
            }
            catch (Exception)
            {
                return Json(Convert.ToString(0));
            }
        }

        /// <summary>
        /// Delete Liquor Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteLAReqDoc(int id)
        {
            try
            {
                var LAReqDoc = new TradingLicense.Entities.SALinkReqDoc() { LAReqDocID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(LAReqDoc).State = System.Data.Entity.EntityState.Deleted;
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