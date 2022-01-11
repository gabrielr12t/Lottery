using Lottery.Core.Infrastructure;
using Lottery.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Data.Startup
{
    public class DataStartup : ILotteryStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
        }

        public void Configure() { }

        public int Order => 10;
    }
}
