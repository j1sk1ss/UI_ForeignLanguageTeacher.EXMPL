using System.Windows;
using System.Windows.Documents;
using UI_ForeignLanguageTeacher.EXMPL.Data;

namespace UI_ForeignLanguageTeacher.EXMPL.Windows {
    public partial class WordsAdding {
        public WordsAdding(string languageName) {
            InitializeComponent();
            Language = languageName;
            foreach (var theme in new Connector().GetData(Connector.DataBase.Themes)) Themes.Items.Add(theme);
        }
        private new string Language { get; set; }
        private void AddTheme(object sender, RoutedEventArgs e) => new ThemeAdding().Show();

        private void SaveWords(object sender, RoutedEventArgs e) {
            var text = new TextRange(Words.Document.ContentStart, Words.Document.ContentEnd).Text;
            
            new Connector().AddWords(Themes.Text, Language, text);
            
            Close();
        }
    }
}