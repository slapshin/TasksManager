using Common;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;

namespace Model.Mapping.Tests
{
    [TestClass]
    public class MappingsTest
    {
        private const string DATABASE = "tasks";
        private const string PASS = "develop";
        private const string SERVER = @"(local)\SQLEXPRESS";
        private const string USER = "develop";
        private Configuration configuration;

        [TestMethod]
        public void Create100Calls()
        {
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session = factory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                User root = session.QueryOver<User>()
                  .Where(u => u.Login == "root")
                  .SingleOrDefault();

                Project project = new Project()
                {
                    Title = "New project",
                    Master = root
                };
                session.Save(project);

                for (int i = 0; i < 100; i++)
                {
                    Call call = new Call()
                    {
                        Title = "call_" + i,
                        Project = project,
                        Created = DateTime.Now
                    };
                    session.Save(call);
                }

                transaction.Commit();
            }
        }

        [TestMethod]
        public void Create100Projects()
        {
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session = factory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                User root = session.QueryOver<User>()
                   .Where(u => u.Login == "root")
                   .SingleOrDefault();

                for (int i = 0; i < 100; i++)
                {
                    Project project = new Project()
                    {
                        Title = "New project" + i,
                        Master = root
                    };
                    session.Save(project);
                }

                transaction.Commit();
            }
        }

        [TestMethod]
        public void Create100Users()
        {
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session = factory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                for (int i = 0; i < 100; i++)
                {
                    User user = new User()
                    {
                        Login = "login" + i,
                        Email = "email" + i + "@mail.ru",
                        Name = "name" + i,
                        Password = "password" + i,
                        Patronymic = "patronymic" + i,
                        Surname = "surname" + i
                    };
                    user.Roles |= UserRole.Executor;
                    session.Save(user);
                }

                transaction.Commit();
            }
        }

        [TestMethod]
        public void CreateProject()
        {
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session = factory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                User root = session.QueryOver<User>()
                    .Where(u => u.Login == "root")
                    .SingleOrDefault();

                Project project = new Project()
                {
                    Title = "New project",
                    Master = root
                };
                session.Save(project);
                transaction.Commit();
            }
        }

        [TestMethod]
        public void CreateRoot()
        {
            ISessionFactory factory = configuration.BuildSessionFactory();

            using (ISession session = factory.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                User root = new User()
                {
                    Login = "root",
                    Password = Helpers.CreateMD5Hash("root")
                };
                root.Roles |= UserRole.Admin | UserRole.Customer | UserRole.Executor | UserRole.Master | UserRole.Router | UserRole.Tester;
                session.Save(root);
                transaction.Commit();
            }
        }

        [TestMethod]
        public void Export()
        {
            if (SERVER == "ivanovo")
            {
                throw new ApplicationException("Нельзя!!!");
            }
            new SchemaExport(configuration).Create(false, true);
        }

        [TestInitialize]
        public void Init()
        {
            configuration = Fluently.Configure().Database(
                MsSqlConfiguration.MsSql2008.ConnectionString(builder =>
                {
                    builder.Username(USER)
                            .Password(PASS)
                            .Server(SERVER)
                            .Database(DATABASE);
                })
                    .ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildConfiguration();
        }

        [TestMethod]
        public void Update()
        {
            new SchemaUpdate(configuration).Execute(true, true);
        }
    }
}