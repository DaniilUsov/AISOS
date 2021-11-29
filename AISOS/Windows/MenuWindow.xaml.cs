using AISOS.ViewModels;
using System.Windows;

namespace AISOS.Windows
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();

            DataContext = new MenuViewModel(this);
        }
    }
}
