using Lottery.Services.Logging;
using Lottery.Shared.ViewModels;

namespace Lottery.Desktop.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Title = "Home"; 
        }

        private string? _title;
        public string? Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
