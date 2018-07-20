﻿using AutoMapper;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using TradingLicense.Data;
using TradingLicense.Entities;
using TradingLicense.Infrastructure;
using TradingLicense.Model;
using TradingLicense.Web.Pages;
using static TradingLicense.Infrastructure.Enums;

namespace TradingLicense.Web.Controllers
{
    public class RegistrationController : Controller
    {
        /// <summary>
        /// Registration.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            Session.Abandon();
            Session.Clear();
            return View();
        }

        /// <summary>
        /// Use to Registration
        /// </summary>
        /// <param name="model">model value</param>
        /// <returns>return action result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var userModel = ctx.USERS.Where(u => u.USERNAME == model.Username.Trim() || u.EMAIL ==model.Email.Trim()).FirstOrDefault();
                    if (userModel != null && userModel.USERSID > 0)
                    {

                        if (userModel.USERNAME.Trim() == model.Username.Trim())
                        {
                            ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.UserNameAvailable);
                            return View(model);
                        }
                        else if (userModel.EMAIL.Trim() == model.Email.Trim())
                        {
                            ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.EmailAvailable);
                            return View(model);
                        }
                    }
                    else
                    {
                        USERS users = Mapper.Map<USERS>(model);
                        users.PASSWORD = Infrastructure.EncryptionDecryption.GetEncrypt(model.Password);
                        users.LOCKED = 0;
                        ctx.USERS.AddOrUpdate(users);
                        ctx.SaveChanges();
                        
                        TempData["openPopup"] = CommonHelper.ShowAlertMessageToastr(MessageType.success.ToString(), Messages.UserRegistration);
                        return RedirectToAction(Actions.LogIn, Pages.Controllers.Account);
                    }
                }
            }
            else
            {
                ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.DataMissing);
            }
            return View(model);
        }
    }
}