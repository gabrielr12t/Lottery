using Lottery.Core.Infrastructure;
using Lottery.Core.Infrastructure.DependencyManagement;
using Lottery.Desktop.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Desktop.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(IServiceCollection services, ITypeFinder typeFinder)
        {
            services.AddScoped(typeof(MainForm));
            services.AddScoped(typeof(HistoricGamesForm));
            services.AddScoped(typeof(PlayForm));
        }

        public int Order => 100;
    }
}
