namespace Lottery.Desktop.Forms
{
    public partial class HistoricGamesForm : Form
    {
        private readonly Action? _loadTheme;

        public HistoricGamesForm(Action? loadTheme = null)
        {
            InitializeComponent();
            _loadTheme = loadTheme;
        }

        private void HistoricGamesFormLoad(object sender, EventArgs e)
        {
            _loadTheme?.Invoke();
        }
    }
}
