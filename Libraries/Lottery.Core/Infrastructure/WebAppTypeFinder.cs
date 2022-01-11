using System.Reflection;

namespace Lottery.Core.Infrastructure
{
    public class WebAppTypeFinder : AppDomainTypeFinder
    {
        #region Fields

        private bool _binFolderAssembliesLoaded;

        #endregion

        #region Ctor

        public WebAppTypeFinder(ILotteryFileProvider fileProvider = null) : base(fileProvider)
        {
        }

        #endregion

        #region Properties

        public bool EnsureBinFolderAssembliesLoaded { get; set; } = true;

        #endregion

        #region Methods

        public virtual string GetBinDirectory()
        {
            return AppContext.BaseDirectory;
        }

        public override IList<Assembly> GetAssemblies()
        {
            if (!EnsureBinFolderAssembliesLoaded || _binFolderAssembliesLoaded)
                return base.GetAssemblies();

            _binFolderAssembliesLoaded = true;
            var binPath = GetBinDirectory();
            //binPath = _webHelper.MapPath("~/bin");
            LoadMatchingAssemblies(binPath);

            return base.GetAssemblies();
        }

        #endregion
    }
}
