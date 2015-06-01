using Microsoft.Practices.Unity;
using Model.Common;
using NHibernate;
using System;
using System.Web.Http;
using Web.Common.Mapper;
using Web.Common.Repository;

namespace Web.SPA.Common
{
    public abstract class BaseApiController : ApiController
    {
        [Dependency]
        public ISessionProvider SessionProvider { get; set; }

        [Dependency]
        public IMapper ModelMapper { get; set; }

        //public BaseApiController()
        //{
        //    SessionProvider = (ISessionProvider)DependencyResolver.Current.GetService(typeof(ISessionProvider));
        //    ModelMapper = (IMapper)DependencyResolver.Current.GetService(typeof(IMapper));
        //}

        protected void ExecuteInTransaction(Action<ISession> action)
        {
            using (ISession session = SessionProvider.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    if (action != null)
                    {
                        action(session);
                        transaction.Commit();
                    }
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        protected void ExecuteInSession(Action<ISession> action)
        {
            using (var session = SessionProvider.OpenSession())
            {
                if (action != null)
                {
                    action(session);
                }
            }
        }

        protected T GetEntity<T>(ISession session, object id, string errorMsg = "") where T : Entity<T>
        {
            T obj = session.Get<T>(id);
            if (obj == null)
            {
                throw new ApplicationException(string.IsNullOrWhiteSpace(errorMsg) ? string.Format("Объект [id: {0}] не найден", id) : errorMsg);
            }
            return obj;
        }

        protected T LoadEntity<T>(ISession session, object id) where T : Entity<T>
        {
            return session.Get<T>(id);
        }
    }
}