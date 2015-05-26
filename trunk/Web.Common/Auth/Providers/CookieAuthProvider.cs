using System;
using System.Web;
using System.Web.Security;

namespace Web.Common.Auth.Providers
{
    public class CookieAuthProvider : IAuthProvider
    {
        public string Authenticate(HttpContext httpContext)
        {
            try
            {
                return FormsAuthentication.Decrypt(httpContext.Request.Cookies.Get(Consts.AuthCookieName).Value).Name;
            }
            catch
            {
            }

            throw new ApplicationException("Не удалось выполнить аутентификацию через cookie");
        }
    }
}