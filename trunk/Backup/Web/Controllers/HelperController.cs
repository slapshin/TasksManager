using Model;
using System.Web.Mvc;
using Web.Utils;

namespace Web.Controllers
{
    public class HelperController : BaseController
    {
        public ActionResult CurrentMasterProjects()
        {
            var projects = (from p in DbSession.QueryOver<Project>()
                            where p.Master == CurrentUser
                            orderby p.Priority
                            select p).Asc.List();
            return PartialView(projects);
        }
    }
}