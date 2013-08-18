using Model;
using Model.Common;
using NHibernate;
using Ninject;
using System;
using System.Web.Http;
using Web.Common.Auth;
using Web.Common.Mapper;
using Web.Common.Repository;

namespace Web.SPA.Common
{
    public abstract class BaseApiController : ApiController
    {
        private ISession dbSession;

        public BaseApiController()
            : base()
        {
        }

        [Inject]
        public ISessionProvider Provider { get; set; }

        [Inject]
        public IAuthentication Auth { get; set; }

        [Inject]
        public IMapper ModelMapper { get; set; }

        public ISession DbSession
        {
            get
            {
                if (dbSession == null)
                {
                    dbSession = Provider.OpenSession();
                };
                return dbSession;
            }
        }

        public User CurrentUser
        {
            get { return ((IUserProvider)Auth.CurrentUser.Identity).User; }
        }

        protected T GetEntity<T>(object id, string errorMsg = "") where T : Entity<T>
        {
            T obj = DbSession.Get<T>(id);
            if (obj == null)
            {
                throw new ApplicationException(string.IsNullOrWhiteSpace(errorMsg) ? string.Format("Объект [id: {0}] не найден", id) : errorMsg);
            }
            return obj;
        }

        protected T LoadEntity<T>(object id) where T : Entity<T>
        {
            return DbSession.Get<T>(id);
        }
    }
}