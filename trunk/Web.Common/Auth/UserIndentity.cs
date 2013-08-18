using Model;
using NHibernate;
using System.Security.Principal;
using Web.Common.Repository;

namespace Web.Common.Auth
{
    public class UserIndentity : IIdentity, IUserProvider
    {
        public User User { get; set; }

        public void Init(string login, ISessionProvider provider)
        {
            if (!string.IsNullOrEmpty(login))
            {
                User = GetUser(login, provider);
            }
        }

        private User GetUser(string login, ISessionProvider provider)
        {
            using (ISession session = provider.OpenSession())
            {
                return (from u in session.QueryOver<User>()
                        where u.Login == login
                        select u).SingleOrDefault<User>();
            }
        }

        #region IIdentity

        public string AuthenticationType
        {
            get { return typeof(User).ToString(); }
        }

        public bool IsAuthenticated
        {
            get { return User != null; }
        }

        public string Name
        {
            get
            {
                if (User != null)
                {
                    return User.Login;
                }

                return "Guest";
            }
        }

        #endregion IIdentity
    }
}