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
                cfg.CreateMap<ACCESSPAGE, AccessPageModel>().ForMember(dest => dest.ROLE_DESC, opt => opt.MapFrom(s => s.ROLE.ROLE_DESC));
                cfg.CreateMap<AccessPageModel, ACCESSPAGE>();
                cfg.CreateMap<DEPARTMENT, DepartmentModel>();
                cfg.CreateMap<RD, RequiredDocModel>();
                cfg.CreateMap<ZONE, ZoneModel>().ForMember(dest => dest.zoneCombineList, opt => opt.Ignore());
                cfg.CreateMap<CompanyModel, COMPANY>();
                cfg.CreateMap<COMPANY, CompanyModel>().ForMember(dest => dest.BusinessTypeDesc, opt => opt.MapFrom(s => s.BT.BT_DESC));
                cfg.CreateMap<ATTACHMENT, AttachmentModel>();
                cfg.CreateMap<ROLE, RoleModel>();
                cfg.CreateMap<APPSTATUS, AppStatusModel>();
                cfg.CreateMap<PREMISETYPE, PremiseTypeModel>();
                cfg.CreateMap<LOGINLOG, LoginLogModel>();
                cfg.CreateMap<USERS, UsersModel>();
                cfg.CreateMap<UsersModel, USERS>().ReverseMap();
                cfg.CreateMap<RegistrationModel, USERS>().ReverseMap();                
                cfg.CreateMap<INDIVIDUAL, IndividualModel>().ForMember(dest => dest.FileName, opt => opt.MapFrom(a => a.ATTACHMENT.FILENAME));
                cfg.CreateMap<IndividualModel, INDIVIDUAL>().ForMember(dest => dest.ATTACHMENT, options => options.Ignore());
                cfg.CreateMap<ROUTEUNIT, RouteUnitModel>()
                            .ForMember(dest => dest.FullName, opt => opt.MapFrom(s => s.USERS.FULLNAME))
                            .ForMember(dest => dest.DepartmentDesc, opt => opt.MapFrom(s => $"{s.DEPARTMENT.DEP_DESC} ({ s.DEPARTMENT.DEP_CODE})"));

                //Combined Application
                cfg.CreateMap<APPLICATION, ApplicationModel>()
                            .ForMember(dest => dest.LicenseTypeDesc, opt => opt.MapFrom(s => s.LIC_TYPE.LIC_TYPEDESC))
                            .ForMember(dest => dest.BusinessTypeDesc, opt => opt.MapFrom(s => s.BT.BT_DESC))
                            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(s => s.COMPANY.C_NAME))
                            .ForMember(dest => dest.StatusDesc, opt => opt.MapFrom(s => s.APPSTATUS.STATUSDESC))
                            .ForMember(dest => dest.FullName, opt => opt.MapFrom(s => s.USERS.FULLNAME));
                cfg.CreateMap<ApplicationModel, APPLICATION>()
                            .ForMember(dest => dest.LIC_TYPE, opt => opt.Ignore());
                cfg.CreateMap<LIC_TYPE, LicenseTypeModel>();
                cfg.CreateMap<SECTOR, SectorModel>();
                cfg.CreateMap<BT, BusinessTypeModel>();
                cfg.CreateMap<BC, BusinessCodeModel>()
                            .ForMember(dest => dest.Lic_TypeDesc, opt => opt.MapFrom(s => s.LIC_TYPE.LIC_TYPEDESC))
                            .ForMember(dest => dest.SectorDesc, opt => opt.MapFrom(s => s.SECTOR.SECTORDESC));
                cfg.CreateMap<B_O, BannerObjectModel>();
                cfg.CreateMap<RD_L_BT, RD_L_BTModel>().ForMember(dest => dest.RD_DESC, opt => opt.MapFrom(s => s.RD.RD_DESC));
                cfg.CreateMap<RD_L_BTModel, RD_L_BT>();
                cfg.CreateMap<RD_L_BC, RD_L_BCModel>()
                            .ForMember(dest => dest.C_R_DESC, opt => opt.MapFrom(s => s.BC.C_R_DESC))
                            .ForMember(dest => dest.RD_DESC, opt => opt.MapFrom(s => s.RD.RD_DESC));
                cfg.CreateMap<COMMENT, CommentModel>().ForMember(dest => dest.FullName, opt => opt.MapFrom(s => s.USERS.FULLNAME));
                cfg.CreateMap<PaymentReceivedModel, PAY_REC>().ForMember(dest => dest.INDIVIDUAL, opt => opt.Ignore());
                
                //Entertainment License related
                cfg.CreateMap<E_P_FEE, EntmtPremiseFeeModel>();
       
            });
        }
    }
}