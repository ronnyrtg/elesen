using System;
using System.Linq;
using TradingLicense.Data;
using TradingLicense.Entities;
using TradingLicense.Model;

namespace TradingLicense.Web.Services
{
    public class PaymentsService
    {
        public static PaymentDueModel AddPaymentDue(ApplicationModel premiseApplicationModel, LicenseApplicationContext ctx, string userName, float? totalDue = null)
        {
            totalDue = totalDue ?? CalculatePaymentDue(premiseApplicationModel, ctx);

            PaymentDueModel paymentDueModel = new PaymentDueModel();
            paymentDueModel.IND_IDs = string.Join(",", ctx.APP_L_INDs
                                                .Where(pa => pa.APP_ID == premiseApplicationModel.APP_ID)
                                                .AsEnumerable()
                                                .Select(pa => $"~{pa.IND_ID}~"));
            paymentDueModel.AMT_DUE = totalDue.Value;
            paymentDueModel.PAY_FOR = premiseApplicationModel.REF_NO;
            paymentDueModel.DATE_BILL = DateTime.Now;

            paymentDueModel.DUE_DATE = DateTime.Now.Date.AddMonths(1);
            paymentDueModel.BILL_BY = userName;

            var paymentDue = AutoMapper.Mapper.Map<PAY_DUE>(paymentDueModel);
            ctx.PAY_DUEs.Add(paymentDue);
            ctx.SaveChanges();

            return paymentDueModel;
        }

        public static float CalculatePaymentDue(ApplicationModel premiseApplicationModel, LicenseApplicationContext ctx)
        {
            var totalDue = 0.0f;

            var businessCodes = ctx.APP_L_BCs.Include("BusinessCode").Where(pa => pa.APP_ID == premiseApplicationModel.APP_ID).Select(pabc => pabc.BC);

            foreach (var bc in businessCodes)
            {
                if (bc.BASE_FEE == 0)
                {
                    totalDue += (premiseApplicationModel.P_AREA * (float)bc.DEF_RATE);
                }
                else
                {
                    totalDue += (float)bc.BASE_FEE;
                }
            }

            return totalDue;
        }

        public static PaymentReceivedModel AddPaymentReceived(ApplicationModel premiseApplicationModel, LicenseApplicationContext ctx, int individualID, string userName)
        {
            PaymentReceivedModel payment = new PaymentReceivedModel();
            payment.IND_ID = individualID;
            payment.PAY_FOR = premiseApplicationModel.REF_NO;
            payment.AMT_PAID = premiseApplicationModel.AmountDue;
            payment.DATE_PAID = DateTime.Now;
            payment.REC_BY = userName;

            var paymentReceived = AutoMapper.Mapper.Map<PAY_REC>(payment);
            ctx.PAY_RECs.Add(paymentReceived);
            ctx.SaveChanges();

            return payment;
        }
    }
}