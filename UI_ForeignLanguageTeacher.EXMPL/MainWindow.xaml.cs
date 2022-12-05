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
                if (!new Connector().Authorize(Password.Text, UserName.Text, Admin.IsChecked!.Value)) return;
            
                switch (Admin.IsChecked!.Value) {
                    case true:
                        new Editor().Show();
                        break;
                    case false:
                        new Self_Trainer().Show();
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