using Lottery.Core;
using Lottery.Core.Infrastructure;
using Lottery.Desktop.Forms;
using Lottery.Services.Logging;
using Lottery.Shared.ServicesForm.Alerts;
using Microsoft.Extensions.DependencyInjection;

namespace Lottery.Desktop
{
    public static class Program
    {

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += Catch;
            AppDomain.CurrentDomain.UnhandledException += Catch;

            LotteryApplication.Instance.Start();
        }

        static async void Catch(object sender, ThreadExceptionEventArgs e)
        {
            await Catch(e?.Exception);
        }

        static async void Catch(object sender, UnhandledExceptionEventArgs e)
        {
            await Catch(e?.ExceptionObject as Exception);
        }

        static async Task Catch(Exception? exception)
        {
            var alertService = EngineContext.Current.Resolve<IAlertService>();

            var errorMessage = exception is LotteryException lotteryException ?
                lotteryException.Message : "Ocorreu um erro interno";

            alertService.ErrorAlert(errorMessage);

            var logger = EngineContext.Current.Resolve<ILogger>();
            await logger.ErrorAsync(exception?.Message, exception) ;
        }
    }

    public class LotteryApplication
    {
        private static LotteryApplication _instance;
        private readonly IServiceCollection _services;
        private readonly ILotteryEnvironment _environment;

        private LotteryApplication()
        {
            _services = new ServiceCollection();
            _environment = new LotteryEnvironment(GetAbsoluteApplicationPath());
        }

        public static LotteryApplication Instance
        {
            get
            {
                return _instance ??= new LotteryApplication();
            }
        }

        internal void Start()
        {
            StartOverviewWindow();
        }

        private void StartOverviewWindow()
        {
            ConfigureDefaultServices();

            CommonHelper.DefaultFileProvider = new LotteryFileProvider(_environment);
            CommonHelper.DefaultFileProvider.CreateDirectory(GetAbsoluteApplicationPath());

            Startup.ConfigureServices(_services, _environment);
            Startup.Configure(new LotteryApplicationBuilder(_services.BuildServiceProvider()));

            var mainForm = EngineContext.Current.Resolve<MainForm>();

            Application.Run(mainForm);
        }

        #region Utilities

        private void ConfigureDefaultServices()
        {
            _services.AddSingleton(_environment);
        }

        private string GetAbsoluteApplicationPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return Combine(programFilesPath, LotteryCoreSettingsDefaults.ApplicationPath);
        }

        private string Combine(params string[] paths)
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

        #endregion
    }
}