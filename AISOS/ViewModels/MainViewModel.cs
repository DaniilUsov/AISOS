using AISOS.Commands;
using AISOS.Data;
using AISOS.Properties;
using AISOS.SQL;
using AISOS.Utils;
using AISOS.Windows;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace AISOS.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private SqlConnection connection;
        private BaseCommand logonCommand;
        private BaseCommand loadCommand;
        private readonly MainWindow mainWindow;

        public string Login { get; set; } = "";
        public BaseCommand LogonCommand
        {
            get => logonCommand ??
                (logonCommand = new BaseCommand(obj =>
                {
                    string password = ((PasswordBox)obj).Password;
                    string commandText = $"SELECT * FROM Users WHERE login = '{Login}' AND password = '{PasswordHasher.Hash(password)}'";

                    SqlCommand command = new SqlCommand(commandText, connection);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            User user = new User(reader);
                            Constants.CurrenUser = user;
                            reader.Close();
                            OkBox.Show($"Успешный вход: {user.SecondName} {user.FirstName} {user.ThirdName}");

                            MenuWindow menuWindow = new MenuWindow();
                            menuWindow.Show();
                            mainWindow.Close();
                        }
                        else OkBox.Show("Неудачный вход, проверьте логин и пароль");
                    }
                },
                (obj) => Login.Length > 0));
        }
        public BaseCommand LoadCommand
        {
            get => loadCommand ??
                (loadCommand = new BaseCommand(obj =>
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Title = "Откройте базу данных";
                    openFile.Filter = "База SQL Server (*.mdf) |*.mdf";
                    if (openFile.ShowDialog() == DialogResult.OK)
                    {
                        string path = openFile.FileName;
                        Settings.Default["DBpath"] = path;
                        Settings.Default.Save();

                        OkBox.Show("Загружена база: " + path);
                    }
                }));
        }

        public MainViewModel(MainWindow mainWindow)
        {
            connection = SqlConnect.Connection;
            string commandText = $"SELECT * FROM Users WHERE is_admin = 1";

            SqlCommand command = new SqlCommand(commandText, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();

                OkBox.Show("Требуется регистрация администратора");

                UsersWindow usersWindow = new UsersWindow();
                usersWindow.Show();
                System.Windows.Application.Current.MainWindow.Close();
            }

            this.mainWindow = mainWindow;
        }
    }
}
