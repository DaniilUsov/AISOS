using AISOS.Commands;
using AISOS.SQL;
using AISOS.Windows;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AISOS.ViewModels
{
    class DutyViewModel : BaseViewModel
    {
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private BaseCommand exitCommand;
        private BaseCommand createCommand;
        private BaseCommand loadCommand;
        private BaseCommand updateCommand;
        private readonly DutyWindow dutyWindow;
        private int selectedMonth = DateTime.Now.Month;
        private string month;
        private DataTable table;
        private Visibility vis29 = Visibility.Visible, vis30 = Visibility.Visible, vis31 = Visibility.Visible;

        public DataTable Table
        {
            get => table; set
            {
                table = value;
                OnPropertyChanged("Table");
            }
        }

        public string Month
        {
            get => month; set
            {
                month = value;
                OnPropertyChanged("Month");
            }
        }
        public string[] Months { get; set; } = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        public int SelectedMonth
        {
            get => selectedMonth;
            set
            {
                selectedMonth = value;
                OnPropertyChanged("SelectedMonth");
            }
        }
        public int RealMonth => SelectedMonth + 1;
        public Visibility Vis29
        {
            get => vis29; set
            {
                vis29 = value;
                OnPropertyChanged("Vis29");
            }
        }
        public Visibility Vis30
        {
            get => vis30; set
            {
                vis30 = value;
                OnPropertyChanged("Vis30");
            }
        }
        public Visibility Vis31
        {
            get => vis31; set
            {
                vis31 = value;
                OnPropertyChanged("Vis31");
            }
        }
        public BaseCommand ExitCommand
        {
            get => exitCommand ??
                (exitCommand = new BaseCommand(obj =>
                {
                    MenuWindow menuWindow = new MenuWindow();
                    menuWindow.Show();
                    dutyWindow.Close();
                }));
        }
        public BaseCommand CreateCommand
        {
            get => createCommand ??
                (createCommand = new BaseCommand(obj =>
                {
                    GenerateTable();
                }));
        }
        public BaseCommand LoadCommand
        {
            get => loadCommand ??
                (loadCommand = new BaseCommand(obj =>
                {
                    LoadDataBase($"Duty_{RealMonth}");
                }));
        }

        public BaseCommand UpdateCommand
        {
            get => updateCommand ??
                (updateCommand = new BaseCommand(obj =>
                {
                    adapter.Update(Table);
                }));
        }

        public DutyViewModel(DutyWindow dutyWindow)
        {
            this.dutyWindow = dutyWindow;

            connection = SqlConnect.Connection;

            Update();
        }

        private void GenerateTable()
        {
            Update();

            string sql = "SELECT id, first_name, second_name, third_name FROM Users";

            SqlCommand command = new SqlCommand(sql, connection);

            Table = new DataTable("Duty");
            Table.Columns.Add(new DataColumn("id"));
            Table.Columns.Add(new DataColumn("name"));
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, RealMonth);
            for (int i = 1; i < daysInMonth + 1; i++)
            {
                DataColumn dataColumn = new DataColumn("n" + i.ToString());
                Table.Columns.Add(dataColumn);
            }

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = Table.NewRow();
                    row.ItemArray = new object[] { reader["id"], reader["second_name"].ToString().Trim() + " " + reader["first_name"].ToString()[0] + ". " + reader["third_name"].ToString()[0] + "." };
                    Table.Rows.Add(row);
                }
            }

            sql = $"IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='Duty_{RealMonth}')) DROP TABLE Duty_{RealMonth}";
            command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();

            sql = $"IF (NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='Duty_{RealMonth}')) BEGIN ";

            sql += $"CREATE TABLE Duty_{RealMonth} (" +
                "id int NOT NULL, " +
                "name nchar(40) NOT NULL, ";
            for (int i = 1; i < daysInMonth + 1; i++)
            {
                sql += $"[n{i}] nchar(64) NULL" + (i == daysInMonth + 1 ? "" : ", ");
            }
            sql += ") ";
            sql += "END";

            command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();

            foreach (DataRow row in Table.Rows)
            {
                sql = $"INSERT INTO Duty_{RealMonth} (id, name) VALUES (@id, @name)";
                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                    new SqlParameter("@id", row["id"]),
                    new SqlParameter("@name", row["name"])
                };
                command = new SqlCommand(sql, connection);
                command.Parameters.AddRange(sqlParameters);
                command.ExecuteNonQuery();
            }

            LoadDataBase($"Duty_{RealMonth}");
        }

        private void LoadDataBase(string dataBaseName)
        {
            string sql = $"IF (EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='{dataBaseName}')) SELECT * FROM {dataBaseName}";

            adapter = new SqlDataAdapter(sql, connection);

            DataSet dataset = new DataSet();
            adapter.Fill(dataset);
            if (dataset.Tables.Count > 0)
            {
                Table = dataset.Tables["Table"];

                string commandText = $"UPDATE {dataBaseName} SET ";

                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.Add("@id", SqlDbType.Int, 8, "id");
                for (int i = 2; i < Table.Columns.Count; i++)
                {
                    DataColumn column = Table.Columns[i];
                    commandText += "[" + column.ColumnName + "] = @" + column.ColumnName + (i == Table.Columns.Count - 1 ? " " : ", ");
                    command.Parameters.Add("@" + column.ColumnName, SqlDbType.NChar, 10, column.ColumnName);
                }
                commandText += "WHERE id = @id";
                command.CommandText = commandText;
                adapter.UpdateCommand = command;

                Update();

            }
            else OkBox.Show("Нет созданной базы");
        }

        private void Update()
        {
            Month = $"График нарядов на {Months[selectedMonth]}";

            CorrectCalendar();
        }

        private void CorrectCalendar()
        {
            int daysNum = DateTime.DaysInMonth(DateTime.Now.Year, RealMonth);

            switch (daysNum)
            {
                case 30:
                    Vis31 = Visibility.Collapsed;
                    Vis29 = Vis30 = Visibility.Visible;
                    break;
                case 29:
                    Vis31 = Vis30 = Visibility.Collapsed;
                    Vis29 = Visibility.Visible;
                    break;
                case 28:
                    Vis29 = Vis30 = Vis31 = Visibility.Collapsed;
                    break;
                default:
                    Vis29 = Vis30 = Vis31 = Visibility.Visible;
                    break;
            }
        }
    }
}
