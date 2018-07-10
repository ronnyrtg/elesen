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
using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

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
        /// Get EntmtGroup Data
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
        

        

        private Func<EntmtCode, Select2ListItem> fnSelectEntmtCode = bc => new Select2ListItem { id = bc.EntmtCodeID, text = $"{bc.EntmtCodeDesc}" };
        private Func<EntmtObject, Select2ListItem> fnSelectEntmtObject = bc => new Select2ListItem { id = bc.EntmtObjectID, text = $"{bc.EntmtObjectDesc}" };
        private Func<Individual, Select2ListItem> fnSelectIndividualFormat = ind => new Select2ListItem { id = ind.IndividualID, text = $"{ind.FullName} ({ind.MykadNo})" };

        /// <summary>
        /// Fill Individual data in Select2
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
                    //Retrieve data from EntmtApplication table where EntmtApplicationID == Id
                    int EntmtApplicationID = Convert.ToInt32(Id);
                    var entmtApplication = ctx.EntmtApplications.Where(a => a.EntmtApplicationID == EntmtApplicationID).FirstOrDefault();
                    entmtApplicationModel = Mapper.Map<EntmtApplicationModel>(entmtApplication);

                    
                    //Get list of Individuals linked to this application
                    var eaLinkInd = ctx.EALinkInds.Where(a => a.EntmtApplicationID == Id).ToList();
                    entmtApplicationModel.Individualids = string.Join(",", eaLinkInd.Select(x => x.IndividualID.ToString()).ToArray());
                    List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
                    var iids = eaLinkInd.Select(b => b.IndividualID).ToList();
                    selectedIndividualList = ctx.Individuals
                        .Where(b => iids.Contains(b.IndividualID))
                        .Select(fnSelectIndividualFormat)
                        .ToList();

                    entmtApplicationModel.selectedIndividualList = selectedIndividualList;

                    //Get list of Documents linked to this application
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
        /// Display Entmt Code based on selected Group
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="selectedMode">The selected mode.</param>
        /// <param name="selectedGroup">The selected entertainment group.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillEntmtCode(string query, int selectedMode, int selectedGroup)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<EntmtCode> primaryQuery = ctx.EntmtCodes;

                if (selectedGroup > 0)
                {
                    primaryQuery = primaryQuery.Where(bc => bc.EntmtGroupID == selectedGroup);
                }
                if (!String.IsNullOrWhiteSpace(query))
                {
                    primaryQuery = primaryQuery.Where(bc => bc.EntmtCodeDesc.ToLower().Contains(query.ToLower()));
                }
                var entmtCode = primaryQuery.Select(fnSelectEntmtCode).ToList();
                return Json(entmtCode, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Display Entmt Objects
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillEntmtObject(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<EntmtObject> primaryQuery = ctx.EntmtObjects;
                if (!String.IsNullOrWhiteSpace(query))
                {
                    primaryQuery = primaryQuery.Where(bc => bc.EntmtObjectDesc.ToLower().Contains(query.ToLower()));
                }
                var entmtObject = primaryQuery.Select(fnSelectEntmtObject).ToList();
                return Json(entmtObject, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// get Entmt Code Data
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
        /// get Entmt Object Data
        /// </summary>
        /// <param name="EntmtObjectids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult entmtObject([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string EntmtObjectids)
        {
            List<EntmtObjectModel> EntmtObject = new List<EntmtObjectModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {

                string[] ids = null;

                if (!string.IsNullOrWhiteSpace(EntmtObjectids))
                {
                    ids = EntmtObjectids.Split(',');
                }

                List<int> EntmtObjectlist = new List<int>();

                foreach (string id in ids)
                {
                    int EntmtObjectID = Convert.ToInt32(id);
                    EntmtObjectlist.Add(EntmtObjectID);
                }

                IQueryable<EntmtObject> query = ctx.EntmtObjects.Where(r => EntmtObjectlist.Contains(r.EntmtObjectID));

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

                var result = Mapper.Map<List<EntmtObjectModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "EntmtObjectID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                EntmtObject = result;

            }
            return Json(new DataTablesResponse(requestModel.Draw, EntmtObject, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// get Individual name and Mykad data
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

        #endregion

        #region Generate Permit License PDF
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
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width)/2, lineheight + 13);
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

    }
}