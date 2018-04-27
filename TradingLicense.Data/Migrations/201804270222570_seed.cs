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
                "LICENSING.AdditionalInfo",
                c => new
                    {
                        AdditionalInfoID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BusinessCodeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        InfoDesc = c.String(maxLength: 50, unicode: false),
                        InfoQuantity = c.String(maxLength: 30, unicode: false),
                    })
                .PrimaryKey(t => t.AdditionalInfoID)
                .ForeignKey("LICENSING.BusinessCode", t => t.BusinessCodeID, cascadeDelete: true)
                .Index(t => t.BusinessCodeID);
            
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
                        Express = c.Decimal(nullable: false, precision: 10, scale: 0),
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
                "LICENSING.Attachment",
                c => new
                    {
                        AttachmentID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        FileName = c.String(maxLength: 255, unicode: false),
                        Business_BusinessID = c.Decimal(precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("LICENSING.Business", t => t.Business_BusinessID)
                .Index(t => t.Business_BusinessID);
            
            CreateTable(
                "LICENSING.BLinkCode",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BusinessID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        BusinessCodeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("LICENSING.Business", t => t.BusinessID, cascadeDelete: true)
                .ForeignKey("LICENSING.BusinessCode", t => t.BusinessCodeID, cascadeDelete: true)
                .Index(t => t.BusinessID)
                .Index(t => t.BusinessCodeID);
            
            CreateTable(
                "LICENSING.Business",
                c => new
                    {
                        BusinessID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BusinessName = c.String(),
                        OfficePhone = c.String(),
                        PremiseAddress = c.String(),
                        PremiseMap = c.String(),
                        PremisePic = c.String(),
                        OwnRent = c.String(),
                        RentFrom = c.DateTime(),
                        RentUntil = c.DateTime(),
                        FloorArea = c.Single(nullable: false),
                        FloorSketch = c.String(),
                        PremiseTypeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        WhichFloor = c.String(),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.BusinessID)
                .ForeignKey("LICENSING.PremiseType", t => t.PremiseTypeID, cascadeDelete: true)
                .Index(t => t.PremiseTypeID);
            
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
                "LICENSING.BusinessActivity",
                c => new
                    {
                        BusinessActivityID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BusinessCodeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        UnitNo = c.String(maxLength: 50, unicode: false),
                        Floor = c.String(maxLength: 50, unicode: false),
                        PremiseArea = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.BusinessActivityID)
                .ForeignKey("LICENSING.BusinessCode", t => t.BusinessCodeID, cascadeDelete: true)
                .Index(t => t.BusinessCodeID);
            
            CreateTable(
                "LICENSING.BussCodLinkDep",
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
                "LICENSING.Department",
                c => new
                    {
                        DepartmentID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        DepartmentCode = c.String(nullable: false, maxLength: 10, unicode: false),
                        DepartmentDesc = c.String(nullable: false, maxLength: 255, unicode: false),
                        Active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
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
                "LICENSING.ILinkB",
                c => new
                    {
                        Id = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        IndividualID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        BusinessID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("LICENSING.Business", t => t.BusinessID, cascadeDelete: true)
                .ForeignKey("LICENSING.Individual", t => t.IndividualID, cascadeDelete: true)
                .Index(t => t.IndividualID)
                .Index(t => t.BusinessID);
            
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
                "LICENSING.PALinkAI",
                c => new
                    {
                        PALinkAIID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PAID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        AdditionalInfoID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.PALinkAIID)
                .ForeignKey("LICENSING.AdditionalInfo", t => t.AdditionalInfoID, cascadeDelete: true)
                .Index(t => t.AdditionalInfoID);
            
            CreateTable(
                "LICENSING.PALinkBAct",
                c => new
                    {
                        PALinkBActID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PAID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        BusinessActivityID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.PALinkBActID)
                .ForeignKey("LICENSING.BusinessActivity", t => t.BusinessActivityID, cascadeDelete: true)
                .Index(t => t.BusinessActivityID);
            
            CreateTable(
                "LICENSING.PALinkReqDoc",
                c => new
                    {
                        PALinkReqDocID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        PAID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SignboardID = c.Decimal(nullable: false, precision: 10, scale: 0),
                    })
                .PrimaryKey(t => t.PALinkReqDocID)
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
                "LICENSING.PAStatus",
                c => new
                    {
                        PAStatusID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        StatusDesc = c.String(maxLength: 100, unicode: false),
                        PercentProgress = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.PAStatusID);
            
            CreateTable(
                "LICENSING.PremiseApplication",
                c => new
                    {
                        PremiseApplicationID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
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
                .ForeignKey("LICENSING.Individual", t => t.IndividualID, cascadeDelete: true)
                .ForeignKey("LICENSING.PAStatus", t => t.PAStatusID, cascadeDelete: true)
                .ForeignKey("LICENSING.PremiseType", t => t.PremiseTypeID, cascadeDelete: true)
                .Index(t => t.IndividualID)
                .Index(t => t.PremiseTypeID)
                .Index(t => t.PAStatusID);
            
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
                "LICENSING.SupportDocs",
                c => new
                    {
                        SupportDocsID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        BusinessCodeID = c.Decimal(nullable: false, precision: 10, scale: 0),
                        SuppDocDesc = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.SupportDocsID)
                .ForeignKey("LICENSING.BusinessCode", t => t.BusinessCodeID, cascadeDelete: true)
                .Index(t => t.BusinessCodeID);
            
            CreateTable(
                "LICENSING.UnitLuar",
                c => new
                    {
                        UnitLuarID = c.Decimal(nullable: false, precision: 10, scale: 0, identity: true),
                        UnitLuarCode = c.String(maxLength: 50, unicode: false),
                        UnitLuarDesc = c.String(maxLength: 255, unicode: false),
                        active = c.Decimal(nullable: false, precision: 1, scale: 0),
                    })
                .PrimaryKey(t => t.UnitLuarID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("LICENSING.SupportDocs", "BusinessCodeID", "LICENSING.BusinessCode");
            DropForeignKey("LICENSING.PremiseApplication", "PremiseTypeID", "LICENSING.PremiseType");
            DropForeignKey("LICENSING.PremiseApplication", "PAStatusID", "LICENSING.PAStatus");
            DropForeignKey("LICENSING.PremiseApplication", "IndividualID", "LICENSING.Individual");
            DropForeignKey("LICENSING.PALinkSign", "SignboardID", "LICENSING.Signboard");
            DropForeignKey("LICENSING.PALinkReqDoc", "SignboardID", "LICENSING.Signboard");
            DropForeignKey("LICENSING.PALinkBAct", "BusinessActivityID", "LICENSING.BusinessActivity");
            DropForeignKey("LICENSING.PALinkAI", "AdditionalInfoID", "LICENSING.AdditionalInfo");
            DropForeignKey("LICENSING.LoginLog", "UsersID", "LICENSING.Users");
            DropForeignKey("LICENSING.Users", "RoleTemplateID", "LICENSING.RoleTemplate");
            DropForeignKey("LICENSING.Users", "DepartmentID", "LICENSING.Department");
            DropForeignKey("LICENSING.IndLinkCom", "IndividualID", "LICENSING.Individual");
            DropForeignKey("LICENSING.IndLinkCom", "CompanyID", "LICENSING.Company");
            DropForeignKey("LICENSING.ILinkB", "IndividualID", "LICENSING.Individual");
            DropForeignKey("LICENSING.ILinkB", "BusinessID", "LICENSING.Business");
            DropForeignKey("LICENSING.BussCodLinkDep", "DepartmentID", "LICENSING.Department");
            DropForeignKey("LICENSING.BussCodLinkDep", "BusinessCodeID", "LICENSING.BusinessCode");
            DropForeignKey("LICENSING.BusinessActivity", "BusinessCodeID", "LICENSING.BusinessCode");
            DropForeignKey("LICENSING.BLinkCode", "BusinessCodeID", "LICENSING.BusinessCode");
            DropForeignKey("LICENSING.BLinkCode", "BusinessID", "LICENSING.Business");
            DropForeignKey("LICENSING.Business", "PremiseTypeID", "LICENSING.PremiseType");
            DropForeignKey("LICENSING.Attachment", "Business_BusinessID", "LICENSING.Business");
            DropForeignKey("LICENSING.AdditionalInfo", "BusinessCodeID", "LICENSING.BusinessCode");
            DropForeignKey("LICENSING.BusinessCode", "SectorID", "LICENSING.Sector");
            DropForeignKey("LICENSING.AccessPage", "RoleTemplateID", "LICENSING.RoleTemplate");
            DropIndex("LICENSING.SupportDocs", new[] { "BusinessCodeID" });
            DropIndex("LICENSING.PremiseApplication", new[] { "PAStatusID" });
            DropIndex("LICENSING.PremiseApplication", new[] { "PremiseTypeID" });
            DropIndex("LICENSING.PremiseApplication", new[] { "IndividualID" });
            DropIndex("LICENSING.PALinkSign", new[] { "SignboardID" });
            DropIndex("LICENSING.PALinkReqDoc", new[] { "SignboardID" });
            DropIndex("LICENSING.PALinkBAct", new[] { "BusinessActivityID" });
            DropIndex("LICENSING.PALinkAI", new[] { "AdditionalInfoID" });
            DropIndex("LICENSING.Users", new[] { "DepartmentID" });
            DropIndex("LICENSING.Users", new[] { "RoleTemplateID" });
            DropIndex("LICENSING.LoginLog", new[] { "UsersID" });
            DropIndex("LICENSING.IndLinkCom", new[] { "CompanyID" });
            DropIndex("LICENSING.IndLinkCom", new[] { "IndividualID" });
            DropIndex("LICENSING.ILinkB", new[] { "BusinessID" });
            DropIndex("LICENSING.ILinkB", new[] { "IndividualID" });
            DropIndex("LICENSING.BussCodLinkDep", new[] { "DepartmentID" });
            DropIndex("LICENSING.BussCodLinkDep", new[] { "BusinessCodeID" });
            DropIndex("LICENSING.BusinessActivity", new[] { "BusinessCodeID" });
            DropIndex("LICENSING.Business", new[] { "PremiseTypeID" });
            DropIndex("LICENSING.BLinkCode", new[] { "BusinessCodeID" });
            DropIndex("LICENSING.BLinkCode", new[] { "BusinessID" });
            DropIndex("LICENSING.Attachment", new[] { "Business_BusinessID" });
            DropIndex("LICENSING.BusinessCode", new[] { "SectorID" });
            DropIndex("LICENSING.AdditionalInfo", new[] { "BusinessCodeID" });
            DropIndex("LICENSING.AccessPage", new[] { "RoleTemplateID" });
            DropTable("LICENSING.UnitLuar");
            DropTable("LICENSING.SupportDocs");
            DropTable("LICENSING.RequiredDoc");
            DropTable("LICENSING.PremiseApplication");
            DropTable("LICENSING.PAStatus");
            DropTable("LICENSING.PALinkSign");
            DropTable("LICENSING.Signboard");
            DropTable("LICENSING.PALinkReqDoc");
            DropTable("LICENSING.PALinkBAct");
            DropTable("LICENSING.PALinkAI");
            DropTable("LICENSING.Users");
            DropTable("LICENSING.LoginLog");
            DropTable("LICENSING.IndLinkCom");
            DropTable("LICENSING.Individual");
            DropTable("LICENSING.ILinkB");
            DropTable("LICENSING.Company");
            DropTable("LICENSING.Department");
            DropTable("LICENSING.BussCodLinkDep");
            DropTable("LICENSING.BusinessActivity");
            DropTable("LICENSING.PremiseType");
            DropTable("LICENSING.Business");
            DropTable("LICENSING.BLinkCode");
            DropTable("LICENSING.Attachment");
            DropTable("LICENSING.Sector");
            DropTable("LICENSING.BusinessCode");
            DropTable("LICENSING.AdditionalInfo");
            DropTable("LICENSING.RoleTemplate");
            DropTable("LICENSING.AccessPage");
        }
    }
}
