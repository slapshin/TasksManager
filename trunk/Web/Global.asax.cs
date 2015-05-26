using NLog;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.Common;
using Web.Common.Auth;

namespace Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private static Logger logger = LogManager.GetLogger(Consts.LoggerName);

        public MvcApplication()
            : base()
        {
            AuthenticateRequest += Authenticate;
        }

        protected void Application_Start()
        {
            logger.Info("Запуск приложения");

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger.Info("Приложение запущено");
        }

        protected void Application_End()
        {
            logger.Info("Приложение завершено");
        }

        private void Authenticate(object sender, EventArgs e)
        {
            HttpApplication app = (sender as HttpApplication);
            IAuthentication auth = DependencyResolver.Current.GetService<IAuthentication>();
            auth.HttpContext = app.Context;
            app.Context.User = auth.CurrentUser;
        }
    }
}