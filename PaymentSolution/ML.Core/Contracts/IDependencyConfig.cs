using StructureMap;

namespace ML.Core
{
    public interface IDependencyConfig
    {
        void Configure(ConfigurationExpression config);
    }
}
