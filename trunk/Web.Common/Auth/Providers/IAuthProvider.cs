using System.Web;

namespace Web.Common.Auth.Providers
{
    public interface IAuthProvider
    {
        string Authenticate(HttpContext httpContext);
    }
}