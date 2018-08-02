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
using PdfSharp.Pdf;
using TradingLicense.Web.Helpers;
using TradingLicense.Web.Services;
using PdfSharp.Drawing.Layout;

namespace TradingLicense.Web.Controllers
{
    public class ProfileController : BaseController
    {
        private Func<BC, Select2ListItem> fnSelectBusinessCode = bc => new Select2ListItem { id = bc.BC_ID, text = $"{bc.C_R}~{bc.C_R_DESC}" };
        private Func<DEPARTMENT, Select2ListItem> fnSelectDepartment = de => new Select2ListItem { id = de.DEP_ID, text = $"{de.DEP_CODE}~{de.DEP_DESC}" };
        private Func<E_P_FEE, Select2ListItem> fnSelectPremiseFee = ep => new Select2ListItem { id = ep.E_P_FEEID, text = $"{ep.E_P_DESC}~{ep.E_S_DESC}" };
        private Func<INDIVIDUAL, Select2ListItem> fnSelectIndividualFormat = ind => new Select2ListItem { id = ind.IND_ID, text = $"{ind.FULLNAME} ({ind.MYKADNO})" };

        #region Profile List Grid

        /// <summary>
        /// GET: Application
        /// </summary>
        /// <returns></returns>
        [AuthorizationPrivilegeFilter(SystemEnum.Page.Profile, SystemEnum.PageRight.CrudLevel)]
        public ActionResult ProfileList()
        {
            return View();
        }

        /// <summary>
        /// Get Application Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="ApplicationId">The premise application identifier.</param>
        /// <param name="individualMkNo">The individual mk no.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ProfileList([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string ReferenceNo, int? LicenseTypeID)
        {
            List<ApplicationModel> Application;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                
                IQueryable<APPLICATION> query = ctx.APPLICATIONs.Where(m => m.APPSTATUSID == (int)Enums.PAStausenum.Complete);

                if (!string.IsNullOrWhiteSpace(ReferenceNo) && LicenseTypeID <= 0)
                {
                    query = query.Where(q => q.REF_NO.ToString().Contains(ReferenceNo));
                }
                else if (string.IsNullOrWhiteSpace(ReferenceNo) && LicenseTypeID > 0)
                {
                    query = query.Where(q => q.LIC_TYPEID == LicenseTypeID);
                }
                else if (!string.IsNullOrWhiteSpace(ReferenceNo) && LicenseTypeID > 0)
                {
                    query = query.Where(q => q.LIC_TYPEID == LicenseTypeID && q.REF_NO.ToString().Contains(ReferenceNo));
                }

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<ApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "APP_ID desc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                Application = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, Application, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region View Profile
        /// <summary>
        /// Get Profile Data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ViewProfile(int? id)
        {
            ApplicationModel applicationModel = new ApplicationModel();

            if (id != null && id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var application = ctx.APPLICATIONs.FirstOrDefault(a => a.APP_ID == id);
                    applicationModel = Mapper.Map<ApplicationModel>(application);

                    var paLinkBc = ctx.APP_L_BCs.Where(a => a.APP_ID == id).ToList();
                    applicationModel.BusinessCodeids = string.Join(",", paLinkBc.Select(x => x.BC_ID.ToString()).ToArray());

                    var routeSettled = ctx.ROUTEUNITs.Where(r => r.APP_ID == id);

                    var bannerObjects = ctx.B_Os.Where(a => a.APP_ID == id).ToList();
                    applicationModel.totalBannerObjects = bannerObjects.Count;

                    //TODO: replaced with this for avoid calling database in foreach loop
                    // TODO: Select2ListItem is just the same as build-in KeyValuePair class
                    List<Select2ListItem> businessCodesList = new List<Select2ListItem>();
                    var ids = paLinkBc.Select(b => b.BC_ID).ToList();
                    businessCodesList = ctx.BCs
                        .Where(b => ids.Contains(b.BC_ID))
                        .Select(fnSelectBusinessCode)
                        .ToList();

                    applicationModel.selectedbusinessCodeList = businessCodesList;

                    List<Select2ListItem> premiseFeeList = new List<Select2ListItem>();
                    premiseFeeList = ctx.E_P_FEEs
                        .Select(fnSelectPremiseFee)
                        .ToList();

                    applicationModel.selectedPremiseFeeList = premiseFeeList;

                    applicationModel.HasPADepSupp = ctx.ROUTEUNITs.Any(pa => pa.APP_ID == id.Value);

                    var paLinkInd = ctx.APP_L_INDs.Where(a => a.APP_ID == id).ToList();
                    applicationModel.Individualids = string.Join(",", paLinkInd.Select(x => x.IND_ID.ToString()).ToArray());
                    List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
                    var iids = paLinkInd.Select(b => b.IND_ID).ToList();
                    selectedIndividualList = ctx.INDIVIDUALs
                        .Where(b => iids.Contains(b.IND_ID))
                        .Select(fnSelectIndividualFormat)
                        .ToList();

                    applicationModel.selectedIndividualList = selectedIndividualList;

                    var paLinkReqDocumentList = ctx.APP_L_RDs.Where(p => p.APP_ID == id && p.RD_TYPE == 1).ToList();
                    if (paLinkReqDocumentList.Count > 0)
                    {
                        applicationModel.UploadRequiredDocids = (string.Join(",", paLinkReqDocumentList.Select(x => x.RD_ID.ToString() + ":" + x.ATT_ID.ToString()).ToArray()));
                    }
                    var paLinkLicDocumentList = ctx.APP_L_RDs.Where(p => p.APP_ID == id && p.RD_TYPE == 3).ToList();
                    if (paLinkLicDocumentList.Count > 0)
                    {
                        applicationModel.UploadLicenseDocids = (string.Join(",", paLinkLicDocumentList.Select(x => x.RD_ID.ToString() + ":" + x.ATT_ID.ToString()).ToArray()));
                    }

                    if (application.APPSTATUSID == (int)PAStausenum.Pendingpayment)
                    {

                    }
                }
            }
            else
            {

                applicationModel.START_RENT = DateTime.Today.AddMonths(-6);
                applicationModel.STOP_RENT = DateTime.Today.AddMonths(6);
                using (var ctx = new LicenseApplicationContext())
                {


                    List<Select2ListItem> premiseFeeList = new List<Select2ListItem>();
                    premiseFeeList = ctx.E_P_FEEs
                        .Select(fnSelectPremiseFee)
                        .ToList();

                    applicationModel.selectedPremiseFeeList = premiseFeeList;

                    var bannerObjects = ctx.B_Os.Where(a => a.APP_ID == id).ToList();
                    applicationModel.totalBannerObjects = bannerObjects.Count;
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.ROLEID > 0)
            {
                applicationModel.UserRollTemplate = ProjectSession.User.ROLEID.Value;
                applicationModel.USERSID = ProjectSession.User.USERSID;
            }


            applicationModel.IsDraft = false;
            return View(applicationModel);
        }
        #endregion

    }
}