using AutoMapper;
using TradingLicense.Entities;
using TradingLicense.Model;

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
                cfg.CreateMap<Company, CompanyModel>();
                cfg.CreateMap<Attachment, AttachmentModel>();
                cfg.CreateMap<RoleTemplate, RoleTemplateModel>();
                cfg.CreateMap<AppStatus, AppStatusModel>();
                cfg.CreateMap<BusinessCode, BusinessCodeModel>().ForMember(dest =>dest.SectorDesc,opt =>opt.MapFrom(s =>s.Sector.SectorDesc));
                cfg.CreateMap<BusinessCodeModel, BusinessCode>();
                cfg.CreateMap<PremiseType, PremiseTypeModel>();
                cfg.CreateMap<LoginLog, LoginLogModel>();
                cfg.CreateMap<Users, UsersModel>();
                cfg.CreateMap<UsersModel, Users>().ReverseMap();
                cfg.CreateMap<RegistrationModel, Users>().ReverseMap();
                cfg.CreateMap<AdditionalDocModel, AdditionalDoc>();
                cfg.CreateMap<Sector, SectorModel>();
                cfg.CreateMap<BTLinkReqDoc, BTLinkReqDocModel>().ForMember(dest => dest.RequiredDocDesc, opt => opt.MapFrom(s => s.RequiredDoc.RequiredDocDesc));
                cfg.CreateMap<BTLinkReqDocModel, BTLinkReqDoc>();
                cfg.CreateMap<PremiseApplication, PremiseApplicationModel>()
                            .ForMember(dest => dest.StatusDesc, opt => opt.MapFrom(s => s.AppStatus.StatusDesc))
                            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(s => s.Company.CompanyName))
                            .ForMember(dest => dest.PremiseDesc, opt => opt.MapFrom(s => s.PremiseType.PremiseDesc));
                cfg.CreateMap<PremiseApplication, ViewPremiseApplicationModel>()
                            .ForMember(dest => dest.Sector, opt => opt.MapFrom(s => s.Sector.SectorDesc))
                            .ForMember(dest => dest.Company, opt => opt.MapFrom(s => s.Company.CompanyName))
                            .ForMember(dest => dest.BusinessType, opt => opt.MapFrom(s => s.BusinessType.BusinessTypeDesc))
                            .ForMember(dest => dest.PremiseType, opt => opt.MapFrom(s => s.PremiseType.PremiseDesc));
                cfg.CreateMap<PremiseApplicationModel, PremiseApplication>();
                cfg.CreateMap<BCLinkAD, BCLinkADModel>().ForMember(dest => dest.DocDesc, opt => opt.MapFrom(s => s.AdditionalDoc.DocDesc));
                cfg.CreateMap<BCLinkADModel, BCLinkAD>();
                cfg.CreateMap<BusinessType, BusinessTypeModel>().ForMember(dest => dest.RequiredDocs, opt => opt.Ignore());
                cfg.CreateMap<BAReqDoc, BAReqDocModel>().ForMember(dest => dest.RequiredDocDesc, opt => opt.MapFrom(s => s.RequiredDoc.RequiredDocDesc));
                cfg.CreateMap<EntmtCode, EntmtCodeModel>().ForMember(dest => dest.EntmtGroupDesc, opt => opt.MapFrom(s => s.EntmtGroup.EntmtGroupDesc));
                cfg.CreateMap<EntmtApplication, EntmtApplicationModel>().ForMember(dest => dest.StatusDesc, opt => opt.MapFrom(s => s.AppStatus.StatusDesc));
                cfg.CreateMap<SAReqDoc, SAReqDocModel>().ForMember(dest => dest.RequiredDocDesc, opt => opt.MapFrom(s => s.RequiredDoc.RequiredDocDesc));
                cfg.CreateMap<HAReqDoc, HAReqDocModel>().ForMember(dest => dest.RequiredDocDesc, opt => opt.MapFrom(s => s.RequiredDoc.RequiredDocDesc));
                cfg.CreateMap<LAReqDoc, LAReqDocModel>().ForMember(dest => dest.RequiredDocDesc, opt => opt.MapFrom(s => s.RequiredDoc.RequiredDocDesc));
                cfg.CreateMap<Individual, IndividualModel>().ForMember(dest => dest.FileName, opt => opt.MapFrom(a => a.Attachment.FileName));

            });
        }
    }
}