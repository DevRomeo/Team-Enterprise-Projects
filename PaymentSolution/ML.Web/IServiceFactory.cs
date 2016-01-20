using System.Security.Cryptography.X509Certificates;

namespace ML.Web
{
    public interface IServiceFactory
    {
        TService GetService<TService>();        
    }
}
