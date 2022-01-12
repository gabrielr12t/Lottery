using Lottery.Core.Infrastructure;
using Lottery.Core.Infrastructure.DependencyManagement;
using Lottery.Shared.Components;
using Lottery.Shared.ServicesForm.Alerts;
using Lottery.Shared.ServicesForm.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Shared.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(IServiceCollection services, ITypeFinder typeFinder)
        {
            #region Forms

            services.AddScoped(typeof(AlertForm));

            #endregion

            #region Servicess

            services.AddSingleton<IAlertService, AlertService>();
            services.AddSingleton<IFormHandleService, FormHandleService>();

            #endregion
        }

        public int Order => 20;
    }
}
