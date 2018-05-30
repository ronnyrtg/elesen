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
                        foreach (var item in ctx.BusinessCodes )
                        {
                            DataRow rw = dt.NewRow();
                            rw["No"] = item.BusinessCodeID.ToString();
                            rw["Code"] = item.CodeNumber.ToString();
                            rw["Description"] = item.CodeDesc.ToString();
                            dt.Rows.Add(rw);
                        }
                    }
                    else if(tblId == "2")
                    {
                        foreach (var item in ctx.BannerCodes)
                        {
                            DataRow rw = dt.NewRow();
                            rw["No"] = item.BannerCodeID.ToString();
                            rw["Code"] = item.BCodeNumber.ToString();
                            rw["Description"] = item.BannerCodeDesc.ToString();
                            dt.Rows.Add(rw);
                        }
                    }
                    else if (tblId == "3")
                    {
                        foreach (var item in ctx.HawkerCodes)
                        {
                            DataRow rw = dt.NewRow();
                            rw["No"] = item.HawkerCodeID.ToString();
                            rw["Code"] = item.HCodeNumber.ToString();
                            rw["Description"] = item.HawkerCodeDesc.ToString();
                            dt.Rows.Add(rw);
                        }
                    }
                    else if (tblId == "4")
                    {
                        foreach (var item in ctx.LiquorCodes)
                        {
                            DataRow rw = dt.NewRow();
                            rw["No"] = item.LiquorCodeID.ToString();
                            rw["Code"] = item.LCodeNumber.ToString();
                            rw["Description"] = item.LiquorCodeDesc.ToString();
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