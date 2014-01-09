using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Web.SPA.Startup))]

namespace Web.SPA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}