using AutoMapper;
using TradingLicense.Entities;
using TradingLicense.Model;
using System;

namespace TradingLicense.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.Initialize(cfg =>
            {
                // When mapping a collection property, if the source value is null AutoMapper will map the destination field to an empty collection 
                // rather than setting the destination value to null. This aligns with the behavior of Entity Framework and Framework Design 
                // Guidelines that believe C# references, arrays, lists, collections, dictionaries and IEnumerables should NEVER be null, ever.
                // The following changes that behavior to all null values!

                cfg.AllowNullCollections = true;
                cfg.CreateMap<AccessPage, AccessPageModel>().ForMember(dest => dest.RoleTemplateDesc, opt => opt.MapFrom(s => s.RoleTemplate.RoleTemplateDesc));
                cfg.CreateMap<AccessPageModel, AccessPage>();
                cfg.CreateMap<Department, DepartmentModel>();
                cfg.CreateMap<RequiredDoc, RequiredDocModel>();
                cfg.CreateMap<CompanyModel, Company>();
                cfg.CreateMap<Company, CompanyModel>().ForMember(dest => dest.BusinessTypeDesc, opt => opt.MapFrom(s => s.BusinessType.BusinessTypeDesc));
                cfg.CreateMap<Attachment, AttachmentModel>();
                cfg.CreateMap<RoleTemplate, RoleTemplateModel>();
                cfg.CreateMap<AppStatus, AppStatusModel>();
                cfg.CreateMap<PremiseType, PremiseTypeModel>();
                cfg.CreateMap<LoginLog, LoginLogModel>();
                cfg.CreateMap<Users, UsersModel>();
                cfg.CreateMap<UsersModel, Users>().ReverseMap();
                cfg.CreateMap<RegistrationModel, Users>().ReverseMap();
                cfg.CreateMap<Sector, SectorModel>();
                cfg.CreateMap<BusinessType, BusinessTypeModel>().ForMember(dest => dest.RequiredDocs, opt => opt.Ignore());
                cfg.CreateMap<BT, BTModel>();
                cfg.CreateMap<BTLinkReqDoc, BTLinkReqDocModel>().ForMember(dest => dest.RequiredDocDesc, opt => opt.MapFrom(s => s.RequiredDoc.RequiredDocDesc));
                cfg.CreateMap<BTLinkReqDocModel, BTLinkReqDoc>();
                cfg.CreateMap<BT_L_RD, BT_L_RDModel>().ForMember(dest => dest.RD_DESC, opt => opt.MapFrom(s => s.RD.RD_DESC)); ;
                cfg.CreateMap<Individual, IndividualModel>().ForMember(dest => dest.FileName, opt => opt.MapFrom(a => a.Attachment.FileName));
                cfg.CreateMap<IndividualModel, Individual>().ForMember(dest => dest.Attachment, options => options.Ignore());
                cfg.CreateMap<RouteUnit, RouteUnitModel>()
                            .ForMember(dest => dest.FullName, opt => opt.MapFrom(s => s.Users.FullName))
                            .ForMember(dest => dest.DepartmentDesc, opt => opt.MapFrom(s => $"{s.Department.DepartmentDesc} ({ s.Department.DepartmentCode})"));

                //Combined Application
                cfg.CreateMap<APPLICATION, ApplicationModel>();
                cfg.CreateMap<LIC_TYPE, LIC_TYPEModel>();
                cfg.CreateMap<BC, BCModel>()
                            .ForMember(dest => dest.LIC_TYPEDESC, opt => opt.MapFrom(s => s.LIC_TYPE.LIC_TYPEDESC))
                            .ForMember(dest => dest.SectorDesc, opt => opt.MapFrom(s => s.Sector.SectorDesc));


                //PremiseApplication related
                cfg.CreateMap<BusinessCode, BusinessCodeModel>().ForMember(dest =>dest.SectorDesc,opt =>opt.MapFrom(s =>s.Sector.SectorDesc));
                cfg.CreateMap<BusinessCodeModel, BusinessCode>();               
                cfg.CreateMap<AdditionalDocModel, AdditionalDoc>();
                cfg.CreateMap<BCLinkAD, BCLinkADModel>().ForMember(dest => dest.DocDesc, opt => opt.MapFrom(s => s.AdditionalDoc.DocDesc));
                cfg.CreateMap<BCLinkADModel, BCLinkAD>();
                cfg.CreateMap<PremiseApplicationModel, PremiseApplication>();
                cfg.CreateMap<PremiseApplication, PremiseApplicationModel>()
                            .ForMember(dest => dest.BusinessTypeDesc, opt => opt.MapFrom(s => s.BusinessType.BusinessTypeDesc))
                            .ForMember(dest => dest.SectorDesc, opt => opt.MapFrom(s => s.Sector.SectorDesc))
                            .ForMember(dest => dest.StatusDesc, opt => opt.MapFrom(s => s.AppStatus.StatusDesc))
                            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(s => s.Company.CompanyName))
                            .ForMember(dest => dest.PremiseDesc, opt => opt.MapFrom(s => s.PremiseType.PremiseDesc));
                cfg.CreateMap<Comment, CommentModel>().ForMember(dest => dest.FullName, opt => opt.MapFrom(s => s.Users.FullName));
                cfg.CreateMap<PaymentReceivedModel, PaymentReceived>().ForMember(dest => dest.Individual, opt => opt.Ignore());
                
                //Entertainment License related
                cfg.CreateMap<E_CODE, E_CODEModel>().ForMember(dest => dest.E_G_DESC, opt => opt.MapFrom(s => s.E_GROUP.E_G_DESC));
                cfg.CreateMap<E_GROUP, E_GROUPModel>();

                //Money Lender Application related
                cfg.CreateMap<MLPermitApplication, MLPermitApplicationModel>();               
            });
        }
    }
}