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
                return RedirectToAction(Actions.Application, Pages.Controllers.Application);
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
                    try
                    {
                        var userModel = ctx.USERS.Where(u => u.USERNAME == model.Username).FirstOrDefault();

                        if (userModel != null && userModel.USERSID > 0)
                        {
                            if (userModel.LOCKED != 1)
                            {
                                string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
                                string ipAddress = Dns.GetHostByName(hostName).AddressList[0].ToString();
                                LOGINLOG loginLog = new LOGINLOG();
                                loginLog.LOGDATE = DateTime.Now;
                                loginLog.USERSID = userModel.USERSID;
                                loginLog.IPADDRESS = ipAddress;
                                loginLog.LOGDESC = userModel.USERNAME;

                                var result = Infrastructure.EncryptionDecryption.GetEncrypt(model.Password);
                                if (userModel.PASSWORD == Infrastructure.EncryptionDecryption.GetEncrypt(model.Password))
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

                                    loginLog.LOGINSTATUS = true;
                                    ctx.LOGINLOGs.AddOrUpdate(loginLog);
                                    ctx.SaveChanges();

                                    UsersModel user = new UsersModel();
                                    user = Mapper.Map<UsersModel>(userModel);
                                    ProjectSession.UserID = userModel.USERSID;
                                    ProjectSession.User = user;

                                    FormsAuthentication.SetAuthCookie(model.Username, false);

                                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                                    {
                                        return Redirect(model.ReturnUrl);
                                    }
                                    else
                                    {
                                        return RedirectToAction(Actions.Application, Pages.Controllers.Application);
                                    }
                                }
                                else
                                {
                                    loginLog.LOGINSTATUS = false;
                                    ctx.LOGINLOGs.AddOrUpdate(loginLog);
                                    ctx.SaveChanges();

                                    DateTime fromDate = DateTime.Now.AddMinutes(-30);
                                    DateTime toDate = DateTime.Now;

                                    var loginLoglist = ctx.LOGINLOGs.Where(l => l.USERSID == userModel.USERSID && l.LOGDATE >= fromDate && l.LOGDATE < toDate && l.LOGINSTATUS == false).ToList();
                                    if (loginLoglist != null && loginLoglist.Count() == 5 || loginLoglist.Count() > 5)
                                    {
                                        userModel.LOCKED = 1;
                                        ctx.USERS.AddOrUpdate(userModel);
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
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message.ToString());
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
                    var user = ctx.USERS.Where(a => a.EMAIL == emailValue).FirstOrDefault();
                    if (user != null && user.USERSID > 0)
                    {
                        string resetPasswordParameter = string.Format("{0}#{1}#{2}", SystemEnum.RoleType.User.GetHashCode(), user.USERSID, DateTime.Now.AddMinutes(ProjectConfiguration.ResetPasswordExpireTime).ToString(ProjectConfiguration.EmailDateTimeFormat));
                        string encryptResetPasswordParameter = EncryptionDecryption.GetEncrypt(resetPasswordParameter);
                        string encryptResetPasswordUrl = string.Format("{0}?q={1}", ProjectConfiguration.SiteUrlBase + TradingLicense.Web.Pages.Controllers.Account + "/" + Actions.ResetPassword, encryptResetPasswordParameter);

                        if (UserMail.SendForgotPassword(user.EMAIL, user.USERNAME, encryptResetPasswordUrl))
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
            catch
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
                                var user = ctx.USERS.Where(u => u.USERSID == id).FirstOrDefault();
                                resetPasswordModel.RoleTypeId = roleTypeId;
                                if (user != null && user.USERSID > 0)
                                {
                                    ViewBag.IsVisible = 1;
                                    resetPasswordModel.Id = user.USERSID;
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

                    var user = ctx.USERS.Where(u => u.USERSID == resetPasswordModel.Id).FirstOrDefault();
                    if (user != null && user.USERSID > 0)
                    {
                        user.PASSWORD = EncryptionDecryption.GetEncrypt(resetPasswordModel.NewPassword);
                        ctx.USERS.AddOrUpdate(user);
                        ctx.SaveChanges();

                        UsersModel usermodel = new UsersModel();
                        usermodel = Mapper.Map<UsersModel>(user);
                        ProjectSession.UserID = user.USERSID;
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