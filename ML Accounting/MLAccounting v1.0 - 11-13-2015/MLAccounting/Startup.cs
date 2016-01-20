using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MLAccountingWeb.Startup))]
namespace MLAccountingWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
