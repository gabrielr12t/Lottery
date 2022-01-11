namespace Lottery.Desktop.Forms.Settings
{
    public class ThemeColor
    {
        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }

        public ThemeColor()
        {
            ColorList = new List<string>()
            {
                "#3F51B5",
                "#009688",
                "#FF5722",
                "#B71C46",
                "#7BCFE9",
            };
        }

        public IList<string> ColorList { get; private set; }

        public Color ChangeColorBrightness(Color color, double corretionFactor)
        {
            double red = color.R;
            double green = color.G;
            double blue = color.B;

            if (corretionFactor < 0)
            {
                corretionFactor += 1;
                red *= corretionFactor;
                green *= corretionFactor;
                blue *= corretionFactor;
            }
            else
            {
                red = (255 - red) * corretionFactor + red;
                green = (255 - green) * corretionFactor + green;
                blue = (255 - blue) * corretionFactor + blue;
            }

            return Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);
        }
    }
}

