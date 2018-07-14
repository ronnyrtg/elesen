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
    public class ApplicationController : BaseController
    {

        private Func<BusinessCode, Select2ListItem> fnSelectBusinessCode = bc => new Select2ListItem { id = bc.BusinessCodeID, text = $"{bc.CodeDesc}~{bc.CodeNumber}" };
        private Func<Individual, Select2ListItem> fnSelectIndividualFormat = ind => new Select2ListItem { id = ind.IndividualID, text = $"{ind.FullName} ({ind.MykadNo})" };

        /// <summary>
        /// GET: Application
        /// </summary>
        /// <returns></returns>
        [AuthorizationPrivilegeFilter(SystemEnum.Page.Application, SystemEnum.PageRight.CrudLevel)]
        public ActionResult Application()
        {
            return View();
        }

        #region Application List Grid
        /// <summary>
        /// Get Application Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="ApplicationId">The premise application identifier.</param>
        /// <param name="individualMkNo">The individual mk no.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Application([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string ApplicationId, string individualMkNo)
        {
            List<ApplicationModel> Application;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                int? rollTemplateID = ProjectSession.User?.RoleTemplateID;
                IQueryable<APPLICATION> query = ctx.Applications;

                if (rollTemplateID.HasValue)
                {
                    switch (rollTemplateID)
                    {
                        case (int)RollTemplate.Public:
                            {
                                query = query.Where(q => q.USERSID == ProjectSession.UserID);
                            }
                            break;
                        case (int)RollTemplate.DeskOfficer:
                            {
                                query = query.Where(q => q.APPSTATUSID <= 3);
                            }
                            break;
                        case (int)RollTemplate.RouteUnit:
                            if (string.IsNullOrEmpty(ApplicationId))
                            {
                                var departmentID = ProjectSession.User?.DepartmentID;
                                if (departmentID.HasValue)
                                {
                                    var paIDs = ctx.RouteUnits
                                            .Where(pa => pa.DepartmentID == departmentID.Value && pa.ApplicationType == (int)Enums.ApplicationTypeID.Application && pa.Active)
                                            .Select(d => d.ApplicationID).Distinct()
                                            .ToList();
                                    query = query.Where(q => paIDs.Contains(q.APP_ID) && q.APPSTATUSID == 5);
                                }
                            }
                            break;
                        case (int)RollTemplate.CEO:
                            {
                                query = query.Where(q => q.APPSTATUSID == 8);
                            }
                            break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(ApplicationId))
                {
                    query = query.Where(q => q.APP_ID.ToString().Contains(ApplicationId));
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

        #region ManageApplication
        /// <summary>
        /// Get Application Data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ManageApplication(int? id)
        {
            ApplicationModel applicationModel = new ApplicationModel();
            applicationModel.START_RENT = DateTime.Today.AddMonths(-6);
            applicationModel.STOP_RENT = DateTime.Today.AddMonths(6);
            if (id != null && id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    var application = ctx.Applications.FirstOrDefault(a => a.APP_ID == id);
                    applicationModel = Mapper.Map<ApplicationModel>(application);

                    var paLinkBc = ctx.PALinkBC.Where(a => a.PremiseApplicationID == id).ToList();
                    applicationModel.BusinessCodeids = string.Join(",", paLinkBc.Select(x => x.BusinessCodeID.ToString()).ToArray());

                    //TODO: replaced with this for avoid calling database in foreach loop
                    // TODO: Select2ListItem is just the same as build-in KeyValuePair class
                    List<Select2ListItem> businessCodesList = new List<Select2ListItem>();
                    var ids = paLinkBc.Select(b => b.BusinessCodeID).ToList();
                    businessCodesList = ctx.BusinessCodes
                        .Where(b => ids.Contains(b.BusinessCodeID))
                        .Select(fnSelectBusinessCode)
                        .ToList();

                    applicationModel.selectedbusinessCodeList = businessCodesList;
                    applicationModel.HasPADepSupp = ctx.RouteUnits.Any(pa => pa.ApplicationID == id.Value);

                    var paLinkInd = ctx.PALinkInd.Where(a => a.PremiseApplicationID == id).ToList();
                    applicationModel.Individualids = string.Join(",", paLinkInd.Select(x => x.IndividualID.ToString()).ToArray());
                    List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
                    var iids = paLinkInd.Select(b => b.IndividualID).ToList();
                    selectedIndividualList = ctx.Individuals
                        .Where(b => iids.Contains(b.IndividualID))
                        .Select(fnSelectIndividualFormat)
                        .ToList();

                    applicationModel.selectedIndividualList = selectedIndividualList;

                    var paLinkReqDocumentList = ctx.PALinkReqDoc.Where(p => p.PremiseApplicationID == id).ToList();
                    if (paLinkReqDocumentList.Count > 0)
                    {
                        applicationModel.UploadRequiredDocids = (string.Join(",", paLinkReqDocumentList.Select(x => x.RequiredDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }

                    var paLinkAddDocumentlist = ctx.PALinkAddDocs.Where(p => p.PremiseApplicationID == id).ToList();
                    if (paLinkAddDocumentlist.Count > 0)
                    {
                        applicationModel.UploadAdditionalDocids = (string.Join(",", paLinkAddDocumentlist.Select(x => x.AdditionalDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }

                    if (application.APPSTATUSID == (int)PAStausenum.Pendingpayment)
                    {
                        var duePayment = ctx.PaymentDues.Where(pd => pd.PaymentFor == applicationModel.REF_NO).FirstOrDefault();
                        if (duePayment != null)
                        {
                            applicationModel.AmountDue = duePayment.AmountDue;
                        }
                    }
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                applicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
                applicationModel.USERSID = ProjectSession.User.UsersID;
            }


            applicationModel.IsDraft = false;
            return View(applicationModel);
        }
        #endregion

        #region Required Doc List Datatable
        /// <summary>
        /// Get Required Document Data
        /// </summary>
        /// <param name="requestModel">The request model.</param>
        /// <param name="businessTypeId">The business type identifier.</param>
        /// <param name="ApplicationId">The premise application identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RequiredDocument([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string businessTypeId)
        {
            List<BT_L_RDModel> requiredDocument;
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<BT_L_RD> query = ctx.BT_L_RD.Where(p => p.BT_ID.ToString().Contains(businessTypeId));

                #region Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                string orderByString = sortedColumns.GetOrderByString();

                var result = Mapper.Map<List<BT_L_RDModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "BT_L_RDID asc" : orderByString).ToList();

                totalRecord = result.Count;

                #endregion Sorting

                requiredDocument = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, requiredDocument, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}