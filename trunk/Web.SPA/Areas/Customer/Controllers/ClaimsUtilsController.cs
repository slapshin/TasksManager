using Model;
using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;
using System.Web.Http;
using Web.SPA.Areas.Customer.Models;
using Web.SPA.Common;
using Web.SPA.Models;

namespace Web.SPA.Areas.Customer.Controllers
{
    [Authorize(Roles = "Customer")]
    [RoutePrefix("api/Customer/Claims")]
    public class ClaimsUtilsController : BaseApiController
    {
        public class ClaimsPageParams : PageParams
        {
        }

        [Route("Page")]
        [HttpPost]
        public IHttpActionResult Page(ClaimsPageParams parameters)
        {
            PageResult result = null;
            ExecuteInSession(session =>
            {
                IList<Claim> data = GetPageCriteriaByParams(session, parameters)
                                    .SetFirstResult((parameters.Page - 1) * parameters.PageSize)
                                    .SetMaxResults(parameters.PageSize)
                                    .List<Claim>();

                result = new PageResult()
                {
                    Total = GetPageCriteriaByParams(session, parameters).SetProjection(Projections.Count(Projections.Id())).UniqueResult<int>(),
                    Data = modelMapper.Map<IEnumerable<Claim>, IEnumerable<ClaimDto>>(data)
                };
            });

            return Ok<PageResult>(result);
        }

        private ICriteria GetPageCriteriaByParams(ISession session, ClaimsPageParams parameters)
        {
            return session.CreateCriteria<Claim>();
        }
    }
}