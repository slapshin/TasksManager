using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Web.Common.Repository;

namespace Web.Common
{
    public class Test
    {
        [Dependency]
        public ISessionProvider SessionProvider { get; set; }

        public void Action()
        {
            IIoCTest prov = ServiceLocator.Current.GetInstance<IIoCTest>("2");

            int y = 9;
        }
    }
}