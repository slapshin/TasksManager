using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using Web.SPA.Areas.Customer.Models;
using Web.SPA.Common;
using ModelClaim = Model.Claim;

namespace Web.SPA.Areas.Customer.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ClaimsController : BaseApiController
    {
        public IHttpActionResult Get()
        {
            IEnumerable<ClaimDto> result = new List<ClaimDto>();
            ExecuteInSession(session =>
                {
                    IEnumerable<ModelClaim> data = session.QueryOver<ModelClaim>().List();
                    result = modelMapper.Map<IEnumerable<ModelClaim>, IEnumerable<ClaimDto>>(data);
                });
            return Ok<IEnumerable<ClaimDto>>(result);
        }

        public IHttpActionResult Get(Guid id)
        {
            ClaimDto claim = null;
            ExecuteInSession(session => claim = modelMapper.Map<ModelClaim, ClaimDto>(session.Get<ModelClaim>(id)));
            return Ok<ClaimDto>(claim);
        }

        [CheckModel]
        public IHttpActionResult Post(ClaimDto dto)
        {
            ExecuteInTransaction(session =>
            {
                string user = (User.Identity as ClaimsIdentity).Claims.Where<Claim>(c => c.Type == ClaimTypes.NameIdentifier).Single().Value;
                ModelClaim claim = dto.Id.HasValue ?
                    session.Get<ModelClaim>(dto.Id.Value) :
                    new ModelClaim()
                        {
                            Customer = LoadEntity<Model.User>(session, Guid.Parse(user)),
                            Created = DateTime.Now
                        };
                modelMapper.Map<ClaimDto, ModelClaim>(dto, claim);
                session.SaveOrUpdate(claim);
                dto = modelMapper.Map<ModelClaim, ClaimDto>(claim, dto);
            });

            return Ok<ClaimDto>(dto);
        }

        public IHttpActionResult Delete(Guid id)
        {
            ExecuteInTransaction(session => session.Delete(session.Load<ModelClaim>(id)));
            return Ok();
        }
    }
}