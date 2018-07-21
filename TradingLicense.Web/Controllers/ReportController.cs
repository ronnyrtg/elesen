using AutoMapper;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;
using TradingLicense.Data;
using TradingLicense.Entities;
using TradingLicense.Infrastructure;
using TradingLicense.Model;
using TradingLicense.Web.Pages;
using static TradingLicense.Infrastructure.Enums;

namespace TradingLicense.Web.Controllers
{
    public class ReportController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}