using Model;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Web.SPA.Areas.Customer.Models;
using Web.SPA.Common;

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
                    IEnumerable<Claim> data = session.QueryOver<Claim>().List();
                    result = ModelMapper.Map<IEnumerable<Claim>, IEnumerable<ClaimDto>>(data);
                });
            return Ok<IEnumerable<ClaimDto>>(result);
        }

        public IHttpActionResult Get(Guid id)
        {
            ClaimDto claim = null;
            ExecuteInSession(session => claim = ModelMapper.Map<Claim, ClaimDto>(session.Get<Claim>(id)));
            return Ok<ClaimDto>(claim);
        }

        [CheckModel]
        public IHttpActionResult Post(ClaimDto dto)
        {
            ExecuteInTransaction(session =>
            {
                Claim claim = dto.Id.HasValue ? session.Get<Claim>(dto.Id.Value) : new Claim();
                ModelMapper.Map<ClaimDto, Claim>(dto, claim);
                session.SaveOrUpdate(claim);
                dto = ModelMapper.Map<Claim, ClaimDto>(claim, dto);
            });

            return Ok<ClaimDto>(dto);
        }

        public IHttpActionResult Delete(Guid id)
        {
            ExecuteInTransaction(session => session.Delete(session.Load<Project>(id)));
            return Ok();
        }
    }
}