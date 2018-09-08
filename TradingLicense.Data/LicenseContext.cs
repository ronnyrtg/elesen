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
        public DbSet<HOLIDAY> HOLIDAYs { get; set; }
        public DbSet<COMPANY> COMPANIES { get; set; }
        public DbSet<DEPARTMENT> DEPARTMENTs { get; set; }
        public DbSet<ADDRESS> ADDRESS { get; set; }
        public DbSet<PREMISETYPE> PREMISETYPEs { get; set; }
        public DbSet<INDIVIDUAL> INDIVIDUALs { get; set; }
        public DbSet<IND_L_COM> IND_L_COMs { get; set; }  // Individual Link Company
        public DbSet<IND_L_ATT> IND_L_ATTs { get; set; }  // Individual Link Attachments
        public DbSet<ZONE_M> ZONEs { get; set; }
        public DbSet<SUBZONE_M> SUBZONEs { get; set; }
        public DbSet<LOCATION> LOCATIONs { get; set; }
        public DbSet<ROAD_M> ROADs { get; set; }
        public DbSet<RACE_M> RACEs { get; set; }
        public DbSet<BANK_M> BANKs { get; set; }
        public DbSet<CITIZEN_M> CITIZENs { get; set; }

        //Combined Applications
        public DbSet<APPLICATION> APPLICATIONs { get; set; }
        public DbSet<LIC_TYPE> LIC_TYPEs { get; set; }
        public DbSet<BC> BCs { get; set; }
        public DbSet<BC_L_DEP> BC_L_DEPs { get; set; }
        public DbSet<BT> BTs { get; set; }
        public DbSet<RD> RDs { get; set; }
        public DbSet<RD_L_BT> RD_L_BTs { get; set; }
        public DbSet<RD_L_BC> RD_L_BCs { get; set; }
        public DbSet<RD_L_LT> RD_L_LTs { get; set; }       
        public DbSet<APP_L_IND> APP_L_INDs { get; set; }
        public DbSet<APP_L_RD> APP_L_RDs { get; set; }
        public DbSet<APP_L_BC> APP_L_BCs { get; set; }
        public DbSet<APP_L_MT> APP_L_MTs { get; set; }
        public DbSet<PROFILE> PROFILEs { get; set; }
        public DbSet<APP_LOG> APP_LOGs { get; set; }


        //Banner Application
        public DbSet<B_O> B_Os { get; set; } //Banner Object

        //Premise Application
        public DbSet<SECTOR> SECTORs { get; set; }               
        
        //Entertainment Premise Fee       
        public DbSet<E_P_FEE> E_P_FEEs { get; set; }

        //Common fields for all applications
        public DbSet<APPSTATUS> APPSTATUS { get; set; }        
        public DbSet<ROUTEUNIT> ROUTEUNITs { get; set; }
        public DbSet<COMMENT> COMMENTs { get; set; }
        public DbSet<CATATAN> CATATANs { get; set; }
        public DbSet<ATTACHMENT> ATTACHMENTs { get; set; }

        //User related
        public DbSet<USERS> USERS { get; set; }
        public DbSet<ROLE> ROLEs { get; set; }
        public DbSet<ACCESSPAGE> ACCESSPAGEs { get; set; }
        public DbSet<LOGINLOG> LOGINLOGs { get; set; }

        //For Developer  
        public DbSet<TUTORIAL> TUTORIALs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("C##LICENSING");
        }
    }
}

