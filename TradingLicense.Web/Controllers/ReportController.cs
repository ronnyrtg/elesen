using System;
using System.Web.Mvc;
using Rotativa;
using System.Collections.Generic;
using System.Linq;
using TradingLicense.Web.Classes;
using TradingLicense.Model;
using AutoMapper;
using TradingLicense.Data;
using TradingLicense.Entities;
using System;
using System.Linq;
using AutoMapper;

namespace TradingLicense.Web.Controllers
{
    public class ReportController : BaseController
    {
        private Func<ZONE, Select2ListItem> fnZoneDisplayFormat = ind => new Select2ListItem { id = ind.ZONEID, text = $" Kod {ind.ZONECODE} | {ind.ZONEDESC}" };

        // GET /Report
        public ActionResult Index()
        {
            return View();
        }

        // Show filter form
        public ActionResult LicenseBusinessTypeMaster()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var licenseCodes = ctx.LIC_TYPEs.ToList();
                foreach(var item in licenseCodes)
                {
                    items.Add(new SelectListItem { Text = item.LIC_TYPECODE + " - " + item.LIC_TYPEDESC, Value = item.LIC_TYPEID.ToString() });
                }
            }
            ViewBag.LicenseCode = items;
            return View();
        }

        // Display generated pdf
        public ActionResult LicenseBusinessTypeMasterPdf(string LicenseCode)
        {
            List<BusinessCodeModel> items = new List<BusinessCodeModel>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var businessCodes = ctx.BCs.ToList();
                items = Mapper.Map<List<BusinessCodeModel>>(businessCodes);
            }
            ViewBag.businessCodes = items;
            ViewBag.date = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");
            return new ViewAsPdf();
        }

        public ActionResult LicenceTypeLookup()
        {
            return View();
        }

        public ActionResult ZoneMaster()
        {
            ZoneModel zoneModel = new ZoneModel();
            List<Select2ListItem> zoneList = new List<Select2ListItem>();
            

            using (var ctx = new LicenseApplicationContext())
            {
                zoneList = ctx.ZONEs
                .Select(fnZoneDisplayFormat)
                .ToList();

                var zoneAll = from ZONE in ctx.ZONEs select new Alldata();
                zoneModel.zListAll = zoneAll.ToList();
            }

            
            zoneModel.zoneCombineList = zoneList;
            return View(zoneModel);
        }

        public ActionResult SubzoneMaster()
        {
            return View();
        }

        public ActionResult RoadMaster()
        {
            return View();
        }

        public ActionResult CitizenMaster()
        {
            return View();
        }

        public ActionResult CitizenLookup()
        {
            return View();
        }

        public ActionResult RaceMaster()
        {
            return View();
        }

        public ActionResult BankMaster()
        {
            return View();
        }

        public ActionResult OwnerMaster()
        {
            return View();
        }

        public ActionResult JointOwner()
        {
            return View();
        }

        public ActionResult PaymentTransaction()
        {
            return View();
        }

        public ActionResult PaymentCollection()
        {
            return View();
        }

        public ActionResult PaymentInquiryPaid()
        {
            return View();
        }

        public ActionResult PaymentInquiryUnpaid()
        {
            return View();
        }

        public ActionResult AdjustmentListing()
        {
            return View();
        }

        public ActionResult Duration()
        {
            return View();
        }

        public ActionResult LicenseListing()
        {
            return View();
        }

        public ActionResult OwnerInfo()
        {
            return View();
        }

        public ActionResult License()
        {
            return View();
        }

        public ActionResult LicenseApplicationStats()
        {
            return View();
        }

        public ActionResult ApplicationStatus()
        {
            return View();
        }

        public ActionResult ApplicationLocationStats()
        {
            return View();
        }

        public ActionResult RenewalStatusResult()
        {
            return View();
        }

        public ActionResult BusinessListing()
        {
            return View();
        }
    }
}