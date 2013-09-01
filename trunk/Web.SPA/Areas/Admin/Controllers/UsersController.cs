using Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Web.SPA.Areas.Admin.Models;
using Web.SPA.Common;

namespace Web.SPA.Areas.Admin.Controllers
{
    public class UsersController : BaseApiController
    {
        public IEnumerable<UserDto> Get()
        {
            List<UserDto> users = new List<UserDto>();
            ExecuteInSession(session =>
            {
                foreach (User user in session.QueryOver<User>().List())
                {
                    users.Add(ModelMapper.Map<User, UserDto>(user));
                }
            });
            return users;
        }

        public UserDto Get(Guid id)
        {
            UserDto user = null;
            ExecuteInSession(session =>
            {
                user = ModelMapper.Map<User, UserDto>(session.Get<User>(id));
            });
            return user;
        }

        [CheckModel]
        public HttpResponseMessage Post(UserDto userDto)
        {
            ExecuteInTransaction(session =>
            {
                User user = userDto.Id.HasValue ? session.Get<User>(userDto.Id.Value) : new User();
                ModelMapper.Map<UserDto, User>(userDto, user);
                session.SaveOrUpdate(user);
                userDto = ModelMapper.Map<User, UserDto>(user, userDto);
            });

            return Request.CreateResponse(HttpStatusCode.OK, userDto);
        }

        public HttpResponseMessage Delete(Guid id)
        {
            ExecuteInTransaction(session =>
            {
                session.Delete(session.Load<User>(id));
            });

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}