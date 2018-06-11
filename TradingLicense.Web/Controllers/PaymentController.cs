using DataTables.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradingLicense.Data;
using TradingLicense.Entities;
using System.Linq.Dynamic;
using TradingLicense.Model;
using AutoMapper;

namespace TradingLicense.Web.Controllers
{
    public class PaymentController : Controller
    {
        #region PaymentDue

        /// <summary>
        /// GET: PaymentDue
        /// </summary>
        /// <returns></returns>

        public ActionResult PaymentDue()
        {
            return View();
        }

        /// <summary>
        /// Get PaymentDue Data by Individual
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PaymentDueByIndividual([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? individualId)
        {
            List<TradingLicense.Model.PaymentDueModel> PaymentDue = new List<Model.PaymentDueModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<PaymentDue> query = ctx.PaymentDues.Where(pd => pd.IndividualID == individualId);

                
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

                var result = Mapper.Map<List<PaymentDueModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "PaymentDueID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                PaymentDue = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, PaymentDue, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PaymentReceived

        /// <summary>
        /// GET: PaymentReceived
        /// </summary>
        /// <returns></returns>

        public ActionResult PaymentReceived()
        {
            return View();
        }

        /// <summary>
        /// Get PaymentReceived Data by Individual
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PaymentReceivedByIndividual([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, int? individualId)
        {
            List<TradingLicense.Model.PaymentReceivedModel> PaymentReceived = new List<Model.PaymentReceivedModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<PaymentReceived> query = ctx.PaymentReceiveds.Where(pr => pr.IndividualID == individualId);


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

                var result = Mapper.Map<List<PaymentReceivedModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "PaymentReceivedID asc" : orderByString).ToList();

                totalRecord = result.Count();

                #endregion Sorting

                // Paging
                result = result.Skip(requestModel.Start).Take(requestModel.Length).ToList();
                PaymentReceived = result;
            }
            return Json(new DataTablesResponse(requestModel.Draw, PaymentReceived, totalRecord, totalRecord), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}