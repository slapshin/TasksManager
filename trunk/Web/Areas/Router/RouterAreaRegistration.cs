using System.Web.Mvc;

namespace Web.Areas.Router
{
    public class RouterAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Router"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Router_default",
                "Router/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
