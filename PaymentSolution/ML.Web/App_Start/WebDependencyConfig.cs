using System.Web.Optimization;
using System.Web.Routing;
using ML.Core;
using StructureMap;

namespace ML.Web
{
    public class WebDependencyConfig : IDependencyConfig
    {
         bool flag = false;

        public void Configure(ConfigurationExpression config)
        {
            config.For<RouteCollection>().Singleton().Use(RouteTable.Routes);
            config.For<BundleCollection>().Singleton().Use(BundleTable.Bundles);

            if (flag)
                config.For<IServiceFactory>().Use<ServiceFactory>();
            else
                config.For<IServiceFactory>().Use<LocalServiceFactory>();
        }
    }
}