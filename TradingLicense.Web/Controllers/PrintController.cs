using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Data.SqlClient;
using System.Data;
using TradingLicense.Entities;
using System.Linq.Dynamic;
using TradingLicense.Model;
using AutoMapper;
using TradingLicense.Web.Classes;
using TradingLicense.Infrastructure;
using TradingLicense.Data;
using System.Diagnostics;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace TradingLicense.Web.Controllers
{
    public class PrintController : Controller
    {
        public ActionResult Print(FormCollection frm, string btnPrint)
        {
            try
            {
                ViewBag.Print = "No";
                if (btnPrint == "Print")
                {
                    string tblId = frm["Items"].ToString();
                    ViewBag.Print = "Yes";
                    GeneratePDF(tblId);
                }
                
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            ViewBag.Items = GetDropDown();
            return View();
        }

        public ActionResult GeneratePDF(string tblId)
        {
            DataTable dt = new DataTable();
            dt.TableName = "dtTablePrint";
            dt.Columns.Add("No");
            dt.Columns.Add("Code");
            dt.Columns.Add("Description");
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    if (tblId == "1")
                    {
                        foreach (var item in ctx.BCs )
                        {
                            DataRow rw = dt.NewRow();
                            rw["No"] = item.BC_ID.ToString();
                            rw["Code"] = item.C_R.ToString();
                            rw["Description"] = item.C_R_DESC.ToString();
                            dt.Rows.Add(rw);
                        }
                    }
                }
                Session["dtPrint"] = dt;
                ViewBag.Items = GetDropDown();
                return View("Print");
            }
            catch (Exception ex)
            {

                throw ex;

            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                }
            }

        }

        public static List<SelectListItem> GetDropDown()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            ls.Add(new SelectListItem() { Text = "BusinessCode", Value = "1" });
            ls.Add(new SelectListItem() { Text = "BannerCode", Value = "2" });
            ls.Add(new SelectListItem() { Text = "HawkerCode", Value = "3" });
            ls.Add(new SelectListItem() { Text = "LiquorCode", Value = "4" });
            return ls;
        }
    }
}