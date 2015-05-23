using Model;
using Web.Areas.Admin.Models;
using Web.Areas.Master.Models;
using Web.Common.Mapper;
using Web.Models;

namespace Web.Utils.Mapper
{
    public class CommonMapper : IMapper
    {
        static CommonMapper()
        {
            AutoMapper.Mapper.CreateMap<User, UserView>()
                                .ForMember(m => m.IsAdmin, opt => opt.MapFrom(s => s.IsAdmin))
                                .ForMember(m => m.IsCustomer, opt => opt.MapFrom(s => s.IsCustomer))
                                .ForMember(m => m.IsExecutor, opt => opt.MapFrom(s => s.IsExecutor))
                                .ForMember(m => m.IsMaster, opt => opt.MapFrom(s => s.IsMaster))
                                .ForMember(m => m.IsRouter, opt => opt.MapFrom(s => s.IsRouter))
                                .ForMember(m => m.IsTester, opt => opt.MapFrom(s => s.IsTester));
            AutoMapper.Mapper.CreateMap<UserView, User>()
                                .ForMember(m => m.Id, opt => opt.Ignore())
                                .ForMember(m => m.Password, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<Claim, ClaimView>()
                                .ForMember(m => m.Project_Id, opt => opt.MapFrom(s => s.Project.Id))
                                .ForMember(m => m.Project_Title, opt => opt.MapFrom(s => s.Project.Title))
                                .ForMember(m => m.Customer_Login, opt => opt.MapFrom(s => s.Customer.Login))
                                .ForMember(m => m.Customer_Name, opt => opt.MapFrom(s => s.Customer.Name))
                                .ForMember(m => m.Customer_Surname, opt => opt.MapFrom(s => s.Customer.Surname));
            AutoMapper.Mapper.CreateMap<ClaimView, Claim>()
                                .ForMember(m => m.Created, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<Project, ProjectView>()
                                .ForMember(m => m.Master, opt => opt.MapFrom(s => s.Master.Id));
            AutoMapper.Mapper.CreateMap<ProjectView, Project>()
                                .ForMember(m => m.Master, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<Call, CallView>()
                                .ForMember(m => m.Project_Id, opt => opt.MapFrom(s => s.Project.Id))
                                .ForMember(m => m.Project_Title, opt => opt.MapFrom(s => s.Project.Title))
                                .ForMember(m => m.Claim_Id, opt => opt.MapFrom(s => s.Claim.Id));
            AutoMapper.Mapper.CreateMap<CallView, Call>()
                                .ForMember(m => m.Created, opt => opt.Ignore())
                                .ForMember(m => m.Status, opt => opt.Ignore());

            AutoMapper.Mapper.CreateMap<Task, TaskView>()
                                .ForMember(m => m.Executor_Id, opt => opt.MapFrom(s => s.Executor.Id));
            AutoMapper.Mapper.CreateMap<TaskView, Task>()
                                .ForMember(m => m.Created, opt => opt.Ignore())
                                .ForMember(m => m.Status, opt => opt.Ignore());
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