using System.Web.Mvc;
using TradingLicense.Web.Classes;
using Rotativa;
using TradingLicense.Model;
using System.Collections.Generic;
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

        public ActionResult LicenseBusinessTypeMaster()
        {
            return View();
        }

        public ActionResult LicenseBusinessTypeMasterPdf()
        {
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
            List<TradingLicense.Model.ZoneModel> zoneAllList = new List<TradingLicense.Model.ZoneModel>();

            using (var ctx = new LicenseApplicationContext())
            {
                zoneList = ctx.ZONEs
                .Select(fnZoneDisplayFormat)
                .ToList();

                var zoneAll = ctx.ZONEs.ToList();
                zoneAllList = Mapper.Map<List<TradingLicense.Model.ZoneModel>>(zoneAll);
            }

            zoneModel.zoneCombineList = zoneList;
            return View(zoneAllList);
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