using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using TradingLicense.Data;
using TradingLicense.Entities;
using TradingLicense.Infrastructure;
using TradingLicense.Model;
using TradingLicense.Web.Helpers;

namespace TradingLicense.Web.Services
{
    // TODO: create a dependency injection and add this service into controller
    // then remove static
    public class DocumentService
    {

        public static void UpdateDocs(ApplicationModel premiseApplicationModel, LicenseApplicationContext ctx, int premiseApplicationId, int roleTemplate)
        {
            string[] ids = premiseApplicationModel.UploadRequiredDocids.Split(',');
            var requiredDoclist = ids.Select(x =>
            {
                var splitted = x.Split(':');
                return new RequiredDocList
                {
                    RequiredDocID = Convert.ToInt32(splitted[0]),
                    AttachmentID = Convert.ToInt32(splitted[1])
                };
            }).ToList();

            var existingRecords = new List<int>();
            var dbEntryRequiredDoc = ctx.APP_L_RDs.Where(q => q.APP_ID == premiseApplicationId).ToList();
            if (dbEntryRequiredDoc.Count > 0)
            {
                foreach (var item in dbEntryRequiredDoc)
                {
                    if (!requiredDoclist.Any(q => q.RequiredDocID == item.RD_ID && q.AttachmentID == item.ATTACHMENTID))
                    {
                        if (roleTemplate == (int)Enums.RollTemplate.Public)
                        {
                            ctx.APP_L_RDs.Remove(item);
                        }
                    }
                    else
                    {
                        existingRecords.Add(item.RD_ID);
                    }
                }
                ctx.SaveChanges();
            }

            foreach (var requiredDoc in requiredDoclist)
            {
                if (existingRecords.All(q => q != requiredDoc.RequiredDocID))
                {
                    APP_L_RD pALinkReqDoc = new APP_L_RD();
                    pALinkReqDoc.APP_ID = premiseApplicationId;
                    pALinkReqDoc.RD_ID = requiredDoc.RequiredDocID;
                    pALinkReqDoc.ATTACHMENTID = requiredDoc.AttachmentID;
                    ctx.APP_L_RDs.AddOrUpdate(pALinkReqDoc);

                }
            }
            ctx.SaveChanges();
        }

        public static void UpdateRequiredDocs(ApplicationModel premiseApplicationModel, LicenseApplicationContext ctx,
            int premiseApplicationId, int roleTemplate)
        {
            var requiredDoclist = premiseApplicationModel.RequiredDocIds.ToIntList();

            List<int> existingRecord = new List<int>();
            var dbEntryRequiredDoc = ctx.APP_L_RDs.Where(q => q.RD_ID == premiseApplicationId).ToList();
            if (dbEntryRequiredDoc.Count > 0)
            {
                foreach (var item in dbEntryRequiredDoc)
                {
                    if (requiredDoclist.Any(q => q == item.RD_ID))
                    {
                        if (roleTemplate == (int)Enums.RollTemplate.Public || roleTemplate == (int)Enums.RollTemplate.DeskOfficer)
                        {
                            ctx.APP_L_RDs.Remove(item);
                        }
                    }
                    else
                    {
                        existingRecord.Add(item.RD_ID);
                    }
                }

                ctx.SaveChanges();
            }

            foreach (var requiredDoc in requiredDoclist)
            {
                if (existingRecord.All(q => q != requiredDoc))
                {
                    APP_L_RD pALinkReqDoc = new APP_L_RD();
                    pALinkReqDoc.APP_ID = premiseApplicationId;
                    pALinkReqDoc.RD_ID = requiredDoc;
                    ctx.APP_L_RDs.AddOrUpdate(pALinkReqDoc);
                }
            }
            ctx.SaveChanges();
        }

        public static void SaveAdditionalDocInfo(ApplicationModel premiseApplicationModel,
           LicenseApplicationContext ctx, int premiseApplicationId, int roleTemplate)
        {
            List<int> additionalDoclistlist = premiseApplicationModel.AdditionalDocIds.ToIntList();

            List<int> existingRecord = new List<int>();
            var dbEntryPaLinkAddDoc = ctx.APP_L_RDs.Where(q => q.APP_ID == premiseApplicationId).ToList();
            if (dbEntryPaLinkAddDoc.Count > 0)
            {
                foreach (var item in dbEntryPaLinkAddDoc)
                {
                    if (additionalDoclistlist.All(q => q != item.RD_ID))
                    {
                        if (roleTemplate == (int)Enums.RollTemplate.Public || roleTemplate == (int)Enums.RollTemplate.DeskOfficer)
                        {
                            ctx.APP_L_RDs.Remove(item);
                        }
                    }
                    else
                    {
                        existingRecord.Add(item.RD_ID);
                    }
                }

                ctx.SaveChanges();
            }

            foreach (var additionalDoc in additionalDoclistlist)
            {
                if (existingRecord.All(q => q != additionalDoc))
                {
                    APP_L_RD paLinkAddDoc = new APP_L_RD();
                    paLinkAddDoc.APP_ID = premiseApplicationId;
                    paLinkAddDoc.RD_ID = additionalDoc;
                    ctx.APP_L_RDs.Add(paLinkAddDoc);

                }
            }
            ctx.SaveChanges();
        }
    }
}