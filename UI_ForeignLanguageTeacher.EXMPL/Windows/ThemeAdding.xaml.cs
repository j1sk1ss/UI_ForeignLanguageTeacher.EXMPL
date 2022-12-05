using System.Windows;
using UI_ForeignLanguageTeacher.EXMPL.Data;

namespace UI_ForeignLanguageTeacher.EXMPL.Windows {
    public partial class ThemeAdding {
        public ThemeAdding() {
            InitializeComponent();
        }
        private void AddTheme(object sender, RoutedEventArgs e) {
            new Connector().InsertToBase(LanguageName.Text ?? "emptyTheme", Connector.DataBase.Themes);
            Close();
        }
    }
}