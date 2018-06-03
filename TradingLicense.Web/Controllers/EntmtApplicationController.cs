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

    }
}