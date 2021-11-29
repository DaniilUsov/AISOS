using AISOS.ViewModels;
using System.Data;
using System.Windows;
using System.Windows.Forms;

namespace AISOS.Windows
{
    public partial class DutyWindow : Window
    {
        public DutyWindow()
        {
            InitializeComponent();

            DataContext = new DutyViewModel(this);
        }
    }
}
