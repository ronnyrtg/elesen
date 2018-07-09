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
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;

namespace TradingLicense.Web.Controllers
{
    public class MoneyLenderApplicationController : BaseController
    {
        #region MLPremiseApplication

        /// <summary>
        /// GET: MLPremiseApplication
        /// </summary>
        /// <returns></returns>

        public ActionResult MLPremiseApplication()
        {
            return View();
        }

        /// <summary>
        /// GET: MLPermitApplication
        /// </summary>
        /// <returns></returns>
        public ActionResult MLPermitApplication()
        {
            return View();
        }

        /// <summary>
        /// Get MLPremiseApplication Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MLPremiseApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string MLPremiseApplicationID, string IndividualMkNo)
        {
            List<TradingLicense.Model.MLPremiseApplicationModel> MLPremiseApplication = new List<Model.MLPremiseApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<MLPremiseApplication> query = (ProjectSession.User != null && ProjectSession.User.RoleTemplateID == (int)RollTemplate.Public) ? ctx.MLPremiseApplications.Where(p => p.UsersID == ProjectSession.User.UsersID) : ctx.MLPremiseApplications;

                if (!string.IsNullOrWhiteSpace(MLPremiseApplicationID))
                {
                    query = query.Where(q => q.MLPremiseApplicationID.ToString().Contains(MLPremiseApplicationID));
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

                var result = Mapper.Map<List<MLPremiseApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "MLPremiseApplicationID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                MLPremiseApplication = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, MLPremiseApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        
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
        /// Get MLPremiseApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageMLPremiseApplication(int? Id)
        {
            MLPremiseApplicationModel mLPremiseApplicationModel = new MLPremiseApplicationModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    //Retrieve data from MLPremiseApplication table where MLPremiseApplicationID == Id
                    int MLPremiseApplicationID = Convert.ToInt32(Id);
                    var mLPremiseApplication = ctx.MLPremiseApplications.Where(a => a.MLPremiseApplicationID == MLPremiseApplicationID).FirstOrDefault();
                    mLPremiseApplicationModel = Mapper.Map<MLPremiseApplicationModel>(mLPremiseApplication);


                    //Get list of Individuals linked to this application
                    var mlLinkInd = ctx.MLLinkInds.Where(a => a.MLPremiseApplicationID == Id).ToList();
                    mLPremiseApplicationModel.Individualids = string.Join(",", mlLinkInd.Select(x => x.IndividualID.ToString()).ToArray());
                    List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
                    var iids = mlLinkInd.Select(b => b.IndividualID).ToList();
                    selectedIndividualList = ctx.Individuals
                        .Where(b => iids.Contains(b.IndividualID))
                        .Select(fnSelectIndividualFormat)
                        .ToList();

                    mLPremiseApplicationModel.selectedIndividualList = selectedIndividualList;

                    //Get list of Documents linked to this application
                    var MLLinkReqDocumentList = ctx.MLLinkReqDocs.Where(p => p.MLPremiseApplicationID == MLPremiseApplicationID).ToList();
                    if (MLLinkReqDocumentList != null && MLLinkReqDocumentList.Count > 0)
                    {
                        mLPremiseApplicationModel.UploadRequiredDocids = (string.Join(",", MLLinkReqDocumentList.Select(x => x.RequiredDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                mLPremiseApplicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
                mLPremiseApplicationModel.UsersID = ProjectSession.User.UsersID;
            }
            mLPremiseApplicationModel.IsDraft = false;
            return View(mLPremiseApplicationModel);
        }

        /// <summary>
        /// Get MLPermitApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageMLPermitApplication(int? Id)
        {
            MLPermitApplicationModel mLPermitApplicationModel = new MLPermitApplicationModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    //Retrieve data from MLPermitApplication table where MLPermitApplicationID == Id
                    int MLPermitApplicationID = Convert.ToInt32(Id);
                    var mLPermitApplication = ctx.MLPermitApplications.Where(a => a.MLPermitApplicationID == MLPermitApplicationID).FirstOrDefault();
                    mLPermitApplicationModel = Mapper.Map<MLPermitApplicationModel>(mLPermitApplication);


                   

                   
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                mLPermitApplicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
                mLPermitApplicationModel.UsersID = ProjectSession.User.UsersID;
            }
            mLPermitApplicationModel.IsDraft = false;
            return View(mLPermitApplicationModel);
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
                    int MLPremiseCodeID = Convert.ToInt32(id);
                    Individuallist.Add(MLPremiseCodeID);
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

        #region Generate License PDF
        public ActionResult GeneratLicense_PremiseApp(Int32? appId)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    //var MLApp = ctx.MLPremiseApplications
                    //                    .Where(x => x.MLPremiseApplicationID == appId).ToList();
                    //if (MLApp.Count == 0)
                    //{
                    //    return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid MLPremise ApplicationID!');</script>");
                    //}
                    //else
                    //{
                    //    foreach (var item in MLApp)
                    //    {
                            int lineheight = 30;
                            PdfDocument pdf = new PdfDocument();
                            pdf.Info.Title = "PDF License";
                            PdfPage pdfPage = pdf.AddPage();
                            XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                            XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                            XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                            XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                            XFont fontN10 = new XFont("Verdana", 10, XFontStyle.Regular);
                            XFont Italikfont = new XFont("Verdana", 11, XFontStyle.Italic);
                            XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                            XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                            XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);
                   

                            graph.DrawString("JADUAL B", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 18;
                            graph.DrawString("AKTA PEMBERI PINJAM  WANG 1951", Italikfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 25;
                            graph.DrawString("PERATURAN-PERATURAN PEMBERI PINJAM WANG", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 11;
                            graph.DrawString("(KAWALAN DAN PELESENAN) 2003", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 17;
                            graph.DrawString("(Perenggan 3(6))", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 25;
                            graph.DrawString("LESEN  PEMBERI PINJAM WANG", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            lineheight = lineheight + 25;
                            graph.DrawString("Nama Pemberi Pinjam Wang", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            XPen lineRed1 = new XPen(XColors.Black, 0.5);
                            System.Drawing.Point pt1 = new System.Drawing.Point(219, lineheight+13);
                            System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight+13);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 18;
                            graph.DrawString("alamat berdaftar", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt1 = new System.Drawing.Point(174, lineheight + 13);
                            pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 15;
                            graph.DrawString("dengan ini diberikan lesen di bawah seksyen 5B Akta Pemberi Pinjam Wang 1951 untuk", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 15;
                            graph.DrawString("menjalankan perniagaan meminjamkan wang di premis dinyatakan di bawah:", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 25;
                            graph.DrawString("Alamat perniagaan", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt1 = new System.Drawing.Point(179, lineheight + 13);
                            pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 20;
                            graph.DrawString("Tertakluk kepada syarat-syarat yang berikut:", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            lineheight = lineheight + 20;
                            pt1 = new System.Drawing.Point(90, lineheight + 13);
                            pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 30;
                            XTextFormatter tf = new XTextFormatter(graph);
                            XRect rect = new XRect(90, lineheight, 230, 110);
                            graph.DrawRectangle(lineRed1, rect);
                            lineheight = lineheight + 5;
                            graph.DrawString("Butir-butir lesen", fontN10, XBrushes.Black, new XRect(160, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt1 = new System.Drawing.Point(90, lineheight + 15);
                            pt2 = new System.Drawing.Point(320, lineheight + 15);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 25;
                            graph.DrawString("No.Lesen:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt1 = new System.Drawing.Point(150, lineheight + 13);
                            pt2 = new System.Drawing.Point(310, lineheight + 13);
                            graph.DrawLine(lineRed1, pt1, pt2);

                            pt1 = new System.Drawing.Point(360, lineheight + 13);
                            pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            graph.DrawString("Pendaftar  Pemberi Pinjam Wang", fontN10, XBrushes.Black, new XRect(362, (lineheight+15), pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                            lineheight = lineheight + 23;
                            graph.DrawString("Sah dari:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt1 = new System.Drawing.Point(145, lineheight + 13);
                            pt2 = new System.Drawing.Point(310, lineheight + 13);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 23;
                            graph.DrawString("hingga:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt1 = new System.Drawing.Point(138, lineheight + 13);
                            pt2 = new System.Drawing.Point(310, lineheight + 13);
                            graph.DrawLine(lineRed1, pt1, pt2);

                            graph.DrawString("Tarikh:", fontN10, XBrushes.Black, new XRect(360, lineheight , pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                            pt1 = new System.Drawing.Point(395, lineheight + 13);
                            pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                            graph.DrawLine(lineRed1, pt1, pt2);

                            pt1 = new System.Drawing.Point(90, lineheight + 50 );
                            pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 50);
                            graph.DrawLine(lineRed1, pt1, pt2);
                            lineheight = lineheight + 60;
                            graph.DrawString("*Potong yang tidak berkenaan", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                            MemoryStream strm = new MemoryStream();
                            pdf.Save(strm, false);
                            return File(strm, "application/pdf");

                        }
                    //}
                //}
            }
            catch (Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }

        public ActionResult GeneratLicense_PermitApp(Int32? appId)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    //var MLApp = ctx.MLPremiseApplications
                    //                    .Where(x => x.MLPremiseApplicationID == appId).ToList();
                    //if (MLApp.Count == 0)
                    //{
                    //    return Content("<script language='javascript' type='text/javascript'>alert('No Data Found Or Invalid MLPremise ApplicationID!');</script>");
                    //}
                    //else
                    //{
                    //    foreach (var item in MLApp)
                    //    {
                    int lineheight = 30;
                    PdfDocument pdf = new PdfDocument();
                    pdf.Info.Title = "PDF License";
                    PdfPage pdfPage = pdf.AddPage();
                    XFont fontitalik = new XFont("Verdana", 8, XFontStyle.Italic);
                    XGraphics graph = XGraphics.FromPdfPage(pdfPage);
                    XFont font = new XFont("Verdana", 9, XFontStyle.Bold);
                    XFont lbfont = new XFont("Verdana", 11, XFontStyle.Bold);
                    XFont fontN10 = new XFont("Verdana", 10, XFontStyle.Regular);
                    XFont Italikfont = new XFont("Verdana", 11, XFontStyle.Italic);
                    XFont nfont = new XFont("Verdana", 9, XFontStyle.Regular);
                    XFont nUfont = new XFont("Verdana", 9, XFontStyle.Underline);
                    XFont sfont = new XFont("Verdana", 8, XFontStyle.Regular);


                    graph.DrawString("JADUAL G", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 18;
                    graph.DrawString("AKTA PEMBERI PINJAM  WANG 1951", Italikfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 25;
                    graph.DrawString("PERATURAN-PERATURAN PEMBERI PINJAM WANG", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 11;
                    graph.DrawString("(KAWALAN DAN PELESENAN) 2003", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 17;
                    graph.DrawString("(Subperturan 3(6))", nfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 25;
                    graph.DrawString("PERMIT IKLAN", lbfont, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    lineheight = lineheight + 25;
                    graph.DrawString("Nama Pemberi Pinjam Wang", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    XPen lineRed1 = new XPen(XColors.Black, 0.5);
                    System.Drawing.Point pt1 = new System.Drawing.Point(219, lineheight + 13);
                    System.Drawing.Point pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 18;
                    graph.DrawString("alamat berdaftar", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(174, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 15;
                    graph.DrawString("dengan ini diberikan permit iklan bagi perniagaan meminjamkan wang di bawah", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("subperaturan 6(8) Peraturan-Peraturan Pemberi Pinjam Wang(Kawalan dan Pelesenan) ", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 15;
                    graph.DrawString("2003 seperti yang dinyatakan di bawah-", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 25;
                    graph.DrawString("Alamat perniagaan", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(179, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 25;
                    graph.DrawString("Tertakluk kepada syarat-syarat yang berikut:", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;

                    XTextFormatter tf = new XTextFormatter(graph);
                    XRect rect = new XRect(90, lineheight, 438, 207);
                    graph.DrawRectangle(lineRed1, rect);

                    pt1 = new System.Drawing.Point(310, 284);
                    pt2 = new System.Drawing.Point(310, 491);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 5;
                    graph.DrawString("Jenis iklan", nfont, XBrushes.Black, new XRect(175, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    graph.DrawString("Tarikh kelulusan", nfont, XBrushes.Black, new XRect(365, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Akhbar", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Papan tanda", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Radio", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Internet", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Televisyen", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Cakera padat-video", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Panggung wayang", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 5;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 69, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 18;
                    graph.DrawString("Lain-lain(sila nyatakan)", nfont, XBrushes.Black, new XRect(101, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    

                    lineheight = lineheight + 30;
                    graph.DrawString("Syarat-Syarat lain:", nfont, XBrushes.Black, new XRect(90, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    lineheight = lineheight + 20;
                    pt1 = new System.Drawing.Point(90, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    lineheight = lineheight + 30;
                     tf = new XTextFormatter(graph);
                    rect = new XRect(90, lineheight, 230, 110);
                    graph.DrawRectangle(lineRed1, rect);
                    lineheight = lineheight + 5;
                    graph.DrawString("Butir-butir permit", fontN10, XBrushes.Black, new XRect(160, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(90, lineheight + 15);
                    pt2 = new System.Drawing.Point(320, lineheight + 15);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 25;
                    graph.DrawString("No.Permit:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(150, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    pt1 = new System.Drawing.Point(360, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    graph.DrawString("Pendaftar  Pemberi Pinjam Wang", fontN10, XBrushes.Black, new XRect(362, (lineheight + 15), pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);

                    lineheight = lineheight + 23;
                    graph.DrawString("Sah dari:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(145, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 23;
                    graph.DrawString("hingga:", fontN10, XBrushes.Black, new XRect(97, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(138, lineheight + 13);
                    pt2 = new System.Drawing.Point(310, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    graph.DrawString("Tarikh:", fontN10, XBrushes.Black, new XRect(360, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopLeft);
                    pt1 = new System.Drawing.Point(395, lineheight + 13);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 13);
                    graph.DrawLine(lineRed1, pt1, pt2);

                    pt1 = new System.Drawing.Point(90, lineheight + 50);
                    pt2 = new System.Drawing.Point(Convert.ToInt32(pdfPage.Width) - 70, lineheight + 50);
                    graph.DrawLine(lineRed1, pt1, pt2);
                    lineheight = lineheight + 60;
                    graph.DrawString("*Potong yang tidak berkenaan", fontN10, XBrushes.Black, new XRect(0, lineheight, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopCenter);
                    MemoryStream strm = new MemoryStream();
                    pdf.Save(strm, false);
                    return File(strm, "application/pdf");

                }
                //}
                //}
            }
            catch (Exception)
            {

            }
            return Content("<script language='javascript' type='text/javascript'>alert('Problem In Generating License!');</script>");
        }
        #endregion

    }
}