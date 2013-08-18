using System.Web.Mvc;
using Web.Utils;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserLogin()
        {
            return PartialView(CurrentUser);
        }

        [Authorize]
        public ActionResult Menu()
        {
            return View(CurrentUser);
        }
    }
}
