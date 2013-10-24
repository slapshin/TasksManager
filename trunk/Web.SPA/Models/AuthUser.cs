using Microsoft.AspNet.Identity;
using Model;

namespace Web.SPA.Models
{
    public class AuthUser : IUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public UserRole Roles { get; set; }
    }
}