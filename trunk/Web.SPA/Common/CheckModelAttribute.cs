using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace Web.SPA.Common
{
    public class CheckModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            StringBuilder errors = new StringBuilder();
            foreach (var modelState in context.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errors.AppendLine(error.ErrorMessage);
                }
            }
            context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errors.ToString());
        }
    }
}