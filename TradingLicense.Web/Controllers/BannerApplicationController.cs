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

namespace TradingLicense.Web.Controllers
{
    public class BannerApplicationController : BaseController
    {
        LicenseApplicationContext db = new LicenseApplicationContext();
        #region BannerCode

        /// <summary>
        /// GET: BannerCode
        /// </summary>
        /// <returns></returns>
        public ActionResult BannerCode()
        {
            return View();
        }

        /// <summary>
        /// Save Banner Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BannerCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string bannerCodeDesc)
        {
            List<TradingLicense.Model.BannerCodeModel> bannerCode = new List<Model.BannerCodeModel>();
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

        /// <summary>
        /// Save Banner Code Infomration
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

        #region BannerApplication

        /// <summary>
        /// GET: BannerApplication
        /// </summary>
        /// <returns></returns>
        public ActionResult BannerApplication()
        {
            return View();
        }

        /// <summary>
        /// Save Banner Application Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BannerApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string bannerApplicationID)
        {
            List<TradingLicense.Model.BannerApplicationModel> bannerApplication = new List<Model.BannerApplicationModel>();
            int totalRecord = 0;

            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    IQueryable<BannerApplication> query = ctx.BannerApplications;
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
                                        .Include("Users")
                                        .Include("Individual").OrderBy(m => m.BannerApplicationID).ToList();
                    foreach (var item in Dtls)
                    {
                        item.Users.RoleTemplateID = ProjectSession.User.RoleTemplateID;
                    }
                    //Mapper.Map<List<BannerApplicationModel>>(query.ToList());
                    return Json(new DataTablesResponse(requestModel.Draw, Dtls.ToList(), totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return null;
                //Do something here
            }


        }

        /// <summary>
        /// Save Banner Application Data By Individual
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

                var qry = ctx.Individuals.Where(e => e.IndividualID == 1);

                if (Id != null && Id > 0)
                {
                    BannerObjectModel = db.BannerObjects.Include("BannerCode")
                                             .Include("Location")
                                             .Where(x => x.BannerApplicationID == Id).ToList();
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

                    int bannerApplicationID = Convert.ToInt32(Id);
                    var bannerApplication = ctx.BannerApplications.Where(a => a.BannerApplicationID == bannerApplicationID).FirstOrDefault();
                    bannerApplicationModel.BannerApplicationID = bannerApplication.BannerApplicationID;
                    Int32? CompId = bannerApplication.CompanyID;
                    bannerApplicationModel.CompanyID = Convert.ToInt32(CompId);
                    bannerApplicationModel.IndividualID = bannerApplication.IndividualID;
                    bannerApplicationModel.AppStatusID = bannerApplication.AppStatusID;  
                    //bannerApplicationModel = Mapper.Map<BannerApplicationModel>(bannerApplication);
                }
            }
            ViewBag.BALinkReqDoc = Attchments;
            ViewBag.BannerObjects = BannerObjectModel;
            ViewBag.UserRole = ProjectSession.User.RoleTemplateID;
            return View(bannerApplicationModel);
        }

        /// <summary>
        /// Save Banner Application Infomration
        /// </summary>
        /// <param name="bannerApplicationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveManageBannerApplication(string IndividualId, string compId, string ImgModel, string gridItems, string BannerApplist, string btnType)
        {


            List<BannerObject> BannerObjectData;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            BannerObjectData = jss.Deserialize<List<BannerObject>>(gridItems);

            List<BannerApplication> BannerApp;
            JavaScriptSerializer jss1 = new JavaScriptSerializer();
            BannerApp = jss1.Deserialize<List<BannerApplication>>(BannerApplist);

            List<Attchments> Attchment;
            JavaScriptSerializer jss2 = new JavaScriptSerializer();
            Attchment = jss2.Deserialize<List<Attchments>>(ImgModel);

            int scope_id = 0;

            int BannerApplicationID = 0;
            if (btnType != "" && IndividualId != "" && compId != "")
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in BannerApp)
                        {
                            int AppStatusId = 0;
                            
                            if (btnType == "btnSubmit")
                            {
                                if (ProjectSession.User.RoleTemplateID == 3)
                                {
                                    AppStatusId = 7;
                                }
                                else if (ProjectSession.User.RoleTemplateID == 2)
                                {
                                    AppStatusId = 2;
                                }
                                else
                                {
                                    AppStatusId = 2;
                                }
                            }
                            else
                            {
                                if (ProjectSession.User.RoleTemplateID == 3)
                                {
                                    AppStatusId = 4;
                                }
                                else
                                {
                                    AppStatusId = 1;
                                }
                            }

                            BannerApplicationID = item.BannerApplicationID;
                            item.UsersID = ProjectSession.UserID;
                            item.UpdatedBy = ProjectSession.User.FullName;
                            item.AppStatusID = AppStatusId;
                            if (BannerApplicationID > 0)
                            {
                                db.Entry(item).State = EntityState.Modified;
                            }
                            else
                            {
                                db.Entry(item).State = EntityState.Added;
                            }
                            db.SaveChanges();
                            scope_id = item.BannerApplicationID;
                        }
                        db.BannerObjects.RemoveRange(db.BannerObjects.Where(x => x.BannerApplicationID == scope_id));
                        db.SaveChanges();
                        foreach (var item in BannerObjectData)
                        {
                            item.BannerApplicationID = scope_id;
                            db.BannerObjects.AddOrUpdate(item);
                            db.SaveChanges();
                        }
                        
                        foreach (var item in Attchment)
                        {
                            Attachment Atch = new Attachment();
                            Atch.AttachmentID = 0;
                            Atch.FileName = item.filename;
                            db.Attachments.AddOrUpdate(Atch);
                            db.SaveChanges();
                            var AtchId = Atch.AttachmentID;
                            BALinkReqDoc ReqDoc = new BALinkReqDoc();
                            ReqDoc.AttachmentID = AtchId;
                            ReqDoc.BALinkReqDocID = 0;
                            ReqDoc.BannerApplicationID = scope_id;
                            ReqDoc.RequiredDocID = item.Id;
                            db.BALinkReqDocs.AddOrUpdate(ReqDoc);
                            db.SaveChanges();
                        }
                        if (Request.Files.Count > 0)
                        {
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
                                if (!System.IO.Directory.Exists(Server.MapPath("~/Documents/Attachment/BannerApplication/" + scope_id)))
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath("~/Documents/Attachment/BannerApplication/" + scope_id));
                                }
                                fname = Path.Combine(Server.MapPath("~/Documents/Attachment/BannerApplication/" + scope_id), fname);
                                file.SaveAs(fname);

                            }
                        }
                        transaction.Commit();
                        TempData["SuccessMessage"] = "Banner Application saved successfully.";
                        return Json(Convert.ToString(1));
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
            return Json(Convert.ToString(0));
        }


        [HttpPost]
        public JsonResult FillIndividual(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var Individual = ctx.Individuals.Where(t => t.FullName.ToLower().Contains(query.ToLower())).Select(x => new { IndividualID = x.IndividualID, FullName = x.FullName }).ToList();
                return Json(Individual, JsonRequestBehavior.AllowGet);
            }
        }
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

        public ActionResult GenerateLetter(Int32? appId)
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
            catch(Exception ex)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating Letter!');</script>");
        }

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