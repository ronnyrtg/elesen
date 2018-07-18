using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TradingLicense.Entities;

namespace TradingLicense.Data
{
    [DbConfigurationType(typeof(MyDbConfiguration))]
    public class LicenseApplicationContext : DbContext
    {

        public LicenseApplicationContext() : base("LicenseContext")
        { }

        //Master Setup
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<AdditionalDoc> AdditionalDocs { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<IndLinkCom> IndLinkComs { get; set; }  // Individual Link Company
        public DbSet<IndLinkAtt> IndLinkAtts { get; set; }  // Individual Link Attachments
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Road> Roads { get; set; }
        public DbSet<Race> Races { get; set; }

        //Combined Applications
        public DbSet<APPLICATION> Applications { get; set; }
        public DbSet<ADDRESS> ADDRESS { get; set; }
        public DbSet<BC> BCs { get; set; }
        public DbSet<BT> BT { get; set; }
        public DbSet<RD> RD { get; set; }
        public DbSet<BT_L_RD> BT_L_RD { get; set; }
        public DbSet<LIC_TYPE> LIC_TYPEs { get; set; }
        public DbSet<B_O> B_Os { get; set; }

        //Premise Application
        public DbSet<PremiseApplication> PremiseApplications { get; set; }
        public DbSet<PremiseType> PremiseTypes { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<BusinessCode> BusinessCodes { get; set; }
        public DbSet<BCLinkDep> BCLinkDeps { get; set; }
        public DbSet<BCLinkAD> BCLinkAD { get; set; }
        public DbSet<PALinkBC> PALinkBC { get; set; }
        public DbSet<PALinkInd> PALinkInd { get; set; }
        public DbSet<PALinkAddDoc> PALinkAddDocs { get; set; }
        public DbSet<BTLinkReqDoc> PALinkReqDocs { get; set; }
        public DbSet<PALinkReqDoc> PALinkReqDoc { get; set; }
        
        

        
               
        public DbSet<E_GROUP> E_GROUPs { get; set; }
        public DbSet<EntmtObject> EntmtObjects { get; set; }
        public DbSet<E_CODE> E_CODEs { get; set; }
        public DbSet<MLPermitApplication> MLPermitApplications { get; set; }
        

        //Common fields for all applications
        public DbSet<BusinessType> BusinessTypes { get; set; }
        public DbSet<AppStatus> AppStatus { get; set; }
        public DbSet<RequiredDoc> RequiredDocs { get; set; }
        public DbSet<PaymentDue> PaymentDues { get; set; }
        public DbSet<PaymentReceived> PaymentReceiveds { get; set; }
        public DbSet<RouteUnit> RouteUnits { get; set; }
        public DbSet<Comment> Comments { get; set; }
        


        //User related
        public DbSet<Users> Users { get; set; }
        public DbSet<ROLE> ROLEs { get; set; }
        public DbSet<AccessPage> AccessPages { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("C##LICENSING");
        }
    }
}

