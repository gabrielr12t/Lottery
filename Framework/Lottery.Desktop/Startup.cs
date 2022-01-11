using Lottery.Core;
using Lottery.Core.Infrastructure;
using Lottery.Framework.Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Desktop
{
    public class Startup
    {    
        public static void ConfigureServices(IServiceCollection services, ILotteryEnvironment environment)
        {
            services.ConfigureApplicationServices(environment);
        }

        public static void Configure(ILotteryApplicationBuilder application)
        {
            EngineContext.Current.ConfigureRequestPipeline(application);
        }
    }
}
