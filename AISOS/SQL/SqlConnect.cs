using AISOS.Properties;
using AISOS.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AISOS.SQL
{
    public static class SqlConnect
    {
        private static SqlConnection connection;

        public static SqlConnection Connection
        {
            get
            {
                string path = Settings.Default.DBpath;
                if (path.Length == 0 || !File.Exists(path))
                {
                    OpenFileDialog openFile = new OpenFileDialog();
                    openFile.Title = "Откройте базу данных";
                    openFile.Filter = "База SQL Server (*.mdf) |*.mdf";
                    if (openFile.ShowDialog() == DialogResult.OK)
                    {
                        path = openFile.FileName;
                        Settings.Default["DBpath"] = path;
                        Settings.Default.Save();

                        OkBox.Show("Загружена база: " + path);
                    }
                }
                if (connection == null)
                    connection = new SqlConnection($@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={path};Integrated Security=True");
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                return connection;
            }
        }
    }
}
