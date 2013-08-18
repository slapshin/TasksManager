using Model;
using System.Security.Principal;
using System.Web;

namespace Web.Common.Auth
{
    public interface IAuthentication
    {
        HttpContext HttpContext { get; set; }

        IPrincipal CurrentUser { get; }

        User Login(string login, string password, bool isPersistent);

        void LogOut();
    }
}