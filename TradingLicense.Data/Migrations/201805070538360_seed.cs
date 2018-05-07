namespace TradingLicense.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "LICENSING.AccessPage",
                c => new
                    {
                        AccessPageID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PageDesc = c.String(nullable: false, maxLength: 100, unicode: false),
                        ScreenId = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CrudLevel = c.Decimal(nullable: false, precision: 10, scale: 0),
                        RoleTemplateID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.AccessPageID)
                .ForeignKey("LICENSING.RoleTemplate", t => t.RoleTemplateID, cascadeDelete: true)
                .Index(t => t.RoleTemplateID);
            
            CreateTable(
                "LICENSING.RoleTemplate",
                c => new
                    {
                        RoleTemplateID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        RoleTemplateDesc = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.RoleTemplateID);
            
            CreateTable(
                "LICENSING.AdditionalDoc",
                c => new
                    {
                        AdditionalDocID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DocDesc = c.String(maxLength: 255, unicode: false),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.AdditionalDocID);
            
            CreateTable(
                "LICENSING.Attachment",
                c => new
                    {
                        AttachmentID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        FileName = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.AttachmentID);
            
            CreateTable(
                "LICENSING.BCLinkAD",
                c => new
                    {
                        BCLinkADID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BusinessCodeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        AdditionalDocID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.BCLinkADID)
                .ForeignKey("LICENSING.AdditionalDoc", t => t.AdditionalDocID, cascadeDelete: true)
                .Index(t => t.AdditionalDocID);
            
            CreateTable(
                "LICENSING.BCLinkDep",
                c => new
                    {
                        BussCodLinkDepID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BusinessCodeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DepartmentID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.BussCodLinkDepID)
                .ForeignKey("LICENSING.BusinessCode", t => t.BusinessCodeID, cascadeDelete: true)
                .ForeignKey("LICENSING.Department", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.BusinessCodeID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "LICENSING.BusinessCode",
                c => new
                    {
                        BusinessCodeID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        CodeNumber = c.String(maxLength: 5, unicode: false),
                        CodeDesc = c.String(maxLength: 255, unicode: false),
                        SectorID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DefaultRate = c.Single(nullable: false),
                        BaseFee = c.Single(nullable: false),
                        ExtraFee = c.Single(nullable: false),
                        ExtraUnit = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Period = c.String(maxLength: 1, unicode: false),
                        PQuantity = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Express = c.Decimal(nullable: false, precision: 1, scale: 0),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.BusinessCodeID)
                .ForeignKey("LICENSING.Sector", t => t.SectorID, cascadeDelete: true)
                .Index(t => t.SectorID);
            
            CreateTable(
                "LICENSING.Sector",
                c => new
                    {
                        SectorID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        SectorDesc = c.String(maxLength: 30, unicode: false),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.SectorID);
            
            CreateTable(
                "LICENSING.Department",
                c => new
                    {
                        DepartmentID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DepartmentCode = c.String(nullable: false, maxLength: 10, unicode: false),
                        DepartmentDesc = c.String(nullable: false, maxLength: 255, unicode: false),
                        Internal = c.Decimal(nullable: false, precision: 1, scale: 0),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "LICENSING.BusinessType",
                c => new
                    {
                        BusinessTypeID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BusinessTypeCode = c.String(nullable: false, maxLength: 1, unicode: false),
                        BusinessTypeDesc = c.String(nullable: false, maxLength: 255, unicode: false),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.BusinessTypeID);
            
            CreateTable(
                "LICENSING.Company",
                c => new
                    {
                        CompanyID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        RegistrationNo = c.String(maxLength: 50, unicode: false),
                        CompanyName = c.String(maxLength: 100, unicode: false),
                        CompanyAddress = c.String(maxLength: 255, unicode: false),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.CompanyID);
            
            CreateTable(
                "LICENSING.Holiday",
                c => new
                    {
                        HolidayID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        HolidayDesc = c.String(maxLength: 100, unicode: false),
                        HolidayDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.HolidayID);
            
            CreateTable(
                "LICENSING.Individual",
                c => new
                    {
                        IndividualID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        FullName = c.String(maxLength: 50, unicode: false),
                        MykadNo = c.String(maxLength: 30, unicode: false),
                        NationalityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        AddressIC = c.String(maxLength: 200, unicode: false),
                        PhoneNo = c.String(maxLength: 200, unicode: false),
                        IndividualEmail = c.String(maxLength: 200, unicode: false),
                        Gender = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Rental = c.Single(nullable: false),
                        Assessment = c.Single(nullable: false),
                        Compound = c.Single(nullable: false),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.IndividualID);
            
            CreateTable(
                "LICENSING.IndLinkCom",
                c => new
                    {
                        IndLinkComID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        IndividualID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        CompanyID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.IndLinkComID)
                .ForeignKey("LICENSING.Company", t => t.CompanyID, cascadeDelete: true)
                .ForeignKey("LICENSING.Individual", t => t.IndividualID, cascadeDelete: true)
                .Index(t => t.IndividualID)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "LICENSING.LoginLog",
                c => new
                    {
                        LoginLogID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        LogDate = c.DateTime(nullable: false),
                        LogDesc = c.String(nullable: false, maxLength: 100, unicode: false),
                        IpAddress = c.String(nullable: false, maxLength: 20, unicode: false),
                        LoginStatus = c.Decimal(nullable: false, precision: 1, scale: 0),
                        UsersID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.LoginLogID)
                .ForeignKey("LICENSING.Users", t => t.UsersID, cascadeDelete: true)
                .Index(t => t.UsersID);
            
            CreateTable(
                "LICENSING.Users",
                c => new
                    {
                        UsersID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        FullName = c.String(maxLength: 50, unicode: false),
                        Username = c.String(maxLength: 30, unicode: false),
                        Email = c.String(maxLength: 200, unicode: false),
                        Password = c.String(maxLength: 32, unicode: false),
                        RoleTemplateID = c.Decimal(precision: 10, scale: 0),
                        DepartmentID = c.Decimal(precision: 10, scale: 0),
                        Locked = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.UsersID)
                .ForeignKey("LICENSING.Department", t => t.DepartmentID)
                .ForeignKey("LICENSING.RoleTemplate", t => t.RoleTemplateID)
                .Index(t => t.RoleTemplateID)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "LICENSING.PALinkBC",
                c => new
                    {
                        PALinkBCID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PremiseApplicationID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        BusinessCodeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.PALinkBCID)
                .ForeignKey("LICENSING.BusinessCode", t => t.BusinessCodeID, cascadeDelete: true)
                .ForeignKey("LICENSING.PremiseApplication", t => t.PremiseApplicationID, cascadeDelete: true)
                .Index(t => t.PremiseApplicationID)
                .Index(t => t.BusinessCodeID);
            
            CreateTable(
                "LICENSING.PremiseApplication",
                c => new
                    {
                        PremiseApplicationID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BusinessTypeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        IndividualID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PremiseAddress = c.String(maxLength: 255, unicode: false),
                        PremiseStatus = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PremiseTypeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        PremiseModification = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DateSubmitted = c.DateTime(nullable: false),
                        UpdatedBy = c.String(maxLength: 50, unicode: false),
                        PAStatusID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.PremiseApplicationID)
                .ForeignKey("LICENSING.BusinessType", t => t.BusinessTypeID, cascadeDelete: true)
                .ForeignKey("LICENSING.Individual", t => t.IndividualID, cascadeDelete: true)
                .ForeignKey("LICENSING.PAStatus", t => t.PAStatusID, cascadeDelete: true)
                .ForeignKey("LICENSING.PremiseType", t => t.PremiseTypeID, cascadeDelete: true)
                .Index(t => t.BusinessTypeID)
                .Index(t => t.IndividualID)
                .Index(t => t.PremiseTypeID)
                .Index(t => t.PAStatusID);
            
            CreateTable(
                "LICENSING.PAStatus",
                c => new
                    {
                        PAStatusID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        StatusDesc = c.String(maxLength: 100, unicode: false),
                        PercentProgress = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.PAStatusID);
            
            CreateTable(
                "LICENSING.PremiseType",
                c => new
                    {
                        PremiseTypeID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PremiseDesc = c.String(nullable: false, maxLength: 255, unicode: false),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.PremiseTypeID);
            
            CreateTable(
                "LICENSING.BTLinkReqDoc",
                c => new
                    {
                        BTLinkReqDocID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BusinessTypeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        RequiredDocID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.BTLinkReqDocID)
                .ForeignKey("LICENSING.BusinessType", t => t.BusinessTypeID, cascadeDelete: true)
                .ForeignKey("LICENSING.RequiredDoc", t => t.RequiredDocID, cascadeDelete: true)
                .Index(t => t.BusinessTypeID)
                .Index(t => t.RequiredDocID);
            
            CreateTable(
                "LICENSING.RequiredDoc",
                c => new
                    {
                        RequiredDocID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        RequiredDocDesc = c.String(nullable: false, maxLength: 255, unicode: false),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.RequiredDocID);
            
            CreateTable(
                "LICENSING.PALinkSign",
                c => new
                    {
                        PALinkSignID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PAID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SignboardID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.PALinkSignID)
                .ForeignKey("LICENSING.Signboard", t => t.SignboardID, cascadeDelete: true)
                .Index(t => t.SignboardID);
            
            CreateTable(
                "LICENSING.Signboard",
                c => new
                    {
                        SignboardID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        Length = c.Single(nullable: false),
                        Width = c.Single(nullable: false),
                        WithLamp = c.Decimal(nullable: false, precision: 10, scale: 0),
                        Quantity = c.Decimal(nullable: false, precision: 10, scale: 0),
                        FaceQty = c.Decimal(nullable: false, precision: 10, scale: 0),
                        DisplayMethod = c.String(maxLength: 15, unicode: false),
                        Location = c.String(maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.SignboardID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("LICENSING.PALinkSign", "SignboardID", "LICENSING.Signboard");
            DropForeignKey("LICENSING.BTLinkReqDoc", "RequiredDocID", "LICENSING.RequiredDoc");
            DropForeignKey("LICENSING.BTLinkReqDoc", "BusinessTypeID", "LICENSING.BusinessType");
            DropForeignKey("LICENSING.PALinkBC", "PremiseApplicationID", "LICENSING.PremiseApplication");
            DropForeignKey("LICENSING.PremiseApplication", "PremiseTypeID", "LICENSING.PremiseType");
            DropForeignKey("LICENSING.PremiseApplication", "PAStatusID", "LICENSING.PAStatus");
            DropForeignKey("LICENSING.PremiseApplication", "IndividualID", "LICENSING.Individual");
            DropForeignKey("LICENSING.PremiseApplication", "BusinessTypeID", "LICENSING.BusinessType");
            DropForeignKey("LICENSING.PALinkBC", "BusinessCodeID", "LICENSING.BusinessCode");
            DropForeignKey("LICENSING.LoginLog", "UsersID", "LICENSING.Users");
            DropForeignKey("LICENSING.Users", "RoleTemplateID", "LICENSING.RoleTemplate");
            DropForeignKey("LICENSING.Users", "DepartmentID", "LICENSING.Department");
            DropForeignKey("LICENSING.IndLinkCom", "IndividualID", "LICENSING.Individual");
            DropForeignKey("LICENSING.IndLinkCom", "CompanyID", "LICENSING.Company");
            DropForeignKey("LICENSING.BCLinkDep", "DepartmentID", "LICENSING.Department");
            DropForeignKey("LICENSING.BCLinkDep", "BusinessCodeID", "LICENSING.BusinessCode");
            DropForeignKey("LICENSING.BusinessCode", "SectorID", "LICENSING.Sector");
            DropForeignKey("LICENSING.BCLinkAD", "AdditionalDocID", "LICENSING.AdditionalDoc");
            DropForeignKey("LICENSING.AccessPage", "RoleTemplateID", "LICENSING.RoleTemplate");
            DropIndex("LICENSING.PALinkSign", new[] { "SignboardID" });
            DropIndex("LICENSING.BTLinkReqDoc", new[] { "RequiredDocID" });
            DropIndex("LICENSING.BTLinkReqDoc", new[] { "BusinessTypeID" });
            DropIndex("LICENSING.PremiseApplication", new[] { "PAStatusID" });
            DropIndex("LICENSING.PremiseApplication", new[] { "PremiseTypeID" });
            DropIndex("LICENSING.PremiseApplication", new[] { "IndividualID" });
            DropIndex("LICENSING.PremiseApplication", new[] { "BusinessTypeID" });
            DropIndex("LICENSING.PALinkBC", new[] { "BusinessCodeID" });
            DropIndex("LICENSING.PALinkBC", new[] { "PremiseApplicationID" });
            DropIndex("LICENSING.Users", new[] { "DepartmentID" });
            DropIndex("LICENSING.Users", new[] { "RoleTemplateID" });
            DropIndex("LICENSING.LoginLog", new[] { "UsersID" });
            DropIndex("LICENSING.IndLinkCom", new[] { "CompanyID" });
            DropIndex("LICENSING.IndLinkCom", new[] { "IndividualID" });
            DropIndex("LICENSING.BusinessCode", new[] { "SectorID" });
            DropIndex("LICENSING.BCLinkDep", new[] { "DepartmentID" });
            DropIndex("LICENSING.BCLinkDep", new[] { "BusinessCodeID" });
            DropIndex("LICENSING.BCLinkAD", new[] { "AdditionalDocID" });
            DropIndex("LICENSING.AccessPage", new[] { "RoleTemplateID" });
            DropTable("LICENSING.Signboard");
            DropTable("LICENSING.PALinkSign");
            DropTable("LICENSING.RequiredDoc");
            DropTable("LICENSING.BTLinkReqDoc");
            DropTable("LICENSING.PremiseType");
            DropTable("LICENSING.PAStatus");
            DropTable("LICENSING.PremiseApplication");
            DropTable("LICENSING.PALinkBC");
            DropTable("LICENSING.Users");
            DropTable("LICENSING.LoginLog");
            DropTable("LICENSING.IndLinkCom");
            DropTable("LICENSING.Individual");
            DropTable("LICENSING.Holiday");
            DropTable("LICENSING.Company");
            DropTable("LICENSING.BusinessType");
            DropTable("LICENSING.Department");
            DropTable("LICENSING.Sector");
            DropTable("LICENSING.BusinessCode");
            DropTable("LICENSING.BCLinkDep");
            DropTable("LICENSING.BCLinkAD");
            DropTable("LICENSING.Attachment");
            DropTable("LICENSING.AdditionalDoc");
            DropTable("LICENSING.RoleTemplate");
            DropTable("LICENSING.AccessPage");
        }
    }
}
