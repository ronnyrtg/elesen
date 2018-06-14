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

        public ActionResult Department(int Type)
        {
            ViewBag.DepartmentType = Type;
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
                
                IQueryable<Department> query = ctx.Departments.Where(d => d.Internal == unitType);
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                query = query.Where(p =>
                                        p.DepartmentCode.Contains(departmentCode) &&
                                        p.DepartmentDesc.ToString().Contains(departmentDesc)
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
            departmentModel.Active = true;
            ViewBag.DepartmentType = Type;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int DepartmentID = Convert.ToInt32(Id);
                    var department = ctx.Departments.Where(a => a.DepartmentID == DepartmentID).FirstOrDefault();
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
                    Department department;
                    if (IsDepartmentDuplicate(departmentModel.DepartmentCode, departmentModel.DepartmentID))
                    {
                        TempData["ErrorMessage"] = "Department Code is already exist in the database.";
                        return View(departmentModel);
                    }
                    department = Mapper.Map<Department>(departmentModel);
                    ctx.Departments.AddOrUpdate(department);
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
                var Department = new TradingLicense.Entities.Department() { DepartmentID = id };
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
               ctx.Departments.FirstOrDefault(
                   c => c.DepartmentID != id && c.DepartmentCode.ToLower() == name.ToLower())
               : ctx.Departments.FirstOrDefault(
                   c => c.DepartmentCode.ToLower() == name.ToLower());
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
                    AccessPage[] accessPages;
                    accessPages = Mapper.Map<AccessPage[]>(accessPageModel.RoleAccess);
                    ctx.AccessPages.AddOrUpdate(accessPages);
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
                var AccessPage = new TradingLicense.Entities.AccessPage() { AccessPageID = id };
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
               ctx.AccessPages.FirstOrDefault(
                   c => c.AccessPageID != id && c.PageDesc.ToLower() == name.ToLower())
               : ctx.AccessPages.FirstOrDefault(
                   c => c.PageDesc.ToLower() == name.ToLower());
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

                IQueryable<RequiredDoc> query = ctx.RequiredDocs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(requiredDocDesc))
                {
                    query = query.Where(p =>
                                            p.RequiredDocDesc.Contains(requiredDocDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "RequiredDocID asc" : orderByString);

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
            requiredDocModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int RequiredDocID = Convert.ToInt32(Id);
                    var RequiredDoc = ctx.RequiredDocs.Where(a => a.RequiredDocID == RequiredDocID).FirstOrDefault();
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
                    RequiredDoc requiredDoc;
                    if (IsRequiredDocDuplicate(requiredDocModel.RequiredDocDesc, requiredDocModel.RequiredDocID))
                    {
                        TempData["ErrorMessage"] = "Required Document is already exist in the database.";
                        return View(requiredDocModel);
                    }
                    requiredDoc = Mapper.Map<RequiredDoc>(requiredDocModel);
                    ctx.RequiredDocs.AddOrUpdate(requiredDoc);
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
                var RequiredDoc = new TradingLicense.Entities.RequiredDoc() { RequiredDocID = id };
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
               ctx.RequiredDocs.FirstOrDefault(
                   c => c.RequiredDocID != id && c.RequiredDocDesc.ToLower() == name.ToLower())
               : ctx.RequiredDocs.FirstOrDefault(
                   c => c.RequiredDocDesc.ToLower() == name.ToLower());
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
                IQueryable<Company> query = ctx.Companies;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(companyName) || !string.IsNullOrWhiteSpace(registrationNo))
                {
                    query = query.Where(p =>
                                        p.CompanyName.Contains(companyName) &&
                                        p.RegistrationNo.ToString().Contains(registrationNo)
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

                query = query.OrderBy(orderByString == string.Empty ? "CompanyID asc" : orderByString);

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
        [HttpPost]
        public JsonResult CompaniesByIndividual([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? individualId)
        {
            List<TradingLicense.Model.CompanyModel> Company = new List<Model.CompanyModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<Company> query = ctx.IndLinkComs.Where(i => i.IndividualID == individualId).Select(l => l.Company);
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

                query = query.OrderBy(orderByString == string.Empty ? "CompanyID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Company = Mapper.Map<List<CompanyModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Company, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        

        /// <summary>
        /// Get Company Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageCompany(int? Id)
        {
            CompanyModel companyModel = new CompanyModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int companyID = Convert.ToInt32(Id);
                    var company = ctx.Companies.Where(a => a.CompanyID == companyID).FirstOrDefault();
                    companyModel = Mapper.Map<CompanyModel>(company);
                }
            }

            return View(companyModel);
        }

        /// <summary>
        /// Save Company Infomration
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
                    Company company;
                    if (IsCompanyDuplicate(companyModel.CompanyName, companyModel.CompanyID))
                    {
                        TempData["ErrorMessage"] = "Comapany Name is already exist in the database.";
                        return View(companyModel);
                    }
                    company = Mapper.Map<Company>(companyModel);
                    ctx.Companies.AddOrUpdate(company);
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
                var Company = new TradingLicense.Entities.Company() { CompanyID = id };
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
               ctx.Companies.FirstOrDefault(
                   c => c.CompanyID != id && c.CompanyName.ToLower() == name.ToLower())
               : ctx.Companies.FirstOrDefault(
                   c => c.CompanyName.ToLower() == name.ToLower());
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
                IQueryable<Company> primaryQuery = ctx.Companies;
                if (!String.IsNullOrWhiteSpace(query))
                {
                    primaryQuery = primaryQuery.Where(c => c.CompanyName.ToLower().Contains(query.ToLower()));
                }
                var company = primaryQuery.ToList();
                var companyModel = Mapper.Map<List<TradingLicense.Model.CompanyModel>>(company);
                return Json(companyModel, JsonRequestBehavior.AllowGet);
            }
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
                // totalRecord = ctx.Attachments.Count();
                // Attachment = ctx.Attachments.OrderByDescending(a => a.AttachmentID).Skip(requestModel.Start).Take(requestModel.Length).ToList();

                IQueryable<Attachment> query = ctx.Attachments;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    query = query.Where(p =>
                                        p.FileName.Contains(fileName)
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

                query = query.OrderBy(orderByString == string.Empty ? "AttachmentID asc" : orderByString);

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
        [HttpPost]
        public JsonResult AttachmentsByIndividual([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? individualId)
        {
            List<IndLinkAttModel> Attachment = new List<IndLinkAttModel>(); 
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                // totalRecord = ctx.Attachments.Count();
                // Attachment = ctx.Attachments.OrderByDescending(a => a.AttachmentID).Skip(requestModel.Start).Take(requestModel.Length).ToList();

                IQueryable<IndLinkAtt> query = from ila in ctx.IndLinkAtts
                                               join a in ctx.Attachments
                                               on ila.AttachmentID equals a.AttachmentID
                                               where ila.IndividualID == individualId
                                               select ila;
                totalRecord = query.Count();
                
                // Paging
                query = query.OrderBy("AttachmentID asc").Skip(requestModel.Start).Take(requestModel.Length);

                Attachment = Mapper.Map<List<IndLinkAttModel>>(query.ToList());

                var hostingPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
                foreach (IndLinkAttModel item in Attachment)
                {
                    if (item.Attachment != null)
                    {
                        var physicalPath = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument), item.Attachment.FileName);
                        item.Attachment.FileNameFullPath = physicalPath.Substring(hostingPath.Length).Replace('\\', '/').Insert(0, "../");
                    }
                }

            }
            return Json(new DataTablesResponse(requestModel.Draw, Attachment, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
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
                    var attachment = ctx.Attachments.Where(a => a.AttachmentID == attachmentID).FirstOrDefault();
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
                    Attachment attachment;
                    if (Request.Files != null)
                    {
                        var file = Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (IsAttachmentDuplicate(fileName, attachmentModel.AttachmentID))
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
                            attachmentModel.FileName = fileName;

                            attachment = Mapper.Map<Attachment>(attachmentModel);
                            ctx.Attachments.AddOrUpdate(attachment);
                            ctx.SaveChanges();

                            TempData["SuccessMessage"] = "Attachment saved successfully.";

                            return RedirectToAction("Attachment");
                        }
                    }

                    if (attachmentModel.AttachmentID > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(attachmentModel.FileName))
                        {
                            attachment = Mapper.Map<Attachment>(attachmentModel);
                            ctx.Attachments.AddOrUpdate(attachment);
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
                var Attachment = new TradingLicense.Entities.Attachment() { AttachmentID = id };
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
               ctx.Attachments.FirstOrDefault(
                   c => c.AttachmentID != id && c.FileName.ToLower() == name.ToLower())
               : ctx.Attachments.FirstOrDefault(
                   c => c.FileName.ToLower() == name.ToLower());
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

                    if (IsAttachmentDuplicate(fname))
                    {
                        return Json("File Name is already exist in the database.");
                    }

                    var fileName = fname;

                    fname = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument), fname);
                    file.SaveAs(fname);


                    attachmentModel.FileName = fileName;

                    using (var ctx = new LicenseApplicationContext())
                    {
                        var attachment = Mapper.Map<Attachment>(attachmentModel);
                        ctx.Attachments.AddOrUpdate(attachment);
                        ctx.SaveChanges();
                        attachmentID = attachment.AttachmentID;
                    }

                    return Json("File Uploaded Successfully!~" + attachmentID.ToString());
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

                        if (IsAttachmentDuplicate(fname))
                        {
                            return Json("File Name '"+ AntiXssEncoder.HtmlEncode(fname, true) + "' already exists in the database.");
                        }
                    }

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

                        fname = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument), fname);
                        file.SaveAs(fname);


                        attachmentModel.FileName = fileName;

                        using (var ctx = new LicenseApplicationContext())
                        {
                            var attachment = Mapper.Map<Attachment>(attachmentModel);
                            ctx.Attachments.AddOrUpdate(attachment);
                            ctx.SaveChanges();
                            attachmentID = attachment.AttachmentID;

                            ctx.IndLinkAtts.AddOrUpdate(new Entities.IndLinkAtt()
                            {
                                IndividualID = individualId,
                                AttachmentID = attachmentID,
                                AttachmentDesc = attachmentDesc
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
            List<TradingLicense.Model.RoleTemplateModel> roleTemplate = new List<Model.RoleTemplateModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<RoleTemplate> query = ctx.RoleTemplates;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(roleTemplateDesc))
                {
                    query = query.Where(p =>
                                       p.RoleTemplateDesc.Contains(roleTemplateDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "RoleTemplateID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                roleTemplate = Mapper.Map<List<RoleTemplateModel>>(query.ToList());

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
            RoleTemplateModel roleTemplateModel = new RoleTemplateModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int roleTemplateID = Convert.ToInt32(Id);
                    var roleTemplate = ctx.RoleTemplates.Where(a => a.RoleTemplateID == roleTemplateID).FirstOrDefault();
                    roleTemplateModel = Mapper.Map<RoleTemplateModel>(roleTemplate);
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
        public ActionResult ManageRoleTemplate(RoleTemplateModel roleTemplateModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    RoleTemplate roleTemplate;
                    if (IsRoleTemplateDuplicate(roleTemplateModel.RoleTemplateDesc, roleTemplateModel.RoleTemplateID))
                    {
                        TempData["ErrorMessage"] = "Role Template is already exist in the database.";
                        return View(roleTemplateModel);
                    }
                    roleTemplate = Mapper.Map<RoleTemplate>(roleTemplateModel);
                    ctx.RoleTemplates.AddOrUpdate(roleTemplate);
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
                var roleTemplate = new TradingLicense.Entities.RoleTemplate() { RoleTemplateID = id };
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
               ctx.RoleTemplates.FirstOrDefault(
                   c => c.RoleTemplateID != id && c.RoleTemplateDesc.ToLower() == name.ToLower())
               : ctx.RoleTemplates.FirstOrDefault(
                   c => c.RoleTemplateDesc.ToLower() == name.ToLower());
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
                IQueryable<AppStatus> query = ctx.AppStatus;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(statusDesc))
                {
                    query = query.Where(p =>
                                        p.StatusDesc.Contains(statusDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "AppStatusID asc" : orderByString);

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
                    var pAStatus = ctx.AppStatus.Where(a => a.AppStatusID == pAStatusID).FirstOrDefault();
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
                    AppStatus pAStatus;
                    if (IsAppStatusDuplicate(appStatusModel.StatusDesc, appStatusModel.AppStatusID))
                    {
                        TempData["ErrorMessage"] = "AppStatus is already exist in the database.";
                        return View(appStatusModel);
                    }
                    pAStatus = Mapper.Map<AppStatus>(appStatusModel);
                    ctx.AppStatus.AddOrUpdate(pAStatus);
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
                var AppStatus = new TradingLicense.Entities.AppStatus() { AppStatusID = id };
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
               ctx.AppStatus.FirstOrDefault(
                   c => c.AppStatusID != id && c.StatusDesc.ToLower() == name.ToLower())
               : ctx.AppStatus.FirstOrDefault(
                   c => c.StatusDesc.ToLower() == name.ToLower());
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
                IQueryable<PremiseType> query = ctx.PremiseTypes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(premiseDesc))
                {
                    query = query.Where(p =>
                                        p.PremiseDesc.Contains(premiseDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "PremiseTypeID asc" : orderByString);

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
            premiseTypeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int premiseTypeID = Convert.ToInt32(Id);
                    var premiseType = ctx.PremiseTypes.Where(a => a.PremiseTypeID == premiseTypeID).FirstOrDefault();
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
                    PremiseType premiseType;
                    if (IsPremiseTypeDuplicate(premiseTypeModel.PremiseDesc, premiseTypeModel.PremiseTypeID))
                    {
                        TempData["ErrorMessage"] = "Premise Type is already exist in the database.";
                        return View(premiseTypeModel);
                    }

                    premiseType = Mapper.Map<PremiseType>(premiseTypeModel);
                    ctx.PremiseTypes.AddOrUpdate(premiseType);
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
                var premiseType = new TradingLicense.Entities.PremiseType() { PremiseTypeID = id };
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
               ctx.PremiseTypes.FirstOrDefault(
                   c => c.PremiseTypeID != id && c.PremiseDesc.ToLower() == name.ToLower())
               : ctx.PremiseTypes.FirstOrDefault(
                   c => c.PremiseDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
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
                IQueryable<Individual> query = ctx.Individuals;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(fullName) || !string.IsNullOrWhiteSpace(mykadPassport) || !string.IsNullOrWhiteSpace(phoneNo))
                {
                    query = query.Where(p =>
                                    p.FullName.Contains(fullName) &&
                                    p.MykadNo.Contains(mykadPassport) &&
                                    p.PhoneNo.Contains(phoneNo)
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

                query = query.OrderBy(orderByString == string.Empty ? "IndividualID asc" : orderByString);

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
            IndividualModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int individualID = Convert.ToInt32(Id);
                    var individual = ctx.Individuals.Where(a => a.IndividualID == individualID).FirstOrDefault();
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
            IndividualModel.Active = true;

            ManageIndividualModel model = new ManageIndividualModel();

            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int individualID = Convert.ToInt32(Id);
                    var individual = ctx.Individuals.Where(a => a.IndividualID == individualID).FirstOrDefault();
                    IndividualModel = Mapper.Map<IndividualModel>(individual);
                    
                    model.CompanyIds = (string.Join(",", ctx.IndLinkComs.Where(x => x.IndividualID == Id).Select(x => x.CompanyID.ToString()).ToArray()));
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
                    Individual Individual;
                    if (IsIndividualDuplicate(model.Individual.IndividualEmail, model.Individual.IndividualID))
                    {
                        TempData["ErrorMessage"] = "Individual Email is already exist in the database.";
                        return View(model);
                    }

                    Individual = Mapper.Map<Individual>(model.Individual);
                    ctx.Individuals.AddOrUpdate(Individual);

                    if(model.Individual.IndividualID == 0)
                    {
                        ctx.SaveChanges();
                        individualId = Individual.IndividualID;
                    }
                    else
                    {
                        individualId = model.Individual.IndividualID;
                        var oldLinkedCompanies = ctx.IndLinkComs.Where(i => i.IndividualID == model.Individual.IndividualID).ToList();
                        ctx.IndLinkComs.RemoveRange(oldLinkedCompanies);
                    }


                    List<IndLinkCom> LinkedCompanies = new List<IndLinkCom>();

                    foreach (string id in model.CompanyIds.Split(',').ToList())
                    {
                        IndLinkCom LinkedCompany = new IndLinkCom()
                        {
                            IndividualID = individualId,
                            CompanyID = int.Parse(id)
                        };

                        LinkedCompanies.Add(LinkedCompany);
                    }

                    ctx.IndLinkComs.AddRange(LinkedCompanies);

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
                var Individual = new TradingLicense.Entities.Individual() { IndividualID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    var individual = ctx.Individuals.Where(i => i.IndividualID == id).FirstOrDefault();
                    int attachmentId = 0;
                    if (individual != null)
                    {
                        attachmentId = individual.AttachmentID ?? 0;
                    }

                    ctx.Individuals.Remove(individual);
                    ctx.SaveChanges();

                    if (attachmentId != 0)
                    {
                        var Attachment = new TradingLicense.Entities.Attachment() { AttachmentID = attachmentId };
                        ctx.Entry(Attachment).State = System.Data.Entity.EntityState.Deleted;
                        ctx.SaveChanges();
                    }
                    
                }
                return Json(new { success = true, message = " Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
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
               ctx.Individuals.FirstOrDefault(
                   c => c.IndividualID != id && c.IndividualEmail.ToLower() == email.ToLower())
               : ctx.Individuals.FirstOrDefault(
                   c => c.IndividualEmail.ToLower() == email.ToLower());
                return existObj != null;
            }
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
                IQueryable<Users> query = ctx.Users.Where(u => u.Locked == 1);
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(usersName) || !string.IsNullOrWhiteSpace(fullName))
                {
                    query = query.Where(p =>
                                        p.Username.Contains(usersName) &&
                                        p.FullName.ToString().Contains(fullName)
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

                query = query.OrderBy(orderByString == string.Empty ? "UsersID asc" : orderByString);

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
                    var Users = ctx.Users.Where(u => u.UsersID == id).FirstOrDefault();
                    if (Users != null && Users.UsersID > 0)
                    {
                        Users.Locked = 0;
                        ctx.Users.AddOrUpdate(Users);
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
        public JsonResult UsersList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string usersName, string fullName)
        {
            List<TradingLicense.Model.UsersModel> Users = new List<Model.UsersModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<Users> query = ctx.Users.Where(u => u.Locked == 0);
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(usersName) || !string.IsNullOrWhiteSpace(fullName))
                {
                    query = query.Where(p =>
                                        p.Username.Contains(usersName) &&
                                        p.FullName.ToString().Contains(fullName)
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

                query = query.OrderBy(orderByString == string.Empty ? "UsersID asc" : orderByString);

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
                    var User = ctx.Users.Where(a => a.UsersID == usersID).FirstOrDefault();
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
                    Users users;
                    if (IsUserNameDuplicate(usersModel.Username, usersModel.UsersID))
                    {
                        TempData["ErrorMessage"] = "UserName is already exist in the database.";
                        return View(usersModel);
                    }

                    if (IsEmailDuplicate(usersModel.Email, usersModel.UsersID))
                    {
                        TempData["ErrorMessage"] = "Email is already exist in the database.";
                        return View(usersModel);
                    }

                    users = Mapper.Map<Users>(usersModel);
                    ctx.Users.AddOrUpdate(users);
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
                var Users = new TradingLicense.Entities.Users() { UsersID = id };
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
               ctx.Users.FirstOrDefault(
                   c => c.UsersID != id && c.Username.ToLower() == name.ToLower())
               : ctx.Users.FirstOrDefault(
                   c => c.Username.ToLower() == name.ToLower());
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
               ctx.Users.FirstOrDefault(
                   c => c.UsersID != id && c.Email.ToLower() == name.ToLower())
               : ctx.Users.FirstOrDefault(
                   c => c.Email.ToLower() == name.ToLower());
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
                IQueryable<Sector> query = ctx.Sectors;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(sectorDesc))
                {
                    query = query.Where(p =>
                                        p.SectorDesc.Contains(sectorDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "SectorID asc" : orderByString);

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
            SectorModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int SectorID = Convert.ToInt32(Id);
                    var Sector = ctx.Sectors.Where(a => a.SectorID == SectorID).FirstOrDefault();
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
                    Sector Sector;
                    if (IsSectorDuplicate(SectorModel.SectorDesc, SectorModel.SectorID))
                    {
                        TempData["ErrorMessage"] = "Sector is already exist in the database.";
                        return View(SectorModel);
                    }
                    Sector = Mapper.Map<Sector>(SectorModel);
                    ctx.Sectors.AddOrUpdate(Sector);
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
                var Sector = new TradingLicense.Entities.Sector() { SectorID = id };
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
               ctx.Sectors.FirstOrDefault(
                   c => c.SectorID != id && c.SectorDesc.ToLower() == name.ToLower())
               : ctx.Sectors.FirstOrDefault(
                   c => c.SectorDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region BusinessType

        /// <summary>
        /// GET: BusinessType
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessType()
        {
            return View();
        }

        /// <summary>
        /// Save BusinessType Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessType([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string BusinessTypeCode, string BusinessTypeDesc)
        {
            List<TradingLicense.Model.BusinessTypeModel> businessType = new List<Model.BusinessTypeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BusinessType> query = ctx.BusinessTypes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(BusinessTypeDesc))
                {
                    query = query.Where(p =>
                                        p.BusinessTypeDesc.Contains(BusinessTypeDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "BusinessTypeID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                businessType = Mapper.Map<List<BusinessTypeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, businessType, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get BusinessType Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageBusinessType(int? Id)
        {
            BusinessTypeModel businessTypeModel = new BusinessTypeModel();
            businessTypeModel.Active = true;
            using (var ctx = new LicenseApplicationContext())
            {
                if (Id != null && Id > 0)
                {
                    int businessTypeID = Convert.ToInt32(Id);
                    var businessType = ctx.BusinessTypes.Where(a => a.BusinessTypeID == businessTypeID).FirstOrDefault();
                    businessTypeModel = Mapper.Map<BusinessTypeModel>(businessType);
                    businessTypeModel.RequiredDocs = ctx.PALinkReqDocs.Where(a => a.BusinessTypeID == businessTypeID).Select(a => a.RequiredDocID).ToList();
                }
                var requiredDocs = ctx.RequiredDocs;
                ViewBag.AllRequiredDocs = Mapper.Map<List<RequiredDocModel>>(requiredDocs.ToList());                
            }

            return View(businessTypeModel);
        }

        /// <summary>
        /// Save Premise Type Infomration
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
                    BusinessType businessType;
                    if (IsBusinessTypeDuplicate(businessTypeModel.BusinessTypeDesc, businessTypeModel.BusinessTypeID))
                    {
                        TempData["ErrorMessage"] = "Business Type already exists in the database.";
                        return View(businessTypeModel);
                    }
                    bool isNew = businessTypeModel.BusinessTypeID == 0;
                    businessType = Mapper.Map<BusinessType>(businessTypeModel);
                    ctx.BusinessTypes.AddOrUpdate(businessType);
                    List<BTLinkReqDoc> addReqDocs = new List<BTLinkReqDoc>();
                    if (isNew)
                    {
                        addReqDocs.AddRange(businessTypeModel
                                                .RequiredDocs
                                                .Select(rd =>
                                                            new BTLinkReqDoc
                                                            {
                                                                BusinessTypeID = businessType.BusinessTypeID,
                                                                RequiredDocID = rd
                                                            }));

                    }
                    else
                    {
                        var selectedDocs = ctx.PALinkReqDocs.Where(bt => bt.BusinessTypeID == businessType.BusinessTypeID).ToList();
                        foreach (var rd in businessTypeModel.RequiredDocs)
                        {
                            if (!selectedDocs.Any(sd => sd.RequiredDocID == rd))
                            {
                                addReqDocs.Add(new BTLinkReqDoc { BusinessTypeID = businessType.BusinessTypeID, RequiredDocID = rd });
                            }
                        }
                        foreach (var btReqDoc in selectedDocs)
                        {
                            if (!businessTypeModel.RequiredDocs.Any(rd => rd == btReqDoc.RequiredDocID))
                            {
                                ctx.Entry(btReqDoc).State = System.Data.Entity.EntityState.Deleted;
                            }
                        }
                    }
                    if (addReqDocs.Count > 0)
                    {
                        ctx.PALinkReqDocs.AddOrUpdate(addReqDocs.ToArray());
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
                var businessType = new TradingLicense.Entities.BusinessType() { BusinessTypeID = id };
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
        private bool IsBusinessTypeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.BusinessTypes.FirstOrDefault(
                   c => c.BusinessTypeID != id && c.BusinessTypeDesc.ToLower() == name.ToLower())
               : ctx.BusinessTypes.FirstOrDefault(
                   c => c.BusinessTypeDesc.ToLower() == name.ToLower());
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
                IQueryable<Holiday> query = ctx.Holidays;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(holidayDesc))
                {
                    query = query.Where(p =>
                                        p.HolidayDesc.Contains(holidayDesc)
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
                    var Holiday = ctx.Holidays.Where(a => a.HolidayID == HolidayID).FirstOrDefault();
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
                    Holiday Holiday;
                    if (IsHolidayDuplicate(HolidayModel.HolidayDesc, HolidayModel.HolidayID))
                    {
                        TempData["ErrorMessage"] = "Holiday already exists in the database.";
                        return View(HolidayModel);
                    }
                    Holiday = Mapper.Map<Holiday>(HolidayModel);
                    ctx.Holidays.AddOrUpdate(Holiday);
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
                var Holiday = new TradingLicense.Entities.Holiday() { HolidayID = id };
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
               ctx.Holidays.FirstOrDefault(
                   c => c.HolidayID != id && c.HolidayDesc.ToLower() == name.ToLower())
               : ctx.Holidays.FirstOrDefault(
                   c => c.HolidayDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region AdditionalDoc

        /// <summary>
        /// GET: AdditionalDoc
        /// </summary>
        /// <returns></returns>
        public ActionResult AdditionalDoc()
        {
            return View();
        }

        /// <summary>
        /// Save AdditionalDoc Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AdditionalDoc([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string docDesc)
        {
            List<TradingLicense.Model.AdditionalDocModel> AdditionalDoc = new List<Model.AdditionalDocModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {

                IQueryable<AdditionalDoc> query = ctx.AdditionalDocs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(docDesc))
                {
                    query = query.Where(p =>
                                            p.DocDesc.Contains(docDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "AdditionalDocID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                AdditionalDoc = Mapper.Map<List<AdditionalDocModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, AdditionalDoc, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get AdditionalDoc Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageAdditionalDoc(int? Id)
        {
            AdditionalDocModel additionalDocModel = new AdditionalDocModel();
            additionalDocModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int AdditionalDocID = Convert.ToInt32(Id);
                    var AdditionalDoc = ctx.AdditionalDocs.Where(a => a.AdditionalDocID == AdditionalDocID).FirstOrDefault();
                    additionalDocModel = Mapper.Map<AdditionalDocModel>(AdditionalDoc);
                }
            }

            return View(additionalDocModel);
        }

        /// <summary>
        /// Save AdditionalDoc Information
        /// </summary>
        /// <param name="additionalDocModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageAdditionalDoc(AdditionalDocModel additionalDocModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    AdditionalDoc additionalDoc;
                    if (IsAdditionalDocDuplicate(additionalDocModel.DocDesc, additionalDocModel.AdditionalDocID))
                    {
                        TempData["ErrorMessage"] = "Required Document is already exist in the database.";
                        return View(additionalDocModel);
                    }
                    additionalDoc = Mapper.Map<AdditionalDoc>(additionalDocModel);
                    ctx.AdditionalDocs.AddOrUpdate(additionalDoc);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Required Document saved successfully.";

                return RedirectToAction("AdditionalDoc");
            }
            else
            {
                return View(additionalDocModel);
            }

        }

        /// <summary>
        /// Delete AdditionalDoc Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteAdditionalDoc(int id)
        {
            try
            {
                var AdditionalDoc = new TradingLicense.Entities.AdditionalDoc() { AdditionalDocID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(AdditionalDoc).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsAdditionalDocDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.AdditionalDocs.FirstOrDefault(
                   c => c.AdditionalDocID != id && c.DocDesc.ToLower() == name.ToLower())
               : ctx.AdditionalDocs.FirstOrDefault(
                   c => c.DocDesc.ToLower() == name.ToLower());
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
                IQueryable<Zone> query = ctx.Zones;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(zoneDesc))
                {
                    query = query.Where(p =>
                                        p.ZoneDesc.Contains(zoneDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "ZoneID asc" : orderByString);

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
            ZoneModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int ZoneID = Convert.ToInt32(Id);
                    var Zone = ctx.Zones.Where(a => a.ZoneID == ZoneID).FirstOrDefault();
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
                    Zone Zone;
                    if (IsZoneDuplicate(ZoneModel.ZoneDesc, ZoneModel.ZoneID))
                    {
                        TempData["ErrorMessage"] = "Zone is already exist in the database.";
                        return View(ZoneModel);
                    }
                    Zone = Mapper.Map<Zone>(ZoneModel);
                    ctx.Zones.AddOrUpdate(Zone);
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
                var Zone = new TradingLicense.Entities.Zone() { ZoneID = id };
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
               ctx.Zones.FirstOrDefault(
                   c => c.ZoneID != id && c.ZoneDesc.ToLower() == name.ToLower())
               : ctx.Zones.FirstOrDefault(
                   c => c.ZoneDesc.ToLower() == name.ToLower());
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
                IQueryable<Location> query = ctx.Locations;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(locationDesc))
                {
                    query = query.Where(p =>
                                        p.LocationDesc.Contains(locationDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "LocationID asc" : orderByString);

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
            LocationModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int LocationID = Convert.ToInt32(Id);
                    var Location = ctx.Locations.Where(a => a.LocationID == LocationID).FirstOrDefault();
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
                    Location Location;
                    if (IsLocationDuplicate(LocationModel.LocationDesc, LocationModel.LocationID))
                    {
                        TempData["ErrorMessage"] = "Location is already exist in the database.";
                        return View(LocationModel);
                    }
                    Location = Mapper.Map<Location>(LocationModel);
                    ctx.Locations.AddOrUpdate(Location);
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
                var Location = new TradingLicense.Entities.Location() { LocationID = id };
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
               ctx.Locations.FirstOrDefault(
                   c => c.LocationID != id && c.LocationDesc.ToLower() == name.ToLower())
               : ctx.Locations.FirstOrDefault(
                   c => c.LocationDesc.ToLower() == name.ToLower());
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
                IQueryable<Road> query = ctx.Roads;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(roadDesc))
                {
                    query = query.Where(p =>
                                        p.RoadDesc.Contains(roadDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "RoadID asc" : orderByString);

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
            RoadModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int RoadID = Convert.ToInt32(Id);
                    var Road = ctx.Roads.Where(a => a.RoadID == RoadID).FirstOrDefault();
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
                    Road Road;
                    if (IsRoadDuplicate(RoadModel.RoadDesc, RoadModel.RoadID))
                    {
                        TempData["ErrorMessage"] = "Road is already exist in the database.";
                        return View(RoadModel);
                    }
                    Road = Mapper.Map<Road>(RoadModel);
                    ctx.Roads.AddOrUpdate(Road);
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
                var Road = new TradingLicense.Entities.Road() { RoadID = id };
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
               ctx.Roads.FirstOrDefault(
                   c => c.RoadID != id && c.RoadDesc.ToLower() == name.ToLower())
               : ctx.Roads.FirstOrDefault(
                   c => c.RoadDesc.ToLower() == name.ToLower());
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
                IQueryable <LoginLog> query = ctx.LoginLogs;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching
                if (!string.IsNullOrWhiteSpace(logDesc))
                {
                    query = query.Where(z => z.LogDesc.Contains(logDesc));
                }


                filteredRecord = query.Count();

                #endregion Filtering

                #region Sorting

                query = query.OrderByDescending(c => c.LogDate);
                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Login = Mapper.Map<List<LoginLogModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Login, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}