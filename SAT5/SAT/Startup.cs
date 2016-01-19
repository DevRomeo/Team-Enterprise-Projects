using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SAT.Startup))]
namespace SAT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
