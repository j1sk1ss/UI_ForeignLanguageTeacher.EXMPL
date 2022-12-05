using System;
using System.Data;
using System.Windows;
using Npgsql;

namespace UI_ForeignLanguageTeacher.EXMPL.Data
{
    public class Connector
    {
        private NpgsqlConnection NpgsqlConnection { get; set; }
        
        private const string Server = "localhost";
        private const string Port = "5432";
        private const string Database = "SeltTeachingApp";
        private const string UserId = "postgres";
        private const string Password = "1234";
        
        private const string UsersDataBase = "users";
        private const string WordsDataBase = "words";
        
        public bool Authorize(string password, string name, bool isAdmin) {
            if (NpgsqlConnection == null) Connect();

            var sqlAuthorizeCommand = $"SELECT CASE WHEN EXISTS ( SELECT* FROM {UsersDataBase} " +
                                      $"WHERE user_password = '{password}' AND user_name = '{name}' " +
                                      $"AND is_admin = '{isAdmin.ToString()}') THEN 1 ELSE 0 END";
            var command         = new NpgsqlCommand(sqlAuthorizeCommand);
            command.Connection  = NpgsqlConnection;
            command.CommandType = CommandType.Text;
                
            var dataAdapter = new NpgsqlDataAdapter(command);
            var dataSet     = new DataSet();
            dataAdapter.Fill(dataSet);

            command.ExecuteNonQuery();
            command.Dispose();
                
            Disconnect();
            
            const int usersTable = 0;
                
            var sqlData = dataSet.Tables[usersTable].Rows;
            
            return int.Parse(sqlData[0]["case"].ToString()!) == 1;
        }
        
        private void Connect() {
            try {
                var connectionCommand = 
                    $"Server={Server};Port={Port};Database={Database};User Id={UserId};Password={Password}";
                
                NpgsqlConnection = new NpgsqlConnection(connectionCommand);
                NpgsqlConnection.Open();
            }
            catch (Exception e) {
                MessageBox.Show($"{e}");
            }
        }
        
        private void Disconnect() {
            try {
                NpgsqlConnection.Close();
            }
            catch (Exception e) {
                MessageBox.Show($"{e}");
            }
        }
    }
}