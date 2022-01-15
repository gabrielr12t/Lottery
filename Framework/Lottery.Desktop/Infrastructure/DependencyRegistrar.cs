using Lottery.Core.Infrastructure;
using Lottery.Core.Infrastructure.DependencyManagement;
using Lottery.Desktop.Forms;
using Lottery.Desktop.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Desktop.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(IServiceCollection services, ITypeFinder typeFinder)
        {
            #region Forms

            services.AddScoped(typeof(MainForm));
            services.AddScoped(typeof(HistoricGamesForm));
            services.AddScoped(typeof(PlayForm));

            #endregion

            #region ViewModels

            services.AddScoped(typeof(MainViewModel));

            #endregion
        }

        public int Order => 100;
    }
}
