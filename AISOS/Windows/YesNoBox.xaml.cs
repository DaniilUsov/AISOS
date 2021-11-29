using System.Windows;

namespace AISOS.Windows
{
    public partial class YesNoBox : Window
    {
        public YesNoBox(string text)
        {
            InitializeComponent();

            TextTB.Text = text;
        }

        private void YesBut_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void NoBut_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public static bool Show(string text)
        {
            YesNoBox msgBox = new YesNoBox(text);
            
            return (bool)msgBox.ShowDialog();
        }
    }
}
