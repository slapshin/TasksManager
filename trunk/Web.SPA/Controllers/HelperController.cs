using Model;
using System.Web.Http;

namespace Web.SPA.Controllers
{
    [RoutePrefix("api/Helper")]
    public class HelperController : ApiController
    {
        [Authorize]
        [Route("MainMenu")]
        [HttpGet]
        public IHttpActionResult MainMenu()
        {
            bool isAdmin = User.IsInRole("Admin");
            bool Test = User.IsInRole("Test");
            //UserRole roles = (UserRole)int.Parse((User as ClaimsPrincipal).FindFirst(ClaimTypes.Role).Value);
            return Ok();
        }
    }
}