using Model;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.SPA.Areas.Admin.Models;
using Web.SPA.Common;

namespace Web.SPA.Areas.Admin.Controllers
{
    [RoutePrefix("api/Admin/Projects")]
    public class ProjectsUtilsController : BaseApiController
    {
        [HttpGet("Page")]
        public IEnumerable<ProjectDto> Page(int page, int pageSize)
        {
            List<ProjectDto> projects = new List<ProjectDto>();
            ExecuteInSession(session =>
            {
                IList<Project> list = session.CreateCriteria<Project>()
                                            .SetFirstResult((page - 1) * pageSize)
                                            .SetMaxResults(pageSize)
                                            .List<Project>();

                foreach (Project user in list)
                {
                    projects.Add(ModelMapper.Map<Project, ProjectDto>(user));
                }
            });
            return projects;
        }

        [HttpGet("Count")]
        public HttpResponseMessage Count()
        {
            int count = 0;
            ExecuteInSession(session =>
            {
                count = session.CreateCriteria<Project>()
                            .SetProjection(Projections.Count(Projections.Id()))
                            .UniqueResult<int>();
            });

            return Request.CreateResponse(HttpStatusCode.OK, count);
        }

        [HttpGet("Masters")]
        public HttpResponseMessage Masters()
        {
            return Request.CreateResponse(HttpStatusCode.OK, Utils.UserComboBoxItems(UserRole.Master));
        }
    }
}