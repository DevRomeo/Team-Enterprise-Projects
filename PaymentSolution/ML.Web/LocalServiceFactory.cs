using StructureMap;

namespace ML.Web
{
    public class LocalServiceFactory : IServiceFactory
    {
        private readonly IContainer _container;
        
        public LocalServiceFactory(IContainer container)
        {
            _container = container;
        }

        public TService GetService<TService>() 
        {
            return _container.GetInstance<TService>();  
        }
    }
}