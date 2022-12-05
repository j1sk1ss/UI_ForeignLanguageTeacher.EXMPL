using System.Windows;
using UI_ForeignLanguageTeacher.EXMPL.Data;
using UI_ForeignLanguageTeacher.EXMPL.Objects;

namespace UI_ForeignLanguageTeacher.EXMPL.Windows
{
    public partial class Editor
    {
        public Editor(User user) {
            InitializeComponent();
            UsersTopText.Content = new Connector().GetUsersNames();

            foreach (var language in new Connector().GetData(Connector.DataBase.Languages)) 
                ChoseLanguage.Items.Add(language);

            foreach (var themes in new Connector().GetData(Connector.DataBase.Themes))
                Themes.Content += $"{themes}\n";
        }

        private void AddLanguage(object sender, RoutedEventArgs e) => new LanguageAdding().Show();

        private void ThemeEditor(object sender, RoutedEventArgs e) => new WordsAdding(ChoseLanguage.Text).Show();
    }
}