using Model;
using Web.Common.Mapper;
using AdminClaimsDto = Web.SPA.Areas.Customer.Models.ClaimDto;
using AdminProjectDto = Web.SPA.Areas.Admin.Models.ProjectDto;
using AdminUserDto = Web.SPA.Areas.Admin.Models.UserDto;

namespace Web.SPA.Common
{
    public class CommonMapper : IMapper
    {
        static CommonMapper()
        {
            AutoMapper.Mapper.CreateMap<User, AdminUserDto>()
                              .ForMember(m => m.IsAdmin, opt => opt.MapFrom(s => s.IsAdmin))
                              .ForMember(m => m.IsCustomer, opt => opt.MapFrom(s => s.IsCustomer))
                              .ForMember(m => m.IsExecutor, opt => opt.MapFrom(s => s.IsExecutor))
                              .ForMember(m => m.IsMaster, opt => opt.MapFrom(s => s.IsMaster))
                              .ForMember(m => m.IsRouter, opt => opt.MapFrom(s => s.IsRouter))
                              .ForMember(m => m.IsTester, opt => opt.MapFrom(s => s.IsTester));
            AutoMapper.Mapper.CreateMap<AdminUserDto, User>()
                                .ForMember(m => m.Id, opt => opt.Ignore())
                                .ForMember(m => m.Password, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<Project, AdminProjectDto>()
                    .ForMember(m => m.Master, opt => opt.MapFrom(s => s.Master.Id))
                    .ForMember(m => m.MasterName, opt => opt.MapFrom(s => s.Master.Login));
            AutoMapper.Mapper.CreateMap<AdminProjectDto, Project>()
                                .ForMember(m => m.Master, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<Claim, AdminClaimsDto>();
            AutoMapper.Mapper.CreateMap<AdminClaimsDto, Claim>()
                                .ForMember(m => m.Id, opt => opt.Ignore())
                                .ForMember(m => m.Created, opt => opt.Ignore());
        }

        public TDest Map<TSource, TDest>(TSource source)
        {
            return AutoMapper.Mapper.Map<TSource, TDest>(source);
        }

        public TDest Map<TSource, TDest>(TSource source, TDest dest)
        {
            return AutoMapper.Mapper.Map<TSource, TDest>(source, dest);
        }
    }
}