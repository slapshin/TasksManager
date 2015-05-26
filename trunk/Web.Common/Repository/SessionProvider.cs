using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Model.Mapping;
using NHibernate;
using NLog;
using System;

namespace Web.Common.Repository
{
    public class SessionProvider : ISessionProvider, IDisposable
    {
        private static Logger logger = LogManager.GetLogger(Consts.LoggerName);

        private ISessionFactory factory;

        public SessionProvider()
        {
            var config = DatabaseConfiguration.GetConfiguration();

            logger.Info("Создание фабрики сессий баз данных [server: {0}; database: {1}, user: {2}]", config.Server, config.Database, config.User);
            factory = Fluently.Configure().Database(
                MsSqlConfiguration.MsSql2008.ConnectionString(builder =>
                {
                    builder.Username(config.User)
                            .Password(config.Password)
                            .Server(config.Server)
                            .Database(config.Database);
                }).ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildSessionFactory();

            logger.Info("Фабрика сессий баз данных создана");
        }

        public ISession OpenSession()
        {
            if (factory == null)
            {
                throw new ApplicationException("Фабрика сессий не создана");
            }

            return factory.OpenSession();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SessionProvider()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (factory == null)
                {
                    return;
                }

                logger.Info("Закрытие фабрики сессий баз данных");

                try
                {
                    factory.Dispose();
                    factory = null;
                }
                catch
                {
                }

                logger.Info("Фабрика сессий баз данных закрыта");
            }
        }
    }
}