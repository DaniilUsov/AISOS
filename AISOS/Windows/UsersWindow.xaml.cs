using AISOS.ViewModels;
using System.Windows;

namespace AISOS.Windows
{
    public partial class UsersWindow : Window
    {
        private UsersViewModel viewModel;

        public UsersWindow()
        {
            InitializeComponent();

            viewModel = new UsersViewModel(this);
            DataContext = viewModel;
        }
    }
}
