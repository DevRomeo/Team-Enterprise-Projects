using System;
using System.ServiceModel;

namespace ML.Web
{
    public class ServiceFactory : IServiceFactory
    {
        private const string BaseAddress = "http://192.168.13.136/wcf";
        public TService GetService<TService>(string servicename)
        {
            var serviceType = typeof(TService);

            var endPoint = new EndpointAddress(new Uri(string.Format("{0}/{1}.svc", BaseAddress, serviceType.Name.Substring(1))));

            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);

            var httpFactory =
                new ChannelFactory<TService>(
                    binding,
                    endPoint);

            var contract = httpFactory.CreateChannel();

            return Equals(contract, null)
                    ? default(TService)
                    : contract;
        }

        public TService GetService<TService>()
        {
            throw new NotImplementedException();
        }

    }
}