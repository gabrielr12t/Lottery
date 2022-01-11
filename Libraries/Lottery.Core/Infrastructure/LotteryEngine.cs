using Lottery.Core.Infrastructure.DependencyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Lottery.Core.Infrastructure
{
    public class LotteryEngine : IEngine
    {
        #region Utilities

        protected IServiceProvider GetServiceProvider(IServiceScope scope = null)
        {
            return ServiceProvider;
        }

        protected virtual void RunStartupTasks(ITypeFinder typeFinder)
        {
            //find startup tasks provided by other assemblies
            var startupTasks = typeFinder.FindClassesOfType<IStartupTask>();

            //create and sort instances of startup tasks
            //we startup this interface even for not installed plugins. 
            //otherwise, DbContext initializers won't run and a plugin installation won't work
            var instances = startupTasks
                .Select(startupTask => Activator.CreateInstance(startupTask) as IStartupTask)
                .OrderBy(startupTask => startupTask?.Order);

            //execute tasks
            foreach (var task in instances)
                task?.ExecuteAsync().Wait();
        }

        public virtual void RegisterDependencies(IServiceCollection services)
        {
            var typeFinder = new WebAppTypeFinder();

            //register engine
            services.AddSingleton<IEngine>(this);

            //register type finder
            services.AddSingleton<ITypeFinder>(typeFinder);

            //find dependency registrars provided by other assemblies
            var dependencyRegistrars = typeFinder.FindClassesOfType<IDependencyRegistrar>();

            //create and sort instances of dependency registrars
            var instances = dependencyRegistrars
                .Select(dependencyRegistrar => Activator.CreateInstance(dependencyRegistrar) as IDependencyRegistrar)
                .OrderBy(dependencyRegistrar => dependencyRegistrar?.Order);

            //register all provided dependencies
            foreach (var dependencyRegistrar in instances)
                dependencyRegistrar?.Register(services, typeFinder);

            services.AddSingleton(services);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //check for assembly already loaded
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            //get assembly from TypeFinder
            var tf = Resolve<ITypeFinder>();
            assembly = tf?.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            return assembly;
        }

        #endregion

        #region Methods

        public void ConfigureServices(IServiceCollection services)
        {
            //find startup configurations provided by other assemblies
            var typeFinder = new WebAppTypeFinder();
            var startupConfigurations = typeFinder.FindClassesOfType<ILotteryStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => Activator.CreateInstance(startup) as ILotteryStartup)
                .OrderBy(startup => startup?.Order);

            //configure services
            foreach (var instance in instances)
                instance?.ConfigureServices(services);

            //run startup tasks
            RunStartupTasks(typeFinder);

            //resolve assemblies here. otherwise, plugins can throw an exception when rendering views
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        public void ConfigureRequestPipeline(ILotteryApplicationBuilder application)
        {
            ServiceProvider = application.ApplicationServices;

            //find startup configurations provided by other assemblies
            var typeFinder = Resolve<ITypeFinder>();
            var startupConfigurations = typeFinder.FindClassesOfType<ILotteryStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => Activator.CreateInstance(startup) as ILotteryStartup)
                .OrderBy(startup => startup?.Order);

            //configure request pipeline
            foreach (var instance in instances)
                instance?.Configure();
        }
 
        public T Resolve<T>(IServiceScope? scope = null) where T : class
        {
            return (T)Resolve(typeof(T), scope);
        }
        
        public object Resolve(Type type, IServiceScope? scope = null)
        {
            return GetServiceProvider(scope)?.GetService(type);
        }

        public virtual IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }
 
        public virtual object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        if (service == null)
                            throw new LotteryException("Unknown dependency");
                        return service;
                    });

                    //all is ok, so create instance
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new LotteryException("No constructor was found that had all the dependencies satisfied.", innerException);
        }

        #endregion

        #region Properties
         
        public virtual IServiceProvider ServiceProvider { get; protected set; }

        #endregion
    }
}
