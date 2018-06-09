﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TradingLicense.Entities;

namespace TradingLicense.Data
{
    [DbConfigurationType(typeof(MyDbConfiguration))]
    public class LicenseApplicationContext : DbContext
    {

        public LicenseApplicationContext() : base("LicenseContext")
        { }

        public DbSet<AccessPage> AccessPages { get; set; }

        public DbSet<AdditionalDoc> AdditionalDocs { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<BusinessCode> BusinessCodes { get; set; }

        public DbSet<BCLinkDep> BCLinkDeps { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Individual> Individuals { get; set; }

        public DbSet<IndLinkCom> IndLinkComs { get; set; }  // Individual Link Company

        public DbSet<IndLinkAtt> IndLinkAtts { get; set; }  // Individual Link Attachments

        public DbSet<LoginLog> LoginLogs { get; set; }

        public DbSet<BCLinkAD> BCLinkAD { get; set; }

        public DbSet<PALinkBC> PALinkBC { get; set; }

        public DbSet<BTLinkReqDoc> PALinkReqDocs { get; set; }

        public DbSet<AppStatus> AppStatus { get; set; }

        public DbSet<PremiseApplication> PremiseApplications { get; set; }

        public DbSet<PremiseType> PremiseTypes { get; set; }

        public DbSet<RequiredDoc> RequiredDocs { get; set; }

        public DbSet<RoleTemplate> RoleTemplates { get; set; }

        public DbSet<Sector> Sectors { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<BusinessType> BusinessTypes { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        public DbSet<PALinkAddDoc> PALinkAddDocs { get; set; }

        public DbSet<PALinkReqDoc> PALinkReqDoc { get; set; }

        public DbSet<PAComment> PAComments { get; set; }

        public DbSet<HawkerCode> HawkerCodes { get; set; }

        public DbSet<StallCode> StallCodes { get; set; }

        public DbSet<BannerCode> BannerCodes { get; set; }

        public DbSet<Zone> Zones { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Road> Roads { get; set; }

        public DbSet<LiquorCode> LiquorCodes { get; set; }

        public DbSet<BannerApplication> BannerApplications { get; set; }

        public DbSet<BannerObject> BannerObjects { get; set; }

        public DbSet<BAReqDoc> BAReqDocs { get; set; }

        public DbSet<BALinkReqDoc> BALinkReqDocs { get; set; }

        public DbSet<EntmtGroup> EntmtGroups { get; set; }

        public DbSet<EntmtObject> EntmtObjects { get; set; }

        public DbSet<EntmtCode> EntmtCodes { get; set; }

        public DbSet<EntmtApplication> EntmtApplications { get; set; }

        public DbSet<EALinkReqDoc> EALinkReqDocs { get; set; }

        public DbSet<EALinkEC> EALinkEC { get; set; }

        public DbSet<EALinkInd> EALinkInds { get; set; }

        public DbSet<ECLinkDep> ECLinkDeps { get; set; }

        public DbSet<EAComment> EAComments { get; set; }

        public DbSet<StallApplication> StallApplications { get; set; }

        public DbSet<SAReqDoc> SAReqDocs { get; set; }

        public DbSet<SALinkReqDoc> SALinkReqDocs { get; set; }

        public DbSet<HawkerApplication> HawkerApplications { get; set; }

        public DbSet<HAReqDoc> HAReqDocs { get; set; }

        public DbSet<PaymentDue> PaymentDues { get; set; }

        public DbSet<PaymentReceived> PaymentReceiveds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("C##LICENSING");
        }
    }
}

