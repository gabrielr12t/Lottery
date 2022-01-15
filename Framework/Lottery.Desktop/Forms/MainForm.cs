using Lottery.Desktop.Forms.Settings;
using Lottery.Desktop.ViewModels;

namespace Lottery.Desktop.Forms
{
    public partial class MainForm : Form
    {
        private Button? _currentButton;
        private Random _random;
        private int tempIndex;
        private ThemeColor _themeColor;
        private Form _activeForm;

        private readonly MainViewModel _mainViewModel;

        public MainForm(MainViewModel mainViewModel)
        {
            InitializeComponent();

            _mainViewModel = mainViewModel;
            _themeColor = new ThemeColor();
            _random = new Random();
            lblTitle.Text = _mainViewModel.Title;
            btnCloseChildForm.Visible = false;
        }

        private Color SelectThemeColor()
        {
            int index = _random.Next(_themeColor.ColorList.Count);
            while (tempIndex == index)
                index = _random.Next(_themeColor.ColorList.Count);

            tempIndex = index;
            string color = _themeColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }

        private void DisableButton()
        {
            foreach (Control previousButton in panelMenu.Controls)
            {
                if (previousButton.GetType() == typeof(Button))
                {
                    previousButton.BackColor = Color.FromArgb(51, 51, 76);
                    previousButton.ForeColor = Color.Gainsboro;
                }
            }
        }

        private void LoadTheme()
        {
            foreach (Control control in _activeForm.Controls)
            {
                if (control.GetType() == typeof(Button))
                {
                    Button button = (Button)control;
                    button.BackColor = _themeColor.PrimaryColor;
                    button.ForeColor = Color.White;
                    button.FlatAppearance.BorderColor = _themeColor.SecondaryColor;
                }
                if (control.GetType() == typeof(Label))
                {
                    Label label = (Label)control;
                    label.ForeColor = _themeColor.PrimaryColor;
                }
            }
        }

        private void OpenChildForm(Form childForm, object sender)
        {
            if (_activeForm != null)
                _activeForm.Close();

            ActivateButton(sender);
            _activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktopPanel.Controls.Add(childForm);
            panelDesktopPanel.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            lblTitle.Text = childForm.Text;
        }

        private void ActivateButton(object sender)
        {
            if (sender != null && _currentButton != (Button)sender)
            {
                DisableButton();

                Color color = SelectThemeColor();
                _currentButton = (Button)sender;
                _currentButton.BackColor = color;
                _currentButton.ForeColor = Color.White;

                panelTitleBar.BackColor = color;
                panelLogo.BackColor = _themeColor.ChangeColorBrightness(color, -0.3);
                _themeColor.PrimaryColor = color;
                _themeColor.SecondaryColor = _themeColor.ChangeColorBrightness(color, -0.3);
                btnCloseChildForm.Visible = true;
            }
        }

        private void ButtonPlayClickHandler(object sender, EventArgs e)
        {
            OpenChildForm(new PlayForm(LoadTheme), sender);
        }

        private void ButtonHistoricClickHandler(object sender, EventArgs e)
        {
            OpenChildForm(new HistoricGamesForm(LoadTheme), sender);
        }

        private void ButtonCloseChildFormClickHandler(object sender, EventArgs e)
        {
            _activeForm?.Close();
            Reset();
        }

        private void Reset()
        {
            DisableButton();

            lblTitle.Text = _mainViewModel.Title; 
            panelTitleBar.BackColor = Color.FromArgb(0, 150, 136);
            panelLogo.BackColor = Color.FromArgb(39, 39, 58);
            _currentButton = null;
            btnCloseChildForm.Visible = false;
        }
    }
}
