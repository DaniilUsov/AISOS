using AISOS.Commands;
using AISOS.SQL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AISOS.Data;
using AISOS.Windows;
using System.Windows;

namespace AISOS.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        private SqlConnection connection;
        private BaseCommand addCommand;
        private BaseCommand updateCommand;
        private BaseCommand deleteCommand;
        private BaseCommand editCommand;
        private BaseCommand exitCommand;
        private readonly UsersWindow usersWindow;

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public User SelectedUser { get; set; }

        public BaseCommand AddCommand
        {
            get => addCommand ??
                (addCommand = new BaseCommand(obj =>
                {
                    AddUserWindow addUserWindow = new AddUserWindow();
                    
                    addUserWindow.ShowDialog();

                    UpdateView();
                }));
        }
        public BaseCommand EditCommand
        {
            get => editCommand ??
                (editCommand = new BaseCommand(obj =>
                {
                    if (SelectedUser != null)
                    {
                        AddUserWindow editUserWindow = new AddUserWindow(SelectedUser);

                        editUserWindow.ShowDialog();

                        UpdateView();
                    }
                },
                (obj) => SelectedUser != null));
        }
        public BaseCommand UpdateCommand
        {
            get => updateCommand ??
                (updateCommand = new BaseCommand(obj =>
                {
                    UpdateView();
                }));
        }
        public BaseCommand DeleteCommand
        {
            get => deleteCommand ??
                (deleteCommand = new BaseCommand(obj =>
                {
                    string msg = $"Вы уверены что хотите удалить сотрудника {SelectedUser.SecondName} {SelectedUser.FirstName} {SelectedUser.ThirdName}?";

                    if (YesNoBox.Show(msg))
                    {
                        string text = $"DELETE FROM Users WHERE id = {SelectedUser.Id}";
                        SqlCommand command = new SqlCommand(text, connection);

                        try
                        {
                            int result = command.ExecuteNonQuery();
                            Console.WriteLine($"DELETED: {result}");

                            UpdateView();
                        }
                        catch (Exception e)
                        {
                            OkBox.Show("При удалении данных возникла ошибка");

                            Console.WriteLine($"ERROR: {e.Message}");
                        }
                    }
                },
                (obj) => SelectedUser != null));
        }
        public BaseCommand ExitCommand
        {
            get => exitCommand ??
                (exitCommand = new BaseCommand(obj =>
                {
                    MenuWindow menuWindow = new MenuWindow();
                    menuWindow.Show();
                    usersWindow.Close();
                }));
        }

        public UsersViewModel(UsersWindow usersWindow)
        {
            connection = SqlConnect.Connection;
            string sql = "IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='Users')) CREATE TABLE [dbo].[Users] (" +
    "[id]              INT        IDENTITY (1, 1) NOT NULL, " +
    "[is_admin]        BIT        NOT NULL, " +
    "[login]           CHAR (32)  NOT NULL, " +
    "[password]        CHAR (64)  NOT NULL, " +
    "[first_name]      NCHAR (32) NOT NULL, " +
    "[second_name]     NCHAR (32) NOT NULL, " +
    "[third_name]      NCHAR (32) NULL, " +
    "[birthday]        DATE       NULL, " +
    "[phone]           BIGINT     NULL, " +
    "[rank]            NCHAR (32) NULL, " +
    "[personal_number] NCHAR (32) NULL, " +
    "[job]             NCHAR (64) NULL, " +
    "[workroom]        NCHAR (32) NULL, " +
    "PRIMARY KEY CLUSTERED ([id] ASC) ); ";
            SqlCommand command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
            UpdateView();
            this.usersWindow = usersWindow;
        }

        internal void UpdateView()
        {
            string text = "SELECT id, is_admin, login, password, first_name, second_name, third_name, birthday, phone, rank, personal_number, job, workroom FROM Users";
            SqlCommand command = new SqlCommand(text, connection);
            
            Users.Clear();
            try
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new User
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            IsAdmin = Convert.ToBoolean(reader["is_admin"]),
                            Login = Convert.ToString(reader["login"]).Trim(),
                            Password = Convert.ToString(reader["password"]).Trim(),
                            FirstName = Convert.ToString(reader["first_name"]).Trim(),
                            SecondName = Convert.ToString(reader["second_name"]).Trim(),
                            ThirdName = Convert.ToString(reader["third_name"]).Trim(),
                            Birthday = DateTime.Parse(Convert.ToString(reader["birthday"]), Constants.DATE_FORMAT),
                            Phone = Convert.ToInt64(reader["phone"]),
                            Rank = Convert.ToString(reader["rank"]).Trim(),
                            PeronalNumber = Convert.ToString(reader["personal_number"]).Trim(),
                            Job = Convert.ToString(reader["job"]).Trim(),
                            Workroom = Convert.ToString(reader["workroom"]).Trim(),
                        };
                        Users.Add(user);
                    }
                }
                
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("При чтении данных возникла ошибка");

                Console.WriteLine($"ERROR: {e.Message}");
            }
        }
    }
}
