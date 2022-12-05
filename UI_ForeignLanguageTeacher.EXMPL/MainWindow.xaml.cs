using System;
using System.Windows;
using UI_ForeignLanguageTeacher.EXMPL.Data;
using UI_ForeignLanguageTeacher.EXMPL.Windows;

namespace UI_ForeignLanguageTeacher.EXMPL {
    public partial class MainWindow
    {
        public MainWindow() {
            InitializeComponent();
        }

        private void UserEnter(object sender, RoutedEventArgs e) {
            try {
                var user = new Connector().Authorize(Password.Text, UserName.Text, Admin.IsChecked!.Value);
                if (user == null) return;
            
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
    }
}