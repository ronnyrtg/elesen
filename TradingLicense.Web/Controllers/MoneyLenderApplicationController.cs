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

namespace TradingLicense.Web.Controllers
{
    public class MoneyLenderApplicationController : BaseController
    {
        #region MLPremiseApplication

        /// <summary>
        /// GET: MLPremiseApplication
        /// </summary>
        /// <returns></returns>

        public ActionResult MLPremiseApplication()
        {
            return View();
        }

        /// <summary>
        /// GET: MLPermitApplication
        /// </summary>
        /// <returns></returns>
        public ActionResult MLPermitApplication()
        {
            return View();
        }

        /// <summary>
        /// Get MLPremiseApplication Data
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MLPremiseApplication([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string MLPremiseApplicationID, string IndividualMkNo)
        {
            List<TradingLicense.Model.MLPremiseApplicationModel> MLPremiseApplication = new List<Model.MLPremiseApplicationModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<MLPremiseApplication> query = (ProjectSession.User != null && ProjectSession.User.RoleTemplateID == (int)RollTemplate.Public) ? ctx.MLPremiseApplications.Where(p => p.UsersID == ProjectSession.User.UsersID) : ctx.MLPremiseApplications;

                if (!string.IsNullOrWhiteSpace(MLPremiseApplicationID))
                {
                    query = query.Where(q => q.MLPremiseApplicationID.ToString().Contains(MLPremiseApplicationID));
                }

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = String.Empty;

                foreach (var column in sortedColumns)
                {
                    orderByString += orderByString != String.Empty ? "," : "";
                    orderByString += (column.Data) +
                      (column.SortDirection ==
                      Column.OrderDirection.Ascendant ? " asc" : " desc");
                }

                var result = Mapper.Map<List<MLPremiseApplicationModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "MLPremiseApplicationID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                MLPremiseApplication = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, MLPremiseApplication, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }
        
        private Func<Individual, Select2ListItem> fnSelectIndividualFormat = ind => new Select2ListItem { id = ind.IndividualID, text = $"{ind.FullName} ({ind.MykadNo})" };

        /// <summary>
        /// Fill Individual data in Select2
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FillIndividual(string query)
        {
            using (var ctx = new LicenseApplicationContext())
            {
                var individual = ctx.Individuals
                                    .Where(t => t.MykadNo.ToLower().Contains(query.ToLower()) || t.FullName.ToLower().Contains(query.ToLower()))
                                    .Select(fnSelectIndividualFormat).ToList();
                return Json(individual, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Get MLPremiseApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageMLPremiseApplication(int? Id)
        {
            MLPremiseApplicationModel mLPremiseApplicationModel = new MLPremiseApplicationModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    //Retrieve data from MLPremiseApplication table where MLPremiseApplicationID == Id
                    int MLPremiseApplicationID = Convert.ToInt32(Id);
                    var mLPremiseApplication = ctx.MLPremiseApplications.Where(a => a.MLPremiseApplicationID == MLPremiseApplicationID).FirstOrDefault();
                    mLPremiseApplicationModel = Mapper.Map<MLPremiseApplicationModel>(mLPremiseApplication);


                    //Get list of Individuals linked to this application
                    var mlLinkInd = ctx.MLLinkInds.Where(a => a.MLPremiseApplicationID == Id).ToList();
                    mLPremiseApplicationModel.Individualids = string.Join(",", mlLinkInd.Select(x => x.IndividualID.ToString()).ToArray());
                    List<Select2ListItem> selectedIndividualList = new List<Select2ListItem>();
                    var iids = mlLinkInd.Select(b => b.IndividualID).ToList();
                    selectedIndividualList = ctx.Individuals
                        .Where(b => iids.Contains(b.IndividualID))
                        .Select(fnSelectIndividualFormat)
                        .ToList();

                    mLPremiseApplicationModel.selectedIndividualList = selectedIndividualList;

                    //Get list of Documents linked to this application
                    var MLLinkReqDocumentList = ctx.MLLinkReqDocs.Where(p => p.MLPremiseApplicationID == MLPremiseApplicationID).ToList();
                    if (MLLinkReqDocumentList != null && MLLinkReqDocumentList.Count > 0)
                    {
                        mLPremiseApplicationModel.UploadRequiredDocids = (string.Join(",", MLLinkReqDocumentList.Select(x => x.RequiredDocID.ToString() + ":" + x.AttachmentID.ToString()).ToArray()));
                    }
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                mLPremiseApplicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
                mLPremiseApplicationModel.UsersID = ProjectSession.User.UsersID;
            }
            mLPremiseApplicationModel.IsDraft = false;
            return View(mLPremiseApplicationModel);
        }

        /// <summary>
        /// Get MLPermitApplication Data by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ManageMLPermitApplication(int? Id)
        {
            MLPermitApplicationModel mLPermitApplicationModel = new MLPermitApplicationModel();
            if (Id != null && Id > 0)
            {
                using (var ctx = new LicenseApplicationContext())
                {
                    //Retrieve data from MLPermitApplication table where MLPermitApplicationID == Id
                    int MLPermitApplicationID = Convert.ToInt32(Id);
                    var mLPermitApplication = ctx.MLPermitApplications.Where(a => a.MLPermitApplicationID == MLPermitApplicationID).FirstOrDefault();
                    mLPermitApplicationModel = Mapper.Map<MLPermitApplicationModel>(mLPermitApplication);


                   

                   
                }
            }

            if (ProjectSession.User != null && ProjectSession.User.RoleTemplateID > 0)
            {
                mLPermitApplicationModel.UserRollTemplate = ProjectSession.User.RoleTemplateID.Value;
                mLPermitApplicationModel.UsersID = ProjectSession.User.UsersID;
            }
            mLPermitApplicationModel.IsDraft = false;
            return View(mLPermitApplicationModel);
        }

        /// <summary>
        /// get Individual name and Mykad data
        /// </summary>
        /// <param name="Individualids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Mykad([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, string Individualids)
        {
            List<TradingLicense.Model.IndividualModel> Individual = new List<Model.IndividualModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {

                string[] ids = null;
                if (!string.IsNullOrWhiteSpace(Individualids))
                {
                    ids = Individualids.Split(',');
                }

                List<int> Individuallist = new List<int>();

                foreach (string id in ids)
                {
                    int MLPremiseCodeID = Convert.ToInt32(id);
                    Individuallist.Add(MLPremiseCodeID);
                }

                IQueryable<Individual> query = ctx.Individuals.Where(r => Individuallist.Contains(r.IndividualID));

                #region Sorting
                // Sorting
                var sortedColumns = requestModel.Columns.GetSortedColumns();
                var orderByString = String.Empty;

                foreach (var column in sortedColumns)
                {
                    orderByString += orderByString != String.Empty ? "," : "";
                    orderByString += (column.Data) +
                      (column.SortDirection ==
                      Column.OrderDirection.Ascendant ? " asc" : " desc");
                }

                var result = Mapper.Map<List<IndividualModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "IndividualID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                Individual = result;

            }
            return Json(new DataTablesResponse(requestModel.Draw, Individual, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}