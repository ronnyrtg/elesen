using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                cfg.CreateMap<PAStatus, PAStatusModel>();
                cfg.CreateMap<BusinessCode, BusinessCodeModel>().ForMember(dest =>dest.SectorDesc,opt =>opt.MapFrom(s =>s.Sector.SectorDesc));
                cfg.CreateMap<BusinessCodeModel, BusinessCode>();
                cfg.CreateMap<Signboard, SignboardModel>();
                cfg.CreateMap<PremiseType, PremiseTypeModel>();
                cfg.CreateMap<LoginLog, LoginLogModel>();
                cfg.CreateMap<Users, UsersModel>();
                cfg.CreateMap<UsersModel, Users>().ReverseMap();
                cfg.CreateMap<RegistrationModel, Users>().ReverseMap();
                cfg.CreateMap<AdditionalDocModel, AdditionalDoc>();
                cfg.CreateMap<Sector, SectorModel>();
            });
        }
    }
}