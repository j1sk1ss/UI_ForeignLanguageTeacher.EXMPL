using System.Windows;
using UI_ForeignLanguageTeacher.EXMPL.Data;

namespace UI_ForeignLanguageTeacher.EXMPL.Windows {
    public partial class LanguageAdding {
        public LanguageAdding() {
            InitializeComponent();
        }
        private void AddLanguage(object sender, RoutedEventArgs e) {
            new Connector().InsertToBase(LanguageName.Text ?? "emptyLanguage", Connector.DataBase.Languages);
            Close();
        }
    }
}