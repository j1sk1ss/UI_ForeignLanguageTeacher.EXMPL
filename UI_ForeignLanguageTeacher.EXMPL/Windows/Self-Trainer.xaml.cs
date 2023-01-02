using System;
using System.Windows;
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
        private void StartQuest(object sender, RoutedEventArgs e) {
            try {
                if (Language.Text == "") {
                    MessageBox.Show("Выберите язык!");
                    return;
                }
                if (Theme.Text == "") {
                    MessageBox.Show("Выберите тему!");
                    return;
                }

                Quest = new Connector().GetQuest(Language.Text.Trim('\n'), Theme.Text.Trim('\n'));
                if (Quest.Questions.Count <= 0) {
                    MessageBox.Show("Тема для данного языка не заполнена!");
                    return;
                } 
                
                Question.Content = Quest.Questions[0];            
                Level.Content    = $"Вопрос {1} из {Quest.Questions.Count}";
                
                StartingGrid.Visibility = Visibility.Hidden;
                TeachingGrid.Visibility = Visibility.Visible;                
            }
            catch (Exception exception) {
                MessageBox.Show($"{exception}");
            }
        }
        private int _position;
        private int _mistakes;
        private void NextQuestion(object sender, RoutedEventArgs e) {
            try {
                if (_position <= Quest.Questions.Count - 1) _position++;
                
                if (_position == Quest.Questions.Count) {
                    Result.Content = "Тест пройден!";
                    if (Quest.Answers[_position - 1].ToLower().Contains(Answer.Text.ToLower())) {
                        Result.Content += "\nМолодец!";
                    }
                    else {
                        Result.Content += "\nНе молодец!";
                        _mistakes++;
                    }

                    var end = MessageBox.Show($"Процент правильных ответов: {(_position / (_position - _mistakes)) * 100}%");
                    if (end != MessageBoxResult.OK) return;
                    StartingGrid.Visibility = Visibility.Visible;
                    TeachingGrid.Visibility = Visibility.Hidden;

                    _mistakes = 0;
                    _position = 0;

                    Result.Content = "";
                    Answer.Text    = "";
                    return;
                }
                
                if (Quest.Answers[_position - 1].ToLower().Contains(Answer.Text.ToLower())) {
                    Result.Content = "\nМолодец!";
                }
                else {
                    Result.Content = "\nНе молодец!";
                    _mistakes++;
                }
                Question.Content = Quest.Questions[_position];            
                Level.Content = $"Вопрос {_position + 1} из {Quest.Questions.Count}";
                Answer.Text = "";
            }
            catch (Exception exception) {
                MessageBox.Show($"{exception}");
            }
        }
    }
}