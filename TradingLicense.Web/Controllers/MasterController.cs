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
        public JsonResult Department([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string departmentCode, string departmentDesc)
        {
            List<TradingLicense.Model.DepartmentModel> Department = new List<Model.DepartmentModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<Department> query = ctx.Departments;
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
        public ActionResult ManageDepartment(int? Id)
        {
            DepartmentModel departmentModel = new DepartmentModel();
            departmentModel.Active = true;
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
        public ActionResult ManageDepartment(DepartmentModel departmentModel)
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

                return RedirectToAction("Department");
            }
            else
            {
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
        public JsonResult AccessPage([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string PageDesc, string CrudLevel)
        {
            List<TradingLicense.Model.AccessPageModel> AccessPage = new List<TradingLicense.Model.AccessPageModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<AccessPage> query = ctx.AccessPages;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(PageDesc) || !string.IsNullOrWhiteSpace(CrudLevel))
                {
                    query = query.Where(p =>
                                            (p.PageDesc.Contains(PageDesc)) &&
                                            (p.CrudLevel.ToString().Contains(CrudLevel))
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

                var result = Mapper.Map<List<AccessPageModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "AccessPageID asc" : orderByString).ToList();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                AccessPage = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, AccessPage, filteredRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get AccessPage Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [AuthorizationPrivilegeFilter(SystemEnum.Page.AccessPages, SystemEnum.PageRight.CrudLevel2)]
        public ActionResult ManageAccessPage(int? Id)
        {
            AccessPageModel accessPageModel = new AccessPageModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int accessPageID = Convert.ToInt32(Id);
                    var accessPage = ctx.AccessPages.Where(a => a.AccessPageID == accessPageID).FirstOrDefault();
                    accessPageModel = Mapper.Map<AccessPageModel>(accessPage);
                }
            }

            return View(accessPageModel);
        }

        /// <summary>
        /// Save AccessPage Infomration
        /// </summary>
        /// <param name="accessPageModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageAccessPage(AccessPageModel accessPageModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    AccessPage accessPage;
                    //if (IsAccessPageDuplicate(accessPageModel.PageDesc, accessPageModel.AccessPageID))
                    //{
                    //    TempData["ErrorMessage"] = "Access Page is already exist in the database.";
                    //    return View(accessPageModel);
                    //}
                    accessPage = Mapper.Map<AccessPage>(accessPageModel);
                    ctx.AccessPages.AddOrUpdate(accessPage);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Access Page saved successfully.";
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

        #region PAStatus

        /// <summary>
        /// GET: PAStatus
        /// </summary>
        /// <returns></returns>
        public ActionResult PAStatus()
        {
            return View();
        }

        /// <summary>
        /// Save PAStatus Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PAStatus([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string statusDesc)
        {
            List<TradingLicense.Model.PAStatusModel> PAStatus = new List<Model.PAStatusModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<PAStatus> query = ctx.PAStatus;
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

                query = query.OrderBy(orderByString == string.Empty ? "PAStatusID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                PAStatus = Mapper.Map<List<PAStatusModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, PAStatus, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get PAStatus Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManagePAStatus(int? Id)
        {
            PAStatusModel pAStatusModel = new PAStatusModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int pAStatusID = Convert.ToInt32(Id);
                    var pAStatus = ctx.PAStatus.Where(a => a.PAStatusID == pAStatusID).FirstOrDefault();
                    pAStatusModel = Mapper.Map<PAStatusModel>(pAStatus);
                }
            }

            return View(pAStatusModel);
        }

        /// <summary>
        /// Save PAStatus Infomration
        /// </summary>
        /// <param name="pAStatusModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManagePAStatus(PAStatusModel pAStatusModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    PAStatus pAStatus;
                    if (IsPAStatusDuplicate(pAStatusModel.StatusDesc, pAStatusModel.PAStatusID))
                    {
                        TempData["ErrorMessage"] = "PAStatus is already exist in the database.";
                        return View(pAStatusModel);
                    }
                    pAStatus = Mapper.Map<PAStatus>(pAStatusModel);
                    ctx.PAStatus.AddOrUpdate(pAStatus);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "PAStatus saved successfully.";

                return RedirectToAction("PAStatus");
            }
            else
            {
                return View(pAStatusModel);
            }

        }

        /// <summary>
        /// Delete PAStatus Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePAStatus(int id)
        {
            try
            {
                var PAStatus = new TradingLicense.Entities.PAStatus() { PAStatusID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(PAStatus).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsPAStatusDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.PAStatus.FirstOrDefault(
                   c => c.PAStatusID != id && c.StatusDesc.ToLower() == name.ToLower())
               : ctx.PAStatus.FirstOrDefault(
                   c => c.StatusDesc.ToLower() == name.ToLower());
                return existObj != null;
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

        #endregion

        #region Signboard

        /// <summary>
        /// GET: Signboard
        /// </summary>
        /// <returns></returns>
        public ActionResult Signboard()
        {
            return View();
        }

        /// <summary>
        /// Save Signboard Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Signboard([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string displayMethod, string location)
        {
            List<TradingLicense.Model.SignboardModel> Signboard = new List<Model.SignboardModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<Signboard> query = ctx.Signboards;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(displayMethod) || !string.IsNullOrWhiteSpace(location))
                {
                    query = query.Where(p =>
                                        p.DisplayMethod.Contains(displayMethod) &&
                                        p.Location.ToString().Contains(location)
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

                query = query.OrderBy(orderByString == string.Empty ? "SignboardID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                Signboard = Mapper.Map<List<SignboardModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, Signboard, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Signboard Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageSignboard(int? Id)
        {
            SignboardModel signboardModel = new SignboardModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int signboardID = Convert.ToInt32(Id);
                    var signboard = ctx.Signboards.Where(a => a.SignboardID == signboardID).FirstOrDefault();
                    signboardModel = Mapper.Map<SignboardModel>(signboard);
                }
            }

            return View(signboardModel);
        }

        /// <summary>
        /// Save Signboard Infomration
        /// </summary>
        /// <param name="signboardModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageSignboard(SignboardModel signboardModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    Signboard signboard;
                    signboard = Mapper.Map<Signboard>(signboardModel);
                    ctx.Signboards.AddOrUpdate(signboard);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Signboard saved successfully.";

                return RedirectToAction("Signboard");
            }
            else
            {
                return View(signboardModel);
            }
        }

        /// <summary>
        /// Delete Signboard Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSignboard(int id)
        {
            try
            {
                var Signboard = new TradingLicense.Entities.Signboard() { SignboardID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(Signboard).State = System.Data.Entity.EntityState.Deleted;
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
        public JsonResult Individual([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string fullName, string individualEmail, string phoneNo)
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

                if (!string.IsNullOrWhiteSpace(fullName) || !string.IsNullOrWhiteSpace(individualEmail) || !string.IsNullOrWhiteSpace(phoneNo))
                {
                    query = query.Where(p =>
                                    p.FullName.Contains(fullName) &&
                                    p.IndividualEmail.Contains(individualEmail) &&
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
        /// Get Individual Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageIndividual(int? Id)
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

            return View(IndividualModel);
        }

        /// <summary>
        /// Save Individual Infomration
        /// </summary>
        /// <param name="IndividualModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageIndividual(IndividualModel IndividualModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    Individual Individual;
                    if (IsIndividualDuplicate(IndividualModel.IndividualEmail, IndividualModel.IndividualID))
                    {
                        TempData["ErrorMessage"] = "Individual Email is already exist in the database.";
                        return View(IndividualModel);
                    }

                    Individual = Mapper.Map<Individual>(IndividualModel);
                    ctx.Individuals.AddOrUpdate(Individual);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Individual saved successfully.";

                return RedirectToAction("Individual");
            }
            else
            {
                return View(IndividualModel);
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
                    ctx.Entry(Individual).State = System.Data.Entity.EntityState.Deleted;
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
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int businessTypeID = Convert.ToInt32(Id);
                    var businessType = ctx.BusinessTypes.Where(a => a.BusinessTypeID == businessTypeID).FirstOrDefault();
                    businessTypeModel = Mapper.Map<BusinessTypeModel>(businessType);
                }
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

                    businessType = Mapper.Map<BusinessType>(businessTypeModel);
                    ctx.BusinessTypes.AddOrUpdate(businessType);
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
    }
}