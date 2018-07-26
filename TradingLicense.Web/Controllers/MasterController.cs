using DataTables.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security.AntiXss;
using TradingLicense.Data;
using TradingLicense.Entities;
using System.Linq.Dynamic;
using TradingLicense.Model;
using AutoMapper;
using TradingLicense.Web.Classes;
using TradingLicense.Infrastructure;
using System.Web.UI;

namespace TradingLicense.Web.Controllers
{
    public class MasterController : BaseController
    {

        #region Department

        /// <summary>
        /// GET: Department
        /// </summary>
        /// <returns></returns>

        public ActionResult Department()
        {
            return View();
        }

        /// <summary>
        /// Save Department Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Department([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string departmentCode, string departmentDesc, int departmentType)
        {
            return FetchDepartmentUnit(requestModel, departmentCode, departmentDesc, departmentType);
        }

        private JsonResult FetchDepartmentUnit(IDataTablesRequest requestModel, string departmentCode, string departmentDesc, int unitType)
        {
            List<DepartmentModel> Department = new List<Model.DepartmentModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {

                IQueryable<DEPARTMENT> query = ctx.DEPARTMENTs.Where(d => d.INTERNAL == unitType);
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                query = query.Where(p =>
                                        p.DEP_CODE.Contains(departmentCode) &&
                                        p.DEP_DESC.ToString().Contains(departmentDesc)
                                    );

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

                query = query.OrderBy(orderByString == string.Empty ? "DepartmentID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Department = Mapper.Map<List<DepartmentModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Department, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Department Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageDepartment(int Type, int? Id)
        {
            DepartmentModel departmentModel = new DepartmentModel();
            departmentModel.ACTIVE = true;
            ViewBag.DepartmentType = Type;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int DepartmentID = Convert.ToInt32(Id);
                    var department = ctx.DEPARTMENTs.Where(a => a.DEP_ID == DepartmentID).FirstOrDefault();
                    departmentModel = Mapper.Map<DepartmentModel>(department);
                }
            }

            return View(departmentModel);
        }

        /// <summary>
        /// Save Department Infomration
        /// </summary>
        /// <param name="departmentModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageDepartment(int Type, DepartmentModel departmentModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    DEPARTMENT department;
                    if (IsDepartmentDuplicate(departmentModel.DEP_CODE, departmentModel.DEP_ID))
                    {
                        TempData["ErrorMessage"] = "Department Code is already exist in the database.";
                        return View(departmentModel);
                    }
                    department = Mapper.Map<DEPARTMENT>(departmentModel);
                    ctx.DEPARTMENTs.AddOrUpdate(department);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Department saved successfully.";
                return RedirectToAction("Department", new { Type = Type });
            }
            else
            {
                ViewBag.DepartmentType = Type;
                return View(departmentModel);
            }

        }

        /// <summary>
        /// Delete Department Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteDepartment(int id)
        {
            try
            {
                var Department = new TradingLicense.Entities.DEPARTMENT() { DEP_ID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(Department).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsDepartmentDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.DEPARTMENTs.FirstOrDefault(
                   c => c.DEP_ID != id && c.DEP_CODE.ToLower() == name.ToLower())
               : ctx.DEPARTMENTs.FirstOrDefault(
                   c => c.DEP_CODE.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region AccessPage

        /// <summary>
        /// GET: AccessPage
        /// </summary>
        /// <returns></returns>
        [AuthorizationPrivilegeFilter(SystemEnum.Page.AccessPages, SystemEnum.PageRight.CrudLevel)]
        public ActionResult AccessPage()
        {
            return View();
        }

        /// <summary>
        /// Save AccessPage Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AccessPage([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int pageId)
        {
            SystemEnum.Page page = (SystemEnum.Page)pageId;
            var pageRoleAccess = TradingLicenseCommon.GetPageAccess(page);
            return Json(new DataTablesResponse(requestModel.Draw, pageRoleAccess, pageRoleAccess.Count, pageRoleAccess.Count), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get AccessPage Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AuthorizationPrivilegeFilter(SystemEnum.Page.AccessPages, SystemEnum.PageRight.CrudLevel2)]
        public ActionResult ManageAccessPage(int Id)
        {
            PageRoleAccessModel model = new PageRoleAccessModel();
            SystemEnum.Page page = (SystemEnum.Page)Id;
            model.PageID = Id;
            model.PageDesc = page.ToString();
            model.RoleAccess = TradingLicenseCommon.GetPageAccess(page);
            return View(model);
        }

        /// <summary>
        /// Save AccessPage Infomration
        /// </summary>
        /// <param name="accessPageModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageAccessPage(PageRoleAccessModel accessPageModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    ACCESSPAGE[] accessPages;
                    accessPages = Mapper.Map<ACCESSPAGE[]>(accessPageModel.RoleAccess);
                    ctx.ACCESSPAGEs.AddOrUpdate(accessPages);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Access Page saved successfully.";
                TempData["BindAccess"] = accessPageModel.PageID;
                return RedirectToAction("AccessPage");
            }
            else
            {
                return View(accessPageModel);
            }
        }

        /// <summary>
        /// Delete AccessPage Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAccessPage(int id)
        {
            try
            {
                var AccessPage = new TradingLicense.Entities.ACCESSPAGE() { ACCESSPAGEID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(AccessPage).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsAccessPageDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.ACCESSPAGEs.FirstOrDefault(
                   c => c.ACCESSPAGEID != id && c.PAGEDESC.ToLower() == name.ToLower())
               : ctx.ACCESSPAGEs.FirstOrDefault(
                   c => c.PAGEDESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region RequiredDoc

        /// <summary>
        /// GET: RequiredDoc
        /// </summary>
        /// <returns></returns>
        public ActionResult RequiredDoc()
        {
            return View();
        }

        /// <summary>
        /// Save RequiredDoc Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RequiredDoc([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string requiredDocDesc)
        {
            List<TradingLicense.Model.RequiredDocModel> RequiredDoc = new List<Model.RequiredDocModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {

                IQueryable<RD> query = ctx.RDs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(requiredDocDesc))
                {
                    query = query.Where(p =>
                                            p.RD_DESC.Contains(requiredDocDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "RD_ID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                RequiredDoc = Mapper.Map<List<RequiredDocModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, RequiredDoc, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get RequiredDoc Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageRequiredDoc(int? Id)
        {
            RequiredDocModel requiredDocModel = new RequiredDocModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int RD_ID = Convert.ToInt32(Id);
                    var RequiredDoc = ctx.RDs.Where(a => a.RD_ID == RD_ID).FirstOrDefault();
                    requiredDocModel = Mapper.Map<RequiredDocModel>(RequiredDoc);
                }
            }

            return View(requiredDocModel);
        }

        /// <summary>
        /// Save RequiredDoc Infomration
        /// </summary>
        /// <param name="requiredDocModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageRequiredDoc(RequiredDocModel requiredDocModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    RD requiredDoc;
                    if (IsRequiredDocDuplicate(requiredDocModel.RD_DESC, requiredDocModel.RD_ID))
                    {
                        TempData["ErrorMessage"] = "Required Document is already exist in the database.";
                        return View(requiredDocModel);
                    }
                    requiredDoc = Mapper.Map<RD>(requiredDocModel);
                    ctx.RDs.AddOrUpdate(requiredDoc);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Required Document saved successfully.";

                return RedirectToAction("RequiredDoc");
            }
            else
            {
                return View(requiredDocModel);
            }

        }

        /// <summary>
        /// Delete RequiredDoc Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRequiredDoc(int id)
        {
            try
            {
                var RequiredDoc = new TradingLicense.Entities.RD() { RD_ID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(RequiredDoc).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsRequiredDocDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.RDs.FirstOrDefault(
                   c => c.RD_ID != id && c.RD_DESC.ToLower() == name.ToLower())
               : ctx.RDs.FirstOrDefault(
                   c => c.RD_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region Company

        /// <summary>
        /// GET: Company
        /// </summary>
        /// <returns></returns>
        public ActionResult Company()
        {
            return View();
        }

        /// <summary>
        /// Save Company Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Company([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string companyName, string registrationNo)
        {
            List<TradingLicense.Model.CompanyModel> Company = new List<Model.CompanyModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<COMPANY> query = ctx.COMPANIES;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(companyName) || !string.IsNullOrWhiteSpace(registrationNo))
                {
                    query = query.Where(p =>
                                        p.C_NAME.Contains(companyName) &&
                                        p.REG_NO.ToString().Contains(registrationNo)
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

                query = query.OrderBy(orderByString == string.Empty ? "COMPANYID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Company = Mapper.Map<List<CompanyModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Company, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// retrieve individual's associated companies data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult CompaniesByIndividual(int? individualId)
        {
            List<TradingLicense.Model.CompanyModel> Company = new List<Model.CompanyModel>();
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<COMPANY> query = ctx.IND_L_COMs.Where(i => i.IND_ID == individualId).Select(l => l.COMPANY);

                #region Sorting
                // Sorting
                var orderByString = String.Empty;
                query = query.OrderBy(orderByString == string.Empty ? "COMPANYID asc" : orderByString);

                #endregion Sorting
                Company = Mapper.Map<List<CompanyModel>>(query.ToList());
            }
            return Json(Company, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get Company Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageCompany(int? Id)
        {
            CompanyModel companyModel = new CompanyModel();
            companyModel.SSMREGDATE = DateTime.Today;
            companyModel.SSMEXPDATE = DateTime.Today;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int companyID = Convert.ToInt32(Id);
                    var company = ctx.COMPANIES.Where(a => a.COMPANYID == companyID).FirstOrDefault();
                    companyModel = Mapper.Map<CompanyModel>(company);
                }
            }
            return View(companyModel);
        }

        /// <summary>
        /// Save Company Information
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageCompany(CompanyModel companyModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    COMPANY company;
                    if (IsCompanyDuplicate(companyModel.C_NAME, companyModel.COMPANYID))
                    {
                        TempData["ErrorMessage"] = "Comapany Name is already exist in the database.";
                        return View(companyModel);
                    }
                    company = Mapper.Map<COMPANY>(companyModel);
                    ctx.COMPANIES.AddOrUpdate(company);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Company saved successfully.";

                return RedirectToAction("Company");
            }
            else
            {
                return View(companyModel);
            }

        }

        /// <summary>
        /// Delete Company Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteCompany(int id)
        {
            try
            {
                var Company = new TradingLicense.Entities.COMPANY() { COMPANYID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(Company).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsCompanyDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.COMPANIES.FirstOrDefault(
                   c => c.COMPANYID != id && c.C_NAME.ToLower() == name.ToLower())
               : ctx.COMPANIES.FirstOrDefault(
                   c => c.C_NAME.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        /// <summary>
        /// Get Company
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillCompany(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<COMPANY> primaryQuery = ctx.COMPANIES;
                if (!String.IsNullOrWhiteSpace(query))
                {
                    primaryQuery = primaryQuery.Where(c => c.C_NAME.ToLower().Contains(query.ToLower()));
                }
                var company = primaryQuery.ToList();
                var companyModel = Mapper.Map<List<TradingLicense.Model.CompanyModel>>(company);
                return Json(companyModel, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Add Company Name & Registration Number
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCompany(int id, string appType, string Cname, string RegNo)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                COMPANY com = new COMPANY();
                com.C_NAME = Cname;
                com.REG_NO = RegNo;
                ctx.COMPANIES.Add(com);
                ctx.SaveChanges();
                TempData["SuccessMessage"] = "Syarikat berjaya ditambah.";

            }

            return Redirect(Url.Action("Manage" + appType, appType) + "?id=" + id);
        }

        #endregion

        #region Attachment

        /// <summary>
        /// GET: Attachment
        /// </summary>
        /// <returns></returns>
        public ActionResult Attachment()
        {
            return View();
        }

        /// <summary>
        /// Save Attachment Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Attachment([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string fileName)
        {
            List<TradingLicense.Model.AttachmentModel> Attachment = new List<Model.AttachmentModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                // totalRecord = ctx.ATTACHMENTs.Count();
                // Attachment = ctx.ATTACHMENTs.OrderByDescending(a => a.ATT_ID).Skip(requestModel.Start).Take(requestModel.Length).ToList();

                IQueryable<ATTACHMENT> query = ctx.ATTACHMENTs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    query = query.Where(p =>
                                        p.FILENAME.Contains(fileName)
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

                query = query.OrderBy(orderByString == string.Empty ? "ATT_ID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Attachment = Mapper.Map<List<AttachmentModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Attachment, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Attachment Data by Individual
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AttachmentsByIndividual(int? individualId)
        {
            List<IndLinkAttModel> Attachment = new List<IndLinkAttModel>();
            using (var ctx = new LicenseApplicationContext())
            {
                // totalRecord = ctx.ATTACHMENTs.Count();
                // Attachment = ctx.ATTACHMENTs.OrderByDescending(a => a.ATT_ID).Skip(requestModel.Start).Take(requestModel.Length).ToList();

                IQueryable<IND_L_ATT> query = from ila in ctx.IND_L_ATTs
                                               join a in ctx.ATTACHMENTs
                                               on ila.ATT_ID equals a.ATT_ID
                                               where ila.IND_ID == individualId
                                               select ila;
                // Paging
                query = query.OrderBy("ATT_ID asc");

                Attachment = Mapper.Map<List<IndLinkAttModel>>(query.ToList());

                var hostingPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                foreach (IndLinkAttModel item in Attachment)
                {
                    if (item.Attachment != null)
                    {
                        var physicalPath = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument + "Individual/" + (individualId ?? 0).ToString("D6")), item.Attachment.FILENAME);
                        item.Attachment.FileNameFullPath = physicalPath.Substring(hostingPath.Length).Replace('\\', '/').Insert(0, "../");
                    }
                }

            }
            return Json(Attachment, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Attachment Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageAttachment(int? Id)
        {
            AttachmentModel attachmentModel = new AttachmentModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int attachmentID = Convert.ToInt32(Id);
                    var attachment = ctx.ATTACHMENTs.Where(a => a.ATT_ID == attachmentID).FirstOrDefault();
                    attachmentModel = Mapper.Map<AttachmentModel>(attachment);
                }
            }

            return View(attachmentModel);
        }

        /// <summary>
        /// Save Attachment Infomration
        /// </summary>
        /// <param name="attachmentModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageAttachment(AttachmentModel attachmentModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    ATTACHMENT attachment;
                    if (Request.Files != null)
                    {
                        var file = Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (IsAttachmentDuplicate(fileName, attachmentModel.ATT_ID))
                            {
                                TempData["ErrorMessage"] = "File Name is already exist in the database.";
                                return View(attachmentModel);
                            }

                            var folder = Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument);
                            var path = Path.Combine(folder, fileName);
                            if (!Directory.Exists(folder))
                            {
                                Directory.CreateDirectory(folder);
                            }
                            file.SaveAs(path);
                            attachmentModel.FILENAME = fileName;

                            attachment = Mapper.Map<ATTACHMENT>(attachmentModel);
                            ctx.ATTACHMENTs.AddOrUpdate(attachment);
                            ctx.SaveChanges();

                            TempData["SuccessMessage"] = "Attachment saved successfully.";

                            return RedirectToAction("Attachment");
                        }
                    }

                    if (attachmentModel.ATT_ID > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(attachmentModel.FILENAME))
                        {
                            attachment = Mapper.Map<ATTACHMENT>(attachmentModel);
                            ctx.ATTACHMENTs.AddOrUpdate(attachment);
                            ctx.SaveChanges();
                            return RedirectToAction("Attachment");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Please select file.";
                            return View(attachmentModel);
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Please select file.";
                        return View(attachmentModel);
                    }
                }
            }
            else
            {
                return View(attachmentModel);
            }
        }

        /// <summary>
        /// Delete Attachment Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAttachment(int id)
        {
            try
            {
                var Attachment = new TradingLicense.Entities.ATTACHMENT() { ATT_ID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(Attachment).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsAttachmentDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.ATTACHMENTs.FirstOrDefault(
                   c => c.ATT_ID != id && c.FILENAME.ToLower() == name.ToLower())
               : ctx.ATTACHMENTs.FirstOrDefault(
                   c => c.FILENAME.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        /// <summary>
        /// Upload File
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFile()
        {
            AttachmentModel attachmentModel = new AttachmentModel();
            int attachmentID;

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;

                    HttpPostedFileBase file = files[0];
                    string fname;

                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    var fileName = fname;
                    var individualIdLoc = Request.Params["individualid"];

                    var individualUploadPath = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument), "Individual");
                    individualUploadPath = Path.Combine(individualUploadPath, individualIdLoc);
                    if (!Directory.Exists(individualUploadPath))
                    {
                        Directory.CreateDirectory(individualUploadPath);
                    }
                    fname = Path.Combine(individualUploadPath, fname);
                    file.SaveAs(fname);

                    attachmentModel.FILENAME = fileName;

                    using (var ctx = new LicenseApplicationContext())
                    {
                        var attachment = Mapper.Map<ATTACHMENT>(attachmentModel);
                        ctx.ATTACHMENTs.AddOrUpdate(attachment);
                        ctx.SaveChanges();
                        attachmentID = attachment.ATT_ID;
                    }

                    return Json("File Uploaded Successfully!#" + attachmentID.ToString() + "#"
                        + TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument + "Individual/" + individualIdLoc + "/" + fileName);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }

        /// <summary>
        /// Upload Files By Individual
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFilesByIndividual()
        {
            AttachmentModel attachmentModel = new AttachmentModel();
            int attachmentID;

            if (Request.Files.Count > 0)
            {
                try
                {
                    string attachmentDesc = Request.Params["attachmentdesc"];
                    int individualId = int.Parse(Request.Params["individualid"]);
                    HttpFileCollectionBase files = Request.Files;

                    //for (int i = 0; i < files.Count; i++)
                    //{
                    //    HttpPostedFileBase file = files[i];
                    //    string fname;

                    //    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    //    {
                    //        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    //        fname = testfiles[testfiles.Length - 1];
                    //    }
                    //    else
                    //    {
                    //        fname = file.FileName;
                    //    }

                    //    if (IsAttachmentDuplicate(fname))
                    //    {
                    //        return Json("File Name '"+ AntiXssEncoder.HtmlEncode(fname, true) + "' already exists in the database.");
                    //    }
                    //}

                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        var fileName = fname;

                        var individualUploadPath = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument), "Individual");
                        individualUploadPath = Path.Combine(individualUploadPath, individualId.ToString("D6"));
                        if (!Directory.Exists(individualUploadPath))
                        {
                            Directory.CreateDirectory(individualUploadPath);
                        }
                        fname = Path.Combine(individualUploadPath, fname);
                        file.SaveAs(fname);

                        attachmentModel.FILENAME = fileName;

                        using (var ctx = new LicenseApplicationContext())
                        {
                            var attachment = Mapper.Map<ATTACHMENT>(attachmentModel);
                            ctx.ATTACHMENTs.AddOrUpdate(attachment);
                            ctx.SaveChanges();
                            attachmentID = attachment.ATT_ID;

                            ctx.IND_L_ATTs.AddOrUpdate(new Entities.IND_L_ATT()
                            {
                                IND_ID = individualId,
                                ATT_ID = attachmentID,
                                ATT_DESC = attachmentDesc
                            });
                            ctx.SaveChanges();
                        }
                    }
                    return Json("Files Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        #endregion

        #region Role Template

        /// <summary>
        /// GET: Role Template
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleTemplate()
        {
            return View();
        }

        /// <summary>
        /// Save RoleTemplate Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RoleTemplate([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string roleTemplateDesc)
        {
            List<TradingLicense.Model.RoleModel> roleTemplate = new List<Model.RoleModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<ROLE> query = ctx.ROLEs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(roleTemplateDesc))
                {
                    query = query.Where(p =>
                                       p.ROLE_DESC.Contains(roleTemplateDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "ROLEID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                roleTemplate = Mapper.Map<List<RoleModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, roleTemplate, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Role Template Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageRoleTemplate(int? Id)
        {
            RoleModel roleTemplateModel = new RoleModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int roleTemplateID = Convert.ToInt32(Id);
                    var roleTemplate = ctx.ROLEs.Where(a => a.ROLEID == roleTemplateID).FirstOrDefault();
                    roleTemplateModel = Mapper.Map<RoleModel>(roleTemplate);
                }
            }

            return View(roleTemplateModel);
        }

        /// <summary>
        /// Save Role Template Infomration
        /// </summary>
        /// <param name="roleTemplateModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageRoleTemplate(RoleModel roleTemplateModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    ROLE roleTemplate;
                    if (IsRoleTemplateDuplicate(roleTemplateModel.ROLE_DESC, roleTemplateModel.ROLEID))
                    {
                        TempData["ErrorMessage"] = "Role Template is already exist in the database.";
                        return View(roleTemplateModel);
                    }
                    roleTemplate = Mapper.Map<ROLE>(roleTemplateModel);
                    ctx.ROLEs.AddOrUpdate(roleTemplate);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Role Template saved successfully.";

                return RedirectToAction("RoleTemplate");
            }
            else
            {
                return View(roleTemplateModel);
            }
        }

        /// <summary>
        /// Delete Role Template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRoleTemplate(int id)
        {
            try
            {
                var roleTemplate = new TradingLicense.Entities.ROLE() { ROLEID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(roleTemplate).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsRoleTemplateDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.ROLEs.FirstOrDefault(
                   c => c.ROLEID != id && c.ROLE_DESC.ToLower() == name.ToLower())
               : ctx.ROLEs.FirstOrDefault(
                   c => c.ROLE_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region AppStatus

        /// <summary>
        /// GET: AppStatus
        /// </summary>
        /// <returns></returns>
        public ActionResult AppStatus()
        {
            return View();
        }

        /// <summary>
        /// Save AppStatus Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AppStatus([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string statusDesc)
        {
            List<TradingLicense.Model.AppStatusModel> AppStatus = new List<Model.AppStatusModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<APPSTATUS> query = ctx.APPSTATUS;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(statusDesc))
                {
                    query = query.Where(p =>
                                        p.STATUSDESC.Contains(statusDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "APPSTATUSID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                AppStatus = Mapper.Map<List<AppStatusModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, AppStatus, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get AppStatus Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageAppStatus(int? Id)
        {
            AppStatusModel appStatusModel = new AppStatusModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int pAStatusID = Convert.ToInt32(Id);
                    var pAStatus = ctx.APPSTATUS.Where(a => a.APPSTATUSID == pAStatusID).FirstOrDefault();
                    appStatusModel = Mapper.Map<AppStatusModel>(pAStatus);
                }
            }

            return View(appStatusModel);
        }

        /// <summary>
        /// Save AppStatus Infomration
        /// </summary>
        /// <param name="appStatusModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageAppStatus(AppStatusModel appStatusModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    APPSTATUS pAStatus;
                    if (IsAppStatusDuplicate(appStatusModel.STATUSDESC, appStatusModel.APPSTATUSID))
                    {
                        TempData["ErrorMessage"] = "AppStatus is already exist in the database.";
                        return View(appStatusModel);
                    }
                    pAStatus = Mapper.Map<APPSTATUS>(appStatusModel);
                    ctx.APPSTATUS.AddOrUpdate(pAStatus);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "AppStatus saved successfully.";

                return RedirectToAction("AppStatus");
            }
            else
            {
                return View(appStatusModel);
            }

        }

        /// <summary>
        /// Delete AppStatus Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAppStatus(int id)
        {
            try
            {
                var AppStatus = new TradingLicense.Entities.APPSTATUS() { APPSTATUSID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(AppStatus).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsAppStatusDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.APPSTATUS.FirstOrDefault(
                   c => c.APPSTATUSID != id && c.STATUSDESC.ToLower() == name.ToLower())
               : ctx.APPSTATUS.FirstOrDefault(
                   c => c.STATUSDESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region PremiseType

        /// <summary>
        /// GET: PremiseType
        /// </summary>
        /// <returns></returns>
        public ActionResult PremiseType()
        {
            return View();
        }

        /// <summary>
        /// Save Promise Type Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PremiseType([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string premiseDesc)
        {
            List<TradingLicense.Model.PremiseTypeModel> premiseType = new List<Model.PremiseTypeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<PREMISETYPE> query = ctx.PREMISETYPEs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(premiseDesc))
                {
                    query = query.Where(p =>
                                        p.PT_DESC.Contains(premiseDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "PT_ID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                premiseType = Mapper.Map<List<PremiseTypeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, premiseType, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get PremiseType Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManagePremiseType(int? Id)
        {
            PremiseTypeModel premiseTypeModel = new PremiseTypeModel();
            premiseTypeModel.ACTIVE = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int premiseTypeID = Convert.ToInt32(Id);
                    var premiseType = ctx.PREMISETYPEs.Where(a => a.PT_ID == premiseTypeID).FirstOrDefault();
                    premiseTypeModel = Mapper.Map<PremiseTypeModel>(premiseType);
                }
            }

            return View(premiseTypeModel);
        }

        /// <summary>
        /// Save Premise Type Infomration
        /// </summary>
        /// <param name="premiseTypeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManagePremiseType(PremiseTypeModel premiseTypeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    PREMISETYPE premiseType;
                    if (IsPremiseTypeDuplicate(premiseTypeModel.PT_DESC, premiseTypeModel.PT_ID))
                    {
                        TempData["ErrorMessage"] = "Premise Type is already exist in the database.";
                        return View(premiseTypeModel);
                    }

                    premiseType = Mapper.Map<PREMISETYPE>(premiseTypeModel);
                    ctx.PREMISETYPEs.AddOrUpdate(premiseType);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Premise Type saved successfully.";

                return RedirectToAction("PremiseType");
            }
            else
            {
                return View(premiseTypeModel);
            }

        }

        /// <summary>
        /// Delete Premise Type Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePremiseType(int id)
        {
            try
            {
                var premiseType = new TradingLicense.Entities.PREMISETYPE() { PT_ID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(premiseType).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsPremiseTypeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.PREMISETYPEs.FirstOrDefault(
                   c => c.PT_ID != id && c.PT_DESC.ToLower() == name.ToLower())
               : ctx.PREMISETYPEs.FirstOrDefault(
                   c => c.PT_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        /// <summary>
        /// Add New Premise Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddPremiseType(int id, string appType, string PTypeDesc)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                PREMISETYPE pre = new PREMISETYPE();
                pre.PT_DESC = PTypeDesc;
                ctx.PREMISETYPEs.Add(pre);
                ctx.SaveChanges();
                TempData["SuccessMessage"] = "Jenis Premis berjaya ditambah.";

            }

            return Redirect(Url.Action("Manage" + appType, appType) + "?id=" + id);
        }

        #endregion

        #region Individual

        /// <summary>
        /// GET: Individual
        /// </summary>
        /// <returns></returns>
        public ActionResult Individual()
        {
            return View();
        }

        /// <summary>
        /// Save Promise Type Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Individual([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string fullName, string mykadPassport, string phoneNo)
        {
            List<TradingLicense.Model.IndividualModel> Individual = new List<Model.IndividualModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<INDIVIDUAL> query = ctx.INDIVIDUALs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(fullName) || !string.IsNullOrWhiteSpace(mykadPassport) || !string.IsNullOrWhiteSpace(phoneNo))
                {
                    query = query.Where(p =>
                                    p.FULLNAME.Contains(fullName) &&
                                    p.MYKADNO.Contains(mykadPassport) &&
                                    p.PHONE.Contains(phoneNo)
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

                query = query.OrderBy(orderByString == string.Empty ? "IND_ID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Individual = Mapper.Map<List<IndividualModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Individual, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// View Individual Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ViewIndividual(int? Id)
        {
            ViewBag.IndividualId = Id;
            ViewBag.ViewName = "ViewIndividual";
            return View();
        }

        /// <summary>
        /// View Master details of an individual by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PartialViewResult MasterDetails(int? Id)
        {
            IndividualModel IndividualModel = new IndividualModel();
            ViewBag.IndividualId = Id;
            IndividualModel.ACTIVE = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int individualID = Convert.ToInt32(Id);
                    var individual = ctx.INDIVIDUALs.Where(a => a.IND_ID == individualID).FirstOrDefault();
                    IndividualModel = Mapper.Map<IndividualModel>(individual);
                }
            }

            return PartialView("_MasterDetails", IndividualModel);
        }

        /// <summary>
        /// View Trading details of an individual by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public PartialViewResult TradingDetail(int? Id)
        {
            ViewBag.IndividualId = Id;

            return PartialView("_TradingDetail");
        }

        /// <summary>
        /// Get Individual Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageIndividual(int? Id)
        {
            IndividualModel IndividualModel = new IndividualModel();
            IndividualModel.ACTIVE = true;
            ViewBag.IndividualId = Id;
            ViewBag.ViewName = "ManageIndividual";
            ManageIndividualModel model = new ManageIndividualModel();

            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int individualID = Convert.ToInt32(Id);
                    var individual = ctx.INDIVIDUALs.Where(a => a.IND_ID == individualID).FirstOrDefault();
                    IndividualModel = Mapper.Map<IndividualModel>(individual);

                    model.CompanyIds = (string.Join(",", ctx.IND_L_COMs.Where(x => x.IND_ID == Id).Select(x => x.COMPANYID.ToString()).ToArray()));
                }
                model.Individual = IndividualModel;
            }
            else
            {
                model.Individual = new IndividualModel();
            }

            return View(model);
        }

        /// <summary>
        /// Save Individual Infomration
        /// </summary>
        /// <param name="IndividualModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageIndividual(ManageIndividualModel model)
        {
            int individualId = 0;

            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    INDIVIDUAL Individual;
                    if (IsIndividualDuplicate(model.Individual.IND_EMAIL, model.Individual.IND_ID))
                    {
                        TempData["ErrorMessage"] = "Individual Email is already exist in the database.";
                        return View(model);
                    }

                    Individual = Mapper.Map<INDIVIDUAL>(model.Individual);
                    ctx.INDIVIDUALs.AddOrUpdate(Individual);

                    if (model.Individual.IND_ID == 0)
                    {
                        ctx.SaveChanges();
                        individualId = Individual.IND_ID;

                        //change Profile Pic upload location
                        var individualUploadPath = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument), "Individual");
                        var individualTempUploadPath = Path.Combine(individualUploadPath, model.TempIndividualLoc);
                        var individualActualUploadPath = Path.Combine(individualUploadPath, individualId.ToString("D6"));
                        if (Directory.Exists(individualUploadPath))
                        {
                            Directory.Move(individualTempUploadPath, individualActualUploadPath);
                        }
                    }
                    else
                    {
                        individualId = model.Individual.IND_ID;
                        var oldLinkedCompanies = ctx.IND_L_COMs.Where(i => i.IND_ID == model.Individual.IND_ID).ToList();
                        ctx.IND_L_COMs.RemoveRange(oldLinkedCompanies);
                    }


                    List<IND_L_COM> LinkedCompanies = new List<IND_L_COM>();

                    if (model.CompanyIds != null)
                    {
                        foreach (string id in model.CompanyIds.Split(',').ToList())
                        {
                            IND_L_COM LinkedCompany = new IND_L_COM()
                            {
                                IND_ID = individualId,
                                COMPANYID = int.Parse(id)
                            };

                            LinkedCompanies.Add(LinkedCompany);
                        }
                    }
                    ctx.IND_L_COMs.AddRange(LinkedCompanies);

                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Individual saved successfully.";

                return RedirectToAction("Individual");
            }
            else
            {
                return View(model);
            }

        }

        /// <summary>
        /// Delete Individual Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteIndividual(int id)
        {
            try
            {
                var Individual = new TradingLicense.Entities.INDIVIDUAL() { IND_ID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    var individual = ctx.INDIVIDUALs.Where(i => i.IND_ID == id).FirstOrDefault();
                    int attachmentId = 0;
                    if (individual != null)
                    {
                        attachmentId = individual.ATT_ID ?? 0;
                    }

                    ctx.INDIVIDUALs.Remove(individual);
                    ctx.SaveChanges();

                    if (attachmentId != 0)
                    {
                        var Attachment = new TradingLicense.Entities.ATTACHMENT() { ATT_ID = attachmentId };
                        ctx.Entry(Attachment).State = System.Data.Entity.EntityState.Deleted;
                        ctx.SaveChanges();
                    }

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
        /// <param name="email"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsIndividualDuplicate(string email, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.INDIVIDUALs.FirstOrDefault(
                   c => c.IND_ID != id && c.IND_EMAIL.ToLower() == email.ToLower())
               : ctx.INDIVIDUALs.FirstOrDefault(
                   c => c.IND_EMAIL.ToLower() == email.ToLower());
                return existObj != null;
            }
        }

        /// <summary>
        /// Add Individual Name & MyKad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddIndividual(int id, string appType, string Fname, string MYKADNO)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                INDIVIDUAL ind = new INDIVIDUAL();
                ind.FULLNAME = Fname;
                ind.MYKADNO = MYKADNO;
                ctx.INDIVIDUALs.Add(ind);
                ctx.SaveChanges();
                TempData["SuccessMessage"] = "Individu berjaya ditambah.";
                
            }

            return Redirect(Url.Action("Manage"+appType, appType) + "?id=" + id);
        }

        #endregion

        #region Unlock User

        /// <summary>
        /// GET: Users
        /// </summary>
        /// <returns></returns>
        public ActionResult Users()
        {
            return View();
        }

        /// <summary>
        /// Save Users Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Users([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string usersName, string fullName)
        {
            List<TradingLicense.Model.UsersModel> Users = new List<Model.UsersModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<USERS> query = ctx.USERS.Where(u => u.LOCKED == 1);
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(usersName) || !string.IsNullOrWhiteSpace(fullName))
                {
                    query = query.Where(p =>
                                        p.USERNAME.Contains(usersName) &&
                                        p.FULLNAME.ToString().Contains(fullName)
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

                query = query.OrderBy(orderByString == string.Empty ? "USERSID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Users = Mapper.Map<List<UsersModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Users, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// UnLock Users Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UnLockUsers(int id)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var Users = ctx.USERS.Where(u => u.USERSID == id).FirstOrDefault();
                    if (Users != null && Users.USERSID > 0)
                    {
                        Users.LOCKED = 0;
                        ctx.USERS.AddOrUpdate(Users);
                        ctx.SaveChanges();
                    }
                }
                return Json(new { success = true, message = "User Unlock Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false, message = "Error While Unlock User Record" }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Users

        /// <summary>
        /// GET: Users
        /// </summary>
        /// <returns></returns>
        public ActionResult Userslist()
        {
            return View();
        }

        /// <summary>
        /// Save Users Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Userslist([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string usersName, string fullName)
        {
            List<TradingLicense.Model.UsersModel> Users = new List<Model.UsersModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<USERS> query = ctx.USERS.Where(u => u.LOCKED == 0);
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(usersName) || !string.IsNullOrWhiteSpace(fullName))
                {
                    query = query.Where(p =>
                                        p.USERNAME.Contains(usersName) &&
                                        p.FULLNAME.ToString().Contains(fullName)
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

                query = query.OrderBy(orderByString == string.Empty ? "USERSID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Users = Mapper.Map<List<UsersModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Users, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get User Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageUsers(int? Id)
        {
            UsersModel userModel = new UsersModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int usersID = Convert.ToInt32(Id);
                    var User = ctx.USERS.Where(a => a.USERSID == usersID).FirstOrDefault();
                    userModel = Mapper.Map<UsersModel>(User);
                }
            }

            return View(userModel);
        }

        /// <summary>
        /// Save User Infomration
        /// </summary>
        /// <param name="companyModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageUsers(UsersModel usersModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    USERS users;
                    if (IsUserNameDuplicate(usersModel.USERNAME, usersModel.USERSID))
                    {
                        TempData["ErrorMessage"] = "UserName is already exist in the database.";
                        return View(usersModel);
                    }

                    if (IsEmailDuplicate(usersModel.EMAIL, usersModel.USERSID))
                    {
                        TempData["ErrorMessage"] = "Email is already exist in the database.";
                        return View(usersModel);
                    }

                    users = Mapper.Map<USERS>(usersModel);
                    ctx.USERS.AddOrUpdate(users);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "User information update successfully.";
                return RedirectToAction("Userslist");
            }
            else
            {
                return View(usersModel);
            }

        }

        /// <summary>
        /// Delete Company Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteUsers(int id)
        {
            try
            {
                var Users = new TradingLicense.Entities.USERS() { USERSID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(Users).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsUserNameDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.USERS.FirstOrDefault(
                   c => c.USERSID != id && c.USERNAME.ToLower() == name.ToLower())
               : ctx.USERS.FirstOrDefault(
                   c => c.USERNAME.ToLower() == name.ToLower());
                return existObj != null;
            }
        }


        /// <summary>
        /// Check Duplicate
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsEmailDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.USERS.FirstOrDefault(
                   c => c.USERSID != id && c.EMAIL.ToLower() == name.ToLower())
               : ctx.USERS.FirstOrDefault(
                   c => c.EMAIL.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region Sector

        /// <summary>
        /// GET: Sector
        /// </summary>
        /// <returns></returns>
        public ActionResult Sector()
        {
            return View();
        }

        /// <summary>
        /// Save Sector Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Sector([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string sectorDesc)
        {
            List<TradingLicense.Model.SectorModel> Sector = new List<Model.SectorModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<SECTOR> query = ctx.SECTORs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(sectorDesc))
                {
                    query = query.Where(p =>
                                        p.SECTORDESC.Contains(sectorDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "SECTORID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Sector = Mapper.Map<List<SectorModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Sector, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Sector Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageSector(int? Id)
        {
            SectorModel SectorModel = new SectorModel();
            SectorModel.ACTIVE = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int SECTORID = Convert.ToInt32(Id);
                    var Sector = ctx.SECTORs.Where(a => a.SECTORID == SECTORID).FirstOrDefault();
                    SectorModel = Mapper.Map<SectorModel>(Sector);
                }
            }

            return View(SectorModel);
        }

        /// <summary>
        /// Save Sector Infomration
        /// </summary>
        /// <param name="SectorModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageSector(SectorModel SectorModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    SECTOR Sector;
                    if (IsSectorDuplicate(SectorModel.SECTORDESC, SectorModel.SECTORID))
                    {
                        TempData["ErrorMessage"] = "Sector is already exist in the database.";
                        return View(SectorModel);
                    }
                    Sector = Mapper.Map<SECTOR>(SectorModel);
                    ctx.SECTORs.AddOrUpdate(Sector);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Sector saved successfully.";

                return RedirectToAction("Sector");
            }
            else
            {
                return View(SectorModel);
            }

        }

        /// <summary>
        /// Delete Sector Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSector(int id)
        {
            try
            {
                var Sector = new TradingLicense.Entities.SECTOR() { SECTORID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(Sector).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsSectorDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.SECTORs.FirstOrDefault(
                   c => c.SECTORID != id && c.SECTORDESC.ToLower() == name.ToLower())
               : ctx.SECTORs.FirstOrDefault(
                   c => c.SECTORDESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region Business Type

        /// <summary>
        /// GET: BT
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessType()
        {
            return View();
        }

        /// <summary>
        /// Save BT Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessType([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string BTCode, string BTDesc)
        {
            List<TradingLicense.Model.BusinessTypeModel> businessType = new List<Model.BusinessTypeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BT> query = ctx.BTs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(BTDesc))
                {
                    query = query.Where(p =>
                                        p.BT_DESC.Contains(BTDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "BT_ID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                businessType = Mapper.Map<List<BusinessTypeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, businessType, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get BT Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageBusinessType(int? Id)
        {
            BusinessTypeModel businessTypeModel = new BusinessTypeModel();
            businessTypeModel.ACTIVE = true;
            using (var ctx = new LicenseApplicationContext())
            {
                if (Id != null && Id > 0)
                {
                    int businessTypeID = Convert.ToInt32(Id);
                    var businessType = ctx.BTs.Where(a => a.BT_ID == businessTypeID).FirstOrDefault();
                    businessTypeModel = Mapper.Map<BusinessTypeModel>(businessType);
                    businessTypeModel.RequiredDocs = ctx.RD_L_BTs.Where(a => a.BT_ID == businessTypeID).Select(a => a.RD_ID).ToList();
                }
                var requiredDocs = ctx.RDs;
                ViewBag.AllRequiredDocs = Mapper.Map<List<RequiredDocModel>>(requiredDocs.ToList());
            }

            return View(businessTypeModel);
        }

        /// <summary>
        /// Get Business Type Infomration
        /// </summary>
        /// <param name="businessTypeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageBusinessType(BusinessTypeModel businessTypeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    BT businessType;
                    if (IsBTDuplicate(businessTypeModel.BT_DESC, businessTypeModel.BT_ID))
                    {
                        TempData["ErrorMessage"] = "Business Type already exists in the database.";
                        return View(businessTypeModel);
                    }
                    bool isNew = businessTypeModel.BT_ID == 0;
                    businessType = Mapper.Map<BT>(businessTypeModel);
                    ctx.BTs.AddOrUpdate(businessType);
                    List<RD_L_BT> addReqDocs = new List<RD_L_BT>();
                    if (isNew)
                    {
                        addReqDocs.AddRange(businessTypeModel.RequiredDocs.Select(rd => new RD_L_BT { BT_ID = businessType.BT_ID, RD_ID = rd }));
                    }
                    else
                    {
                        var selectedDocs = ctx.RD_L_BTs.Where(bt => bt.BT_ID == businessType.BT_ID).ToList();
                        foreach (var rd in businessTypeModel.RequiredDocs)
                        {
                            if (!selectedDocs.Any(sd => sd.RD_ID == rd))
                            {
                                addReqDocs.Add(new RD_L_BT { BT_ID = businessType.BT_ID, RD_ID = rd });
                            }
                        }
                        foreach (var btReqDoc in selectedDocs)
                        {
                            if (!businessTypeModel.RequiredDocs.Any(rd => rd == btReqDoc.RD_ID))
                            {
                                ctx.Entry(btReqDoc).State = System.Data.Entity.EntityState.Deleted;
                            }
                        }
                    }
                    if (addReqDocs.Count > 0)
                    {
                        ctx.RD_L_BTs.AddOrUpdate(addReqDocs.ToArray());
                    }
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Business Type saved successfully.";

                return RedirectToAction("BusinessType");
            }
            else
            {
                return View(businessTypeModel);
            }

        }

        /// <summary>
        /// Delete Business Type Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteBusinessType(int id)
        {
            try
            {
                var businessType = new TradingLicense.Entities.BT() { BT_ID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(businessType).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsBTDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.BTs.FirstOrDefault(
                   c => c.BT_ID != id && c.BT_DESC.ToLower() == name.ToLower())
               : ctx.BTs.FirstOrDefault(
                   c => c.BT_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region Holiday

        /// <summary>
        /// GET: Holiday
        /// </summary>
        /// <returns></returns>
        public ActionResult Holiday()
        {
            return View();
        }

        /// <summary>
        /// Save Holiday Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Holiday([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string holidayDesc)
        {
            List<TradingLicense.Model.HolidayModel> Holiday = new List<Model.HolidayModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<HOLIDAY> query = ctx.HOLIDAYs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(holidayDesc))
                {
                    query = query.Where(p =>
                                        p.H_DESC.Contains(holidayDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "HolidayID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Holiday = Mapper.Map<List<HolidayModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Holiday, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Holiday Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageHoliday(int? Id)
        {
            HolidayModel HolidayModel = new HolidayModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int HolidayID = Convert.ToInt32(Id);
                    var Holiday = ctx.HOLIDAYs.Where(a => a.HOLIDAYID == HolidayID).FirstOrDefault();
                    HolidayModel = Mapper.Map<HolidayModel>(Holiday);
                }
            }

            return View(HolidayModel);
        }

        /// <summary>
        /// Save Holiday Infomration
        /// </summary>
        /// <param name="HolidayModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageHoliday(HolidayModel HolidayModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    HOLIDAY Holiday;
                    if (IsHolidayDuplicate(HolidayModel.H_DESC, HolidayModel.HOLIDAYID))
                    {
                        TempData["ErrorMessage"] = "Holiday already exists in the database.";
                        return View(HolidayModel);
                    }
                    Holiday = Mapper.Map<HOLIDAY>(HolidayModel);
                    ctx.HOLIDAYs.AddOrUpdate(Holiday);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Holiday saved successfully.";

                return RedirectToAction("Holiday");
            }
            else
            {
                return View(HolidayModel);
            }

        }

        /// <summary>
        /// Delete Holiday Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteHoliday(int id)
        {
            try
            {
                var Holiday = new TradingLicense.Entities.HOLIDAY() { HOLIDAYID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(Holiday).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsHolidayDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.HOLIDAYs.FirstOrDefault(
                   c => c.HOLIDAYID != id && c.H_DESC.ToLower() == name.ToLower())
               : ctx.HOLIDAYs.FirstOrDefault(
                   c => c.H_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region Zone

        /// <summary>
        /// GET: Zone
        /// </summary>
        /// <returns></returns>
        public ActionResult Zone()
        {
            return View();
        }

        /// <summary>
        /// Save Zone Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Zone([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string zoneDesc)
        {
            List<TradingLicense.Model.ZoneModel> Zone = new List<Model.ZoneModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<ZONE_M> query = ctx.ZONEs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(zoneDesc))
                {
                    query = query.Where(p =>
                                        p.ZONE_DESC.Contains(zoneDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "ZONEID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Zone = Mapper.Map<List<ZoneModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Zone, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Zone Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageZone(int? Id)
        {
            ZoneModel ZoneModel = new ZoneModel();
            ZoneModel.ACTIVE = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int zoneID = Convert.ToInt32(Id);
                    var Zone = ctx.ZONEs.Where(a => a.ZONEID == zoneID).FirstOrDefault();
                    ZoneModel = Mapper.Map<ZoneModel>(Zone);
                }
            }

            return View(ZoneModel);
        }

        /// <summary>
        /// Save Zone Infomration
        /// </summary>
        /// <param name="ZoneModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageZone(ZoneModel ZoneModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    ZONE_M Zone;
                    if (IsZoneDuplicate(ZoneModel.ZONEDESC, ZoneModel.ZONEID))
                    {
                        TempData["ErrorMessage"] = "Zone is already exist in the database.";
                        return View(ZoneModel);
                    }
                    Zone = Mapper.Map<ZONE_M>(ZoneModel);
                    ctx.ZONEs.AddOrUpdate(Zone);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Zone saved successfully.";

                return RedirectToAction("Zone");
            }
            else
            {
                return View(ZoneModel);
            }

        }

        /// <summary>
        /// Delete Zone Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteZone(int id)
        {
            try
            {
                var Zone = new TradingLicense.Entities.ZONE_M() { ZONEID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(Zone).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsZoneDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.ZONEs.FirstOrDefault(
                   c => c.ZONEID != id && c.ZONE_DESC.ToLower() == name.ToLower())
               : ctx.ZONEs.FirstOrDefault(
                   c => c.ZONE_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region Location

        /// <summary>
        /// GET: Location
        /// </summary>
        /// <returns></returns>
        public ActionResult Location()
        {
            return View();
        }

        /// <summary>
        /// Save Location Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Location([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string locationDesc)
        {
            List<TradingLicense.Model.LocationModel> Location = new List<Model.LocationModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<LOCATION> query = ctx.LOCATIONs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(locationDesc))
                {
                    query = query.Where(p =>
                                        p.L_DESC.Contains(locationDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "LOCATIONID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Location = Mapper.Map<List<LocationModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Location, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Location Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageLocation(int? Id)
        {
            LocationModel LocationModel = new LocationModel();
            LocationModel.ACTIVE = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int LOCATIONID = Convert.ToInt32(Id);
                    var Location = ctx.LOCATIONs.Where(a => a.LOCATIONID == LOCATIONID).FirstOrDefault();
                    LocationModel = Mapper.Map<LocationModel>(Location);
                }
            }

            return View(LocationModel);
        }

        /// <summary>
        /// Save Location Infomration
        /// </summary>
        /// <param name="LocationModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageLocation(LocationModel LocationModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    LOCATION Location;
                    if (IsLocationDuplicate(LocationModel.L_DESC, LocationModel.LOCATIONID))
                    {
                        TempData["ErrorMessage"] = "Location is already exist in the database.";
                        return View(LocationModel);
                    }
                    Location = Mapper.Map<LOCATION>(LocationModel);
                    ctx.LOCATIONs.AddOrUpdate(Location);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Location saved successfully.";

                return RedirectToAction("Location");
            }
            else
            {
                return View(LocationModel);
            }

        }

        /// <summary>
        /// Delete Location Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteLocation(int id)
        {
            try
            {
                var Location = new TradingLicense.Entities.LOCATION() { LOCATIONID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(Location).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsLocationDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.LOCATIONs.FirstOrDefault(
                   c => c.LOCATIONID != id && c.L_DESC.ToLower() == name.ToLower())
               : ctx.LOCATIONs.FirstOrDefault(
                   c => c.L_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region Road

        /// <summary>
        /// GET: Road
        /// </summary>
        /// <returns></returns>
        public ActionResult Road()
        {
            return View();
        }

        /// <summary>
        /// Save Road Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Road([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string roadDesc)
        {
            List<TradingLicense.Model.RoadModel> Road = new List<Model.RoadModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<ROAD_M> query = ctx.ROADs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(roadDesc))
                {
                    query = query.Where(p =>
                                        p.ROAD_DESC.Contains(roadDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "ROADID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Road = Mapper.Map<List<RoadModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Road, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Road Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageRoad(int? Id)
        {
            RoadModel RoadModel = new RoadModel();
            RoadModel.ACTIVE = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int ROADID = Convert.ToInt32(Id);
                    var Road = ctx.ROADs.Where(a => a.ROADID == ROADID).FirstOrDefault();
                    RoadModel = Mapper.Map<RoadModel>(Road);
                }
            }

            return View(RoadModel);
        }

        /// <summary>
        /// Save Road Infomration
        /// </summary>
        /// <param name="RoadModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageRoad(RoadModel RoadModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    ROAD_M Road;
                    if (IsRoadDuplicate(RoadModel.ROADDESC, RoadModel.ROADID))
                    {
                        TempData["ErrorMessage"] = "Road is already exist in the database.";
                        return View(RoadModel);
                    }
                    Road = Mapper.Map<ROAD_M>(RoadModel);
                    ctx.ROADs.AddOrUpdate(Road);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Road saved successfully.";

                return RedirectToAction("Road");
            }
            else
            {
                return View(RoadModel);
            }

        }

        /// <summary>
        /// Delete Road Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRoad(int id)
        {
            try
            {
                var Road = new TradingLicense.Entities.ROAD_M() { ROADID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(Road).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsRoadDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.ROADs.FirstOrDefault(
                   c => c.ROADID != id && c.ROAD_DESC.ToLower() == name.ToLower())
               : ctx.ROADs.FirstOrDefault(
                   c => c.ROAD_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region loginlog
        /// <summary>
        /// GET: LoginLog
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LoginLog()
        {
            return View();
        }

        /// <summary>
        /// Request Login Log
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoginLog([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string logDesc)
        {
            List<TradingLicense.Model.LoginLogModel> Login = new List<Model.LoginLogModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<LOGINLOG> query = ctx.LOGINLOGs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(logDesc))
                {
                    query = query.Where(z => z.LOGDESC.Contains(logDesc));
                }


                filteredRecord = query.Count();

                #endregion Filtering

                #region Sorting

                query = query.OrderByDescending(c => c.LOGDATE);
                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Login = Mapper.Map<List<LoginLogModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Login, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Race

        /// <summary>
        /// GET: Race
        /// </summary>
        /// <returns></returns>
        public ActionResult Race()
        {
            return View();
        }

        /// <summary>
        /// Save Race Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Race([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string raceDesc)
        {
            List<TradingLicense.Model.RaceModel> raceType = new List<Model.RaceModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<RACE_M> query = ctx.RACEs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(raceDesc))
                {
                    query = query.Where(p =>
                                        p.RACE_DESC.Contains(raceDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "RaceID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                raceType = Mapper.Map<List<RaceModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, raceType, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Race Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageRace(int? Id)
        {
            RaceModel raceTypeModel = new RaceModel();
            raceTypeModel.ACTIVE = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int raceTypeID = Convert.ToInt32(Id);
                    var raceType = ctx.RACEs.Where(a => a.RACEID == raceTypeID).FirstOrDefault();
                    raceTypeModel = Mapper.Map<RaceModel>(raceType);
                }
            }

            return View(raceTypeModel);
        }

        /// <summary>
        /// Save Race Type Infomration
        /// </summary>
        /// <param name="raceTypeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageRace(RaceModel raceTypeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    RACE_M raceType;
                    if (IsRaceDuplicate(raceTypeModel.RACEDESC, raceTypeModel.RACEID))
                    {
                        TempData["ErrorMessage"] = "Race Type is already exist in the database.";
                        return View(raceTypeModel);
                    }

                    raceType = Mapper.Map<RACE_M>(raceTypeModel);
                    ctx.RACEs.AddOrUpdate(raceType);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Race Type saved successfully.";

                return RedirectToAction("Race");
            }
            else
            {
                return View(raceTypeModel);
            }

        }

        /// <summary>
        /// Delete Race Type Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRace(int id)
        {
            try
            {
                var raceType = new TradingLicense.Entities.RACE_M() { RACEID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(raceType).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsRaceDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.RACEs.FirstOrDefault(
                   c => c.RACEID != id && c.RACE_DESC.ToLower() == name.ToLower())
               : ctx.RACEs.FirstOrDefault(
                   c => c.RACE_DESC.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region License Type

        /// <summary>
        /// GET: LIC_TYPE
        /// </summary>
        /// <returns></returns>
        public ActionResult LicenseType()
        {
            return View();
        }

        /// <summary>
        /// Get LIC_TYPE Data for Datatable
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LicenseType([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string licTypeDesc)
        {
            List<TradingLicense.Model.LicenseTypeModel> licType = new List<Model.LicenseTypeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<LIC_TYPE> query = ctx.LIC_TYPEs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(licTypeDesc))
                {
                    query = query.Where(p =>
                                        p.LIC_TYPEDESC.Contains(licTypeDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "LIC_TYPEID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                licType = Mapper.Map<List<LicenseTypeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, licType, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get LIC_TYPE Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageLicenseType(int? Id)
        {
            LicenseTypeModel licTypeModel = new LicenseTypeModel();
            
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int licTypeID = Convert.ToInt32(Id);
                    var licType = ctx.LIC_TYPEs.Where(a => a.LIC_TYPEID == licTypeID).FirstOrDefault();
                    licTypeModel = Mapper.Map<LicenseTypeModel>(licType);
                    licTypeModel.RequiredDocs = ctx.RD_L_LTs.Where(a => a.LIC_TYPEID == licTypeID).Select(a => a.RD_ID).ToList();
                    var requiredDocs = ctx.RDs;
                    ViewBag.AllRequiredDocs = Mapper.Map<List<RequiredDocModel>>(requiredDocs.ToList());
                }
            }

            return View(licTypeModel);
        }

        /// <summary>
        /// Save LIC_TYPE Type Infomration
        /// </summary>
        /// <param name="licTypeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageLicenseType(LicenseTypeModel licTypeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    LIC_TYPE licType;
                    if (IsLicenseTypeDuplicate(licTypeModel.LIC_TYPEDESC, licTypeModel.LIC_TYPEID))
                    {
                        TempData["ErrorMessage"] = "License Type already exists in the database.";
                        return View(licTypeModel);
                    }
                    
                    licType = Mapper.Map<LIC_TYPE>(licTypeModel);
                    licType.ACTIVE = true;
                    ctx.LIC_TYPEs.AddOrUpdate(licType);

                    List<RD_L_LT> addReqDocs = new List<RD_L_LT>();
                    var selectedDocs = ctx.RD_L_LTs.Where(bt => bt.LIC_TYPEID == licType.LIC_TYPEID).ToList();
                    foreach (var rd in licTypeModel.RequiredDocs)
                    {
                        if (!selectedDocs.Any(sd => sd.RD_ID == rd))
                        {
                            addReqDocs.Add(new RD_L_LT { LIC_TYPEID = licType.LIC_TYPEID, RD_ID = rd });
                        }
                    }
                    foreach (var btReqDoc in selectedDocs)
                    {
                        if (!licTypeModel.RequiredDocs.Any(rd => rd == btReqDoc.RD_ID))
                        {
                            ctx.Entry(btReqDoc).State = System.Data.Entity.EntityState.Deleted;
                        }
                    }
                    
                    if (addReqDocs.Count > 0)
                    {
                        ctx.RD_L_LTs.AddOrUpdate(addReqDocs.ToArray());
                    }
                    
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "License Type saved successfully.";

                return RedirectToAction("LicenseType");
            }
            else
            {
                return View(licTypeModel);
            }

        }

        /// <summary>
        /// Check Duplicate
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsLicenseTypeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.LIC_TYPEs.FirstOrDefault(
                   c => c.LIC_TYPEID != id && c.LIC_TYPEDESC.ToLower() == name.ToLower())
               : ctx.LIC_TYPEs.FirstOrDefault(
                   c => c.LIC_TYPEDESC.ToLower() == name.ToLower());
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
        /// Get Entmt Entmt Premise Fee Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EntmtPremiseFee([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string premiseDesc)
        {
            List<TradingLicense.Model.EntmtPremiseFeeModel> EntmtPremiseFee = new List<Model.EntmtPremiseFeeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<E_P_FEE> query = ctx.E_P_FEEs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(premiseDesc))
                {
                    query = query.Where(p =>
                                        p.E_P_DESC.Contains(premiseDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "EP_FEEID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                EntmtPremiseFee = Mapper.Map<List<EntmtPremiseFeeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, EntmtPremiseFee, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get EntmtPremiseFee Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageEntmtPremiseFee(int? Id)
        {
            EntmtPremiseFeeModel EntmtPremiseFeeModel = new EntmtPremiseFeeModel();
            EntmtPremiseFeeModel.ACTIVE = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int premiseTypeID = Convert.ToInt32(Id);
                    var EntmtPremiseFee = ctx.E_P_FEEs.Where(a => a.E_P_FEEID == premiseTypeID).FirstOrDefault();
                    EntmtPremiseFeeModel = Mapper.Map<EntmtPremiseFeeModel>(EntmtPremiseFee);
                }
            }

            return View(EntmtPremiseFeeModel);
        }

        /// <summary>
        /// Save Entmt Premise Fee Infomration
        /// </summary>
        /// <param name="EntmtPremiseFeeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManagePremiseType(EntmtPremiseFeeModel EntmtPremiseFeeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    E_P_FEE EntmtPremiseFee;
                    if (IsEntmtPremiseTypeDuplicate(EntmtPremiseFeeModel.E_P_DESC, EntmtPremiseFeeModel.E_P_FEEID))
                    {
                        TempData["ErrorMessage"] = "Entmt Premise Fee is already exist in the database.";
                        return View(EntmtPremiseFeeModel);
                    }

                    EntmtPremiseFee = Mapper.Map<E_P_FEE>(EntmtPremiseFeeModel);
                    ctx.E_P_FEEs.AddOrUpdate(EntmtPremiseFee);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Entmt Premise Fee saved successfully.";

                return RedirectToAction("EntmtPremiseFee");
            }
            else
            {
                return View(EntmtPremiseFeeModel);
            }

        }

        /// <summary>
        /// Delete Entmt Premise Fee Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteEntmtPremiseFee(int id)
        {
            try
            {
                var EntmtPremiseFee = new TradingLicense.Entities.E_P_FEE() { E_P_FEEID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(EntmtPremiseFee).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsEntmtPremiseTypeDuplicate(string name, int? id = null)
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

        /// <summary>
        /// Add New Entmt Premise Fee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEntmtPremiseFee(int id, string PTypeDesc)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                E_P_FEE pre = new E_P_FEE();
                pre.E_P_DESC = PTypeDesc;
                ctx.E_P_FEEs.Add(pre);
                ctx.SaveChanges();
                TempData["SuccessMessage"] = "Jenis Premis Hiburan berjaya ditambah.";

            }

            return Redirect(Url.Action("ManageApplication", "Application") + "?id=" + id);
        }

        #endregion

    }
}