using Model;
using Web.Common.Mapper;
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