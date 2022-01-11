using Lottery.Core;
using Lottery.Core.Infrastructure;
using Lottery.Desktop.Forms;
using Lottery.Services.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Desktop
{
    public static class Program
    {
        private static readonly IServiceCollection _services;
        private static readonly ILotteryEnvironment _environment;

        static Program()
        {
            _services = new ServiceCollection();
            _environment = new LotteryEnvironment(GetAbsoluteApplicationPath());
        }

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;

            ConfigureDefaultServices();

            CommonHelper.DefaultFileProvider = new LotteryFileProvider(_environment);
            CommonHelper.DefaultFileProvider.CreateDirectory(GetAbsoluteApplicationPath());

            Startup.ConfigureServices(_services, _environment);
            Startup.Configure(new LotteryApplicationBuilder(_services.BuildServiceProvider()));

            Application.Run(new MainForm());
        }

        private static void ConfigureDefaultServices()
        {
            _services.AddSingleton(_environment);
        }

        private static string GetAbsoluteApplicationPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return Combine(programFilesPath, LotteryCoreSettingsDefaults.ApplicationPath);
        }

        private static string Combine(params string[] paths)
        {
            var path = Path.Combine(paths.SelectMany(p => IsUncPath(p) ? new[] { p } : p.Split('\\', '/')).ToArray());

            if (Environment.OSVersion.Platform == PlatformID.Unix && !IsUncPath(path))
                path = "/" + path;

            return path;
        }

        private static bool IsUncPath(string path)
        {
            return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is LotteryException lotteryException)
            {
                //exibir msg no form
            }
            else
            {
                //exibir mensagem default no form
            }

            var logger = EngineContext.Current.Resolve<ILogger>();
            logger.ErrorAsync(e?.ExceptionObject?.ToString(), (Exception)e?.ExceptionObject).GetAwaiter().GetResult();
        }
    }
}