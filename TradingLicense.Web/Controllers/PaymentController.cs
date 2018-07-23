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
        [HttpGet]
        public JsonResult PaymentDueByIndividual(int? individualId)
        {
            List<TradingLicense.Model.PaymentDueModel> PaymentDue = new List<Model.PaymentDueModel>();
            int totalRecord = 0;
            using (var ctx = new LicenseApplicationContext())
            {
                try
                {
                    //IQueryable<PaymentDue> query = ctx.PaymentDues.Where(pd => pd.IndividualIDs.Contains($"~{individualId}~"));
                    IQueryable<PAY_DUE> query = ctx.PAY_DUEs.ToList().AsQueryable().Where(pd => pd.IND_IDS.Contains(individualId.ToString()));
                    #region Sorting
                    // Sorting
                    var orderByString = String.Empty;
                    var result = Mapper.Map<List<PaymentDueModel>>(query.ToList());
                    result = result.OrderBy(orderByString == string.Empty ? "PAY_DUEID asc" : orderByString).ToList();
                    totalRecord = result.Count();
                    #endregion Sorting
                    PaymentDue = result;
                }
                catch
                {
                    PaymentDue = null;
                }
            }
            return Json(PaymentDue, JsonRequestBehavior.AllowGet);
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
        [HttpGet]
        public JsonResult PaymentReceivedByIndividual(int? individualId)
        {
            List<TradingLicense.Model.PaymentReceivedModel> PaymentReceived = new List<Model.PaymentReceivedModel>();
            using (var ctx = new LicenseApplicationContext())
            {
                IQueryable<PAY_REC> query = ctx.PAY_RECs.Where(pr => pr.IND_ID == individualId).OrderByDescending(o => o.DATE_PAID).Take(20);

                #region Sorting
                // Sorting
                var orderByString = String.Empty;

                var result = Mapper.Map<List<PaymentReceivedModel>>(query.ToList());
                result = result.OrderBy(orderByString == string.Empty ? "PAY_RECID asc" : orderByString).ToList();

                #endregion Sorting

                // Paging
                PaymentReceived = result;
            }
            return Json(PaymentReceived, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}