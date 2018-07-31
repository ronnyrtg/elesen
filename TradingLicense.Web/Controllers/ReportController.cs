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

namespace TradingLicense.Web.Controllers
{
    public class ReportController : BaseController
    {
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
            List<SelectListItem> items = new List<SelectListItem>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var licenseCodes = ctx.ZONEs.ToList();
                foreach (var item in licenseCodes)
                {
                    items.Add(new SelectListItem { Text = item.ZONE_CODE + " - " + item.ZONE_DESC, Value = item.ZONEID.ToString() });
                }
            }
            ViewBag.ZoneList = items;
            return View();
        }

        // Display generated pdf
        public ActionResult ZoneMasterPdf(string LicenseCode)
        {
            List<ZoneModel> items = new List<ZoneModel>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var zones = ctx.ZONEs.ToList();
                items = Mapper.Map<List<ZoneModel>>(zones);
            }
            ViewBag.zones = items;
            ViewBag.date = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");
            return new ViewAsPdf();
        }

        public ActionResult SubzoneMaster()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var licenseCodes = ctx.SUBZONEs.ToList();
                foreach (var item in licenseCodes)
                {
                    items.Add(new SelectListItem { Text = item.SUBZONE_CODE + " - " + item.SUBZONE_DESC, Value = item.SUBZONEID.ToString() });
                }
            }
            ViewBag.SubzoneList = items;
            return View();
        }

        // Display generated pdf
        public ActionResult SubzoneMasterPdf(string LicenseCode)
        {
            List<ZoneModel> items = new List<ZoneModel>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var zones = ctx.SUBZONEs.ToList();
                items = Mapper.Map<List<ZoneModel>>(zones);
            }
            ViewBag.zones = items;
            ViewBag.date = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");
            return new ViewAsPdf();
        }

        public ActionResult RoadMaster()
        {
            List<RoadModel> items = new List<RoadModel>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var roads = ctx.ROADs.ToList();
                items = Mapper.Map<List<RoadModel>>(roads);
            }
            ViewBag.roads = items;
            ViewBag.date = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");
            return new ViewAsPdf();
        }

        public ActionResult RaceMaster()
        {
            List<RaceModel> items = new List<RaceModel>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var races = ctx.RACEs.ToList();
                items = Mapper.Map<List<RaceModel>>(races);
            }
            ViewBag.races = items;
            ViewBag.date = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");
            return new ViewAsPdf();
        }

        public ActionResult CitizenMaster()
        {
            List<CitizenModel> items = new List<CitizenModel>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var citizens = ctx.CITIZENs.ToList();
                items = Mapper.Map<List<CitizenModel>>(citizens);
            }
            ViewBag.citizens = items;
            ViewBag.date = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");
            return new ViewAsPdf();
        }

        public ActionResult OwnerMaster()
        {
            return View();
        }

        public ActionResult JointOwner()
        {
            return View();
        }

        public ActionResult BankMaster()
        {
            List<BankModel> items = new List<BankModel>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var citizens = ctx.BANKs.ToList();
                items = Mapper.Map<List<BankModel>>(citizens);
            }
            ViewBag.banks = items;
            ViewBag.date = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");
            return new ViewAsPdf();
        }

        public ActionResult PaymentTransaction()
        {
            return View();
        }

        public ActionResult PaymentTransactionPdf()
        {
            List<PaymentModel> items = new List<PaymentModel>();
            using (var ctx = new Data.LicenseApplicationContext())
            {
                var payments = ctx.BANKs.ToList();
                items = Mapper.Map<List<PaymentModel>>(payments);
            }
            ViewBag.payments = items;
            ViewBag.date = DateTime.Now.ToString("dd-MMM-yyyy");
            ViewBag.time = DateTime.Now.ToString("hh:mm:ss tt");
            return new ViewAsPdf();
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
    }
}