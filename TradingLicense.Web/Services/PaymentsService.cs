using System;
using System.Linq;
using TradingLicense.Data;
using TradingLicense.Entities;
using TradingLicense.Model;

namespace TradingLicense.Web.Services
{
    public class PaymentsService
    {
        public static PaymentDueModel AddPaymentDue(PremiseApplicationModel premiseApplicationModel, LicenseApplicationContext ctx, string userName)
        {
            var totalDue = 0.0f;

            var businessCodes = ctx.PALinkBC.Include("BusinessCode").Where(pa => pa.PremiseApplicationID == premiseApplicationModel.PremiseApplicationID).Select(pabc => pabc.BusinessCode);

            foreach(var bc in businessCodes)
            {
                if(bc.BaseFee == 0)
                {
                    totalDue += (premiseApplicationModel.PremiseArea * bc.DefaultRate);
                }
                else
                {
                    totalDue += bc.BaseFee;
                }
            }

            PaymentDueModel paymentDueModel = new PaymentDueModel();
            paymentDueModel.IndividualIDs = string.Join(",", ctx.PALinkInd
                                                .Where(pa => pa.PremiseApplicationID == premiseApplicationModel.PremiseApplicationID)
                                                .Select(pa => $"~{pa.IndividualID}~"));
            paymentDueModel.AmountDue = totalDue;
            paymentDueModel.PaymentFor = premiseApplicationModel.ReferenceNo;
            paymentDueModel.DateBilled = DateTime.Now;

            paymentDueModel.DueDate = DateTime.Now.Date.AddMonths(1);
            paymentDueModel.BilledBy = userName;

            var paymentDue = AutoMapper.Mapper.Map<PaymentDue>(paymentDueModel);
            ctx.PaymentDues.Add(paymentDue);
            ctx.SaveChanges();

            return paymentDueModel;
        }
    }
}