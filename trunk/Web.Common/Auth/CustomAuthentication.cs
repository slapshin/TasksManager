using Common;
using Model;
using NHibernate;
using Ninject;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Web.Common.Auth.Providers;
using Web.Common.Repository;

namespace Web.Common.Auth
{
    public class CustomAuthentication : IAuthentication
    {
        private static NLog.Logger logger = NLog.LogManager.GetLogger(Consts.LOGGER_NAME);

        private IPrincipal currentUser = null;

        private IAuthProvider[] authProviders = {
                                                    new CookieAuthProvider()
                                                };

        [Inject]
        public ISessionProvider SessionProvider { get; set; }

        public HttpContext HttpContext { get; set; }

        #region IAuthentication

        public IPrincipal CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    try
                    {
                        AuthenticateViaProviders();
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Failed authentication: " + ex.Message);
                        currentUser = new UserProvider(null, SessionProvider);
                    }
                }
                return currentUser;
            }
        }

        public User Login(string login, string password, bool isPersistent)
        {
            User user = null;
            using (ISession session = SessionProvider.OpenSession())
            {
                string md5pswd = Helpers.CreateMD5Hash(password);
                user = (from u in session.QueryOver<User>()
                        where u.Login == login && u.Password == md5pswd
                        select u).SingleOrDefault<User>();
            }

            if (user != null)
            {
                CreateCookie(login, isPersistent);
            }
            return user;
        }

        public void LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies[Consts.AUTH_COOKIE_NAME];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        private UserProvider CreateUserProvider(string login)
        {
            return new UserProvider(login, SessionProvider);
        }

        private void AuthenticateViaProviders()
        {
            foreach (IAuthProvider provider in authProviders)
            {
                try
                {
                    currentUser = new UserProvider(provider.Authenticate(HttpContext), SessionProvider);
                    return;
                }
                catch
                {
                }
            }

            if (currentUser == null)
            {
                throw new ApplicationException("Не удалось авторизовать пользователя");
            }
        }

        private void CreateCookie(string login, bool isPersistent = false)
        {
            logger.Info("CreateCookie. Name: " + login);
            HttpContext.Response.Cookies.Set(new HttpCookie(Consts.AUTH_COOKIE_NAME)
            {
                Value = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(login, isPersistent, int.MaxValue)),
                Expires = DateTime.Now.AddYears(1)
            });
        }

        #endregion IAuthentication
    }
}