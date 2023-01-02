using System;
using System.Windows;
using UI_ForeignLanguageTeacher.EXMPL.Data;
using UI_ForeignLanguageTeacher.EXMPL.Windows;

namespace UI_ForeignLanguageTeacher.EXMPL {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
        }

        private void UserEnter(object sender, RoutedEventArgs e) {
            try {
                if (UserName.Text == "") {
                    MessageBox.Show("Введите имя пользователя!");
                    return;
                }
                if (Password.Text == "") {
                    MessageBox.Show("Введите пароль!");
                    return;
                }
                
                var user = new Connector().Authorize(Password.Text, UserName.Text, Admin.IsChecked!.Value);
                if (user == null) {
                    MessageBox.Show("Введённые данные не верны!");
                    return;
                }
            
                switch (Admin.IsChecked!.Value) {
                    case true:
                        new Editor(user).Show();
                        break;
                    case false:
                        new SelfTrainer(user).Show();
                        break;
                }
                Close();
            }
            catch (Exception exception) {
                MessageBox.Show(exception.Message);
            }
        }

        private void Registration(object sender, RoutedEventArgs e) {
            if (UserName.Text == "") {
                MessageBox.Show("Введите имя пользователя!");
                return;
            }
            if (Password.Text == "") {
                MessageBox.Show("Введите пароль!");
                return;
            }
            
            new Connector().AddUser(UserName.Text, Password.Text, false);
            MessageBox.Show("Пользователь зарегестрирован без прав администратора!");
            UserEnter(null, null);
        }
    }
}