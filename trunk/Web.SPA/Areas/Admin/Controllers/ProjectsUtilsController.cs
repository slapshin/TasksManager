using Model;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.SPA.Areas.Admin.Models;
using Web.SPA.Common;
using Web.SPA.Models;

namespace Web.SPA.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/Admin/Projects")]
    public class ProjectsUtilsController : BaseApiController
    {
        public class ProjectsPageParams : PageParams
        {
        }

        [Route("Page")]
        [HttpPost]
        public HttpResponseMessage Page(ProjectsPageParams parameters)
        {
            PageResult result = null;
            ExecuteInSession(session =>
            {
                IList<Project> data = GetPageCriteriaByParams(session, parameters)
                                    .SetFirstResult((parameters.Page - 1) * parameters.PageSize)
                                    .SetMaxResults(parameters.PageSize)
                                    .List<Project>();

                result = new PageResult()
                {
                    Total = GetPageCriteriaByParams(session, parameters).SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(),
                    Data = modelMapper.Map<IEnumerable<Project>, IEnumerable<ProjectDto>>(data)
                };
            });

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        private ICriteria GetPageCriteriaByParams(ISession session, ProjectsPageParams parameters)
        {
            return session.CreateCriteria<Project>();
        }

        [Route("Masters")]
        [HttpGet]
        public HttpResponseMessage Masters()
        {
            return Request.CreateResponse(HttpStatusCode.OK, Utils.UserComboBoxItems(UserRole.Master));
        }
    }
}