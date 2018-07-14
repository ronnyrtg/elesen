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
        public static void UploadAdditionalDocs(PremiseApplicationModel premiseApplicationModel, LicenseApplicationContext ctx,
            int premiseApplicationId, int roleTemplate)
        {
            string[] ids = premiseApplicationModel.UploadAdditionalDocids.Split(',');
            var additionalDoclistlist = ids.Select(x =>
            {
                var splitted = x.Split(':');
                return new AdditionalDocList
                {
                    AdditionalDocID = Convert.ToInt32(splitted[0]),
                    AttachmentID = Convert.ToInt32(splitted[1])
                };
            }).ToList();

            var existingRecord = new List<int>();
            var dbEntryPaLinkAddDoc = ctx.PALinkAddDocs.Where(q => q.PremiseApplicationID == premiseApplicationId).ToList();
            if (dbEntryPaLinkAddDoc.Count > 0)
            {
                foreach (var item in dbEntryPaLinkAddDoc)
                {
                    if (!additionalDoclistlist.Any(q => q.AdditionalDocID == item.AdditionalDocID && q.AttachmentID == item.AttachmentID))
                    {
                        if (roleTemplate == (int)Enums.RollTemplate.Public)
                        {
                            ctx.PALinkAddDocs.Remove(item);
                        }
                    }
                    else
                    {
                        existingRecord.Add(item.AdditionalDocID);
                    }
                }

                ctx.SaveChanges();
            }

            foreach (var additionalDoc in additionalDoclistlist)
            {
                if (existingRecord.All(q => q != additionalDoc.AdditionalDocID))
                {
                    PALinkAddDoc paLinkAddDoc = new PALinkAddDoc();
                    paLinkAddDoc.PremiseApplicationID = premiseApplicationId;
                    paLinkAddDoc.AdditionalDocID = additionalDoc.AdditionalDocID;
                    paLinkAddDoc.AttachmentID = additionalDoc.AttachmentID;
                    ctx.PALinkAddDocs.Add(paLinkAddDoc);
                }
            }
            ctx.SaveChanges();
        }

        public static void UpdateDocs(PremiseApplicationModel premiseApplicationModel, LicenseApplicationContext ctx, int premiseApplicationId, int roleTemplate)
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
            var dbEntryRequiredDoc = ctx.PALinkReqDoc.Where(q => q.PremiseApplicationID == premiseApplicationId).ToList();
            if (dbEntryRequiredDoc.Count > 0)
            {
                foreach (var item in dbEntryRequiredDoc)
                {
                    if (!requiredDoclist.Any(q => q.RequiredDocID == item.RequiredDocID && q.AttachmentID == item.AttachmentID))
                    {
                        if (roleTemplate == (int)Enums.RollTemplate.Public)
                        {
                            ctx.PALinkReqDoc.Remove(item);
                        }
                    }
                    else
                    {
                        existingRecords.Add(item.RequiredDocID);
                    }
                }
                ctx.SaveChanges();
            }

            foreach (var requiredDoc in requiredDoclist)
            {
                if (existingRecords.All(q => q != requiredDoc.RequiredDocID))
                {
                    PALinkReqDoc pALinkReqDoc = new PALinkReqDoc();
                    pALinkReqDoc.PremiseApplicationID = premiseApplicationId;
                    pALinkReqDoc.RequiredDocID = requiredDoc.RequiredDocID;
                    pALinkReqDoc.AttachmentID = requiredDoc.AttachmentID;
                    ctx.PALinkReqDoc.AddOrUpdate(pALinkReqDoc);

                }
            }
            ctx.SaveChanges();
        }

        public static void UpdateRequiredDocs(PremiseApplicationModel premiseApplicationModel, LicenseApplicationContext ctx,
            int premiseApplicationId, int roleTemplate)
        {
            var requiredDoclist = premiseApplicationModel.RequiredDocIds.ToIntList();

            List<int> existingRecord = new List<int>();
            var dbEntryRequiredDoc = ctx.PALinkReqDoc.Where(q => q.PremiseApplicationID == premiseApplicationId).ToList();
            if (dbEntryRequiredDoc.Count > 0)
            {
                foreach (var item in dbEntryRequiredDoc)
                {
                    if (requiredDoclist.Any(q => q == item.RequiredDocID))
                    {
                        if (roleTemplate == (int)Enums.RollTemplate.Public || roleTemplate == (int)Enums.RollTemplate.DeskOfficer)
                        {
                            ctx.PALinkReqDoc.Remove(item);
                        }
                    }
                    else
                    {
                        existingRecord.Add(item.RequiredDocID);
                    }
                }

                ctx.SaveChanges();
            }

            foreach (var requiredDoc in requiredDoclist)
            {
                if (existingRecord.All(q => q != requiredDoc))
                {
                    PALinkReqDoc pALinkReqDoc = new PALinkReqDoc();
                    pALinkReqDoc.PremiseApplicationID = premiseApplicationId;
                    pALinkReqDoc.RequiredDocID = requiredDoc;
                    ctx.PALinkReqDoc.AddOrUpdate(pALinkReqDoc);
                }
            }
            ctx.SaveChanges();
        }

        public static void SaveAdditionalDocInfo(PremiseApplicationModel premiseApplicationModel,
           LicenseApplicationContext ctx, int premiseApplicationId, int roleTemplate)
        {
            List<int> additionalDoclistlist = premiseApplicationModel.AdditionalDocIds.ToIntList();

            List<int> existingRecord = new List<int>();
            var dbEntryPaLinkAddDoc = ctx.PALinkAddDocs.Where(q => q.PremiseApplicationID == premiseApplicationId).ToList();
            if (dbEntryPaLinkAddDoc.Count > 0)
            {
                foreach (var item in dbEntryPaLinkAddDoc)
                {
                    if (additionalDoclistlist.All(q => q != item.AdditionalDocID))
                    {
                        if (roleTemplate == (int)Enums.RollTemplate.Public || roleTemplate == (int)Enums.RollTemplate.DeskOfficer)
                        {
                            ctx.PALinkAddDocs.Remove(item);
                        }
                    }
                    else
                    {
                        existingRecord.Add(item.AdditionalDocID);
                    }
                }

                ctx.SaveChanges();
            }

            foreach (var additionalDoc in additionalDoclistlist)
            {
                if (existingRecord.All(q => q != additionalDoc))
                {
                    PALinkAddDoc paLinkAddDoc = new PALinkAddDoc();
                    paLinkAddDoc.PremiseApplicationID = premiseApplicationId;
                    paLinkAddDoc.AdditionalDocID = additionalDoc;
                    ctx.PALinkAddDocs.Add(paLinkAddDoc);

                }
            }
            ctx.SaveChanges();
        }
    }
}