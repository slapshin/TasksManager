using System.Security.Principal;
using Web.Common.Repository;

namespace Web.Common.Auth
{
    public class UserProvider : IPrincipal
    {
        public UserProvider(string login, ISessionProvider provider)
        {
            userIdentity = new UserIndentity();
            userIdentity.Init(login, provider);
        }

        private UserIndentity userIdentity { get; set; }

        #region IPrincipal

        public IIdentity Identity
        {
            get { return userIdentity; }
        }

        public bool IsInRole(string role)
        {
            if (userIdentity.User == null)
            {
                return false;
            }

            return userIdentity.User.InRoles(role);
        }

        #endregion IPrincipal

        public override string ToString()
        {
            return userIdentity.ToString();
        }
    }
}