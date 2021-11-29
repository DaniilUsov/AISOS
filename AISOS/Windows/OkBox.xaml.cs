using System.Windows;

namespace AISOS.Windows
{
    public partial class OkBox : Window
    {
        public OkBox(string text)
        {
            InitializeComponent();

            TextTB.Text = text;
        }

        private void OkBut_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        public static bool Show(string text)
        {
            OkBox okBox = new OkBox(text);
            return (bool)okBox.ShowDialog();
        }
    }
}
