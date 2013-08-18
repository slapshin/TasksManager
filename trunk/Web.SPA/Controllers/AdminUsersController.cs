using Model;
using NHibernate;
using System.Collections.Generic;
using System.Web.Http;
using Web.SPA.Common;
using Web.SPA.Models;

namespace Web.SPA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : BaseApiController
    {
        // GET api/AdminUsers
        public IEnumerable<AdminUserDto> GetAdminUsers()
        {
            using (ISession session = Provider.OpenSession())
            {
                List<AdminUserDto> users = new List<AdminUserDto>();
                foreach (User user in session.QueryOver<User>().List())
                {
                    users.Add(ModelMapper.Map<User, AdminUserDto>(user));
                }
                return users;
            }
        }
    }
}