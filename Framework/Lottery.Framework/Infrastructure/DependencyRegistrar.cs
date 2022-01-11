using Lottery.Core.Caching;
using Lottery.Core.Events;
using Lottery.Core.Infrastructure;
using Lottery.Core.Infrastructure.DependencyManagement;
using Lottery.Services.Events;
using Lottery.Services.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Framework.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(IServiceCollection services, ITypeFinder typeFinder)
        {
            services.AddScoped<ILocker, MemoryCacheManager>();
            services.AddScoped<IStaticCacheManager, MemoryCacheManager>();
            services.AddScoped<ILotteryFileProvider, LotteryFileProvider>();
            services.AddScoped<ILogger, DefaultLogger>();

            services.AddSingleton<IEventPublisher, EventPublisher>();

            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IConsumer<>)))
                    services.AddScoped(findInterface, consumer);
        }

        public int Order => 0;
    }
}
