using System;
using System.Web;

namespace Web.Common.Auth.Providers
{
    public class ADAuthProvider : IAuthProvider
    {
        public string Authenticate(HttpContext httpContext)
        {
            //try
            //{
            //    string userIdentity = ((WindowsIdentity)HttpContext.Current.User.Identity).Name;
            //}
            //catch (Exception e)
            //{
            //    int y = 9;

            //}
            throw new NotImplementedException();
        }
    }
}