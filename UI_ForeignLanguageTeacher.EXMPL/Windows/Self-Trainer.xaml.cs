using System;
using System.Windows;
using System.Windows.Documents;
using UI_ForeignLanguageTeacher.EXMPL.Data;
using UI_ForeignLanguageTeacher.EXMPL.Objects;

namespace UI_ForeignLanguageTeacher.EXMPL.Windows {
    public partial class SelfTrainer {
        public SelfTrainer(User user) {
            InitializeComponent();
            StartText.Content = $"Приветствую, {user.Name}";
            
            foreach (var language in new Connector().GetData(Connector.DataBase.Languages)) 
                Language.Items.Add(language);

            foreach (var theme in new Connector().GetData(Connector.DataBase.Themes))
                Theme.Items.Add(theme);

            Quest = new Quest();
        }
        private Quest Quest { get; set; }
        private void StartQuest(object sender, RoutedEventArgs e)
        {
            try
            {
                StartingGrid.Visibility = Visibility.Hidden;
                TeachingGrid.Visibility = Visibility.Visible;

                Quest = new Connector().GetQuest(Language.Text.Trim('\n'), Theme.Text.Trim('\n'));
                NextQuestion(null, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception}");
            }
        }
        private int _position;
        private void NextQuestion(object sender, RoutedEventArgs e) {
            if (_position == Quest.Questions.Count) return;
            
            Level.Content = $"Вопрос {_position + 1} из {Quest.Questions.Count}";
            
            var text = new TextRange(Answer.Document.ContentStart, Answer.Document.ContentEnd).Text;

            if (text == Quest.Answers[_position]) MessageBox.Show("1");
            Question.Content = Quest.Questions[_position++];
        }
    }
}