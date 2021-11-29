using AISOS.Utils;
using System.Windows;

namespace AISOS.Windows
{
    public partial class PassBox : Window
    {
        public string Password { get; set; }

        public PassBox()
        {
            InitializeComponent();
        }

        private bool CheckPass()
        {
            string pass1 = FirstPassBox.Password;
            string pass2 = SecondPassBox.Password;

            if (pass1.Length < 8)
            {
                CheckTB.Text = "Пароль должен быть длиной от 8 до 32 символов";
                CheckTB.Visibility = Visibility.Visible;
                return false;
            }

            for (int i = 0; i < pass1.Length; i++)
            {
                char c = pass1[i];
                if (!(('a' <= c && c <= 'z') || ('A' <= c && c <= 'Z') || ('0' <= c && c <= '9')))
                {
                    CheckTB.Text = "Пароль может содержать только латинские символы (a-z A-Z) и цифры (0-9)";
                    CheckTB.Visibility = Visibility.Visible;
                    return false;
                }
            }

            if (pass1 != pass2)
            {
                CheckTB.Text = "Пароли не совпадают";
                CheckTB.Visibility = Visibility.Visible;
                return false;
            }

            CheckTB.Visibility = Visibility.Collapsed;
            return true;
        }

        private void OkBut_Click(object sender, RoutedEventArgs e)
        {
            if (CheckPass())
            {
                Password = PasswordHasher.Hash(FirstPassBox.Password);

                DialogResult = true;
                Close();
            }
        }

        private void CancelBut_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void FirstPassBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPass();
        }

        private void SecondPassBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CheckPass();
        }
    }
}
