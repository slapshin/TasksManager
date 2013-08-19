using Model;
using NHibernate;
using System.Collections.Generic;
using Web.SPA.Areas.Admin.Models;
using Web.SPA.Common;

namespace Web.SPA.Areas.Admin.Controllers
{
    public class UsersController : BaseApiController
    {
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

        public IEnumerable<UserDto> Get(string query, int offset, int limit)
        {
            using (ISession session = Provider.OpenSession())
            {
                List<UserDto> users = new List<UserDto>();
                ICriteria criteria = session.CreateCriteria<User>();
                IList<User> list = criteria.SetFirstResult(offset)
                            .SetMaxResults(limit).List<User>();

                foreach (User user in list)
                {
                    users.Add(ModelMapper.Map<User, UserDto>(user));
                }
                return users;
            }
        }

        //[ActionName("UsersCount")]
        //public HttpResponseMessage GetUsersCount()
        //{
        //    using (ISession session = Provider.OpenSession())
        //    {
        //        int count = session.CreateCriteria<User>()
        //                    .SetProjection(Projections.Count(Projections.Id()))
        //                    .UniqueResult<int>();

        // return Request.CreateResponse(HttpStatusCode.OK, count); }
        //}
    }
}