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
using PdfSharp.Drawing.Layout;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using iTextSharp.text;

namespace TradingLicense.Web.Controllers
{
    public class StallApplicationController : BaseController
    {
        #region StallCode

        /// <summary>
        /// GET: StallCode
        /// </summary>
        /// <returns></returns>
        public ActionResult StallCode()
        {
            return View();
        }

        /// <summary>
        /// Save Stall Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StallCode([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string stallCodeDesc)
        {
            List<TradingLicense.Model.StallCodeModel> stallCode = new List<Model.StallCodeModel>();
            int totalRecord = 0;
            int filteredRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<StallCode> query = ctx.StallCodes;
                totalRecord = query.Count();

                #region Filtering
                // Apply filters for searching

                if (!string.IsNullOrWhiteSpace(stallCodeDesc))
                {
                    query = query.Where(p =>
                                        p.StallCodeDesc.Contains(stallCodeDesc)
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

                query = query.OrderBy(orderByString == string.Empty ? "StallCodeID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                stallCode = Mapper.Map<List<StallCodeModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, stallCode, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get StallCode Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageStallCode(int? Id)
        {
            StallCodeModel stallCodeModel = new StallCodeModel();
            stallCodeModel.Active = true;
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int stallCodeID = Convert.ToInt32(Id);
                    var stallCode = ctx.StallCodes.Where(a => a.StallCodeID == stallCodeID).FirstOrDefault();
                    stallCodeModel = Mapper.Map<StallCodeModel>(stallCode);
                }
            }

            return View(stallCodeModel);
        }

        /// <summary>
        /// Save Stall Code Infomration
        /// </summary>
        /// <param name="stallCodeModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageStallCode(StallCodeModel stallCodeModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    StallCode stallCode;
                    if (IsStallCodeDuplicate(stallCodeModel.StallCodeDesc, stallCodeModel.StallCodeID))
                    {
                        TempData["ErrorMessage"] = "Stall Code already exists in the database.";
                        return View(stallCodeModel);
                    }

                    stallCode = Mapper.Map<StallCode>(stallCodeModel);
                    ctx.StallCodes.AddOrUpdate(stallCode);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Stall Code saved successfully.";

                return RedirectToAction("StallCode");
            }
            else
            {
                return View(stallCodeModel);
            }

        }

        /// <summary>
        /// Delete Stall Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteStallCode(int id)
        {
            try
            {
                var stallCode = new TradingLicense.Entities.StallCode() { StallCodeID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(stallCode).State = System.Data.Entity.EntityState.Deleted;
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
        private bool IsStallCodeDuplicate(string name, int? id = null)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var existObj = id != null ?
               ctx.StallCodes.FirstOrDefault(
                   c => c.StallCodeID != id && c.StallCodeDesc.ToLower() == name.ToLower())
               : ctx.StallCodes.FirstOrDefault(
                   c => c.StallCodeDesc.ToLower() == name.ToLower());
                return existObj != null;
            }
        }

        #endregion

        #region StallApplication

        /// <summary>
        /// GET: StallApplication
        /// </summary>
        /// <returns></returns>
        public ActionResult StallApplication()
        {
            return View();
        }

        /// <summary>
        /// Save Stall Application Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult StallApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string stallApplicationID)
        {
            List<TradingLicense.Model.StallApplicationModel> stallApplication = new List<Model.StallApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<StallApplication> query = ctx.StallApplications;
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

                query = query.OrderBy(orderByString == string.Empty ? "StallApplicationID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                stallApplication = Mapper.Map<List<StallApplicationModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, stallApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get StallApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageStallApplication(int? Id)
        {
            List<TradingLicense.Model.SAReqDocModel> SAReqDoc = new List<Model.SAReqDocModel>();
            StallApplicationModel stallApplicationModel = new StallApplicationModel();
            stallApplicationModel.ValidStart = DateTime.Today;
            stallApplicationModel.ValidStop = DateTime.Today;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<SAReqDoc> query = ctx.SAReqDocs;
                SAReqDoc = Mapper.Map<List<SAReqDocModel>>(query.ToList());
                ViewBag.stallDocList = ctx.SAReqDocs.ToList();
                if (Id != null && Id > 0)
                {

                    int stallApplicationID = Convert.ToInt32(Id);
                    var stallApplication = ctx.StallApplications.Where(a => a.StallApplicationID == stallApplicationID).FirstOrDefault();
                    stallApplicationModel = Mapper.Map<StallApplicationModel>(stallApplication);
                }

            }
            if (Id != null && Id > 0)
            {
                List<HAReqDocModel> HAReqDoc = new List<HAReqDocModel>();
                using (var ctx = new LicenseApplicationContext())
                {
                    var haLinkInd = ctx.StallApplications.Where(a => a.StallApplicationID == Id).ToList();
                    stallApplicationModel.Individualids = string.Join(",", haLinkInd.Select(x => x.IndividualID.ToString()).ToArray());
                    List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
                    var iids = haLinkInd.Select(b => b.IndividualID).ToList();
                    selectedIndividualList = ctx.Individuals
                        .Where(b => iids.Contains(b.IndividualID))
                        .Select(fnSelectIndividualFormat)
                        .ToList();

                    stallApplicationModel.selectedIndividualList = selectedIndividualList;


                    IQueryable<SAReqDoc> query = ctx.SAReqDocs;
                    SAReqDoc = Mapper.Map<List<SAReqDocModel>>(query.ToList());
                    ViewBag.stallDocList = ctx.SAReqDocs.ToList();
                    if (Id != null && Id > 0)
                    {

                        int StallApplicationID = Convert.ToInt32(Id);
                        var StallApplication = ctx.HawkerApplications.Where(a => a.HawkerApplicationID == StallApplicationID).FirstOrDefault();
                        stallApplicationModel = Mapper.Map<StallApplicationModel>(StallApplication);
                    }

                }
            }
            ViewBag.StallApplicationID = Convert.ToInt32(Id);
            return View(stallApplicationModel);
        }

        /// <summary>
        /// Save Stall Application Infomration
        /// </summary>
        /// <param name="stallApplicationModel"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageStallApplication(StallApplicationModel stallApplicationModel)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    StallApplication stallApplication;


                    stallApplication = Mapper.Map<StallApplication>(stallApplicationModel);
                    ctx.StallApplications.AddOrUpdate(stallApplication);
                    ctx.SaveChanges();
                }

                TempData["SuccessMessage"] = "Stall Application saved successfully.";

                return RedirectToAction("StallApplication");
            }
            else
            {
                return View(stallApplicationModel);
            }

        }

        /// <summary>
        /// Delete Stall Application Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteStallApplication(int id)
        {
            try
            {
                var stallApplication = new TradingLicense.Entities.StallApplication() { StallApplicationID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(stallApplication).State = System.Data.Entity.EntityState.Deleted;
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

        private Func<Individual, Select2ListItem> fnSelectIndividualFormat = ind => new Select2ListItem { id = ind.IndividualID, text = $"{ind.FullName} ({ind.MykadNo})" };

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

        #region SAReqDoc

        /// <summary>
        /// GET: SAReqDoc
        /// </summary>
        /// <returns></returns>
        public ActionResult SAReqDoc()
        {
            return View();
        }

        /// <summary>
        /// Save Stall Code Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SAReqDoc([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string SAReqDocDesc)
        {
            List<TradingLicense.Model.SAReqDocModel> SAReqDoc = new List<Model.SAReqDocModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<SAReqDoc> query = ctx.SAReqDocs;
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

                query = query.OrderBy(orderByString == string.Empty ? "SAReqDocID asc" : orderByString);

                #endregion Sorting

                // Paging
                query = query.Skip(requestModel.Start).Take(requestModel.Length);

                SAReqDoc = Mapper.Map<List<SAReqDocModel>>(query.ToList());

            }
            return Json(new DataTablesResponse(requestModel.Draw, SAReqDoc, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get SAReqDoc Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageSAReqDoc(int? Id)
        {
            SAReqDocModel SAReqDocModel = new SAReqDocModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    int SAReqDocID = Convert.ToInt32(Id);
                    var SAReqDoc = ctx.SAReqDocs.Where(a => a.SAReqDocID == SAReqDocID).FirstOrDefault();
                    SAReqDocModel = Mapper.Map<SAReqDocModel>(SAReqDoc);
                }
            }

            return View(SAReqDocModel);
        }

        [HttpPost]
        public JsonResult SaveSAReqDoc(List<SAReqDoc> lstBarReqDoc)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    foreach (var item in lstBarReqDoc)
                    {
                        var DocCnt = ctx.SAReqDocs.Where(x => x.RequiredDocID == item.RequiredDocID).Count();
                        if (DocCnt == 0)
                        {
                            SAReqDoc SAReqDoc = new SAReqDoc();
                            SAReqDoc.SAReqDocID = 0;
                            SAReqDoc.RequiredDocID = item.RequiredDocID;
                            ctx.SAReqDocs.AddOrUpdate(SAReqDoc);
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
        /// Delete Stall Code Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSAReqDoc(int id)
        {
            try
            {
                var SAReqDoc = new TradingLicense.Entities.SAReqDoc() { SAReqDocID = id };
                using (var ctx = new LicenseApplicationContext())
                {
                    ctx.Entry(SAReqDoc).State = System.Data.Entity.EntityState.Deleted;
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
                    var StallApp = ctx.StallApplications
                                        .Where(x => x.StallApplicationID == appId).ToList();
                    if (StallApp.Count == 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid Stall ApplicationID!');</script>");
                    }
                    else
                    {
                        foreach (var item in StallApp)
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
                            graph.DrawString(": www.pf.gov.my", nfont, XBrushes.Black, new XRect(205, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 25;
                            XPen lineRed1 = new XPen(XColors.Black, 0.5);
                            System.Drawing.Point pt1 = new System.Drawing.Point(30, lineheight);
                            System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width)-30, lineheight);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 10;
                            graph.DrawString("LESEN", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter );
                            lineheight = lineheight + 15;
                            graph.DrawString("UNDANG-UNDANG KECIL PASAR(WP LABUAN) 2016", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter );
                            lineheight = lineheight + 15;
                            graph.DrawString("PEMILIK", nUfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft );
                            lineheight = lineheight + 15;
                            XTextFormatter tf = new XTextFormatter(graph);
                            XRect rect = new XRect(30, lineheight, 100, 100);
                            graph.DrawRectangle(lineRed1, rect);

                            var individualUploadPath = Path.Combine(Server.MapPath(TradingLicense.Infrastructure.ProjectConfiguration.AttachmentDocument), "Individual");
                            var individualActualPath = Path.Combine(individualUploadPath, item.IndividualID.ToString("D6"));

                            var IndItm = ctx.Individuals.Where(x => x.IndividualID == item.IndividualID).ToList(); 
                            if(IndItm != null && IndItm.Count() > 0)
                            {
                                if(IndItm[0].Attachment != null && IndItm[0].Attachment.FileName != null)
                                {
                                   var individualActualPath1 = Path.Combine(individualActualPath, IndItm[0].Attachment.FileName);
                                    if (System.IO.File.Exists(individualActualPath1))
                                    {
                                        xImage1 = XImage.FromFile(individualActualPath1);
                                        graph.DrawImage(xImage1, 30, lineheight, 100, 100);
                                    }
                                }
                            }
                            graph.DrawString("NO.LESEN", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.ReferenceNo != null)
                            {
                                graph.DrawString(item.ReferenceNo, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            graph.DrawString("Bayaran Lesen :", nfont, XBrushes.Black, new XRect(375, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (item.TotalFee != null)
                            {
                                graph.DrawString("RM" + string.Format("{0:0.00}", item.TotalFee) , nUfont, XBrushes.Black, new XRect(450, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("Nama Pemilik", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(item.IndividualID.ToString("D6"), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("NO.K/P", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if(item.Individual != null && item.Individual.MykadNo != null)
                            {
                                graph.DrawString(item.Individual.MykadNo, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            
                        if(item.Individual != null && item.Individual.AddressIC != null)
                            {
                                if(item.Individual.AddressIC.ToString().Length  >55)
                                {
                                    graph.DrawString(item.Individual.AddressIC.Substring(0,55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    lineheight = lineheight + 15;
                                    graph.DrawString(item.Individual.AddressIC.Substring(55, item.Individual.AddressIC.ToString().Length-55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                                else
                                {
                                    graph.DrawString(item.Individual.AddressIC, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 25;
                            graph.DrawString("NO.PREMIS", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if(item.StallLocation != null)
                            {
                                graph.DrawString(item.StallLocation, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("MASA PERNIAGAAN", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            System.Drawing.Point pt3 = new System.Drawing.Point(305, lineheight+13);
                            System.Drawing.Point pt4 = new System.Drawing.Point(410, lineheight+13);
                            graph.DrawLine(lineRed1, pt3, pt4);


                            lineheight = lineheight + 15;
                            graph.DrawString("JENIS JUALAN", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if(item.StallCode != null && item.StallCode.StallCodeDesc != null)
                            {
                                graph.DrawString(item.StallCode.StallCodeDesc, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("TEMPOH SAH LESEN", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if(item.ValidStart != null && item.ValidStop != null)
                            {
                                var strDate=  string.Format("{0:dd MMMM yyyy}", item.ValidStart) + " - " + string.Format("{0:dd MMMM yyyy}", item.ValidStop);
                                graph.DrawString(strDate, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            }
                            lineheight = lineheight + 30;
                            graph.DrawString("PEMBANTU", nUfont, XBrushes.Black, new XRect(30, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            tf = new XTextFormatter(graph);
                            rect = new XRect(30, lineheight, 100, 100);
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
                                        graph.DrawImage(xImage1, 30, lineheight, 100, 100);
                                    }
                                }
                            }
                            lineheight = lineheight + 30;
                            graph.DrawString("NAMA PEMBANTU", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if(IndItm[0].FullName != null)
                                {
                                    graph.DrawString(IndItm[0].FullName, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("NO.KP", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].MykadNo  != null)
                                {
                                    graph.DrawString(IndItm[0].MykadNo, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].AddressIC != null)
                                {
                                    if (IndItm[0].AddressIC.ToString().Length > 55)
                                    {
                                        graph.DrawString(IndItm[0].AddressIC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        lineheight = lineheight + 15;
                                        graph.DrawString(IndItm[0].AddressIC.Substring(55, IndItm[0].AddressIC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                    else
                                    {
                                        graph.DrawString(IndItm[0].AddressIC, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                            }

                            lineheight = lineheight + 90;
                            tf = new XTextFormatter(graph);
                            rect = new XRect(30, lineheight, 100, 100);
                            graph.DrawRectangle(lineRed1, rect);
                            if(item.HelperB == null)
                            {
                                IndItm = ctx.Individuals.Where(x => x.IndividualID == 0).ToList();
                            }
                            else
                            {
                                IndItm = ctx.Individuals.Where(x => x.IndividualID == item.HelperB).ToList();
                            }
                                if (IndItm != null && IndItm.Count() > 0)
                                {
                                    if (IndItm[0].Attachment.FileName != null)
                                    {
                                    individualActualPath = Path.Combine(individualUploadPath, IndItm[0].IndividualID.ToString("D6"));
                                    var individualActualPath3 = Path.Combine(individualActualPath, IndItm[0].Attachment.FileName);
                                        if (System.IO.File.Exists(individualActualPath3))
                                        {
                                            xImage1 = XImage.FromFile(individualActualPath3);
                                            graph.DrawImage(xImage1, 30, lineheight, 100, 100);
                                        }
                                    }
                                }
                            
                            lineheight = lineheight + 30;
                            graph.DrawString("NAMA PEMBANTU", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].FullName != null)
                                {
                                    graph.DrawString(IndItm[0].FullName, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("NO.KP", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].MykadNo != null)
                                {
                                    graph.DrawString(IndItm[0].MykadNo, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                }
                            }
                            lineheight = lineheight + 15;
                            graph.DrawString("ALAMAT", nfont, XBrushes.Black, new XRect(180, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString(":", nfont, XBrushes.Black, new XRect(300, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            if (IndItm != null && IndItm.Count() > 0)
                            {
                                if (IndItm[0].AddressIC != null)
                                {
                                    if (IndItm[0].AddressIC.ToString().Length > 55)
                                    {
                                        graph.DrawString(IndItm[0].AddressIC.Substring(0, 55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                        lineheight = lineheight + 15;
                                        graph.DrawString(IndItm[0].AddressIC.Substring(55, IndItm[0].AddressIC.ToString().Length - 55), nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                    else
                                    {
                                        graph.DrawString(IndItm[0].AddressIC, nUfont, XBrushes.Black, new XRect(305, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                                    }
                                }
                            }
                            lineheight = lineheight + 75;
                            pt3 = new System.Drawing.Point(30, lineheight);
                            pt4 = new System.Drawing.Point(170, lineheight);
                            graph.DrawLine(lineRed1, pt3, pt4);
                            lineheight = lineheight + 5;
                            graph.DrawString("b.p.", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            graph.DrawString("KETUA PEGAWAI EKSEKUTIF", nfont, XBrushes.Black, new XRect(50, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 12;
                            graph.DrawString("PERBADANAN LABUAN", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            graph.DrawString("Tarikh:", nfont, XBrushes.Black, new XRect(32, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt3 = new System.Drawing.Point(65, lineheight+13);
                            pt4 = new System.Drawing.Point(150, lineheight+13);
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
            catch(Exception ex)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion
    }
}