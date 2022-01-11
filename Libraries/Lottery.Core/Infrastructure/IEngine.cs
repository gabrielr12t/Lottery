using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Core.Infrastructure
{
    public interface IEngine
    {
        void ConfigureServices(IServiceCollection services);

        void ConfigureRequestPipeline(ILotteryApplicationBuilder provider);

        T Resolve<T>(IServiceScope? scope = null) where T : class;

        object Resolve(Type type, IServiceScope? scope = null);

        IEnumerable<T> ResolveAll<T>();

        object ResolveUnregistered(Type type);

        void RegisterDependencies(IServiceCollection services);
    }
}
