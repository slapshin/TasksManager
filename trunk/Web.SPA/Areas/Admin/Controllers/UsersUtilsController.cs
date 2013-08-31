using Common;
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
    [RoutePrefix("api/Admin/Users")]
    public class UsersUtilsController : BaseApiController
    {
        [HttpGet("Page")]
        public IEnumerable<UserDto> Page(int page, int pageSize)
        {
            List<UserDto> users = new List<UserDto>();
            ExecuteInSession(session =>
            {
                IList<User> list = session.CreateCriteria<User>()
                                            .SetFirstResult((page - 1) * pageSize)
                                            .SetMaxResults(pageSize)
                                            .List<User>();

                foreach (User user in list)
                {
                    users.Add(ModelMapper.Map<User, UserDto>(user));
                }
            });
            return users;
        }

        [HttpGet("Count")]
        public HttpResponseMessage Count()
        {
            int count = 0;
            ExecuteInSession(session =>
            {
                count = session.CreateCriteria<User>()
                            .SetProjection(Projections.Count(Projections.Id()))
                            .UniqueResult<int>();
            });

            return Request.CreateResponse(HttpStatusCode.OK, count);
        }

        [HttpPost("ChangePassword")]
        [CheckModel]
        public HttpResponseMessage ChangePassword(ChangePassDto view)
        {
            ExecuteInTransaction(session =>
            {
                GetEntity<User>(session, view.Id).Password = Helpers.CreateMD5Hash(view.Password);
            });
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}