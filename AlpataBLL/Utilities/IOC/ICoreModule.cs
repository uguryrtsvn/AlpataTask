using Microsoft.Extensions.DependencyInjection;

namespace AlpataBLL.Utilities.IOC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection services);
    }
}
