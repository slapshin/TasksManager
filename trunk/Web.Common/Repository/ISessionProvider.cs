using NHibernate;

namespace Web.Common.Repository
{
    public interface ISessionProvider
    {
        ISession OpenSession();
    }
}