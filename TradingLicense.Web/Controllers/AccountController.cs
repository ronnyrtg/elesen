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
    public class AccountController : Controller
    {

        /// <summary>
        /// Logs the in.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult LogIn(string returnUrl)
        {
            if (ProjectSession.UserID > 0)
            {
                return RedirectToAction(Actions.Individual, Pages.Controllers.Master);
            }

            LoginModel loginModel = new LoginModel();
            if (Request.Cookies["TradingLicenseIsRemember"] != null && Request.Cookies["TradingLicenseIsRemember"] != null)
            {
                loginModel.RememberMe = ConvertTo.Boolean(Request.Cookies["TradingLicenseIsRemember"].Value);
                if (loginModel.RememberMe)
                {
                    if (Request.Cookies["TradingLicenseUserName"] != null)
                    {
                        loginModel.Username = Request.Cookies["TradingLicenseUserName"].Value;
                    }

                    if (Request.Cookies["TradingLicensePassword"] != null)
                    {
                        loginModel.Password = EncryptionDecryption.GetDecrypt(Request.Cookies["TradingLicensePassword"].Value);
                    }
                }
            }

            if (TempData["openPopup"] != null)
                ViewBag.openPopup = TempData["openPopup"];

            loginModel.ReturnUrl = returnUrl;
            return View(loginModel);
        }

        /// <summary>
        /// Use to index
        /// </summary>
        /// <param name="model">model value</param>
        /// <returns>return action result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var userModel = ctx.Users.Where(u => u.Username == model.Username).FirstOrDefault();

                    if (userModel != null && userModel.UsersID > 0)
                    {
                        if (userModel.Locked != 1)
                        {
                            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                            string ipAddress = Dns.GetHostByName(hostName).AddressList[0].ToString();
                            LoginLog loginLog = new LoginLog();
                            loginLog.LogDate = DateTime.Now;
                            loginLog.UsersID = userModel.UsersID;
                            loginLog.IpAddress = ipAddress;
                            loginLog.LogDesc = userModel.Username;

                            var result = Infrastructure.EncryptionDecryption.GetEncrypt(model.Password);
                            if (userModel.Password == Infrastructure.EncryptionDecryption.GetEncrypt(model.Password))
                            {
                                if (model.RememberMe)
                                {
                                    Response.Cookies["TradingLicenseUserName"].Value = model.Username;
                                    Response.Cookies["TradingLicensePassword"].Value = Infrastructure.EncryptionDecryption.GetEncrypt(model.Password);
                                    Response.Cookies["TradingLicenseIsRemember"].Value = Convert.ToString(model.RememberMe);
                                    Response.Cookies["TradingLicenseIsRemember"].Expires = DateTime.Now.AddMonths(1);
                                    Response.Cookies["TradingLicenseUserName"].Expires = DateTime.Now.AddMonths(1);
                                    Response.Cookies["TradingLicensePassword"].Expires = DateTime.Now.AddMonths(1);
                                }
                                else
                                {
                                    Response.Cookies["TradingLicenseUserName"].Expires = DateTime.Now.AddMonths(-1);
                                    Response.Cookies["TradingLicensePassword"].Expires = DateTime.Now.AddMonths(-1);
                                    Response.Cookies["TradingLicenseIsRemember"].Expires = DateTime.Now.AddMonths(-1);
                                }

                                loginLog.LoginStatus = true;
                                ctx.LoginLogs.AddOrUpdate(loginLog);
                                ctx.SaveChanges();
                                
                                UsersModel user = new UsersModel();
                                user = Mapper.Map<UsersModel>(userModel);
                                ProjectSession.UserID = userModel.UsersID;
                                ProjectSession.User = user;

                                FormsAuthentication.SetAuthCookie(model.Username, false);

                                if (!string.IsNullOrEmpty(model.ReturnUrl))
                                {
                                    return Redirect(model.ReturnUrl);
                                }
                                else
                                {
                                    return RedirectToAction(Actions.Individual, Pages.Controllers.Master);
                                }
                            }
                            else
                            {
                                loginLog.LoginStatus = false;
                                ctx.LoginLogs.AddOrUpdate(loginLog);
                                ctx.SaveChanges();

                                DateTime fromDate = DateTime.Now.AddMinutes(-30);
                                DateTime toDate = DateTime.Now;

                                var loginLoglist = ctx.LoginLogs.Where(l => l.UsersID == userModel.UsersID && l.LogDate >= fromDate && l.LogDate < toDate && l.LoginStatus == false).ToList();
                                if (loginLoglist != null && loginLoglist.Count() == 5 || loginLoglist.Count() > 5)
                                {
                                    userModel.Locked = 1;
                                    ctx.Users.AddOrUpdate(userModel);
                                    ctx.SaveChanges();
                                }

                                ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.InValidCredential);
                                return View(model);
                            }
                        }
                        else
                        {
                            ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.AccountLock);
                            return View(model);
                        }
                    }
                    else
                    {
                        ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.InValidCredential);
                        return View(model);
                    }
                }
            }
            else if (string.IsNullOrEmpty(model.Username))
            {
                ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.UserName);
                return View(model);
            }
            else if (string.IsNullOrEmpty(model.Password))
            {
                ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.Password);
                return View(model);
            }

            return View(model);
        }


        /// <summary>
        /// Forgot password page
        /// </summary>
        /// <param name="emailValue"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgotPassword(string emailValue, int Type)
        {
            try
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    string link = string.Empty;
                    var user = ctx.Users.Where(a => a.Email == emailValue).FirstOrDefault();
                    if (user != null && user.UsersID > 0)
                    {
                        string resetPasswordParameter = string.Format("{0}#{1}#{2}", SystemEnum.RoleType.User.GetHashCode(), user.UsersID, DateTime.Now.AddMinutes(ProjectConfiguration.ResetPasswordExpireTime).ToString(ProjectConfiguration.EmailDateTimeFormat));
                        string encryptResetPasswordParameter = EncryptionDecryption.GetEncrypt(resetPasswordParameter);
                        string encryptResetPasswordUrl = string.Format("{0}?q={1}", ProjectConfiguration.SiteUrlBase + TradingLicense.Web.Pages.Controllers.Account + "/" + Actions.ResetPassword, encryptResetPasswordParameter);

                        if (UserMail.SendForgotPassword(user.Email, user.Username, encryptResetPasswordUrl))
                        {
                            return Json(new object[] { Convert.ToInt32(MessageType.success), MessageType.success.ToString(), Messages.Mailsend }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new object[] { Convert.ToInt32(MessageType.danger), MessageType.danger.ToString(), Messages.ContactToAdmin }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new object[] { Convert.ToInt32(MessageType.danger), MessageType.danger.ToString(), Messages.InvalidEmail }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                // ErrorLogHelper.Log(ex);
                return Json(new object[] { Convert.ToInt32(MessageType.danger), MessageType.danger.ToString(), Messages.ContactToAdmin }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Reset Password Page
        /// </summary>
        /// <param name="q">query parameter</param>
        /// <returns>View Reset Password</returns>
        [ActionName(Actions.ResetPassword)]
        public ActionResult ResetPassword(string q)
        {
            Session.Abandon();
            Session.Clear();
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel();
            ViewBag.IsVisible = 0;
            if (!string.IsNullOrEmpty(q))
            {
                try
                {
                    string parameterString = EncryptionDecryption.GetDecrypt(q);
                    var parameters = parameterString.Split('#');

                    if (parameters != null && parameters.Count() == 3)
                    {
                        DateTime urlExpiredTime = DateTime.ParseExact(parameters[2], ProjectConfiguration.EmailDateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                        int roleTypeId = Convert.ToInt32(parameters[0]);
                        int id = Convert.ToInt32(parameters[1]);

                        if (DateTime.Now > urlExpiredTime)
                        {
                            ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.ResetPasswordMessage);
                            return View(Actions.ResetPassword, resetPasswordModel);
                        }

                        using (var ctx = new LicenseApplicationContext())
                        {
                            if (roleTypeId == SystemEnum.RoleType.User.GetHashCode())
                            {
                                var user = ctx.Users.Where(u => u.UsersID == id).FirstOrDefault();
                                resetPasswordModel.RoleTypeId = roleTypeId;
                                if (user != null && user.UsersID > 0)
                                {
                                    ViewBag.IsVisible = 1;
                                    resetPasswordModel.Id = user.UsersID;
                                }
                                else
                                {
                                    ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.UrlNotExist);
                                    return View(Actions.ResetPassword, resetPasswordModel);
                                }
                            }
                            else
                            {
                                ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.UrlNotvalid);
                                return View(Actions.ResetPassword, resetPasswordModel);
                            }
                        }
                    }
                    else
                    {
                        ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.UrlNotvalid);
                        return View(Actions.ResetPassword, resetPasswordModel);
                    }
                }
                catch (Exception)
                {
                    ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.UrlNotvalid);
                    return View(Actions.ResetPassword, resetPasswordModel);
                }
            }
            else
            {
                ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.UrlNotvalid);
            }

            return View(Actions.ResetPassword, resetPasswordModel);
        }

        /// <summary>
        /// Reset Password Page
        /// </summary>
        /// <param name="resetPassword">reset Password model</param>
        /// <returns>View Reset Password</returns>
        [ActionName(Actions.ResetPassword)]
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                if (resetPasswordModel.NewPassword != resetPasswordModel.ConfirmPassword)
                {
                    ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.ResetPasswordMatch);
                    return View(Actions.ResetPassword, resetPasswordModel);
                }

                if (resetPasswordModel.RoleTypeId == SystemEnum.RoleType.User.GetHashCode())
                {

                    var user = ctx.Users.Where(u => u.UsersID == resetPasswordModel.Id).FirstOrDefault();
                    if (user != null && user.UsersID > 0)
                    {
                        user.Password = EncryptionDecryption.GetEncrypt(resetPasswordModel.NewPassword);
                        ctx.Users.AddOrUpdate(user);
                        ctx.SaveChanges();

                        UsersModel usermodel = new UsersModel();
                        usermodel = Mapper.Map<UsersModel>(user);
                        ProjectSession.UserID = user.UsersID;
                        ProjectSession.User = usermodel;

                        FormsAuthentication.SetAuthCookie(ProjectSession.UserName, false);
                        return RedirectToAction(Actions.Individual, Pages.Controllers.Master);
                    }
                    else
                    {
                        ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.UrlNotExist);
                        return View(Actions.ResetPassword, resetPasswordModel);
                    }
                }
                else
                {
                    ViewBag.openPopup = CommonHelper.ShowAlertMessageToastr(MessageType.danger.ToString(), Messages.ResetPasswordRequest);
                    return View(Actions.ResetPassword, resetPasswordModel);
                }
            }
        }

        /// <summary>
        /// use to log out
        /// </summary>
        /// <returns>return action result</returns>
        [ActionName(Actions.Logout)]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction(Actions.LogIn, Pages.Controllers.Account);
        }
    }
}