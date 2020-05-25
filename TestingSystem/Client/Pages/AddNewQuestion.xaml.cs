using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DTO;

namespace Client.Pages
{
    /// <summary>
    /// Interaction logic for AddNewQuestion.xaml
    /// </summary>
    public partial class AddNewQuestion : Page
    {
        ClientObject client;
        Frame main = null;
        Window mw = null;
        int testId = -1;
        int questionTypeId = -1;
        List<DTOSubject> subjects = null;
        List<DTOQuestionType> types = null;
        
        public AddNewQuestion(ClientObject _cl, Window _mw, Frame _mn, int _testId)
        {
            InitializeComponent();
            client = _cl;
            main = _mn;
            mw = _mw;
            testId = _testId;
            AddItemsToQuestionTypesComboBox();
        }

        public void AddItemsToQuestionTypesComboBox()
        {
            types = client.GetQuestionTypes();
            QuestionTypeComboBox.Items.Clear();
            if (types.Count > 0)
            {
                foreach (DTOQuestionType item in types)
                {
                    ComboBoxItem gnItem = new ComboBoxItem() { Content = item.QuestionTypeName };
                    QuestionTypeComboBox.Items.Add(gnItem);
                }
            }
            else
            {
                ComboBoxItem gnItem = new ComboBoxItem() { Content = "No types" };
                QuestionTypeComboBox.Items.Add(gnItem);
            }
        }

        private void QuestionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var QuestionComboBox = sender as ComboBox;
            var selectQuestionType = QuestionComboBox.SelectedIndex;
            questionTypeId = types[selectQuestionType].Id;  
        }

        private void CreateTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (questionTypeId != -1)
            {
                if (QuestionText.Text != "" && AnswerOptions.Text != "" && CorrectAnswer.Text != "" && QuestionCountScores.Text != "")
                {
                    double num2;
                    bool isDoubleNum = double.TryParse(QuestionCountScores.Text, out num2);
                    if (isDoubleNum == true && double.Parse(QuestionCountScores.Text)>0)
                    {
                        QueDTOQuestionstion dTOQuestionstion = new QueDTOQuestionstion() { QuestionText = QuestionText.Text, AnswerOption = AnswerOptions.Text, QuestionAnswer = CorrectAnswer.Text, QuestionCountScores = double.Parse(QuestionCountScores.Text), QuestionTypeId = questionTypeId };
                        int newId = client.SaveQuestion(dTOQuestionstion);
                        string answer = client.AddQuestionIdInTest(testId, newId);
                        if (answer == "succesfull")
                            main.Content = new AdminTestQuestions(client, mw, main, testId);
                        else
                            MessageBox.Show("Some is bad!!!");
                    }
                    else
                    {
                        MessageBox.Show("CountScores must be a numbe!!!");
                    }
                }
                else
                {
                    MessageBox.Show("Please fill in all fields!!!");
                }
            }
            else
            {
                MessageBox.Show("Please chose question type");
            }
        }
    }
}
