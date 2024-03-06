using AlpataBLL.Utilities.IOC;

namespace AlpataAPI.Extentions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection services, params ICoreModule[] modules)
        {
            foreach (var module in modules)
                module.Load(services);

            return services;
        }
    }
}
