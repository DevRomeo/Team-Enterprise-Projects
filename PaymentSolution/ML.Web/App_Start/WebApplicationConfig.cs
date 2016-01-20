using System.Web.Mvc;
using ML.Core;
using ML.Core.Contracts;

namespace ML.Web
{
    public class WebApplicationConfig : IInitializer
    {
        private readonly StructureMapDependencyResolver dependencyResolver;

        public WebApplicationConfig(StructureMapDependencyResolver dependencyResolver) 
        {
            this.dependencyResolver = dependencyResolver;
        }

        public void Initialize()
        {
            DependencyResolver.SetResolver(this.dependencyResolver);
        }
    }
}