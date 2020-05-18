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
    /// Interaction logic for StudentPassTest.xaml
    /// </summary>
    public partial class StudentPassTest : Page
    {
        ClientObject client;
        DTOTest dtoTest;
        List<QueDTOQuestionstion> questions = null;
        List<DTOQuestionType> questionType = null;
        Window mw;

        public StudentPassTest(ClientObject cl, int testId, Window _mw)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            dtoTest = client.GetTest(testId);
            questions = client.GetQuestions(testId);
            questionType = client.GetQuestionTypes();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mw.Visibility = Visibility.Visible;
        }
    }
}
