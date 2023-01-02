using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using Npgsql;
using UI_ForeignLanguageTeacher.EXMPL.Objects;

namespace UI_ForeignLanguageTeacher.EXMPL.Data {
    public class Connector {
        private NpgsqlConnection NpgsqlConnection { get; set; }
        
        private const string Server   = "localhost";
        private const string Port     = "5432";
        private const string Database = "SeltTeachingApp";
        private const string UserId   = "postgres";
        private const string Password = "1234";
        
        private const string UsersDataBase     = "users";
        private const string WordsDataBase     = "words";
        private const string ThemesDataBase    = "themes";
        private const string LanguagesDataBase = "languages";

        public void AddUser(string name, string password, bool isAdmin) {
            try {
                if (NpgsqlConnection == null) Connect();
                
                var sqlSetCommand = $"INSERT INTO {UsersDataBase} values ('{name}', '{password}', '{isAdmin.ToString().ToLower()}')";
                SendCommand(sqlSetCommand);
                
                Disconnect();
            }
            catch (Exception e) {
                MessageBox.Show($"{e}");
            }
        }
        
        public void AddWords(string themeName, string languageName, string quest) {
            try {
                if (NpgsqlConnection == null) Connect();
                
                var lines = quest.Split("\n", StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < lines.Length; i += 2) {
                    var sqlSetCommand = $"INSERT INTO {WordsDataBase} values ('{lines[i]}', '{lines[i + 1]}', '{languageName.Trim('\n')}', " +
                                        $"'{themeName.Trim('\n')}')";
                    
                    SendCommand(sqlSetCommand);
                }
                Disconnect();
            }
            catch (Exception e) {
                MessageBox.Show($"{e}");
            }
        }
        
        public void InsertToBase(string value, DataBase dataBase) {
            if (NpgsqlConnection == null) Connect();

            var name = dataBase switch { 
                DataBase.Languages => LanguagesDataBase, 
                DataBase.Themes    => ThemesDataBase
            };
        
            var sqlSetCommand = $"INSERT INTO {name} values ('{value}')";
        
            SendCommand(sqlSetCommand);
            Disconnect();
        }
        
        public Quest GetQuest(string languageName, string themeName) {
            if (NpgsqlConnection == null) Connect();

            var sqlGetCommand = $"SELECT * FROM {WordsDataBase} WHERE language_name = '{languageName}' AND " +
                                $"theme_name = '{themeName}'";

            var sqlData = GetRows(sqlGetCommand);
            var getData = new Quest();

            for (var i = 0; i < sqlData.Count; i++) {
                getData.Questions.Add($"{sqlData[i]["word"]}");
                getData.Answers.Add($"{sqlData[i]["translation"]}");
            }
            
            return getData;
        }
        
        public List<string> GetData(DataBase dataBase) {
            if (NpgsqlConnection == null) Connect();
            
            var data = dataBase switch { 
                DataBase.Languages => LanguagesDataBase, 
                DataBase.Themes    => ThemesDataBase
            };
            
            var sqlGetCommand = $"SELECT * FROM {data}";

            var sqlData = GetRows(sqlGetCommand);
            var getData = new List<string>();

            var name = dataBase switch { 
                DataBase.Languages => "language_name", 
                DataBase.Themes    => "theme_name" 
            };
            
            for (var i = 0; i < sqlData.Count; i++) getData.Add($"{sqlData[i][name]}");
            
            return getData;
        }
        public string GetUsersNames() {
            if (NpgsqlConnection == null) Connect();
            
            var sqlGetCommand = $"SELECT * FROM {UsersDataBase}";

            var sqlData = GetRows(sqlGetCommand);
            var usersNames = "";

            for (var i = 0; i < sqlData.Count; i++) usersNames += $"{sqlData[i]["user_name"]}\n";
            
            return usersNames;
        }
        
        public User Authorize(string password, string name, bool isAdmin) {
            if (NpgsqlConnection == null) Connect();

            var sqlAuthorizeCommand = $"SELECT CASE WHEN EXISTS (SELECT * FROM {UsersDataBase} " +
                                      $"WHERE user_password = '{password}' AND user_name = '{name}' " +
                                      $"AND is_admin = '{isAdmin.ToString()}') THEN 1 ELSE 0 END";

            var sqlData = GetRows(sqlAuthorizeCommand);

            return int.Parse(sqlData[0]["case"].ToString()!) == 1 ? new User{Name = name} : null;
        }

        private DataRowCollection GetRows(string sqlCommand) {
            var command         = new NpgsqlCommand(sqlCommand);
            command.Connection  = NpgsqlConnection;
            command.CommandType = CommandType.Text;
                
            var dataAdapter = new NpgsqlDataAdapter(command);
            var dataSet     = new DataSet();
            dataAdapter.Fill(dataSet);

            command.ExecuteNonQuery();
            command.Dispose();
                
            Disconnect();
            
            const int usersTable = 0;
                
            return dataSet.Tables[usersTable].Rows;
        }
        
        private void SendCommand(string sqlCommand) {
            var command         = new NpgsqlCommand(sqlCommand);
            command.Connection  = NpgsqlConnection;
            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
            command.Dispose();
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

        public enum DataBase {
            Languages,
            Themes,
            Users,
            Words
        }
    }
}