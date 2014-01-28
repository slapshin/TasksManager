using Bootstrap;
using Bootstrap.Unity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using Web.Common;
using Web.Common.Mapper;
using Web.Common.Repository;
using Web.SPA.Common;

namespace Web.SPA.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            UnityServiceLocator locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
            Bootstrapper.With.Unity().WithContainer(container).Start();
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        #endregion Unity Container

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your types here
            // container.RegisterType<IProductRepository, ProductRepository>();

            container.RegisterInstance<ISessionProvider>(InitSessionProvider());
            container.RegisterInstance<IMapper>(new CommonMapper());
            container.RegisterInstance<IIoCTest>("1", new IoCTest() { Name = "1" });
            container.RegisterInstance<IIoCTest>("2", new IoCTest() { Name = "2" });
        }

        private static SessionProvider InitSessionProvider()
        {
            SessionProvider provider = new SessionProvider();
            provider.Init();
            return provider;
        }
    }
}