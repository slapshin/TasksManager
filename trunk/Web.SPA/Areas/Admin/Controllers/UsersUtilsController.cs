using Common;
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
    [RoutePrefix("api/Admin/Users")]
    public class UsersUtilsController : BaseApiController
    {
        public class UsersPageParams : PageParams
        {
        }

        [Route("Page")]
        [HttpPost()]
        public HttpResponseMessage Page(UsersPageParams parameters)
        {
            PageResult result = null;
            ExecuteInSession(session =>
            {
                IList<User> data = GetPageCriteriaByParams(session, parameters)
                                    .SetFirstResult((parameters.Page - 1) * parameters.PageSize)
                                    .SetMaxResults(parameters.PageSize)
                                    .List<User>();

                result = new PageResult()
                {
                    Total = GetPageCriteriaByParams(session, parameters).SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(),
                    Data = modelMapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(data)
                };
            });

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        private ICriteria GetPageCriteriaByParams(ISession session, UsersPageParams parameters)
        {
            return session.CreateCriteria<User>();
        }

        [Route("ChangePassword")]
        [HttpPost]
        [CheckModel]
        public HttpResponseMessage ChangePassword(ChangePassDto view)
        {
            ExecuteInTransaction(session => GetEntity<User>(session, view.Id).Password = Helpers.CreateMD5Hash(view.Password));
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}