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

namespace TradingLicense.Web.Controllers
{
    public class EntmtApplicationController : BaseController
    {

        #region EntmtGroup

        /// <summary>
        /// GET: EntmtGroup
        /// </summary>
        /// <returns></returns>
        public ActionResult EntmtGroup()
        {
            return View();
        }

        /// <summary>
        /// Save EntmtGroup Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EntmtGroup([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string entmtGroupDesc)
        {
            List<TradingLicense.Model.EntmtGroupModel> EntmtGroup = new List<Model.EntmtGroupModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<EntmtGroup> query = ctx.EntmtGroups;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(entmtGroupDesc))
                {
                    query = query.Where(p =>
                                        p.EntmtGroupDesc.Contains(entmtGroupDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "EntmtGroupID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                EntmtGroup = Mapper.Map<List<EntmtGroupModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, EntmtGroup, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get EntmtGroup Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageEntmtGroup(int? Id)
        {
            EntmtGroupModel EntmtGroupModel = new EntmtGroupModel();
            EntmtGroupModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int EntmtGroupID = Convert.ToInt32(Id);
                    var EntmtGroup = ctx.EntmtGroups.Where(a => a.EntmtGroupID == EntmtGroupID).FirstOrDefault();
                    EntmtGroupModel = Mapper.Map<EntmtGroupModel>(EntmtGroup);
                }
            }

            return View(EntmtGroupModel);
        }

        /// <summary>
        /// Save EntmtGroup Infomration
        /// </summary>
        /// <param name="EntmtGroupModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageEntmtGroup(EntmtGroupModel EntmtGroupModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    EntmtGroup EntmtGroup;
                    if (IsEntmtGroupDuplicate(EntmtGroupModel.EntmtGroupDesc, EntmtGroupModel.EntmtGroupID))
                    {
                        TempData["ErrorMessage"] = "EntmtGroup already exists in the database.";
                        return View(EntmtGroupModel);
                    }
                    EntmtGroup = Mapper.Map<EntmtGroup>(EntmtGroupModel);
                    ctx.EntmtGroups.AddOrUpdate(EntmtGroup);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "EntmtGroup saved successfully.";

                return RedirectToAction("EntmtGroup");
            }
            else
            {
                return View(EntmtGroupModel);
            }

        }

        /// <summary>
        /// Delete EntmtGroup Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEntmtGroup(int id)
        {
            try
            {
                var EntmtGroup = new TradingLicense.Entities.EntmtGroup() { EntmtGroupID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(EntmtGroup).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsEntmtGroupDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
                ctx.EntmtGroups.FirstOrDefault(
                    c => c.EntmtGroupID != id && c.EntmtGroupDesc.ToLower() == name.ToLower())
                : ctx.EntmtGroups.FirstOrDefault(
                    c => c.EntmtGroupDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region EntmtObject

        /// <summary>
        /// GET: EntmtObject
        /// </summary>
        /// <returns></returns>
        public ActionResult EntmtObject()
        {
            return View();
        }

        /// <summary>
        /// Save EntmtObject Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EntmtObject([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string entmtObjectDesc)
        {
            List<TradingLicense.Model.EntmtObjectModel> EntmtObject = new List<Model.EntmtObjectModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<EntmtObject> query = ctx.EntmtObjects;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(entmtObjectDesc))
                {
                    query = query.Where(p =>
                                        p.EntmtObjectDesc.Contains(entmtObjectDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "EntmtObjectID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                EntmtObject = Mapper.Map<List<EntmtObjectModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, EntmtObject, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get EntmtObject Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageEntmtObject(int? Id)
        {
            EntmtObjectModel EntmtObjectModel = new EntmtObjectModel();
            EntmtObjectModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int EntmtObjectID = Convert.ToInt32(Id);
                    var EntmtObject = ctx.EntmtObjects.Where(a => a.EntmtObjectID == EntmtObjectID).FirstOrDefault();
                    EntmtObjectModel = Mapper.Map<EntmtObjectModel>(EntmtObject);
                }
            }

            return View(EntmtObjectModel);
        }

        /// <summary>
        /// Save EntmtObject Infomration
        /// </summary>
        /// <param name="EntmtObjectModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageEntmtObject(EntmtObjectModel EntmtObjectModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    EntmtObject EntmtObject;
                    if (IsEntmtObjectDuplicate(EntmtObjectModel.EntmtObjectDesc, EntmtObjectModel.EntmtObjectID))
                    {
                        TempData["ErrorMessage"] = "EntmtObject already exists in the database.";
                        return View(EntmtObjectModel);
                    }
                    EntmtObject = Mapper.Map<EntmtObject>(EntmtObjectModel);
                    ctx.EntmtObjects.AddOrUpdate(EntmtObject);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "EntmtObject saved successfully.";

                return RedirectToAction("EntmtObject");
            }
            else
            {
                return View(EntmtObjectModel);
            }

        }

        /// <summary>
        /// Delete EntmtObject Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEntmtObject(int id)
        {
            try
            {
                var EntmtObject = new TradingLicense.Entities.EntmtObject() { EntmtObjectID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(EntmtObject).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsEntmtObjectDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
                ctx.EntmtObjects.FirstOrDefault(
                    c => c.EntmtObjectID != id && c.EntmtObjectDesc.ToLower() == name.ToLower())
                : ctx.EntmtObjects.FirstOrDefault(
                    c => c.EntmtObjectDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region EntmtCode

        /// <summary>
        /// GET: EntmtCode
        /// </summary>
        /// <returns></returns>
        public ActionResult EntmtCode()
        {
            return View();
        }

        /// <summary>
        /// Save EntmtCode Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EntmtCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string entmtCodeDesc)
        {
            List<TradingLicense.Model.EntmtCodeModel> EntmtCode = new List<Model.EntmtCodeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<EntmtCode> query = ctx.EntmtCodes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(entmtCodeDesc))
                {
                    query = query.Where(p =>
                                        p.EntmtCodeDesc.Contains(entmtCodeDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "EntmtCodeID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                EntmtCode = Mapper.Map<List<EntmtCodeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, EntmtCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get EntmtCode Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageEntmtCode(int? Id)
        {
            EntmtCodeModel EntmtCodeModel = new EntmtCodeModel();
            EntmtCodeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int EntmtCodeID = Convert.ToInt32(Id);
                    var EntmtCode = ctx.EntmtCodes.Where(a => a.EntmtCodeID == EntmtCodeID).FirstOrDefault();
                    EntmtCodeModel = Mapper.Map<EntmtCodeModel>(EntmtCode);
                }
            }

            return View(EntmtCodeModel);
        }

        /// <summary>
        /// Save EntmtCode Infomration
        /// </summary>
        /// <param name="EntmtCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageEntmtCode(EntmtCodeModel EntmtCodeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    EntmtCode EntmtCode;
                    if (IsEntmtCodeDuplicate(EntmtCodeModel.EntmtCodeDesc, EntmtCodeModel.EntmtCodeID))
                    {
                        TempData["ErrorMessage"] = "EntmtCode already exists in the database.";
                        return View(EntmtCodeModel);
                    }
                    EntmtCode = Mapper.Map<EntmtCode>(EntmtCodeModel);
                    ctx.EntmtCodes.AddOrUpdate(EntmtCode);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "EntmtCode saved successfully.";

                return RedirectToAction("EntmtCode");
            }
            else
            {
                return View(EntmtCodeModel);
            }

        }

        /// <summary>
        /// Delete EntmtCode Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEntmtCode(int id)
        {
            try
            {
                var EntmtCode = new TradingLicense.Entities.EntmtCode() { EntmtCodeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(EntmtCode).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsEntmtCodeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
                ctx.EntmtCodes.FirstOrDefault(
                    c => c.EntmtCodeID != id && c.EntmtCodeDesc.ToLower() == name.ToLower())
                : ctx.EntmtCodes.FirstOrDefault(
                    c => c.EntmtCodeDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region EntmtApplication

        /// <summary>
        /// GET: EntmtApplication
        /// </summary>
        /// <returns></returns>

        public ActionResult EntmtApplication()
        {
            return View();
        }

        /// <summary>
        /// Get EntmtApplication Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EntmtApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string EntmtApplicationID, string IndividualMkNo)
        {
            List<TradingLicense.Model.EntmtApplicationModel> EntmtApplication = new List<Model.EntmtApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<EntmtApplication> query = (ProjectSession.User != null && ProjectSession.User.RoleTemplateID == (int)RollTemplate.Public) ? ctx.EntmtApplications.Where(p => p.UsersID == ProjectSession.User.UsersID) : ctx.EntmtApplications;

                if (!string.IsNullOrWhiteSpace(EntmtApplicationID))
                {
                    query = query.Where(q => q.EntmtApplicationID.ToString().Contains(EntmtApplicationID));
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

                var result = Mapper.Map<List<EntmtApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "EntmtApplicationID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                EntmtApplication = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, EntmtApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get Required Document Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        

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
        /// Get EntmtApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageEntmtApplication(int? Id)
        {
            EntmtApplicationModel entmtApplicationModel = new EntmtApplicationModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int EntmtApplicationID = Convert.ToInt32(Id);
                    var entmtApplication = ctx.EntmtApplications.Where(a => a.EntmtApplicationID == EntmtApplicationID).FirstOrDefault();
                    entmtApplicationModel = Mapper.Map<EntmtApplicationModel>(entmtApplication);

                    var paLinkBC = ctx.EALinkEC.Where(a => a.EntmtApplicationID == EntmtApplicationID).ToList();
                    if (paLinkBC != null && paLinkBC.Count > 0)
                    {
                        entmtApplicationModel.EntmtCodeids = (string.Join(",", paLinkBC.Select(x => x.EntmtCodeID.ToString()).ToArray()));

                        List<SelectedEntmtCodeModel> entmtCodesList = new List<SelectedEntmtCodeModel>();
                        foreach (var item in paLinkBC)
                        {
                            var entmtcode = ctx.EntmtCodes.Where(b => b.EntmtCodeID == item.EntmtCodeID).FirstOrDefault();
                            if (entmtcode != null && entmtcode.EntmtCodeID > 0)
                            {
                                SelectedEntmtCodeModel selectedEntmtCodeModel = new SelectedEntmtCodeModel();
                                selectedEntmtCodeModel.id = entmtcode.EntmtCodeID;
                                selectedEntmtCodeModel.text = entmtcode.EntmtCodeDesc;
                                entmtCodesList.Add(selectedEntmtCodeModel);
                            }
                        }
                        entmtApplicationModel.selectedEntmtCodeList = entmtCodesList;
                    }

                    var paLinkInd = ctx.EALinkInds.Where(a => a.EntmtApplicationID == EntmtApplicationID).ToList();
                    if (paLinkInd != null && paLinkInd.Count > 0)
                    {
                        entmtApplicationModel.Individualids = (string.Join(",", paLinkInd.Select(x => x.IndividualID.ToString()).ToArray()));

                        List<SelectedIndividualModel> individualList = new List<SelectedIndividualModel>();
                        foreach (var item in paLinkInd)
                        {
                            var Individual = ctx.Individuals.Where(b => b.IndividualID == item.IndividualID).FirstOrDefault();
                            if (Individual != null && Individual.IndividualID > 0)
                            {
                                SelectedIndividualModel selectedIndividualModel = new SelectedIndividualModel();
                                selectedIndividualModel.id = Individual.IndividualID;
                                selectedIndividualModel.text = Individual.MykadNo;
                                individualList.Add(selectedIndividualModel);
                            }
                        }
                        entmtApplicationModel.selectedIndividualList = individualList;

                    }

                    var EALinkReqDocumentList = ctx.EALinkReqDocs.Where(p => p.EntmtApplicationID == EntmtApplicationID).ToList();
                    if (EALinkReqDocumentList != null && EALinkReqDocumentList.Count > 0)
                    {
                        entmtApplicationModel.UploadRequiredDocids = (string.Join(",", EALinkReqDocumentList.Select(x => x.RequiredDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                entmtApplicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
                entmtApplicationModel.UsersID = ProjectSession.User.UsersID;
            }
            entmtApplicationModel.IsDraft = false;
            return View(entmtApplicationModel);
        }

        /// <summary>
        /// Save EntmtApplication Information
        /// </summary>
        /// <param name="entmtApplicationModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageEntmtApplication(EntmtApplicationModel entmtApplicationModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    EntmtApplication entmtApplication;
                    entmtApplication = Mapper.Map<EntmtApplication>(entmtApplicationModel);

                    int UserroleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.UserID > 0)
                    {
                        entmtApplication.UpdatedBy = ProjectSession.User.Username;

                        #region Set PAStatus Value 

                        if (ProjectSession.User.RoleTemplateID != null)
                        {
                            UserroleTemplate = ProjectSession.User.RoleTemplateID.Value;
                        }

                        if (entmtApplicationModel.IsDraft)
                        {
                            if (UserroleTemplate == (int)RollTemplate.Public || UserroleTemplate == (int)RollTemplate.DeskOfficer)
                            {
                                entmtApplication.AppStatusID = (int)PAStausenum.submittedtoclerk;
                            }
                        }
                        else
                        {
                            if (UserroleTemplate == (int)RollTemplate.Public || UserroleTemplate == (int)RollTemplate.DeskOfficer)
                            {
                                entmtApplication.AppStatusID = (int)PAStausenum.draftcreated;
                            }
                        }

                        if (UserroleTemplate == (int)RollTemplate.Clerk)
                        {
                            if (!string.IsNullOrWhiteSpace(entmtApplicationModel.EntmtCodeids))
                            {
                                var IslinkDept = false;
                                string[] ids = entmtApplicationModel.EntmtCodeids.Split(',');
                                foreach (var id in ids)
                                {
                                    int EntmtCodeID = Convert.ToInt32(id);
                                    var businesslinkDepartment = ctx.ECLinkDeps.Where(p => p.EntmtCodeID == EntmtCodeID).FirstOrDefault();
                                    if (businesslinkDepartment != null && businesslinkDepartment.EntCodLinkDepID > 0)
                                    {
                                        IslinkDept = true;
                                        break;
                                    }
                                }

                                if (IslinkDept)
                                {
                                    entmtApplication.AppStatusID = (int)PAStausenum.unitroute;
                                }
                                else
                                {
                                    entmtApplication.AppStatusID = (int)PAStausenum.supervisorcheck;
                                }
                            }
                            else
                            {
                                entmtApplication.AppStatusID = (int)PAStausenum.supervisorcheck;
                            }
                        }

                        #endregion
                    }

                    entmtApplication.DateSubmitted = DateTime.Now;

                    ctx.EntmtApplications.AddOrUpdate(entmtApplication);
                    ctx.SaveChanges();

                    int entmtApplicationID = entmtApplication.EntmtApplicationID;

                    int roleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
                    {
                        roleTemplate = ProjectSession.User.RoleTemplateID.Value;
                    }

                    if (UserroleTemplate == (int)RollTemplate.Public)
                    {
                        if (!string.IsNullOrWhiteSpace(entmtApplicationModel.UploadRequiredDocids))
                        {
                            string[] ids = entmtApplicationModel.UploadRequiredDocids.Split(',');
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
                            var dbEntryRequiredDoc = ctx.EALinkReqDocs.Where(q => q.EntmtApplicationID == entmtApplicationID).ToList();
                            if (dbEntryRequiredDoc != null && dbEntryRequiredDoc.Count > 0)
                            {
                                foreach (var item in dbEntryRequiredDoc)
                                {
                                    if (RequiredDoclist.Where(q => q.RequiredDocID == item.RequiredDocID && q.AttachmentID == item.AttachmentID).Count() == 0)
                                    {
                                        if (roleTemplate == (int)RollTemplate.Public)
                                        {
                                            ctx.EALinkReqDocs.Remove(item);
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
                                    EALinkReqDoc pALinkReqDoc = new EALinkReqDoc();
                                    pALinkReqDoc.EntmtApplicationID = entmtApplicationID;
                                    pALinkReqDoc.RequiredDocID = requiredDoc.RequiredDocID;
                                    pALinkReqDoc.AttachmentID = requiredDoc.AttachmentID;
                                    ctx.EALinkReqDocs.AddOrUpdate(pALinkReqDoc);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            if (roleTemplate == (int)RollTemplate.Public)
                            {
                                var EALinkReqDocumentList = ctx.EALinkReqDocs.Where(p => p.EntmtApplicationID == entmtApplicationID).ToList();
                                if (EALinkReqDocumentList != null && EALinkReqDocumentList.Count > 0)
                                {
                                    ctx.EALinkReqDocs.RemoveRange(EALinkReqDocumentList);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                    }
                    else if (UserroleTemplate == (int)RollTemplate.DeskOfficer)
                    {
                        if (!string.IsNullOrWhiteSpace(entmtApplicationModel.RequiredDocIds))
                        {
                            string[] ids = entmtApplicationModel.RequiredDocIds.Split(',');
                            List<int> RequiredDoclist = new List<int>();

                            foreach (string id in ids)
                            {
                                int RequiredDocID = Convert.ToInt32(id);
                                RequiredDoclist.Add(RequiredDocID);
                            }

                            List<int> existingRecord = new List<int>();
                            var dbEntryRequiredDoc = ctx.EALinkReqDocs.Where(q => q.EntmtApplicationID == entmtApplicationID).ToList();
                            if (dbEntryRequiredDoc != null && dbEntryRequiredDoc.Count > 0)
                            {
                                foreach (var item in dbEntryRequiredDoc)
                                {
                                    if (RequiredDoclist.Where(q => q == item.RequiredDocID).Count() == 0)
                                    {
                                        if (roleTemplate == (int)RollTemplate.Public || roleTemplate == (int)RollTemplate.DeskOfficer)
                                        {
                                            ctx.EALinkReqDocs.Remove(item);
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
                                    EALinkReqDoc pALinkReqDoc = new EALinkReqDoc();
                                    pALinkReqDoc.EntmtApplicationID = entmtApplicationID;
                                    pALinkReqDoc.RequiredDocID = requiredDoc;
                                    ctx.EALinkReqDocs.AddOrUpdate(pALinkReqDoc);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            if (!entmtApplicationModel.IsDraft && roleTemplate == (int)RollTemplate.Public || roleTemplate == (int)RollTemplate.DeskOfficer)
                            {
                                var EALinkReqDocumentList = ctx.EALinkReqDocs.Where(p => p.EntmtApplicationID == entmtApplicationID).ToList();
                                if (EALinkReqDocumentList != null && EALinkReqDocumentList.Count > 0)
                                {
                                    ctx.EALinkReqDocs.RemoveRange(EALinkReqDocumentList);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(entmtApplicationModel.EntmtCodeids))
                    {
                        string[] ids = entmtApplicationModel.EntmtCodeids.Split(',');
                        List<int> EntmtCodelist = new List<int>();

                        foreach (string id in ids)
                        {
                            int EntmtCodeID = Convert.ToInt32(id);
                            EntmtCodelist.Add(EntmtCodeID);
                        }

                        List<int> existingRecord = new List<int>();
                        var dbEntryPALinkBAct = ctx.EALinkEC.Where(q => q.EntmtApplicationID == entmtApplicationID).ToList();
                        if (dbEntryPALinkBAct != null && dbEntryPALinkBAct.Count > 0)
                        {
                            foreach (var item in dbEntryPALinkBAct)
                            {
                                if (EntmtCodelist.Where(q => q == item.EntmtCodeID).Count() == 0)
                                {
                                    ctx.EALinkEC.Remove(item);
                                }
                                else
                                {
                                    existingRecord.Add(item.EntmtCodeID);
                                }
                            }
                            ctx.SaveChanges();
                        }

                        foreach (var businessCode in EntmtCodelist)
                        {
                            if (existingRecord.Where(q => q == businessCode).Count() == 0)
                            {
                                EALinkEC EALinkEC = new EALinkEC();
                                EALinkEC.EntmtApplicationID = entmtApplicationID;
                                EALinkEC.EntmtCodeID = businessCode;
                                ctx.EALinkEC.Add(EALinkEC);
                                ctx.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var dbEntryPALinkBActs = ctx.EALinkEC.Where(va => va.EntmtApplicationID == entmtApplicationID).ToList();
                        if (dbEntryPALinkBActs != null && dbEntryPALinkBActs.Count > 0)
                        {
                            ctx.EALinkEC.RemoveRange(dbEntryPALinkBActs);
                            ctx.SaveChanges();
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(entmtApplicationModel.Individualids))
                    {
                        string[] ids = entmtApplicationModel.Individualids.Split(',');
                        List<int> Individualidslist = new List<int>();

                        foreach (string id in ids)
                        {
                            int IndividualID = Convert.ToInt32(id);
                            Individualidslist.Add(IndividualID);
                        }

                        List<int> existingRecord = new List<int>();
                        var dbEntryIndividual = ctx.EALinkInds.Where(q => q.EntmtApplicationID == entmtApplicationID).ToList();
                        if (dbEntryIndividual != null && dbEntryIndividual.Count > 0)
                        {
                            foreach (var item in dbEntryIndividual)
                            {
                                if (Individualidslist.Where(q => q == item.IndividualID).Count() == 0)
                                {
                                    ctx.EALinkInds.Remove(item);
                                }
                                else
                                {
                                    existingRecord.Add(item.IndividualID);
                                }
                            }
                            ctx.SaveChanges();
                        }

                        foreach (var individual in Individualidslist)
                        {
                            if (existingRecord.Where(q => q == individual).Count() == 0)
                            {
                                EALinkInd EALinkInd = new EALinkInd();
                                EALinkInd.EntmtApplicationID = entmtApplicationID;
                                EALinkInd.IndividualID = individual;
                                ctx.EALinkInds.Add(EALinkInd);
                                ctx.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var dbEntryEALinkInds = ctx.EALinkInds.Where(va => va.EntmtApplicationID == entmtApplicationID).ToList();
                        if (dbEntryEALinkInds != null && dbEntryEALinkInds.Count > 0)
                        {
                            ctx.EALinkInds.RemoveRange(dbEntryEALinkInds);
                            ctx.SaveChanges();
                        }
                    }
                }

                TempData["SuccessMessage"] = "Premise License Application saved successfully.";

                return RedirectToAction("EntmtApplication");
            }
            else
            {
                return View(entmtApplicationModel);
            }
        }

        /// <summary>
        /// Delete EntmtApplication Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEntmtApplication(int id)
        {
            try
            {
                var EntmtApplication = new TradingLicense.Entities.EntmtApplication() { EntmtApplicationID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(EntmtApplication).State = System.Data.Entity.EntityState.Deleted;
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
        public JsonResult FillEntmtCode(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var businessCode = ctx.EntmtCodes.Where(t => t.EntmtCodeDesc.ToLower().Contains(query.ToLower())).Select(x => new { id = x.EntmtCodeID, text = x.EntmtCodeDesc }).ToList();
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
                var individual = ctx.Individuals.Where(t => t.MykadNo.ToLower().Contains(query.ToLower())).Select(x => new { id = x.IndividualID, text = x.MykadNo }).ToList();
                return Json(individual, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// get Business Code Data
        /// </summary>
        /// <param name="EntmtCodeids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult entmtCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string EntmtCodeids)
        {
            List<TradingLicense.Model.EntmtCodeModel> EntmtCode = new List<Model.EntmtCodeModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {

                string[] ids = null;

                if (!string.IsNullOrWhiteSpace(EntmtCodeids))
                {
                    ids = EntmtCodeids.Split(',');
                }

                List<int> EntmtCodelist = new List<int>();

                foreach (string id in ids)
                {
                    int EntmtCodeID = Convert.ToInt32(id);
                    EntmtCodelist.Add(EntmtCodeID);
                }

                IQueryable<EntmtCode> query = ctx.EntmtCodes.Where(r => EntmtCodelist.Contains(r.EntmtCodeID));

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

                var result = Mapper.Map<List<EntmtCodeModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "EntmtCodeID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                EntmtCode = result;

            }
            return Json(new DataTablesResponse(requestModel.Draw, EntmtCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
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
                    int EntmtCodeID = Convert.ToInt32(id);
                    Individuallist.Add(EntmtCodeID);
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
        /// <param name="EntmtCodeids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AdditionalDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string EntmtCodeids, string entmtApplicationID)
        {
            List<TradingLicense.Model.BCLinkADModel> RequiredDocument = new List<Model.BCLinkADModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                string[] ids = null;

                if (!string.IsNullOrWhiteSpace(EntmtCodeids))
                {
                    ids = EntmtCodeids.Split(',');
                }

                List<int> EntmtCodelist = new List<int>();

                if (ids != null)
                {
                    foreach (string id in ids)
                    {
                        int EntmtCodeID = Convert.ToInt32(id);
                        EntmtCodelist.Add(EntmtCodeID);
                    }
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

                #endregion Sorting


                #region IsChecked

                if (!string.IsNullOrWhiteSpace(entmtApplicationID))
                {
                    int premiseAppId;
                    int.TryParse(entmtApplicationID, out premiseAppId);
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
                            var premisevalue = Request["EntmtApplicationID"];
                            var reqDocvalue = Request["reqDocid"];
                            var addDocvalue = Request["addDocid"];
                            var isReqvalue = Request["isReqDoc"];

                            int entmtApplicationID;
                            if (int.TryParse(premisevalue, out entmtApplicationID) && entmtApplicationID > 0)
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

                                    var folder = Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.PremiseAttachmentDocument + "\\" + entmtApplicationID.ToString());
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
                                            EALinkReqDoc paLinkReqDoc = new EALinkReqDoc();
                                            paLinkReqDoc = ctx.EALinkReqDocs.Where(p => p.EntmtApplicationID == entmtApplicationID && p.RequiredDocID == requiredDocID).FirstOrDefault();
                                            if (paLinkReqDoc != null)
                                            {
                                                paLinkReqDoc.AttachmentID = attachment.AttachmentID;
                                                ctx.EALinkReqDocs.AddOrUpdate(paLinkReqDoc);
                                                ctx.SaveChanges();
                                            }
                                            else
                                            {
                                                EALinkReqDoc paLinkReqDocument = new EALinkReqDoc();
                                                paLinkReqDocument.EntmtApplicationID = entmtApplicationID;
                                                paLinkReqDocument.RequiredDocID = requiredDocID;
                                                paLinkReqDocument.AttachmentID = attachment.AttachmentID;
                                                ctx.EALinkReqDocs.AddOrUpdate(paLinkReqDocument);
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
                                        //EALinkReqDoc paLinkReqDoc = new EALinkReqDoc();
                                        //paLinkReqDoc = ctx.EALinkReqDoc.Where(p => p.EntmtApplicationID == entmtApplicationID && p.RequiredDocID == requiredDocID).FirstOrDefault();
                                        //if (paLinkReqDoc != null)
                                        //{
                                        //    paLinkReqDoc.AttachmentID = attachment.AttachmentID;
                                        //    ctx.EALinkReqDoc.AddOrUpdate(paLinkReqDoc);
                                        //    ctx.SaveChanges();
                                        //}
                                        //else
                                        //{
                                        //    EALinkReqDoc paLinkReqDocument = new EALinkReqDoc();
                                        //    paLinkReqDocument.EntmtApplicationID = entmtApplicationID;
                                        //    paLinkReqDocument.RequiredDocID = requiredDocID;
                                        //    paLinkReqDocument.AttachmentID = attachment.AttachmentID;
                                        //    ctx.EALinkReqDoc.AddOrUpdate(paLinkReqDocument);
                                        //    ctx.SaveChanges();
                                        //}

                                        return Json(new { status = "1", result = new { status = "1", RequiredDocID = requiredDocID, AttachmentID = attachment.AttachmentID, AttachmentName = attachment.FileName } }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        //PALinkAddDoc paLinkAddDoc = new PALinkAddDoc();
                                        //paLinkAddDoc = ctx.PALinkAddDocs.Where(p => p.EntmtApplicationID == entmtApplicationID && p.AdditionalDocID == additionalDocID).FirstOrDefault();
                                        //if (paLinkAddDoc != null)
                                        //{
                                        //    paLinkAddDoc.AttachmentID = attachment.AttachmentID;
                                        //    ctx.PALinkAddDocs.AddOrUpdate(paLinkAddDoc);
                                        //    ctx.SaveChanges();
                                        //}
                                        //else
                                        //{
                                        //    PALinkAddDoc paLinkAddDocument = new PALinkAddDoc();
                                        //    paLinkAddDocument.EntmtApplicationID = entmtApplicationID;
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
                    var premiseID = Request["EntmtApplicationID"];
                    var comment = Request["comment"];
                    var approveRejectType = Request["approveRejectType"];  /// 1) Approve  2) Reject 

                    int EntmtApplicationID;
                    int.TryParse(premiseID, out EntmtApplicationID);

                    int UsersID = 0;
                    int UserroleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
                    {
                        UsersID = ProjectSession.User.UsersID;
                        UserroleTemplate = ProjectSession.User.RoleTemplateID.Value;
                    }

                    if (EntmtApplicationID > 0 && UsersID > 0 && UserroleTemplate > 0)
                    {
                        EAComment eaComment = new EAComment();
                        eaComment.Comment = comment;
                        eaComment.EntmtApplicationID = EntmtApplicationID;
                        eaComment.UsersID = UsersID;
                        eaComment.CommentDate = DateTime.Now;
                        ctx.EAComments.AddOrUpdate(eaComment);
                        ctx.SaveChanges();

                        if (eaComment.EACommentID > 0)
                        {
                            var entmtApplication = ctx.EntmtApplications.Where(p => p.EntmtApplicationID == EntmtApplicationID).FirstOrDefault();
                            var paLinkBC = ctx.EALinkEC.Where(t => t.EntmtApplicationID == EntmtApplicationID).ToList();

                            if (UserroleTemplate == (int)RollTemplate.Clerk)
                            {
                                if (approveRejectType == "Approve")
                                {
                                    var paLinkEntmtCode = ctx.EALinkEC.Where(t => t.EntmtApplicationID == EntmtApplicationID && t.EntmtCode != null).ToList();
                                    if (paLinkEntmtCode != null && paLinkEntmtCode.Count > 0)
                                    {
                                        if (entmtApplication != null && entmtApplication.EntmtApplicationID > 0)
                                        {
                                            entmtApplication.AppStatusID = (int)PAStausenum.unitroute;
                                            ctx.EntmtApplications.AddOrUpdate(entmtApplication);
                                            ctx.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        if (entmtApplication != null && entmtApplication.EntmtApplicationID > 0)
                                        {
                                            entmtApplication.AppStatusID = (int)PAStausenum.LetterofnotificationApproved;
                                            ctx.EntmtApplications.AddOrUpdate(entmtApplication);
                                            ctx.SaveChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    entmtApplication.AppStatusID = (int)PAStausenum.draftcreated;
                                    ctx.EntmtApplications.AddOrUpdate(entmtApplication);
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

    }
}