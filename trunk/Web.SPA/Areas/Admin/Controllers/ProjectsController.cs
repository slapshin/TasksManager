using Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Web.SPA.Areas.Admin.Models;
using Web.SPA.Common;

namespace Web.SPA.Areas.Admin.Controllers
{
    public class ProjectsController : BaseApiController
    {
        public IEnumerable<ProjectDto> Get()
        {
            List<ProjectDto> projects = new List<ProjectDto>();
            ExecuteInSession(session =>
            {
                foreach (Project project in session.QueryOver<Project>().List())
                {
                    projects.Add(ModelMapper.Map<Project, ProjectDto>(project));
                }
            });
            return projects;
        }

        public ProjectDto Get(Guid id)
        {
            ProjectDto project = null;
            ExecuteInSession(session =>
            {
                project = ModelMapper.Map<Project, ProjectDto>(session.Get<Project>(id));
            });
            return project;
        }

        [CheckModel]
        public HttpResponseMessage Post(ProjectDto dto)
        {
            ExecuteInTransaction(session =>
            {
                Project project = dto.Id.HasValue ? session.Get<Project>(dto.Id.Value) : new Project();
                ModelMapper.Map<ProjectDto, Project>(dto, project);
                project.Master = dto.Master.HasValue ? session.Load<User>(dto.Master) : null;
                session.SaveOrUpdate(project);
                dto = ModelMapper.Map<Project, ProjectDto>(project, dto);
            });

            return Request.CreateResponse(HttpStatusCode.OK, dto);
        }

        public HttpResponseMessage Delete(Guid id)
        {
            ExecuteInTransaction(session =>
            {
                session.Delete(session.Load<Project>(id));
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}