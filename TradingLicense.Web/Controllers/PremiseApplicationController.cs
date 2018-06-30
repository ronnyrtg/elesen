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
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using TradingLicense.Web.Helpers;
using TradingLicense.Web.Services;
using PdfSharp.Drawing.Layout;

namespace TradingLicense.Web.Controllers
{
    public class PremiseApplicationController : BaseController
    {

        public const string OnSubmit = "Submitted";
        public const string OnRouteSubmit = "SubmittedToRoute";
        public const string OnRejected = "Rejected";
        public const string OnKIV = "KIV";

        private Func<BusinessCode, Select2ListItem> fnSelectBusinessCode = bc => new Select2ListItem { id = bc.BusinessCodeID, text = $"{bc.CodeDesc}~{bc.CodeNumber}" };
        private Func<Individual, Select2ListItem> fnSelectIndividualFormat = ind => new Select2ListItem { id = ind.IndividualID, text = $"{ind.FullName} ({ind.MykadNo})" };

        #region PremiseApplication

        /// <summary>
        /// GET: PremiseApplication
        /// </summary>
        /// <returns></returns>
        [AuthorizationPrivilegeFilter(SystemEnum.Page.PremiseApplication, SystemEnum.PageRight.CrudLevel)]
        public ActionResult PremiseApplication()
        {           
            return View();
        }

        #region PremiseApplication List page
        /// <summary>
        /// Get PremiseApplication Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="premiseApplicationId">The premise application identifier.</param>
        /// <param name="individualMkNo">The individual mk no.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PremiseApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string premiseApplicationId, string individualMkNo)
        {
            List<PremiseApplicationModel> premiseApplication;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                int? rollTemplateID = ProjectSession.User?.RoleTemplateID;
                IQueryable<PremiseApplication> query = ctx.PremiseApplications;

                if (rollTemplateID.HasValue)
                {
                    switch (rollTemplateID)
                    {
                        case (int)RollTemplate.Public:
                            {
                                query = query.Where(q => q.UsersID == ProjectSession.UserID);
                            }
                            break;
                        case (int)RollTemplate.RouteUnit:
                            if (string.IsNullOrEmpty(premiseApplicationId))
                            {
                                var departmentID = ProjectSession.User?.DepartmentID;
                                if (departmentID.HasValue)
                                {
                                    var paIDs = ctx.PADepSupps.Where(pa => pa.DepartmentID == departmentID.Value && string.IsNullOrEmpty(pa.SubmittedBy) && pa.IsActive)
                                                                .Select(d => d.PremiseApplicationID).Distinct().ToList();
                                    query = query.Where(q => paIDs.Contains(q.PremiseApplicationID) && q.AppStatusID == 5);
                                }
                            }
                            break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(premiseApplicationId))
                {
                    query = query.Where(q => q.PremiseApplicationID.ToString().Contains(premiseApplicationId));
                }

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<PremiseApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "PremiseApplicationID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                premiseApplication = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, premiseApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PremiseApplication By Individual
        /// <summary>
        /// Get PremiseApplication Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PremiseApplicationsByIndividual([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? individualId)
        {
            List<PremiseApplicationModel> premiseApplication;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<PremiseApplication> query = ProjectSession.User != null && ProjectSession.User.RoleTemplateID == (int)RollTemplate.Public
                    ? ctx.PremiseApplications.Where(p => p.UsersID == ProjectSession.User.UsersID)
                    : ctx.PremiseApplications;

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<PremiseApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "PremiseApplicationID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                premiseApplication = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, premiseApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Required Doc List Datatable
        /// <summary>
        /// Get Required Document Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="businessTypeId">The business type identifier.</param>
        /// <param name="premiseApplicationId">The premise application identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RequiredDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessTypeId, string premiseApplicationId)
        {
            List<BTLinkReqDocModel> requiredDocument;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BTLinkReqDoc> query = ctx.PALinkReqDocs.Where(p => p.BusinessTypeID.ToString().Contains(businessTypeId));

                #region Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                string orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<BTLinkReqDocModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BTLinkReqDocID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                requiredDocument = result;

                #region IsChecked

                if (!string.IsNullOrWhiteSpace(premiseApplicationId))
                {
                    int premiseAppId;
                    int.TryParse(premiseApplicationId, out premiseAppId);

                    var palinkReq = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == premiseAppId).ToList();
                    if (palinkReq.Count > 0)
                    {
                        //TODO: try Join 
                        foreach (var item in requiredDocument)
                        {
                            var resultpalinkReq = palinkReq.FirstOrDefault(p => p.RequiredDocID == item.RequiredDocID && p.PremiseApplicationID == premiseAppId);
                            if (resultpalinkReq != null)
                            {
                                item.IsChecked = "checked";
                                var attechmentdetails = ctx.Attachments.FirstOrDefault(a => a.AttachmentID == resultpalinkReq.AttachmentID);
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
            return Json(new DataTablesResponse(requestModel.Draw, requiredDocument, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Comments List Datatable
        /// <summary>
        /// Get Comments for the premise applicaiton
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="premiseApplicationID">The premise application identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PremiseComments([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? premiseApplicationID)
        {
            List<PACommentModel> premiseComments = new List<PACommentModel>();
            int totalRecord = 0;
            if (premiseApplicationID.HasValue)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    IQueryable<PAComment> query = ctx.PAComments.Include("Users").Where(pac => pac.PremiseApplicationID == premiseApplicationID.Value);

                    #region Sorting
                    // Sorting
                    var sortedColumns = requestModel.Columns.GetSortedColumns();
                    var orderByString = sortedColumns.GetOrderByString();

                    var result = Mapper.Map<List<PACommentModel>>(query.ToList());
                    result = result.OrderBy(orderByString == string.Empty ? "CommentDate desc" : orderByString).ToList();

                    totalRecord = result.Count;

                    #endregion Sorting

                    // Paging
                    result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                    premiseComments = result;
                }
            }            
            return Json(new DataTablesResponse(requestModel.Draw, premiseComments, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Route Comments Datatable
        /// <summary>
        /// Get Department support/non-supported Comments for the premise applicaiton
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="premiseApplicationID">The premise application identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PremiseRouteComments([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? premiseApplicationID)
        {
            List<PADepSuppModel> premiseRouteComments = new List<PADepSuppModel>();
            int totalRecord = 0;
            if (premiseApplicationID.HasValue)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    IQueryable<PADepSupp> query = ctx.PADepSupps
                                                        .Include("Department")
                                                        .Where(pac => pac.PremiseApplicationID == premiseApplicationID.Value);

                    #region Sorting
                    // Sorting
                    var sortedColumns = requestModel.Columns.GetSortedColumns();
                    var orderByString = sortedColumns.GetOrderByString();

                    var result = Mapper.Map<List<PADepSuppModel>>(query.ToList());
                    result = result.OrderBy(orderByString == string.Empty ? "SubmittedDate desc" : orderByString).ToList();

                    totalRecord = result.Count;

                    #endregion Sorting

                    // Paging
                    result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                    premiseRouteComments = result;
                }
            }
            return Json(new DataTablesResponse(requestModel.Draw, premiseRouteComments, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Download Attachment Files
        /// <summary>
        /// Download
        /// </summary>
        /// <param name="attechmentId"></param>
        /// /// <param name="premiseId"></param>
        /// <returns></returns>
        public FileResult Download(int? attechmentId, int? premiseId)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var attechment = ctx.Attachments.FirstOrDefault(a => a.AttachmentID == attechmentId);
                var folder = Server.MapPath(ProjectConfiguration.AttachmentDocument);
                try
                {
                    try
                    {
                        if (attechment != null && attechment.AttachmentID > 0)
                        {
                            var path = Path.Combine(folder, attechment.FileName);
                            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, attechment.FileName);
                        }

                        return null;
                    }
                    catch
                    {
                        // todo: this is very bad code with empty catch. Log or write or do anything to notify  about error
                        
                    }
                }
                catch
                {
                    // todo: this is very bad code with empty catch. Log or write or do anything to notify  about error
                    
                }
                return null;
            }
        }
        #endregion

        #region ManagePremiseApplication
        /// <summary>
        /// Get PremiseApplication Data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ManagePremiseApplication(int? id)
        {
            PremiseApplicationModel premiseApplicationModel = new PremiseApplicationModel();
            //TODO: I guess 2012 year is outdated
            premiseApplicationModel.StartRent = DateTime.Today;
            premiseApplicationModel.StopRent = DateTime.Today;
            if (id != null && id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var premiseApplication = ctx.PremiseApplications.FirstOrDefault(a => a.PremiseApplicationID == id);
                    premiseApplicationModel = Mapper.Map<PremiseApplicationModel>(premiseApplication);

                    var paLinkBc = ctx.PALinkBC.Where(a => a.PremiseApplicationID == id).ToList();
                    premiseApplicationModel.BusinessCodeids = string.Join(",", paLinkBc.Select(x => x.BusinessCodeID.ToString()).ToArray());

                    //TODO: replaced with this for avoid calling database in foreach loop
                    // TODO: Select2ListItem is just the same as build-in KeyValuePair class
                    List<Select2ListItem> businessCodesList = new List<Select2ListItem>();
                    var ids = paLinkBc.Select(b => b.BusinessCodeID).ToList();
                    businessCodesList = ctx.BusinessCodes
                        .Where(b => ids.Contains(b.BusinessCodeID))
                        .Select(fnSelectBusinessCode)
                        .ToList();

                    premiseApplicationModel.selectedbusinessCodeList = businessCodesList;
                    premiseApplicationModel.HasPADepSupp = ctx.PADepSupps.Any(pa => pa.PremiseApplicationID == id.Value);

                    var paLinkInd = ctx.PALinkInd.Where(a => a.PremiseApplicationID == id).ToList();
                    premiseApplicationModel.Individualids = string.Join(",", paLinkInd.Select(x => x.IndividualID.ToString()).ToArray());
                    List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
                    var iids = paLinkInd.Select(b => b.IndividualID).ToList();
                    selectedIndividualList = ctx.Individuals
                        .Where(b => iids.Contains(b.IndividualID))
                        .Select(fnSelectIndividualFormat)
                        .ToList();

                    premiseApplicationModel.selectedIndividualList = selectedIndividualList;

                    var paLinkReqDocumentList = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == id).ToList();
                    if (paLinkReqDocumentList.Count > 0)
                    {
                        premiseApplicationModel.UploadRequiredDocids = (string.Join(",", paLinkReqDocumentList.Select(x => x.RequiredDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }

                    var paLinkAddDocumentlist = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == id).ToList();
                    if (paLinkAddDocumentlist.Count > 0)
                    {
                        premiseApplicationModel.UploadAdditionalDocids = (string.Join(",", paLinkAddDocumentlist.Select(x => x.AdditionalDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }

                    if (premiseApplication.AppStatusID == (int)PAStausenum.Pendingpayment)
                    {
                        var duePayment = ctx.PaymentDues.Where(pd => pd.PaymentFor == premiseApplicationModel.ReferenceNo).FirstOrDefault();
                        if (duePayment != null)
                        {
                            premiseApplicationModel.AmountDue = duePayment.AmountDue;
                        }
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
        #endregion

        #region ViewPremiseApplication
        /// <summary>
        /// View PremiseApplication Data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewPremiseApplication(int? id)
        {
            PremiseApplicationModel premiseApplicationModel = new PremiseApplicationModel();
            
            if (id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    
                    int premiseApplicationId = Convert.ToInt32(id);
                    var premiseApplication = ctx.PremiseApplications.FirstOrDefault(a => a.PremiseApplicationID == premiseApplicationId);
                    premiseApplicationModel = Mapper.Map<PremiseApplicationModel>(premiseApplication);
                    
                    //List all Selected Business Codes
                    var paLinkBc = ctx.PALinkBC.Where(a => a.PremiseApplicationID == premiseApplicationId).ToList();
                    var bids = paLinkBc.Select(b => b.BusinessCodeID).ToList();
                    var businessList = ctx.BusinessCodes
                        .Where(b => bids.Contains(b.BusinessCodeID))
                        .Select(b => b.CodeDesc)
                        .ToList();
                    premiseApplicationModel.businessCodeList = businessList;

                    //List all individual names
                    var paLinkInd = ctx.PALinkInd.Where(a => a.PremiseApplicationID == id).ToList();
                    var iids = paLinkInd.Select(b => b.IndividualID).ToList();
                    var indList = ctx.Individuals
                        .Where(b => iids.Contains(b.IndividualID))
                        .Select(b => new IndividualLink()
                        {
                            IndListID = b.IndividualID,
                            IndName = b.FullName,
                        })
                        .ToList();
                    premiseApplicationModel.indLinkList = indList;

                    //List required Documents
                    var requiredDocNames = from pa in ctx.PALinkReqDoc
                                           join re in ctx.RequiredDocs on pa.RequiredDocID equals re.RequiredDocID
                                           join at in ctx.Attachments on pa.AttachmentID equals at.AttachmentID
                                           where pa.PremiseApplicationID == id
                                           select new reqDocLink
                                           {
                                               AttName = at.FileName,
                                               reqDocDesc = re.RequiredDocDesc
                                           };
                                                               
                    premiseApplicationModel.RequiredDocNames = requiredDocNames.ToList();


                    //List Additional Documents
                    var addDocDescs = from pa in ctx.PALinkAddDocs
                                           join re in ctx.AdditionalDocs on pa.AdditionalDocID equals re.AdditionalDocID
                                           join at in ctx.Attachments on pa.AttachmentID equals at.AttachmentID
                                           where pa.PremiseApplicationID == id
                                           select new addDocLink
                                           {
                                               AttName = at.FileName,
                                               addDocDesc = re.DocDesc
                                           };


                    premiseApplicationModel.AdditionalDocDescs = addDocDescs.ToList();

                    
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                premiseApplicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
            }
            return View(premiseApplicationModel);
        }
        #endregion

        #region Save Route Comments data
        [HttpPost]
        public ActionResult SaveRouteUnitComment(int premiseApplicationID, string comment, int supported)
        {
            PremiseApplicationModel premiseApplicationModel = new PremiseApplicationModel();

            if (premiseApplicationID > 0)
            {
                var departmentID = ProjectSession.User?.DepartmentID;
                using (var ctx = new LicenseApplicationContext())
                {
                    var paDepSupp = ctx.PADepSupps.Where(pa => pa.PremiseApplicationID == premiseApplicationID && pa.DepartmentID == departmentID && pa.IsActive).FirstOrDefault();
                    if (paDepSupp != null)
                    {
                        paDepSupp.IsSupported = supported == 1;
                        paDepSupp.Comment = comment;
                        paDepSupp.UserId = ProjectSession.UserID;
                        paDepSupp.SubmittedBy = ProjectSession.User?.FullName ?? ProjectSession.UserName;
                        paDepSupp.SubmittedDate = DateTime.Now;
                        paDepSupp.IsActive = false;
                        ctx.PADepSupps.AddOrUpdate(paDepSupp);
                        ctx.SaveChanges();

                        TempData["SuccessMessage"] = "Premise License Application draft saved successfully.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = paDepSupp == null ?
                            "Unable to Find Route unit request" :
                            $"Other user: {paDepSupp.SubmittedBy} from your department already submitted response";
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to find Premise application";
            }

            return RedirectToAction("PremiseApplication", "PremiseApplication");
        }
        #endregion

        #region Generate License PDF
        public ActionResult GeneratLicense(Int32? appId)
        {
            PremiseApplicationModel premiseApplicationModel = new PremiseApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var premiseApp = ctx.PremiseApplications
                                        .Include("Company").Where(x => x.PremiseApplicationID == appId).ToList();
                    if (premiseApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid Premise ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in premiseApp)
                        {
                            int lineheight = 50;
                            PdfDocument pdf = new PdfDocument();
                            pdf.Info.Title = "PDF License";
                            PdfPage pdfPage = pdf.AddPage();
                            XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                            XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                            XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                            XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                            XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);

                            XImage xImage = XImage.FromFile(Server.MapPath("~\\images\\wm1.png"));
                            graph.DrawImage(xImage, 0, 0, pdfPage.Width, pdfPage.Height);

                            XImage xImage1 = XImage.FromFile(Server.MapPath("~\\images\\logoPL.png"));
                            graph.DrawImage(xImage1, 10, 70, 100, 75);

                            graph.DrawString("TARIKH CETAKAN :", font, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(string.Format("{0:dd MMMM yyyy}", DateTime.Now) , nfont, XBrushes.Black, new XRect(500, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            graph.DrawString("PERBADANAN LABUAN", lbfont, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Wisma Perbadanan Labuan", font, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Peti Surat 81245", font, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("87022 Willayah Persekutuan Labuan", font, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Tel No 		:", font, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("087-408692/600", font, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Faks No    :", font, XBrushes.Black, new XRect(115, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("087-408348", font, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph.DrawString("LESEN", lbfont, XBrushes.Black, new XRect(260, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("(Lesen ini dikeluarkan dibawah Undang-Undang Kecil Tred,Perniagaan dan Perindustrian Wilayah Persekutuan Labuan 2016)", sfont, XBrushes.Black, new XRect(50, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                           
                            lineheight = lineheight + 30;
                            string compName = "";
                            if (item.Company.CompanyName == null)
                            {
                                compName = "";
                            }
                            else
                            {
                                compName = item.Company.CompanyName;
                            }
                            graph.DrawString(compName, lbfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("SSM No", font, XBrushes.Black, new XRect(310, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.Company.RegistrationNo != null)
                            {
                                graph.DrawString(item.Company.RegistrationNo, nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            
                            lineheight = lineheight + 15;
                            graph.DrawString("No Rujukan", font, XBrushes.Black, new XRect(310, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(DateTime.Now.Year + "/PA/LB/" + string.Format("{0:0000000}", appId), nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            //if (item.ReferenceNo != null)
                            //{
                            //    graph.DrawString(DateTime.Now.Year + "/PA/LB" + string.Format("{0:000000}",appId) , nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            //}
                            string compAdd = "";
                            if (item.Company.CompanyAddress == null)
                            {
                                compAdd = "";
                            }
                            else
                            {
                                compAdd = item.Company.CompanyAddress;
                            }
                            XTextFormatter tf = new XTextFormatter(graph);
                            XRect rect = new XRect(30, lineheight, 250,30);
                            graph.DrawRectangle(XBrushes.Transparent, rect);
                            tf.DrawString(compAdd.ToString(), nfont, XBrushes.Black, rect, XStringFormats.TopLeft);

                            //graph.DrawString(compAdd.ToString(), nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;

                            graph.DrawString("Taraf Lesen", font, XBrushes.Black, new XRect(310, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.LicenseStatus != null)
                            {
                                graph.DrawString(item.LicenseStatus, nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }

                            string compPhone = "";
                            if (item.Company.CompanyPhone == null)
                            {
                                compPhone = "";
                            }
                            else
                            {
                                compPhone = item.Company.CompanyPhone;
                            }

                            //graph.DrawString(compPhone, nfont, XBrushes.Black, new XRect(30, lineheight+15, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Tempoh Sah", font, XBrushes.Black, new XRect(310, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(380, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.DatePaid != null && item.DatePaid.ToString() != "")
                            {
                                DateTime dt = new DateTime(item.DatePaid.Value.Year, item.DatePaid.Value.Month, item.DatePaid.Value.Day);
                                var mDate = string.Format("{0:dd MMMM yyyy}", dt);
                                graph.DrawString(mDate, nfont, XBrushes.Black, new XRect(390, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                                graph.DrawString("Hingga", font, XBrushes.Black, new XRect(453, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                DateTime? hDate;
                                hDate = item.DatePaid;
                                if (Convert.ToInt32(item.Mode) ==1)
                                {
                                    hDate = hDate.Value.AddMonths(6); 
                                }
                                else
                                {
                                    hDate = hDate.Value.AddMonths(12);
                                }
                                graph.DrawString(string.Format("{0:dd MMMM yyyy}", hDate), nfont, XBrushes.Black, new XRect(490, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 30;
                            graph.DrawRectangle(XBrushes.LightSalmon, 10, lineheight, pdfPage.Width - 30 , 20);
                            graph.DrawString("MAKLUMAT LESEN", font, XBrushes.Black, new XRect(15, lineheight +5, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 30;

                            graph.DrawString("KOD AKTIVITI", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("KETERANGAN", font, XBrushes.Black, new XRect(((pdfPage.Width)/2)-70 , lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("AMAUN", font, XBrushes.Black, new XRect(510, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            XPen lineRed1 = new XPen(XColors.Black , 0.5);
                            System.Drawing.Point pt4 = new System.Drawing.Point(10, lineheight);
                            System.Drawing.Point pt5 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width)-20, lineheight);
                            graph.DrawLine(lineRed1, pt4, pt5);
                            lineheight = lineheight + 3;
                            float TotAmount = 0;
                            int TotHeight = 0;
                            foreach (var item1 in ctx.PALinkBC.Where(x => x.PremiseApplicationID == appId))
                            {
                                if(Convert.ToInt32(item1.BusinessCodeID) > 0)
                                {
                                    foreach (var item2 in ctx.BusinessCodes.Where(x => x.BusinessCodeID == item1.BusinessCodeID))
                                    {
                                        if (item2.CodeNumber != null)
                                        {
                                          graph.DrawString(item2.CodeNumber, nfont, XBrushes.Black, new XRect(40, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            
                                        }
                                        if (item2.CodeDesc != null)
                                        {
                                            XTextFormatter tf1 = new XTextFormatter(graph);
                                            XRect rect1 = new XRect(((pdfPage.Width) / 2) - 100, lineheight, 300, 30);
                                            graph.DrawRectangle(XBrushes.Transparent, rect1);
                                            tf1.DrawString(item2.CodeDesc, nfont, XBrushes.Black, rect1, XStringFormats.TopLeft);
                                            //graph.DrawString(item2.CodeDesc, nfont, XBrushes.Black, new XRect(((pdfPage.Width) / 2) - 100, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        }
                                        float Amount = 0;
                                        if(item2.BaseFee == 0)
                                        {
                                            Amount = (item2.DefaultRate) * (item.PremiseArea);
                                        }
                                        else
                                        {
                                            Amount = item2.BaseFee;
                                        }
                                        graph.DrawString("RM " + string.Format("{0:0.00}",Amount) , nfont, XBrushes.Black, new XRect(510, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        TotAmount = TotAmount + Amount;
                                        lineheight = lineheight + 20;
                                        TotHeight = TotHeight + lineheight;
                                        XPen lineRed2 = new XPen(XColors.Black, 0.5);
                                        System.Drawing.Point pt6 = new System.Drawing.Point(10, lineheight);
                                        System.Drawing.Point pt7 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 20, lineheight);
                                        graph.DrawLine(lineRed1, pt6, pt7);
                                    }
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("JUMLAH", font, XBrushes.Black, new XRect(450, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("RM " + string.Format("{0:0.00}", TotAmount) , nfont, XBrushes.Black, new XRect(510, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (TotAmount == 0)
                            {
                                lineheight = lineheight + 100;
                            }
                            else { lineheight = lineheight + 30; }
                            
                            graph.DrawString("PEMILIK / PEKONGSI", font, XBrushes.Black, new XRect(20, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            Int32 cnt = 1;
                            foreach (var item3 in ctx.PALinkInd.Where(x => x.PremiseApplicationID == appId))
                            {
                                if (Convert.ToInt32(item3.IndividualID) > 0)
                                {
                                    foreach (var item4 in ctx.Individuals.Where(x => x.IndividualID == item3.IndividualID))
                                    {
                                        {
                                            if (item4.FullName != null)
                                            {
                                                string fName = item4.FullName;
                                                string itm = cnt.ToString() + " .    "  + fName;
                                                graph.DrawString(itm, nfont, XBrushes.Black, new XRect(20, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                                cnt = cnt + 1;
                                            }
                                            if(item4.MykadNo != null)
                                            {
                                                graph.DrawString(item4.MykadNo, nfont, XBrushes.Black, new XRect(480, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                            }
                                            lineheight = lineheight + 15;
                                        }
                                    }
                                }
                            }
                            lineheight = lineheight + 50;
                            string str = "LESEN TAHUN  ";
                            if(item.DateApproved != null)
                            {
                                str = str + item.DateApproved.Value.Year;
                            }
                            graph.DrawString(str, font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph.DrawString("LESEN INI HENDAKLAH DIPAMERKAN", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            XPen lineRed3 = new XPen(XColors.Black, 0.5);
                            System.Drawing.Point pt8 = new System.Drawing.Point(400, lineheight -20);
                            System.Drawing.Point pt9 = new System.Drawing.Point(570, lineheight-20);
                            graph.DrawLine(lineRed1, pt8, pt9);
                            graph.DrawString("KETUA PEGAWAI EKSEKUTIF", font, XBrushes.Black, new XRect(420, lineheight-15, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("PERBADANAN LABUAN ", font, XBrushes.Black, new XRect(430, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("Pihak Berkuasa Pelesenan", nfont, XBrushes.Black, new XRect(434, lineheight+15, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
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
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion

        #region Save ManagePremiseApplication Data
        /// <summary>
        /// Save PremiseApplication Information
        /// </summary>
        /// <param name="premiseApplicationModel">The premise application model.</param>
        /// <param name="btnSubmit">The BTN submit.</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManagePremiseApplication(PremiseApplicationModel premiseApplicationModel, string btnSubmit)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    bool saveSuccess = false;
                    using (var ctx = new LicenseApplicationContext())
                    {
                        saveSuccess = SavePremiseApplication(premiseApplicationModel, ctx);
                    }
                    if (saveSuccess && premiseApplicationModel.IsDraft)
                    {
                        TempData["SuccessMessage"] = "Premise License Application draft saved successfully.";

                        return Redirect(Url.Action("ManagePremiseApplication", "PremiseApplication") + "?id=" + premiseApplicationModel.PremiseApplicationID);
                    }
                    if (saveSuccess)
                    {
                        TempData["SuccessMessage"] = "Premise License Application saved successfully.";
                        return RedirectToAction("PremiseApplication");
                    }
                    return Redirect(Url.Action("ManagePremiseApplication", "PremiseApplication") + "?id=" + premiseApplicationModel.PremiseApplicationID);
                }

                
                return View(premiseApplicationModel);
            }
            catch (Exception)
            {
                
                return View(premiseApplicationModel);
            }
        }
        #endregion

        #region Save PaymentDue Data
        [HttpPost]
        public ActionResult SavePaymentDue(int premiseApplicationID, float totalDue)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var pa = ctx.PremiseApplications.Where(p => p.PremiseApplicationID == premiseApplicationID).FirstOrDefault();
                if(pa != null)
                {
                    PremiseApplicationModel paModel = Mapper.Map<PremiseApplicationModel>(pa);
                    PaymentsService.AddPaymentDue(paModel, ctx, ProjectSession.User?.FullName ?? ProjectSession.UserName, totalDue);
                    UpdateStatusId(paModel, ctx, (int)PAStausenum.Pendingpayment, pa);
                    TempData["SuccessMessage"] = "Premise License Application payments saved successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to find matching premise application ID";
                }
            }

            return Redirect(Url.Action("ManagePremiseApplication", "PremiseApplication") + "?id=" + premiseApplicationID);
        }
        #endregion

        #region Save Received Payment Data
        [HttpPost]
        public ActionResult SaveReceivedPayment(int premiseApplicationID, int individualID)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var pa = ctx.PremiseApplications.Where(p => p.PremiseApplicationID == premiseApplicationID).FirstOrDefault();
                if (pa != null)
                {
                    PremiseApplicationModel paModel = Mapper.Map<PremiseApplicationModel>(pa);
                    var duePayment = ctx.PaymentDues.Where(pd => pd.PaymentFor == pa.ReferenceNo).FirstOrDefault();
                    if (duePayment != null)
                    {
                        paModel.AmountDue = duePayment.AmountDue;
                    }
                    PaymentsService.AddPaymentReceived(paModel, ctx, individualID, ProjectSession.User?.FullName ?? ProjectSession.UserName);
                    if (pa.Mode == 0)
                    {
                        pa.LicenseStatus = "Lulus Bersyarat";
                    }
                    pa.AppStatusID = (int)PAStausenum.LicenseGenerated;
                    ctx.PremiseApplications.AddOrUpdate(pa);
                    ctx.SaveChanges();
                    TempData["SuccessMessage"] = "Premise License Application payments saved successfully.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Unable to find matching premise application ID";
                }
            }

            return Redirect(Url.Action("ManagePremiseApplication", "PremiseApplication") + "?id=" + premiseApplicationID);
        }
        #endregion

        #region Get PaymentDue data
        [HttpPost]
        public ActionResult GetPaymentDue(int premiseApplicationID)
        {
            bool success = false;
            bool allowEdit = false;
            var totalDue = 0.0f;
            using (var ctx = new LicenseApplicationContext())
            {
                var pa = ctx.PremiseApplications.Where(p => p.PremiseApplicationID == premiseApplicationID).FirstOrDefault();
                if (pa != null)
                {
                    PremiseApplicationModel paModel = Mapper.Map<PremiseApplicationModel>(pa);
                    var duePayment = ctx.PaymentDues.Where(pd => pd.PaymentFor == pa.ReferenceNo).FirstOrDefault();
                    if (duePayment != null)
                    {
                        totalDue = duePayment.AmountDue;
                    }
                    else
                    {
                        totalDue = PaymentsService.CalculatePaymentDue(paModel, ctx);
                        allowEdit = true;
                    }
                    success = true;
                }
            }
            return Json(new { success = success, allowEdit = allowEdit, totalDue = totalDue }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save PremiseApplication data
        private bool SavePremiseApplication(PremiseApplicationModel premiseApplicationModel, LicenseApplicationContext ctx)
        {
            var premiseApplication = Mapper.Map<PremiseApplication>(premiseApplicationModel);

            int userroleTemplate = 0;
            if (ProjectSession.User != null && ProjectSession.UserID > 0)
            {
                userroleTemplate = GetUserRoleTemplate(premiseApplicationModel, premiseApplication, ctx);
            }
            var finalStatus = GetStatusOnSubmit(premiseApplicationModel, ctx, premiseApplication, userroleTemplate);
            if (finalStatus != 0)
            {
                premiseApplication.AppStatusID = finalStatus;
            }
            premiseApplication.DateSubmitted = DateTime.Now;

            ctx.PremiseApplications.AddOrUpdate(premiseApplication);
            ctx.SaveChanges();

            int premiseApplicationId = premiseApplication.PremiseApplicationID;
            if (premiseApplicationModel.PremiseApplicationID == 0)
            {
                premiseApplicationModel.PremiseApplicationID = premiseApplicationId;
                premiseApplication.ReferenceNo = PremiseApplicationModel.GetReferenceNo(premiseApplicationId, premiseApplication.DateSubmitted);
                ctx.PremiseApplications.AddOrUpdate(premiseApplication);
                ctx.SaveChanges();
            }

            int roleTemplate = 0;
            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                roleTemplate = ProjectSession.User.RoleTemplateID.Value;
            }

            if (userroleTemplate == (int)RollTemplate.Public)
            {
                if (!string.IsNullOrWhiteSpace(premiseApplicationModel.UploadRequiredDocids))
                {
                    DocumentService.UpdateDocs(premiseApplicationModel, ctx, premiseApplicationId, roleTemplate);
                }
                else
                {
                    if (roleTemplate == (int)RollTemplate.Public)
                    {
                        var paLinkReqDocUmentList = ctx.PALinkReqDoc
                            .Where(p => p.PremiseApplicationID == premiseApplicationId).ToList();
                        if (paLinkReqDocUmentList.Count > 0)
                        {
                            ctx.PALinkReqDoc.RemoveRange(paLinkReqDocUmentList);
                            ctx.SaveChanges();
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(premiseApplicationModel.UploadAdditionalDocids))
                {
                    DocumentService.UploadAdditionalDocs(premiseApplicationModel, ctx, premiseApplicationId, roleTemplate);
                }
                else
                {
                    if (roleTemplate == (int)RollTemplate.Public)
                    {
                        var paLinkAddDocumentlist = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == premiseApplicationId).ToList();
                        if (paLinkAddDocumentlist.Count > 0)
                        {
                            ctx.PALinkAddDocs.RemoveRange(paLinkAddDocumentlist);
                            ctx.SaveChanges();
                        }
                    }
                }
            }
            else if (userroleTemplate == (int)RollTemplate.DeskOfficer)
            {
                if (!string.IsNullOrWhiteSpace(premiseApplicationModel.RequiredDocIds))
                {
                    DocumentService.UpdateRequiredDocs(premiseApplicationModel, ctx, premiseApplicationId, roleTemplate);
                }
                else
                {
                    if (!premiseApplicationModel.IsDraft && roleTemplate == (int)RollTemplate.Public || roleTemplate == (int)RollTemplate.DeskOfficer)
                    {
                        var paLinkReqDocUmentList = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == premiseApplicationId).ToList();
                        if (paLinkReqDocUmentList.Count > 0)
                        {
                            ctx.PALinkReqDoc.RemoveRange(paLinkReqDocUmentList);
                            ctx.SaveChanges();
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(premiseApplicationModel.AdditionalDocIds))
                {
                    DocumentService.SaveAdditionalDocInfo(premiseApplicationModel, ctx, premiseApplicationId, roleTemplate);
                }
                else
                {
                    if (!premiseApplicationModel.IsDraft && roleTemplate == (int)RollTemplate.Public || roleTemplate == (int)RollTemplate.DeskOfficer)
                    {
                        var paLinkAddDocumentlist = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == premiseApplicationId).ToList();
                        if (paLinkAddDocumentlist.Count > 0)
                        {
                            ctx.PALinkAddDocs.RemoveRange(paLinkAddDocumentlist);
                            ctx.SaveChanges();
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(premiseApplicationModel.BusinessCodeids))
            {
                var businessCodelist = premiseApplicationModel.BusinessCodeids.ToIntList();

                List<int> existingRecord = new List<int>();
                var dbEntryPaLinkBAct = ctx.PALinkBC.Where(q => q.PremiseApplicationID == premiseApplicationId).ToList();
                if (dbEntryPaLinkBAct.Count > 0)
                {
                    foreach (var item in dbEntryPaLinkBAct)
                    {
                        if (businessCodelist.All(q => q != item.BusinessCodeID))
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

                foreach (var businessCode in businessCodelist)
                {
                    if (existingRecord.All(q => q != businessCode))
                    {
                        PALinkBC paLinkBc = new PALinkBC();
                        paLinkBc.PremiseApplicationID = premiseApplicationId;
                        paLinkBc.BusinessCodeID = businessCode;
                        ctx.PALinkBC.Add(paLinkBc);

                    }
                }
                ctx.SaveChanges();
            }
            else
            {
                var dbEntryPaLinkBActs = ctx.PALinkBC.Where(va => va.PremiseApplicationID == premiseApplicationId).ToList();
                if (dbEntryPaLinkBActs.Count > 0)
                {
                    ctx.PALinkBC.RemoveRange(dbEntryPaLinkBActs);
                    ctx.SaveChanges();
                }
            }

            if (!string.IsNullOrWhiteSpace(premiseApplicationModel.Individualids))
            {
                //todo: I guess it's a draft for new logic
                var individualidslist = premiseApplicationModel.Individualids.ToIntList();
                List<int> existingRecord = new List<int>();
                var dbEntryPaLinkInd = ctx.PALinkInd.Where(q => q.PremiseApplicationID == premiseApplicationId).ToList();
                if (dbEntryPaLinkInd.Count > 0)
                {
                    foreach (var item in dbEntryPaLinkInd)
                    {
                        if (individualidslist.All(q => q != item.IndividualID))
                        {
                            ctx.PALinkInd.Remove(item);
                        }
                        else
                        {
                            existingRecord.Add(item.IndividualID);
                        }
                    }
                    ctx.SaveChanges();
                }

                foreach (var individual in individualidslist)
                {
                    if (existingRecord.All(q => q != individual))
                    {
                        PALinkInd paLinkInd = new PALinkInd();
                        paLinkInd.PremiseApplicationID = premiseApplicationId;
                        paLinkInd.IndividualID = individual;
                        ctx.PALinkInd.Add(paLinkInd);

                    }
                }
                ctx.SaveChanges();
            }

            if (!string.IsNullOrWhiteSpace(premiseApplicationModel.newIndividualsList))
            {
                List<NewIndividualModel> individuals = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<List<NewIndividualModel>>(premiseApplicationModel.newIndividualsList);
                foreach (var indModel in individuals)
                {
                    Individual ind = new Individual();
                    ind.FullName = indModel.fullName;
                    ind.MykadNo = indModel.passportNo;
                    ctx.Individuals.Add(ind);

                }
                ctx.SaveChanges();
            }

            if (!string.IsNullOrWhiteSpace(premiseApplicationModel.newComment))
            {
                PAComment comment = new PAComment();
                comment.Comment = premiseApplicationModel.newComment;
                comment.CommentDate = DateTime.Now;
                comment.PremiseApplicationID = premiseApplicationId;
                comment.UsersID = ProjectSession.UserID;
                ctx.PAComments.Add(comment);
                ctx.SaveChanges();
            }
            premiseApplicationModel.PremiseApplicationID = premiseApplicationId;
            return true;
        }
        #endregion

        #region Get AppStatusID Upon Submit Button
        private int GetStatusOnSubmit(PremiseApplicationModel premiseApplicationModel, LicenseApplicationContext ctx, PremiseApplication premiseApplication, int roleTemplate)
        {
            PAStausenum finalStatus = 0;
            if (!premiseApplicationModel.IsDraft)
            {
                switch (roleTemplate)
                {
                    case (int)RollTemplate.DeskOfficer:
                        finalStatus = PAStausenum.submittedtoclerk;
                        if (premiseApplicationModel.AppStatusID == (int)PAStausenum.meeting)
                        {
                            if (premiseApplicationModel.SubmitType == OnSubmit)
                            {
                                finalStatus = PAStausenum.LetterofnotificationApproved;
                            }
                            else if (premiseApplicationModel.SubmitType == OnRejected)
                            {
                                finalStatus = PAStausenum.LetterofnotificationRejected;
                            }
                        }
                        break;
                    case (int)RollTemplate.Clerk:
                        if (premiseApplicationModel.AppStatusID == (int)PAStausenum.meeting)
                        {
                            if (premiseApplicationModel.SubmitType == OnSubmit)
                            {
                                finalStatus = PAStausenum.LetterofnotificationApproved;
                            }
                            else if (premiseApplicationModel.SubmitType == OnRejected)
                            {
                                finalStatus = PAStausenum.LetterofnotificationRejected;
                            }
                        }
                        else if (premiseApplicationModel.SubmitType == OnRouteSubmit)
                        {
                            RouteApplication(premiseApplicationModel, ctx);
                            finalStatus = PAStausenum.unitroute;
                        }
                        else if (premiseApplicationModel.SubmitType == OnSubmit)
                        {
                            finalStatus = PAStausenum.directorcheck;
                        }
                        break;
                    case (int)RollTemplate.Director:
                        if (premiseApplicationModel.AppStatusID == (int)PAStausenum.meeting)
                        {
                            if (premiseApplicationModel.SubmitType == OnSubmit)
                            {
                                finalStatus = PAStausenum.LetterofnotificationApproved;
                            }
                            else if (premiseApplicationModel.SubmitType == OnRejected)
                            {
                                finalStatus = PAStausenum.LetterofnotificationRejected;
                            }
                        }
                        else if (premiseApplicationModel.SubmitType == OnSubmit)
                        {
                            switch (premiseApplicationModel.Mode)
                            {
                                case 2:
                                    finalStatus = PAStausenum.LetterofnotificationApproved;
                                    break;
                                case 3:
                                    finalStatus = PAStausenum.CEOcheck;
                                    break;
                                case 4:
                                    finalStatus = PAStausenum.meeting;
                                    break;
                            }
                        }
                        else if (premiseApplicationModel.SubmitType == OnRejected)
                        {
                            finalStatus = PAStausenum.LetterofnotificationRejected;
                        }
                        break;
                    case (int)RollTemplate.CEO:
                        if (premiseApplicationModel.SubmitType == OnSubmit)
                        {
                            finalStatus = PAStausenum.LetterofnotificationApproved;
                        }
                        else if (premiseApplicationModel.SubmitType == OnRejected)
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

        #region Get List of Route Departments in Datatable
        /// <summary>
        /// Gets List of department details to which this premise application will be routed.
        /// </summary>
        /// <param name="businessCodeIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FillRouteDepartments([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessCodeIds)
        {
            using(var ctx = new LicenseApplicationContext())
            {
                var businessCodelist = businessCodeIds.ToIntList();
                var departmentIds = ctx.BCLinkDeps.Where(bc => businessCodelist.Contains(bc.BusinessCodeID)).Select(bc => bc.DepartmentID).Distinct().ToList();
                var departmentList = ctx.Departments.Where(dep => departmentIds.Contains(dep.DepartmentID)).ToList();
                int totalRecord = departmentList.Count;
                return Json(new DataTablesResponse(requestModel.Draw, Mapper.Map<List<DepartmentModel>>(departmentList), totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Update AppStatusID

        private void UpdateStatusId(PremiseApplicationModel premiseApplicationModel, LicenseApplicationContext ctx, int appStatusId, PremiseApplication premiseApplication = null)
        {
            premiseApplication = premiseApplication ?? ctx.PremiseApplications.Where(pa => pa.PremiseApplicationID == premiseApplicationModel.PremiseApplicationID).First();
            premiseApplication.AppStatusID = appStatusId;
            ctx.PremiseApplications.AddOrUpdate(premiseApplication);
            ctx.SaveChanges();
        }
        #endregion

        #region Save Departments data linked to Selected BusinessCode in PADepSupp table
        private bool RouteApplication(PremiseApplicationModel premiseApplicationModel, LicenseApplicationContext ctx)
        {
            if (!string.IsNullOrWhiteSpace(premiseApplicationModel.BusinessCodeids))
            {
                var businessCodelist = premiseApplicationModel.BusinessCodeids.ToIntList();
                var departmentIds = ctx.BCLinkDeps.Where(bc => businessCodelist.Contains(bc.BusinessCodeID)).Select(bc => bc.DepartmentID).Distinct().ToList();

                var oldPADepSupps = ctx.PADepSupps.Where(pa => pa.PremiseApplicationID == premiseApplicationModel.PremiseApplicationID && pa.IsActive).ToList();
                if (oldPADepSupps != null && oldPADepSupps.Count > 0)
                {
                    foreach (var oldDepSupp in oldPADepSupps)
                    {
                        oldDepSupp.IsActive = false;
                        ctx.PADepSupps.AddOrUpdate(oldDepSupp);
                    }
                }

                foreach (var depId in departmentIds)
                {
                    var paDepSupp = new PADepSupp();
                    paDepSupp.DepartmentID = depId;
                    paDepSupp.PremiseApplicationID = premiseApplicationModel.PremiseApplicationID;
                    paDepSupp.IsActive = true;
                    ctx.PADepSupps.Add(paDepSupp);
                }

                ctx.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "No Business codes attached to this application unable to route";
                return false;
            }
            return true;
        }
        #endregion

        #region Get Roletemplate from ProjectSession

        private static int GetUserRoleTemplate(PremiseApplicationModel premiseApplicationModel,
            PremiseApplication premiseApplication, LicenseApplicationContext ctx)
        {
            int userroleTemplate = 0;
            premiseApplication.UpdatedBy = ProjectSession.User.Username;

            if (ProjectSession.User.RoleTemplateID != null)
            {
                userroleTemplate = ProjectSession.User.RoleTemplateID.Value;
            }

            return userroleTemplate;
        }
        #endregion

        #region Generate Letter PDF

        public ActionResult GenerateLetter(Int32? appId)
        {
            PremiseApplicationModel premiseApplicationModel = new PremiseApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var qry = ctx.PremiseApplications
                                        .Include("Company").Where(x => x.PremiseApplicationID == appId);
                    var premiseApp = ctx.PremiseApplications
                                        .Include("Company").Where(x => x.PremiseApplicationID == appId).ToList();
                    if (premiseApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid Premise ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in premiseApp)
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
                            string add2 = "";
                            string add3 = "";
                            if (item.Addra1 != null)
                            {
                                add1 = add1 + item.Addra1 + ",";
                            }
                            if (item.Addra2 != null)
                            {
                                add1 = add1 + item.Addra2;
                            }

                            if (item.Addra3 != null)
                            {
                                add2 = add2 + item.Addra3 + ",";
                            }
                            if (item.Addra4 != null)
                            {
                                add2 = add2 + item.Addra4;
                            }
                            if (item.StateA != null)
                            {
                                add3 = add3 + item.StateA + ",";

                            }
                            if (item.PcodeA != null)
                            {
                                add3 = add3 + item.PcodeA;

                            }
                            if (add1 != "")
                            {
                                graph.DrawString(add1, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            if (add2 != "")
                            {
                                lineheight = lineheight + 15;
                                graph.DrawString(add2, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            if (add3 != "")
                            {
                                lineheight = lineheight + 15;
                                graph.DrawString(add3, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 20;
                            graph.DrawString("ACTIVITI", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            int cnt = 1;
                            foreach (var item1 in ctx.PALinkBC.Where(x => x.PremiseApplicationID == appId))
                            {
                                if (Convert.ToInt32(item1.BusinessCodeID) > 0)
                                {
                                    foreach (var item2 in ctx.BusinessCodes.Where(x => x.BusinessCodeID == item1.BusinessCodeID))
                                    {
                                        {
                                            if (item2.CodeDesc != null)
                                            {
                                                string itm = cnt.ToString() + ")    " + item2.CodeDesc;
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
                            foreach (var item3 in ctx.PALinkInd.Where(x => x.PremiseApplicationID == appId))
                            {
                                if (Convert.ToInt32(item3.IndividualID) > 0)
                                {
                                    foreach (var item4 in ctx.Individuals.Where(x => x.IndividualID == item3.IndividualID))
                                    {
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
                                }
                            }
                            lineheight = lineheight + 20;
                            graph.DrawString("KEPUTUSAN", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            string modeValue = "";
                            if (item.AppStatusID == 12)
                            {
                                modeValue = "LULUS BERSYARAT";
                            }
                            else if (item.AppStatusID == 10)
                            {
                                modeValue = "LULUS";
                            }
                            else if (item.AppStatusID == 11)
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
                            
                            foreach (var item4 in ctx.PAComments.Where(x => x.PremiseApplicationID == appId))
                            {
                                {
                                    if (item4.Comment != null)
                                    {
                                        string itm = cnt.ToString() + ") " + item4.Comment;
                                        XTextFormatter tf = new XTextFormatter(graph);
                                        XRect rect = new XRect(300, lineheight, 290, 50);
                                        graph.DrawRectangle(XBrushes.White, rect);
                                        tf.DrawString(itm, font, XBrushes.Black, rect, XStringFormats.TopLeft);
                                        //graph.DrawString(itm, font, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        cnt = cnt + 1;
                                        if(itm.Length < 60)
                                        {
                                            lineheight = lineheight + 16;
                                        }
                                        else if(itm.Length < 100)
                                        {
                                            lineheight = lineheight + 25;
                                        }
                                        else
                                        {
                                            lineheight = lineheight + 45;
                                        }
                                        
                                    }
                                }

                            }
                            lineheight = lineheight + 20;
                            graph.DrawString("BAYARAN", font, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", font, XBrushes.Black, new XRect(250, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.ProcessingFee != null)
                            {
                                var mval = string.Format("{0:0.00}", item.ProcessingFee);
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
                            foreach (var item3 in ctx.PALinkInd.Where(x => x.PremiseApplicationID == appId))
                            {
                                if (Convert.ToInt32(item3.IndividualID) > 0)
                                {
                                    foreach (var item4 in ctx.Individuals.Where(x => x.IndividualID == item3.IndividualID))
                                    {
                                        {
                                            if (item4.FullName != null)
                                            {
                                                XPen pen1 = new XPen(XColors.Black, 1);
                                                System.Drawing.Point pt6 = new System.Drawing.Point(20, lineheight);
                                                System.Drawing.Point pt7 = new System.Drawing.Point(150, lineheight);
                                                graph2.DrawLine(lineRed, pt6, pt7);
                                                lineheight = lineheight + 5;
                                                graph2.DrawString(item4.FullName, font, XBrushes.Black, new XRect(30, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                                                lineheight = lineheight + 35;
                                            }
                                        }
                                    }
                                }
                            }

                            graph2.DrawString("s.k  Penolong Pengarah", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            List<string> lstDept = new List<string>();
                            foreach (var item1 in ctx.PALinkBC.Where(x => x.PremiseApplicationID == appId))
                            {
                                foreach (var item2 in ctx.BCLinkDeps.Where(x => x.BusinessCodeID == item1.BusinessCodeID))
                                {
                                    if (Convert.ToInt32(item2.DepartmentID) > 0)
                                    {
                                        foreach (var item3 in ctx.Departments.Where(x => x.DepartmentID == item2.DepartmentID))
                                        {
                                            if (item3.DepartmentDesc != null)
                                            {
                                                if(!lstDept.Contains(item3.DepartmentDesc))
                                                { 
                                                    graph2.DrawString(" - " + item3.DepartmentDesc, font, XBrushes.Black, new XRect(40, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                                                    cnt = cnt + 1;
                                                    lineheight = lineheight + 15;
                                                    lstDept.Add(item3.DepartmentDesc);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
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
                            if(item.ReferenceNo != null)
                            {
                                graph2.DrawString(item.ReferenceNo, font, XBrushes.Black, new XRect(300, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
                            }
                            //graph2.DrawString(DateTime.Now.Year.ToString() + "/P/00000" + appId, font, XBrushes.Black, new XRect(300, lineheight, pdfPage2.Width.Point, pdfPage2.Height.Point), XStringFormats.TopLeft);
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
                            MemoryStream  strm= new MemoryStream();
                            pdf.Save(strm,false);
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

        #region Delete PremiseApplication
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
                var premiseApplication = new PremiseApplication() { PremiseApplicationID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(premiseApplication).State = System.Data.Entity.EntityState.Deleted;
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

        #region Get Business Code data for Datatable
        /// <summary>
        /// Get Business Code
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="selectedMode">The selected mode.</param>
        /// <param name="selectedSector">The selected sector.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillBusinessCode(string query, int selectedMode, int selectedSector)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BusinessCode> primaryQuery = ctx.BusinessCodes;
                
                if (selectedSector > 0)
                {
                    primaryQuery = primaryQuery.Where(bc => bc.SectorID == selectedSector);
                }
                if (!String.IsNullOrWhiteSpace(query))
                {
                    primaryQuery = primaryQuery.Where(bc => bc.CodeDesc.ToLower().Contains(query.ToLower()) || bc.CodeNumber.ToLower().Contains(query.ToLower()));
                }
                var businessCode = primaryQuery.Select(fnSelectBusinessCode).ToList();
                return Json(businessCode, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Get Individual Name (MyKad) for Datatable
        /// <summary>
        /// Get Individual Code
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
        #endregion

        #region Get MyKad data for Datatable
        /// <summary>
        /// get Mykad Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="individualids">The individualids.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Mykad([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string individualids)
        {
            List<IndividualModel> individual;
            int totalRecord = 0;

            //todo: what if individualids == null ?
            using (var ctx = new LicenseApplicationContext())
            {
                List<int> individuallist = individualids.ToIntList();

                IQueryable<Individual> query = ctx.Individuals.Where(r => individuallist.Contains(r.IndividualID));

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<IndividualModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "IndividualID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                individual = result;

            }
            return Json(new DataTablesResponse(requestModel.Draw, individual, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get Additional Doc data for Datatable
        /// <summary>
        /// get Additional Document Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="businessCodeids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AdditionalDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessCodeids, string premiseApplicationId)
        {
            List<BCLinkADModel> requiredDocument = new List<BCLinkADModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                string[] ids = null;

                if (!string.IsNullOrWhiteSpace(businessCodeids))
                {
                    ids = businessCodeids.Split(',');
                }

                List<int> businessCodelist = new List<int>();

                if (ids != null)
                {
                    foreach (string id in ids)
                    {
                        int businessCodeId = Convert.ToInt32(id);
                        businessCodelist.Add(businessCodeId);
                    }
                }

                IQueryable<BCLinkAD> query = ctx.BCLinkAD.Where(p => businessCodelist.Contains(p.BusinessCodeID));

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<BCLinkADModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BCLinkADID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                requiredDocument = result;

                #region IsChecked

                if (!string.IsNullOrWhiteSpace(premiseApplicationId))
                {
                    int premiseAppId;
                    int.TryParse(premiseApplicationId, out premiseAppId);

                    var palinkAdd = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == premiseAppId).ToList();
                    foreach (var item in requiredDocument)
                    {
                        if (palinkAdd.Count > 0)
                        {
                            var resultpalinkReq = palinkAdd.FirstOrDefault(p => p.AdditionalDocID == item.AdditionalDocID && p.PremiseApplicationID == premiseAppId);
                            if (resultpalinkReq != null)
                            {
                                item.IsChecked = "checked";
                                var attechmentdetails = ctx.Attachments.FirstOrDefault(a => a.AttachmentID == resultpalinkReq.AttachmentID);
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
            return Json(new DataTablesResponse(requestModel.Draw, requiredDocument, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Save Attachment data and Upload Files
        /// <summary>
        /// Save Attachment Information
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadDocument(HttpPostedFileBase documentFile)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    if (documentFile != null)
                    {
                        var file = documentFile;
                        if (file.ContentLength > 0)
                        {
                            var premisevalue = Request["PremiseApplicationID"];
                            var reqDocvalue = Request["reqDocid"];
                            var addDocvalue = Request["addDocid"];
                            var isReqvalue = Request["isReqDoc"];

                            int premiseApplicationId;
                            if (int.TryParse(premisevalue, out premiseApplicationId) && premiseApplicationId > 0)
                            {
                                int requiredDocId;
                                int.TryParse(reqDocvalue, out requiredDocId);

                                int additionalDocId;
                                int.TryParse(addDocvalue, out additionalDocId);

                                if (requiredDocId > 0 || additionalDocId > 0)
                                {
                                    int isReq;
                                    int.TryParse(isReqvalue, out isReq);

                                    var fileName = Path.GetFileName(file.FileName);

                                    var folder = Server.MapPath("~/Documents/Attachment/PremiseApplication/" + premiseApplicationId.ToString());
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
                                            PALinkReqDoc paLinkReqDoc;
                                            paLinkReqDoc = ctx.PALinkReqDoc.FirstOrDefault(p => p.PremiseApplicationID == premiseApplicationId && p.RequiredDocID == requiredDocId);
                                            if (paLinkReqDoc != null)
                                            {
                                                paLinkReqDoc.AttachmentID = attachment.AttachmentID;
                                                ctx.PALinkReqDoc.AddOrUpdate(paLinkReqDoc);
                                                ctx.SaveChanges();
                                            }
                                            else
                                            {
                                                PALinkReqDoc paLinkReqDocument = new PALinkReqDoc();
                                                paLinkReqDocument.PremiseApplicationID = premiseApplicationId;
                                                paLinkReqDocument.RequiredDocID = requiredDocId;
                                                paLinkReqDocument.AttachmentID = attachment.AttachmentID;
                                                ctx.PALinkReqDoc.AddOrUpdate(paLinkReqDocument);
                                                ctx.SaveChanges();
                                            }
                                        }
                                        else
                                        {
                                            PALinkAddDoc paLinkAddDoc;
                                            paLinkAddDoc = ctx.PALinkAddDocs.FirstOrDefault(p => p.PremiseApplicationID == premiseApplicationId && p.AdditionalDocID == additionalDocId);
                                            if (paLinkAddDoc != null)
                                            {
                                                paLinkAddDoc.AttachmentID = attachment.AttachmentID;
                                                ctx.PALinkAddDocs.AddOrUpdate(paLinkAddDoc);
                                                ctx.SaveChanges();
                                            }
                                            else
                                            {
                                                PALinkAddDoc paLinkAddDocument = new PALinkAddDoc();
                                                paLinkAddDocument.PremiseApplicationID = premiseApplicationId;
                                                paLinkAddDocument.AdditionalDocID = additionalDocId;
                                                paLinkAddDocument.AttachmentID = attachment.AttachmentID;
                                                ctx.PALinkAddDocs.AddOrUpdate(paLinkAddDocument);
                                                ctx.SaveChanges();
                                            }
                                        }

                                        return Json(new { status = "1", message = "Document Upload Successfully" }, JsonRequestBehavior.AllowGet);
                                    }

                                    return Json(new { status = "2", message = "Error While Saving Record" }, JsonRequestBehavior.AllowGet);
                                }

                                return Json(new { status = "2", message = "Data Missing" }, JsonRequestBehavior.AllowGet);
                            }

                            return Json(new { status = "2", message = "Data Missing" }, JsonRequestBehavior.AllowGet);
                        }

                        return Json(new { status = "2", message = "Please select File" }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { status = "2", message = "Please select File" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Save Attachment/ UploadAttechment
        /// <summary>
        /// Save Attachment Information
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UploadAttechment(HttpPostedFileBase documentFile)
        {
            //todo: this method is hard to understasnd, a lot of commented code
            //todo and too similar with method UploadDocument
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    if (documentFile != null)
                    {
                        var file = documentFile;
                        if (file.ContentLength > 0)
                        {
                            var reqDocvalue = Request["reqDocid"];
                            var addDocvalue = Request["addDocid"];
                            var isReqvalue = Request["isReqDoc"];

                            int requiredDocId;
                            int.TryParse(reqDocvalue, out requiredDocId);

                            int additionalDocId;
                            int.TryParse(addDocvalue, out additionalDocId);

                            if (requiredDocId > 0 || additionalDocId > 0)
                            {
                                int isReq;
                                int.TryParse(isReqvalue, out isReq);

                                var fileName = Path.GetFileName(file.FileName);

                                var folder = Server.MapPath(ProjectConfiguration.AttachmentDocument);
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

                                        return Json(new { status = "1", result = new { status = "1", RequiredDocID = requiredDocId, AttachmentID = attachment.AttachmentID, AttachmentName = attachment.FileName } }, JsonRequestBehavior.AllowGet);
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

                                        return Json(new { status = "1", result = new { status = "1", AdditionalDocID = additionalDocId, AttachmentID = attachment.AttachmentID, AttachmentName = attachment.FileName } }, JsonRequestBehavior.AllowGet);
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
            catch
            {
                return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Save Comments
        /// <summary>
        /// Save PAComment
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
                    var premiseId = Request["PremiseApplicationID"];
                    var comment = Request["comment"];
                    var approveRejectType = Request["approveRejectType"];  // 1) Approve  2) Reject 

                    int premiseApplicationId;
                    int.TryParse(premiseId, out premiseApplicationId);

                    int usersId = 0;
                    int userroleTemplate = 0;
                    if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
                    {
                        usersId = ProjectSession.User.UsersID;
                        userroleTemplate = ProjectSession.User.RoleTemplateID.Value;
                    }

                    if (premiseApplicationId > 0 && usersId > 0 && userroleTemplate > 0)
                    {
                        PAComment paComment = new PAComment();
                        paComment.Comment = comment;
                        paComment.PremiseApplicationID = premiseApplicationId;
                        paComment.UsersID = usersId;
                        paComment.CommentDate = DateTime.Now;
                        ctx.PAComments.AddOrUpdate(paComment);
                        ctx.SaveChanges();

                        if (paComment.PACommentID > 0)
                        {
                            var premiseApplication = ctx.PremiseApplications.FirstOrDefault(p => p.PremiseApplicationID == premiseApplicationId);
                            var paLinkBc = ctx.PALinkBC.Where(t => t.PremiseApplicationID == premiseApplicationId).ToList();

                            if (userroleTemplate == (int)RollTemplate.Clerk)
                            {
                                if (approveRejectType == "Approve")
                                {
                                    var paLinkBusinessCode = ctx.PALinkBC.Where(t => t.PremiseApplicationID == premiseApplicationId && t.BusinessCode != null).ToList();
                                    if (paLinkBusinessCode.Count > 0)
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
                            //todo: what is it for ?
                            if (userroleTemplate == (int)RollTemplate.RouteUnit)
                            {

                            }

                            if (userroleTemplate == (int)RollTemplate.Supervisor)
                            {

                            }

                            if (userroleTemplate == (int)RollTemplate.Director)
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
            catch
            {
                return Json(new { status = "3", message = "Something went wrong, Please try again" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #endregion

        //TODO: Move all business code  logic to BusinessCodeController or at least a BusinessCodeService

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
        /// <param name="requestModel">The request model.</param>
        /// <param name="codeNumber">The code number.</param>
        /// <param name="codeDesc">The code desc.</param>
        /// <param name="sectorId">The sector identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string codeNumber, string codeDesc, string sectorId)
        {
            List<BusinessCodeModel> businessCode;
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

                if (!string.IsNullOrWhiteSpace(sectorId))
                {
                    query = query.Where(p => p.SectorID.ToString().Contains(sectorId));
                }

                // Filter End

                #endregion Filtering

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<BusinessCodeModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BusinessCodeID asc" : orderByString).ToList();

                totalRecord = result.Count;
                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();

                businessCode = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, businessCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get BusinessCode Data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ManageBusinessCode(int? id)
        {
            BusinessCodeModel businessCodeModel = new BusinessCodeModel
            {
                Active = true
            };
            if (id != null && id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int businessCodeId = Convert.ToInt32(id);
                    var businessCode = ctx.BusinessCodes.FirstOrDefault(a => a.BusinessCodeID == businessCodeId);
                    businessCodeModel = Mapper.Map<BusinessCodeModel>(businessCode);

                    var additionalDocs = ctx.BCLinkAD.Where(blAd => blAd.BusinessCodeID == businessCodeId);
                    businessCodeModel.AdditionalDocs = additionalDocs.Any()
                        ? additionalDocs.Select(blAd => blAd.AdditionalDocID).ToList()
                        : new List<int>();

                    var departments = ctx.BCLinkDeps.Where(blD => blD.BusinessCodeID == businessCodeId);
                    if (departments.Any())
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
                            if (selectedDeps.All(sd => sd.DepartmentID != depId))
                            {
                                selectedDepartments.Add(new BCLinkDep { BusinessCodeID = businessCode.BusinessCodeID, DepartmentID = depId });
                            }
                        }
                        if (selectedDeps.Count > 0)
                        {
                            foreach (var bcDep in selectedDeps)
                            {
                                if (deptIds.All(rd => rd != bcDep.DepartmentID.ToString()))
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
                            if (selectedADocs.All(sd => sd.AdditionalDocID != addDocId))
                            {
                                selectedAdditionalDocs.Add(new BCLinkAD { BusinessCodeID = businessCode.BusinessCodeID, AdditionalDocID = addDocId });
                            }
                        }
                        if (selectedADocs.Count > 0)
                        {
                            foreach (var bcDep in selectedADocs)
                            {
                                if (addDocIds.All(rd => rd != bcDep.AdditionalDocID))
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
                var businessCode = new BusinessCode() { BusinessCodeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(businessCode).State = System.Data.Entity.EntityState.Deleted;
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