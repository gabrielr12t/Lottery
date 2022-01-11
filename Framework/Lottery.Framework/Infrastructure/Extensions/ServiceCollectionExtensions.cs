using Lottery.Core;
using Lottery.Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Framework.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IEngine ConfigureApplicationServices(this IServiceCollection services, ILotteryEnvironment environment)
        {
            CommonHelper.DefaultFileProvider = new LotteryFileProvider(environment);

            services.AddMemoryCache();

            var engine = EngineContext.Create();

            engine.ConfigureServices(services);
            engine.RegisterDependencies(services);

            return engine;
        }
    }
}
