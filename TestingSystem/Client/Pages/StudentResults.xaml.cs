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
    /// Interaction logic for StudentResults.xaml
    /// </summary>
    public partial class StudentResults : Page
    {
        ClientObject client;
        List<DTOTestSession> testSessions = null;
        List<QueDTOQuestionstion> questions = null;
        List<StudentResultModel> studentResultModels = null;
        public StudentResults(ClientObject _cl)
        {
            InitializeComponent();
            client = _cl;
            studentResultModels = new List<StudentResultModel>();
            testSessions = client.GetTestSessionsForSomeStudent();
            questions = client.GetQuestionsWithoutParams();
            this.AddStudentResultMogels();
        }

        public void AddStudentResultMogels()
        {
            foreach (var testSession in testSessions)
            {
                StudentResultModel studentResultModel = new StudentResultModel();
                studentResultModel.TestName = client.GetTestName(testSession.TestId);
                studentResultModel.StartTime = testSession.StartTime;
                studentResultModel.EndTime = testSession.EndTime;
                List<DTOAnswer> answers = client.GetAnswersForTestSession(testSession.TestSessionId).ToList();
                studentResultModel.QuestionCount = answers.Count;
                answers = answers.Where(x => x.AnswerCorrects == true).ToList();
                studentResultModel.CountCorrectAnswers = answers.Count;
                studentResultModel.CountInCorrectAnswers = studentResultModel.QuestionCount - studentResultModel.CountCorrectAnswers;
                foreach (var item in answers)
                {
                    studentResultModel.CountScores += questions.FirstOrDefault(q => q.Id == item.QuestionId).QuestionCountScores;
                }
                studentResultModels.Add(studentResultModel);
            }

            if (studentResultModels.Count != 0)
                resultsGrid.ItemsSource = studentResultModels;
        }
       
    }
}
