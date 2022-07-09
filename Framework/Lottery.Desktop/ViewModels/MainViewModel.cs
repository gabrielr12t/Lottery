using Lottery.Desktop.Forms.Settings;
using Lottery.Shared.ViewModels;
using System.ComponentModel;

namespace Lottery.Desktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Ctor

        public MainViewModel()
        {
            base.PropertyChanged += MainViewModelPropertyChanged;
        }

        #endregion

        #region Utilities

        private void MainViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                default:
                    break;
            }
        }

        #endregion

        #region Properties


        private string? _title = "Home";
        public string? Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _closeChildForm;
        public bool CloseChild
        {
            get { return _closeChildForm; }
            set { SetProperty(ref _closeChildForm, value); }
        }

        #endregion
    }
}
