using Model;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.SPA.Areas.Admin.Models;
using Web.SPA.Common;

namespace Web.SPA.Areas.Admin.Controllers
{
    public class UsersController : BaseApiController
    {
        public IEnumerable<UserDto> Get()
        {
            using (ISession session = Provider.OpenSession())
            {
                List<UserDto> users = new List<UserDto>();
                foreach (User user in session.QueryOver<User>().List())
                {
                    users.Add(ModelMapper.Map<User, UserDto>(user));
                }
                return users;
            }
        }

        public UserDto Get(Guid id)
        {
            using (ISession session = Provider.OpenSession())
            {
                return ModelMapper.Map<User, UserDto>(session.Get<User>(id));
            }
        }

        public HttpResponseMessage PutTodoList(Guid id, UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != userDto.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                using (ISession session = Provider.OpenSession())
                using (ITransaction trans = session.BeginTransaction())
                {
                    User user = session.Get<User>(id);
                    ModelMapper.Map<UserDto, User>(userDto, user);
                    trans.Commit();
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage PostTodoList(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                using (ISession session = Provider.OpenSession())
                using (ITransaction trans = session.BeginTransaction())
                {
                    User user = userDto.Id.HasValue ? session.Get<User>(userDto.Id.Value) : new User();
                    ModelMapper.Map<UserDto, User>(userDto, user);
                    session.SaveOrUpdate(user);
                    trans.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        public HttpResponseMessage Delete(Guid id)
        {
            using (ISession session = Provider.OpenSession())
            using (ITransaction trans = session.BeginTransaction())
            {
                try
                {
                    session.Delete(session.Load<User>(id));
                    trans.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }
        }

        [HttpGet("api/Admin/Users/Page")]
        public IEnumerable<UserDto> Page(int page, int pageSize)
        {
            using (ISession session = Provider.OpenSession())
            {
                List<UserDto> users = new List<UserDto>();
                ICriteria criteria = session.CreateCriteria<User>();

                IList<User> list = criteria.SetFirstResult((page - 1) * pageSize)
                            .SetMaxResults(pageSize).List<User>();

                foreach (User user in list)
                {
                    users.Add(ModelMapper.Map<User, UserDto>(user));
                }
                return users;
            }
        }

        [HttpGet("api/Admin/Users/Count")]
        public HttpResponseMessage Count()
        {
            using (ISession session = Provider.OpenSession())
            {
                int count = session.CreateCriteria<User>()
                            .SetProjection(Projections.Count(Projections.Id()))
                            .UniqueResult<int>();

                return Request.CreateResponse(HttpStatusCode.OK, count);
            }
        }
    }
}