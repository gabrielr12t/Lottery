using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Core.Infrastructure.DependencyManagement
{
    public interface IDependencyRegistrar
    {
        void Register(IServiceCollection services, ITypeFinder typeFinder);

        int Order { get; }
    }
}
