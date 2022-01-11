using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Core.Infrastructure
{
    public interface ILotteryStartup
    {
        void ConfigureServices(IServiceCollection services);

        void Configure();

        int Order { get; }
    }
}
