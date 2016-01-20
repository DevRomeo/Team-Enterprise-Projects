using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;
using ML.PaymentSolution.Services.Contracts;

namespace ML.Web
{
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public StructureMapDependencyResolver(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if(typeof(IService).IsAssignableFrom(serviceType))
            {

            }
            if(serviceType.IsInterface || serviceType.IsAbstract)
            {
                return GetInterfaceService(serviceType);
            }
            return GetConcreteService(serviceType);
        }

        private object GetConcreteService(Type serviceType)
        {
            try 
            {
                return _container.GetInstance(serviceType);
            }
            catch(Exception)
            {
                return null;
            }
        }

        private object GetInterfaceService(Type serviceType)
        {
            return _container.TryGetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType).Cast<object>();
        }
    }
}