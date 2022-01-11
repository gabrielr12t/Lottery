using Lottery.Desktop.Forms.Settings;

namespace Lottery.Desktop.Forms
{
    public partial class PlayForm : Form
    {
        private readonly Action? _loadTheme;

        public PlayForm(Action? loadTheme = null)
        {
            InitializeComponent();
            _loadTheme = loadTheme; 
        }

        private void PlayFormLoad(object sender, EventArgs e)
        {
            _loadTheme?.Invoke();
        } 
    }
}
