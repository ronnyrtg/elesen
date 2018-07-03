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
using TradingLicense.Web.Helpers;
using TradingLicense.Infrastructure;
using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using System.IO;
using PdfSharp.Pdf;

namespace TradingLicense.Web.Controllers
{
    public class HawkerApplicationController : BaseController
    {
        #region HawkerCode

        /// <summary>
        /// GET: HawkerCode
        /// </summary>
        /// <returns></returns>
        public ActionResult HawkerCode()
        {
            return View();
        }

        /// <summary>
        /// Save Hawker Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HawkerCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string hawkerCodeDesc)
        {
            List<TradingLicense.Model.HawkerCodeModel> hawkerCode = new List<Model.HawkerCodeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<HawkerCode> query = ctx.HawkerCodes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(hawkerCodeDesc))
                {
                    query = query.Where(p =>
                                        p.HawkerCodeDesc.Contains(hawkerCodeDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "HawkerCodeID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                hawkerCode = Mapper.Map<List<HawkerCodeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, hawkerCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get HawkerCode Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageHawkerCode(int? Id)
        {
            HawkerCodeModel hawkerCodeModel = new HawkerCodeModel();
            hawkerCodeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int hawkerCodeID = Convert.ToInt32(Id);
                    var hawkerCode = ctx.HawkerCodes.Where(a => a.HawkerCodeID == hawkerCodeID).FirstOrDefault();
                    hawkerCodeModel = Mapper.Map<HawkerCodeModel>(hawkerCode);
                }
            }

            return View(hawkerCodeModel);
        }

        /// <summary>
        /// Save Hawker Code Infomration
        /// </summary>
        /// <param name="hawkerCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageHawkerCode(HawkerCodeModel hawkerCodeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    HawkerCode hawkerCode;
                    if (IsHawkerCodeDuplicate(hawkerCodeModel.HawkerCodeDesc, hawkerCodeModel.HawkerCodeID))
                    {
                        TempData["ErrorMessage"] = "Hawker Code already exists in the database.";
                        return View(hawkerCodeModel);
                    }

                    hawkerCode = Mapper.Map<HawkerCode>(hawkerCodeModel);
                    ctx.HawkerCodes.AddOrUpdate(hawkerCode);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Hawker Code saved successfully.";

                return RedirectToAction("HawkerCode");
            }
            else
            {
                return View(hawkerCodeModel);
            }

        }

        /// <summary>
        /// Delete Hawker Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteHawkerCode(int id)
        {
            try
            {
                var hawkerCode = new TradingLicense.Entities.HawkerCode() { HawkerCodeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(hawkerCode).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsHawkerCodeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.HawkerCodes.FirstOrDefault(
                   c => c.HawkerCodeID != id && c.HawkerCodeDesc.ToLower() == name.ToLower())
               : ctx.HawkerCodes.FirstOrDefault(
                   c => c.HawkerCodeDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region HawkerApplication

        /// <summary>
        /// GET: HawkerApplication
        /// </summary>
        /// <returns></returns>
        public ActionResult HawkerApplication()
        {
            return View();
        }

        /// <summary>
        /// Save Hawker Application Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HawkerApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string HawkerApplicationID)
        {
            List<TradingLicense.Model.HawkerApplicationModel> HawkerApplication = new List<Model.HawkerApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<HawkerApplication> query = ctx.HawkerApplications;
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

                query = query.OrderBy(orderByString == string.Empty ? "HawkerApplicationID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                HawkerApplication = Mapper.Map<List<HawkerApplicationModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, HawkerApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        private Func<Individual, Select2ListItem> fnSelectIndividualFormat = ind => new Select2ListItem { id = ind.IndividualID, text = $"{ind.FullName} ({ind.MykadNo})" };

        /// <summary>
        /// Get HawkerApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageHawkerApplication(int? Id)
        {
            HawkerApplicationModel hawkerApplicationModel = new HawkerApplicationModel();
            hawkerApplicationModel.ValidStart = DateTime.Today;
            hawkerApplicationModel.ValidStop = DateTime.Today;
            List<HAReqDocModel> HAReqDoc = new List<HAReqDocModel>();
            List<HALinkReqDoc> HALinkReqDoc = new List<HALinkReqDoc>();
            List<Attchments> Attchments = new List<Attchments>();

            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<HAReqDoc> query = ctx.HAReqDocs;
                HAReqDoc = Mapper.Map<List<HAReqDocModel>>(query.ToList());
                ViewBag.hawkerDocList = ctx.HAReqDocs.ToList();

                var haLinkInd = ctx.HawkerApplications.Where(a => a.HawkerApplicationID == Id).ToList();
                hawkerApplicationModel.Individualids = string.Join(",", haLinkInd.Select(x => x.IndividualID.ToString()).ToArray());
                List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
                var iids = haLinkInd.Select(b => b.IndividualID).ToList();
                selectedIndividualList = ctx.Individuals
                    .Where(b => iids.Contains(b.IndividualID))
                    .Select(fnSelectIndividualFormat)
                    .ToList();

                hawkerApplicationModel.selectedIndividualList = selectedIndividualList;

                if (Id != null && Id > 0)
                {
                    

                    int HawkerApplicationID = Convert.ToInt32(Id);
                    var HawkerApplication = ctx.HawkerApplications.Where(a => a.HawkerApplicationID == HawkerApplicationID).FirstOrDefault();
                    hawkerApplicationModel = Mapper.Map<HawkerApplicationModel>(HawkerApplication);
                }

            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                hawkerApplicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
                hawkerApplicationModel.UsersID = ProjectSession.User.UsersID;
            }

            ViewBag.HALinkReqDoc = Attchments;
            ViewBag.UserRole = ProjectSession.User.RoleTemplateID;
            ViewBag.HawkerApplicationID = Convert.ToInt32(Id);
            return View(hawkerApplicationModel);
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
                var individual = ctx.Individuals
                                    .Where(t => t.MykadNo.ToLower().Contains(query.ToLower()) || t.FullName.ToLower().Contains(query.ToLower()))
                                    .Select(fnSelectIndividualFormat).ToList();
                return Json(individual, JsonRequestBehavior.AllowGet);
            }
        }

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

        /// <summary>
        /// Save Hawker Application Infomration
        /// </summary>
        /// <param name="HawkerApplicationModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageHawkerApplication(HawkerApplicationModel HawkerApplicationModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    HawkerApplication HawkerApplication;


                    HawkerApplication = Mapper.Map<HawkerApplication>(HawkerApplicationModel);
                    ctx.HawkerApplications.AddOrUpdate(HawkerApplication);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Hawker Application saved successfully.";

                return RedirectToAction("HawkerApplication");
            }
            else
            {
                return View(HawkerApplicationModel);
            }

        }

        /// <summary>
        /// Delete Hawker Application Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteHawkerApplication(int id)
        {
            try
            {
                var HawkerApplication = new TradingLicense.Entities.HawkerApplication() { HawkerApplicationID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(HawkerApplication).State = System.Data.Entity.EntityState.Deleted;
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

        #region HAReqDoc

        /// <summary>
        /// GET: HAReqDoc
        /// </summary>
        /// <returns></returns>
        public ActionResult HAReqDoc()
        {
            return View();
        }

        /// <summary>
        /// Save Hawker Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HAReqDoc([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string HAReqDocDesc)
        {
            List<TradingLicense.Model.HAReqDocModel> HAReqDoc = new List<Model.HAReqDocModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<HAReqDoc> query = ctx.HAReqDocs;
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

                query = query.OrderBy(orderByString == string.Empty ? "HAReqDocID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                HAReqDoc = Mapper.Map<List<HAReqDocModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, HAReqDoc, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get HAReqDoc Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageHAReqDoc(int? Id)
        {
            HAReqDocModel HAReqDocModel = new HAReqDocModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int HAReqDocID = Convert.ToInt32(Id);
                    var HAReqDoc = ctx.HAReqDocs.Where(a => a.HAReqDocID == HAReqDocID).FirstOrDefault();
                    HAReqDocModel = Mapper.Map<HAReqDocModel>(HAReqDoc);
                }
            }

            return View(HAReqDocModel);
        }

        [HttpPost]
        public JsonResult SaveHAReqDoc(List<HAReqDoc> lstBarReqDoc)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    foreach (var item in lstBarReqDoc)
                    {
                        var DocCnt = ctx.HAReqDocs.Where(x => x.RequiredDocID == item.RequiredDocID).Count();
                        if (DocCnt == 0)
                        {
                            HAReqDoc HAReqDoc = new HAReqDoc();
                            HAReqDoc.HAReqDocID = 0;
                            HAReqDoc.RequiredDocID = item.RequiredDocID;
                            ctx.HAReqDocs.AddOrUpdate(HAReqDoc);
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

        /// <summary>
        /// Delete Hawker Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteHAReqDoc(int id)
        {
            try
            {
                var HAReqDoc = new TradingLicense.Entities.HAReqDoc() { HAReqDocID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(HAReqDoc).State = System.Data.Entity.EntityState.Deleted;
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

        #region Generate License PDF
        public ActionResult GeneratLicense(Int32? appId)
        {
            PremiseApplicationModel premiseApplicationModel = new PremiseApplicationModel();
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var StallApp = ctx.HawkerApplications
                                        .Where(x => x.HawkerApplicationID == appId).ToList();
                    if (StallApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid Hawker ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in StallApp)
                        {
                            int lineheight = 20;
                            PdfDocument pdf = new PdfDocument();
                            pdf.Info.Title = "PDF License";
                            PdfPage pdfPage = pdf.AddPage();
                            XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                            XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                            XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                            XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                            XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                            XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);

                            XImage xImage1 = XImage.FromFile(Server.MapPath("~\\images\\logoPL.png"));
                            graph.DrawImage(xImage1, 30, 50, 100, 75);

                            graph.DrawString("PERBADANAN LABUAN", lbfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Wisma Perbadanan Labuan", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Peti Surat 81245", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("87022 Willayah Persekutuan Labuan", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Tel No 		", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(": 087-408692/596", nfont, XBrushes.Black, new XRect(205, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("Faks No    ", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(": 087-408348", nfont, XBrushes.Black, new XRect(205, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("WEBSITE  ", nfont, XBrushes.Black, new XRect(135, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(": www.pl.gov.my", nfont, XBrushes.Black, new XRect(205, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 25;
                            XPen lineRed1 = new XPen(XColors.Black, 0.5);
                            System.Drawing.Point pt1 = new System.Drawing.Point(30, lineheight);
                            System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 30, lineheight);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 10;
                            graph.DrawString("LESEN", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 15;
                            graph.DrawString("UNDANG-UNDANG KECIL PELESENAN PENJAJA(WILAYAH PERSEKUTUAN LABUAN) 2016", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 15;
                            
                            lineheight = lineheight + 15;
                            XTextFormatter tf = new XTextFormatter(graph);
                            XRect rect = new XRect(450, lineheight, 100, 100);
                            graph.DrawRectangle(lineRed1, rect);

                            var individualUploadPath = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument), "Individual");
                            var individualActualPath = Path.Combine(individualUploadPath, item.IndividualID.ToString("D6"));

                            var IndItm = ctx.Individuals.Where(x => x.IndividualID == item.IndividualID).ToList();
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].Attachment != null && IndItm[0].Attachment.FileName != null)
                                {
                                    var individualActualPath1 = Path.Combine(individualActualPath, IndItm[0].Attachment.FileName);
                                    if (System.IO.File.Exists(individualActualPath1))
                                    {
                                        xImage1 = XImage.FromFile(individualActualPath1);
                                        graph.DrawImage(xImage1,450 , lineheight, 100, 100);
                                    }
                                }
                            }
                            graph.DrawString("NO.LESEN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.ReferenceNo != null)
                            {
                                graph.DrawString(item.ReferenceNo, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            
                            lineheight = lineheight + 15;
                            graph.DrawString("Nama Pemilik", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if(item.Individual !=null && item.Individual.FullName != null )
                            {
                                graph.DrawString(item.Individual.FullName, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            
                            lineheight = lineheight + 15;
                            graph.DrawString("NO.K/P", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.Individual != null && item.Individual.MykadNo != null)
                            {
                                graph.DrawString(item.Individual.MykadNo, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            if (item.Individual != null && item.Individual.AddressIC != null)
                            {
                                if (item.Individual.AddressIC.ToString().Length > 55)
                                {
                                    graph.DrawString(item.Individual.AddressIC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    lineheight = lineheight + 15;
                                    graph.DrawString(item.Individual.AddressIC.Substring(55, item.Individual.AddressIC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                else
                                {
                                    graph.DrawString(item.Individual.AddressIC, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("JENIS LESEN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.HawkerCode != null && item.HawkerCode.HawkerCodeDesc != null)
                            {
                                graph.DrawString(item.HawkerCode.HawkerCodeDesc, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("TEMPOH SAH LESEN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            string HW = item.HawkerCode.PeriodQuantity.ToString();
                            if(item.HawkerCode != null && item.HawkerCode.Period == 1)
                            {
                                HW = HW + " " + "Tahun";
                            }
                            else if(item.HawkerCode != null &&  item.HawkerCode.Period == 2)
                            {
                                HW = HW + " " + "Bulan";
                            }
                            else if (item.HawkerCode != null &&  item.HawkerCode.Period == 3)
                            {
                                HW = HW + " " + "Minggu";
                            }
                            else if (item.HawkerCode != null &&  item.HawkerCode.Period == 4)
                            {
                                HW = HW + " " + "Hari";
                            }
                            graph.DrawString(HW, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("JENIS BARANG DIJAJA", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.GoodsType != null)
                            {
                                graph.DrawString(item.GoodsType, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("MASA PERNIAGAAN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.OperationHours != null)
                            {
                                graph.DrawString(item.OperationHours, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("LOKASI PERNIAGAAN", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.HawkerLocation != null)
                            {
                                graph.DrawString(item.HawkerLocation, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }

                            lineheight = lineheight + 20;

                            tf = new XTextFormatter(graph);
                            rect = new XRect(450, lineheight, 100, 100);
                            graph.DrawRectangle(lineRed1, rect);
                            IndItm = ctx.Individuals.Where(x => x.IndividualID == item.HelperA).ToList();
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].Attachment != null && IndItm[0].Attachment.FileName != null)
                                {
                                    individualActualPath = Path.Combine(individualUploadPath, IndItm[0].IndividualID.ToString("D6"));
                                    var individualActualPath2 = Path.Combine(individualActualPath, IndItm[0].Attachment.FileName);
                                    if (System.IO.File.Exists(individualActualPath2))
                                    {
                                        xImage1 = XImage.FromFile(individualActualPath2);
                                        graph.DrawImage(xImage1, 450, lineheight, 100, 100);
                                    }
                                }
                            }
                            lineheight = lineheight + 20;
                            graph.DrawString("NAMA PEMBANTU", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].FullName != null)
                                {
                                    graph.DrawString(IndItm[0].FullName, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("NO.KP", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].MykadNo != null)
                                {
                                    graph.DrawString(IndItm[0].MykadNo, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].AddressIC != null)
                                {
                                    if (IndItm[0].AddressIC.ToString().Length > 55)
                                    {
                                        graph.DrawString(IndItm[0].AddressIC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        lineheight = lineheight + 15;
                                        graph.DrawString(IndItm[0].AddressIC.Substring(55, IndItm[0].AddressIC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                    else
                                    {
                                        graph.DrawString(IndItm[0].AddressIC, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                            }

                            lineheight = lineheight + 90;
                            tf = new XTextFormatter(graph);
                            rect = new XRect(450, lineheight, 100, 100);
                            graph.DrawRectangle(lineRed1, rect);
                            if (item.HelperB == null)
                            {
                                IndItm = ctx.Individuals.Where(x => x.IndividualID == 0).ToList();
                            }
                            else
                            {
                                IndItm = ctx.Individuals.Where(x => x.IndividualID == item.HelperB).ToList();
                            }
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].Attachment != null && IndItm[0].Attachment.FileName != null)
                                {
                                    individualActualPath = Path.Combine(individualUploadPath, IndItm[0].IndividualID.ToString("D6"));
                                    var individualActualPath3 = Path.Combine(individualActualPath, IndItm[0].Attachment.FileName);
                                    if (System.IO.File.Exists(individualActualPath3))
                                    {
                                        xImage1 = XImage.FromFile(individualActualPath3);
                                        graph.DrawImage(xImage1, 450, lineheight, 100, 100);
                                    }
                                }
                            }

                            lineheight = lineheight + 15;
                            graph.DrawString("NAMA PEMBANTU", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].FullName != null)
                                {
                                    graph.DrawString(IndItm[0].FullName, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("NO.KP", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].MykadNo != null)
                                {
                                    graph.DrawString(IndItm[0].MykadNo, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].AddressIC != null)
                                {
                                    if (IndItm[0].AddressIC.ToString().Length > 55)
                                    {
                                        graph.DrawString(IndItm[0].AddressIC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        lineheight = lineheight + 15;
                                        graph.DrawString(IndItm[0].AddressIC.Substring(55, IndItm[0].AddressIC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                    else
                                    {
                                        graph.DrawString(IndItm[0].AddressIC, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                            }

                            lineheight = lineheight + 80;
                            tf = new XTextFormatter(graph);
                            rect = new XRect(450, lineheight, 100, 100);
                            graph.DrawRectangle(lineRed1, rect);
                            if (item.HelperC == null)
                            {
                                IndItm = ctx.Individuals.Where(x => x.IndividualID == 0).ToList();
                            }
                            else
                            {
                                IndItm = ctx.Individuals.Where(x => x.IndividualID == item.HelperC).ToList();
                            }
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].Attachment != null && IndItm[0].Attachment.FileName != null)
                                {
                                    individualActualPath = Path.Combine(individualUploadPath, IndItm[0].IndividualID.ToString("D6"));
                                    var individualActualPath3 = Path.Combine(individualActualPath, IndItm[0].Attachment.FileName);
                                    if (System.IO.File.Exists(individualActualPath3))
                                    {
                                        xImage1 = XImage.FromFile(individualActualPath3);
                                        graph.DrawImage(xImage1, 450, lineheight, 100, 100);
                                    }
                                }
                            }

                            lineheight = lineheight + 15;
                            graph.DrawString("NAMA PEMBANTU", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].FullName != null)
                                {
                                    graph.DrawString(IndItm[0].FullName, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("NO.KP", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].MykadNo != null)
                                {
                                    graph.DrawString(IndItm[0].MykadNo, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(145, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].AddressIC != null)
                                {
                                    if (IndItm[0].AddressIC.ToString().Length > 55)
                                    {
                                        graph.DrawString(IndItm[0].AddressIC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        lineheight = lineheight + 15;
                                        graph.DrawString(IndItm[0].AddressIC.Substring(55, IndItm[0].AddressIC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                    else
                                    {
                                        graph.DrawString(IndItm[0].AddressIC, nUfont, XBrushes.Black, new XRect(150, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                            }

                            lineheight = lineheight + 65;
                            System.Drawing.Point pt3 = new System.Drawing.Point(30, lineheight);
                            System.Drawing.Point pt4 = new System.Drawing.Point(170, lineheight);
                            graph.DrawLine(lineRed1, pt3, pt4);
                            lineheight = lineheight + 5;
                            graph.DrawString("b.p.", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("KETUA PEGAWAI EKSEKUTIF", nfont, XBrushes.Black, new XRect(50, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 12;
                            graph.DrawString("PERBADANAN LABUAN", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph.DrawString("Tarikh:", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if(item.DateApproved != null )
                            {
                                graph.DrawString(string.Format("{0:dd MMMM yyyy}", item.DateApproved ), nfont, XBrushes.Black, new XRect(67, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            pt3 = new System.Drawing.Point(65, lineheight + 13);
                            pt4 = new System.Drawing.Point(160, lineheight + 13);
                            graph.DrawLine(lineRed1, pt3, pt4);
                            lineheight = lineheight + 20;
                            graph.DrawString("***LESEN INI HENDAKLAH DIPAMERKAN", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);


                            MemoryStream strm = new MemoryStream();
                            pdf.Save(strm, false);
                            return File(strm, "application/pdf");

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion

        public class Attchments
        {
            public int RequiredDocID { get; set; }
            public int Id { get; set; }
            public string filename { get; set; }
        }
    }
}