using ML.Core.Contracts;
using StructureMap;
using StructureMap.Graph;
using ML.Core;

namespace ML.Web
{
    public class StructureMapConfig
    {
        public static void Configure()
        {
#pragma warning disable 618
            var container = ObjectFactory.Container;
#pragma warning restore 618

            container.Configure(c =>
                c.Scan(scanner =>
                {
                    var assemblies = new[]
                        {
                            "ML.Web",
                            "ML.PaymentSolution.Services.Service",
                            "ML.Core"
                        };
                    scanner.TheCallingAssembly();
                    foreach (var assembly in assemblies)
                        scanner.Assembly(assembly);
                    scanner.WithDefaultConventions();
                    RegisterApplicationTypes(scanner);
                }));

            var dependencyConfigs = container.GetAllInstances<IDependencyConfig>();
            foreach (var dependencyConfig in dependencyConfigs)
                container.Configure(dependencyConfig.Configure);

            var initializers = container.GetAllInstances<IInitializer>();
            foreach (var initializer in initializers)
                initializer.Initialize(); 

        }
         
        private static void RegisterApplicationTypes(IAssemblyScanner scanner)
        {
            scanner.AddAllTypesOf<IDependencyConfig>();
            scanner.AddAllTypesOf<IInitializer>();
        }
    }
}