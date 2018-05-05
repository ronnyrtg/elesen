﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingLicense.Entities;

namespace TradingLicense.Data
{
    [DbConfigurationType(typeof(MyDbConfiguration))]
    public class LicenseApplicationContext : DbContext
    {

        public LicenseApplicationContext() : base("LicenseContext")
        { }

        public DbSet<AccessPage> AccessPages { get; set; }

        public DbSet<AdditionalDoc> AdditionalInfos { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<BusinessCode> BusinessCodes { get; set; }

        public DbSet<BCLinkDep> BussCodLinkDeps { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Individual> Individuals { get; set; }

        public DbSet<IndLinkCom> IndLinkComs { get; set; }  // Individual Link Com

        public DbSet<LoginLog> LoginLogs { get; set; }

        public DbSet<BCLinkAD> PALinkAIs { get; set; }

        public DbSet<PALinkBC> PALinkBActs { get; set; }

        public DbSet<BTLinkReqDoc> PALinkReqDocs { get; set; }

        public DbSet<PALinkSign> PALinkSigns { get; set; }

        public DbSet<PAStatus> PAStatus { get; set; }

        public DbSet<PremiseApplication> PremiseApplications { get; set; }

        public DbSet<PremiseType> PremiseTypes { get; set; }

        public DbSet<RequiredDoc> RequiredDocs { get; set; }

        public DbSet<RoleTemplate> RoleTemplates { get; set; }

        public DbSet<Sector> Sectors { get; set; }

        public DbSet<Signboard> Signboards { get; set; }

        public DbSet<SupportDocs> SupportDocs { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<BusinessType> BusinessTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("LICENSING");
        }
    }
}

