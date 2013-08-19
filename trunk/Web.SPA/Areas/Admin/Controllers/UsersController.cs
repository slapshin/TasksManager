using Model;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.SPA.Areas.Admin.Models;
using Web.SPA.Common;

namespace Web.SPA.Areas.Admin.Controllers
{
    public class UsersController : BaseApiController
    {
        [ActionName("DefaultAction")]
        public IEnumerable<UserDto> Get()
        {
            using (ISession session = Provider.OpenSession())
            {
                List<UserDto> users = new List<UserDto>();
                foreach (User user in session.QueryOver<User>().List())
                {
                    users.Add(ModelMapper.Map<User, UserDto>(user));
                }
                return users;
            }
        }

        [HttpGet]
        public IEnumerable<UserDto> Page(int page, int pageSize)
        {
            using (ISession session = Provider.OpenSession())
            {
                List<UserDto> users = new List<UserDto>();
                ICriteria criteria = session.CreateCriteria<User>();

                IList<User> list = criteria.SetFirstResult((page - 1) * pageSize)
                            .SetMaxResults(pageSize).List<User>();

                foreach (User user in list)
                {
                    users.Add(ModelMapper.Map<User, UserDto>(user));
                }
                return users;
            }
        }

        [HttpGet]
        public HttpResponseMessage Count()
        {
            using (ISession session = Provider.OpenSession())
            {
                int count = session.CreateCriteria<User>()
                            .SetProjection(Projections.Count(Projections.Id()))
                            .UniqueResult<int>();

                return Request.CreateResponse(HttpStatusCode.OK, count);
            }
        }
    }
}