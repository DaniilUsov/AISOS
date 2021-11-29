using AISOS.Utils;
using AISOS.ViewModels;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace AISOS.Data
{
    public class User : BaseViewModel
    {
        private int id = 1;
        private bool isAdmin = false;
        private string login = "";
        private string password = "";
        private string firstName = "";
        private string secondName = "";
        private string thirdName = "";
        private DateTime birthday = DateTime.Parse("01.01.2000");
        private long phone = 89000000000;
        private string rank = "";
        private string peronalNumber = "";
        private string job = "";
        private string workroom = "";

        public int Id
        {
            get => id; set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public bool IsAdmin
        {
            get => isAdmin; set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }
        public string Login
        {
            get => login; set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }
        public string Password
        {
            get => password; set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public string FirstName
        {
            get => firstName; set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string SecondName
        {
            get => secondName; set
            {
                secondName = value;
                OnPropertyChanged("SecondName");
            }
        }
        public string ThirdName
        {
            get => thirdName; set
            {
                thirdName = value;
                OnPropertyChanged("ThirdName");
            }
        }
        public DateTime Birthday
        {
            get => birthday; set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }
        public long Phone
        {
            get => phone; set
            {
                phone = value;
                OnPropertyChanged("Phone");
            }
        }
        public string Rank
        {
            get => rank; set
            {
                rank = value;
                OnPropertyChanged("Rank");
            }
        }
        public string PeronalNumber
        {
            get => peronalNumber; set
            {
                peronalNumber = value;
                OnPropertyChanged("PeronalNumber");
            }
        }
        public string Job
        {
            get => job; set
            {
                job = value;
                OnPropertyChanged("Job");
            }
        }
        public string Workroom
        {
            get => workroom; set
            {
                workroom = value;
                OnPropertyChanged("Workroom");
            }
        }

        public User()
        {

        }

        public User(SqlDataReader reader)
        {
            Id = Convert.ToInt32(reader["id"]);
            IsAdmin = Convert.ToBoolean(reader["is_admin"]);
            Login = Convert.ToString(reader["login"]).TrimEnd(' ');
            Password = Convert.ToString(reader["password"]).TrimEnd(' ');
            FirstName = Convert.ToString(reader["first_name"]).TrimEnd(' ');
            SecondName = Convert.ToString(reader["second_name"]).TrimEnd(' ');
            ThirdName = Convert.ToString(reader["third_name"]).TrimEnd(' ');
            Birthday = DateTime.Parse(Convert.ToString(reader["birthday"]), Constants.DATE_FORMAT);
            Phone = Convert.ToInt64(reader["phone"]);
            Rank = Convert.ToString(reader["rank"]).TrimEnd(' ');
            PeronalNumber = Convert.ToString(reader["personal_number"]).TrimEnd(' ');
            Job = Convert.ToString(reader["job"]).TrimEnd(' ');
            Workroom = Convert.ToString(reader["workroom"]).TrimEnd(' ');
        }
    }
}
