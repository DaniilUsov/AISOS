using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using AISOS.Commands;
using AISOS.Data;
using AISOS.SQL;
using AISOS.Utils;
using AISOS.Windows;

namespace AISOS.ViewModels
{
    internal class AddUserViewModel : BaseViewModel
    {
        private SqlConnection connection;
        private BaseCommand addCommand;
        private BaseCommand updateCommand;
        private BaseCommand cancelCommand;
        private BaseCommand changePasswordCommand;
        private readonly AddUserWindow addUserWindow;

        public User NewUser { get; set; }
        public bool IsReadOnly => !NewUser.IsAdmin;
        public Visibility AddButtonVisibility { get; set; } = Visibility.Visible;

        public BaseCommand AddCommand
        {
            get => addCommand ??
                (addCommand = new BaseCommand(obj =>
                {
                    string commandText = "INSERT INTO Users " +
                                  "(is_admin, login, password, first_name, second_name, third_name, birthday, phone, rank, personal_number, job, workroom) " +
                                  "VALUES " +
                                  "(@isadmin, @login, @password, @firstname, @secondname, @thirdname, @birthday, @phone, @rank, @personalnumber, @job, @workroom)";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@isadmin", NewUser.IsAdmin),
                        new SqlParameter("@login", NewUser.Login),
                        new SqlParameter("@password", NewUser.Password),
                        new SqlParameter("@firstname", NewUser.FirstName),
                        new SqlParameter("@secondname", NewUser.SecondName),
                        new SqlParameter("@thirdname", NewUser.ThirdName),
                        new SqlParameter("@birthday", NewUser.Birthday.ToString("yyyy-MM-dd")),
                        new SqlParameter("@phone", NewUser.Phone),
                        new SqlParameter("@rank", NewUser.Rank),
                        new SqlParameter("@personalnumber", NewUser.PeronalNumber),
                        new SqlParameter("@job", NewUser.Job),
                        new SqlParameter("@workroom", NewUser.Workroom),
                    };
                    
                    SqlCommand command = new SqlCommand(commandText, connection);
                    command.Parameters.AddRange(parameters);

                    try
                    {
                        int result = command.ExecuteNonQuery();
                        Console.WriteLine($"INSERT: {result}");

                        AddButtonVisibility = Visibility.Collapsed;
                    }
                    catch (Exception e)
                    {
                        OkBox.Show("При записи данных возникла ошибка");

                        Console.WriteLine($"ERROR: {e.Message}");
                    }
                },
                (obj) => AddButtonVisibility == Visibility.Visible));
        }

        public BaseCommand UpdateCommand
        {
            get => updateCommand ??
                (updateCommand = new BaseCommand(obj =>
                {
                    string commandText = "UPDATE Users SET " +
                                  "is_admin = @isadmin, " +
                                  "login = @login, " +
                                  "password = @password, " +
                                  "first_name = @firstname, " +
                                  "second_name = @secondname, " +
                                  "third_name = @thirdname, " +
                                  "birthday = @birthday, " +
                                  "phone = @phone, " +
                                  "rank = @rank, " +
                                  "personal_number = @personalnumber, " +
                                  "job = @job, " +
                                  "workroom =  @workroom " +
                                  "WHERE id = @id";
                    string s = NewUser.Birthday.ToString("yyyy-MM-dd");
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@id", NewUser.Id),
                        new SqlParameter("@isadmin", NewUser.IsAdmin),
                        new SqlParameter("@login", NewUser.Login),
                        new SqlParameter("@password", NewUser.Password),
                        new SqlParameter("@firstname", NewUser.FirstName),
                        new SqlParameter("@secondname", NewUser.SecondName),
                        new SqlParameter("@thirdname", NewUser.ThirdName),
                        new SqlParameter("@birthday", s),
                        new SqlParameter("@phone", NewUser.Phone),
                        new SqlParameter("@rank", NewUser.Rank),
                        new SqlParameter("@personalnumber", NewUser.PeronalNumber),
                        new SqlParameter("@job", NewUser.Job),
                        new SqlParameter("@workroom", NewUser.Workroom),
                    };

                    SqlCommand command = new SqlCommand(commandText, connection);
                    command.Parameters.AddRange(parameters);

                    try
                    {
                        int result = command.ExecuteNonQuery();
                        Console.WriteLine($"UPDATE: {result}");
                    }
                    catch (Exception e)
                    {
                        OkBox.Show("При обновлении данных возникла ошибка");

                        Console.WriteLine($"ERROR: {e.Message}");
                    }
                }));
        }

        public BaseCommand ChangePasswordCommand
        {
            get => changePasswordCommand ??
                (changePasswordCommand = new BaseCommand(obj =>
                {
                    PassBox passwordBox = new PassBox();

                    if ((bool)passwordBox.ShowDialog())
                    {
                        NewUser.Password = passwordBox.Password;
                    }
                }));
        }
        
        public BaseCommand CancelCommand
        {
            get => cancelCommand ??
                (cancelCommand = new BaseCommand(obj =>
                {
                    addUserWindow.Close();
                }));
        }

        public AddUserViewModel(AddUserWindow addUserWindow)
        {
            NewUser = new User();

            connection = SqlConnect.Connection;
            this.addUserWindow = addUserWindow;
        }

        public AddUserViewModel(AddUserWindow addUserWindow, User editUser)
        {
            this.addUserWindow = addUserWindow;
            NewUser = editUser;
            AddButtonVisibility = Visibility.Collapsed;

            connection = SqlConnect.Connection;
        }
    }
}
