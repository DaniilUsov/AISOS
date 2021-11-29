using AISOS.Data;
using AISOS.ViewModels;
using System.Windows;

namespace AISOS.Windows
{
    public partial class AddUserWindow : Window
    {
        private AddUserViewModel viewModel;

        public AddUserWindow()
        {
            InitializeComponent();

            viewModel = new AddUserViewModel(this);
            DataContext = viewModel;
        }

        public AddUserWindow(User editUser)
        {
            InitializeComponent();

            viewModel = new AddUserViewModel(this, editUser);
            DataContext = viewModel;
        }
    }
}
