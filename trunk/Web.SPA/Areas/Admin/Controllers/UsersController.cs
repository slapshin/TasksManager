using Model;
using NHibernate;
using System.Collections.Generic;
using Web.SPA.Areas.Admin.Models;
using Web.SPA.Common;

namespace Web.SPA.Areas.Admin.Controllers
{
    public class UsersController : BaseApiController
    {
        public IEnumerable<UserDto> GetAdminUsers()
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
    }
}