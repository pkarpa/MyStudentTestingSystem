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
    /// Interaction logic for TestSomeGroup.xaml
    /// </summary>
    public partial class TestSomeGroup : Page
    {

        ClientObject client;
        Window mw;
        Frame main = null;
        DTOGroup dtoGroup = null;
        List<DTOTest> tests = null;
        List<DTOTheme> themes = null;
        List<TestViewModel> testsV = null;
        public TestSomeGroup(ClientObject cl, Window _mw, Frame _mn, DTOGroup _group)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            main = _mn;
            dtoGroup = _group;
            themes = client.GetTheme();
            testsV = new List<TestViewModel>();
            TestOfGroupLabel.Content = "Тести доступні групі " + dtoGroup.GroupName + ":";
            SetListOfTest();
        }


        public void SetListOfTest()
        {
            tests = client.GetGroupTests(dtoGroup.TestsId.ToList());
            if (tests.Count != 0)
            {
                foreach (var t in tests)
                {
                    string theme = themes.FirstOrDefault(f => f.Id == t.ThemeId).ThemeName;
                    TestViewModel testV = new TestViewModel() { TestId = t.TestId, TestName = t.TestName, MixQuestionsOrder = t.MixQuestionsOrder, MixAnswersOrder = t.MixAnswersOrder, TestTime = t.TestTime, ThemeId = t.ThemeId, ThemeName = theme };
                    testsV.Add(testV);
                }
                testOfGroupGrid.ItemsSource = testsV;
            }
        }
        private void AddTestForGroupButton_Click(object sender, RoutedEventArgs e)
        {
            main.Content = new AddTestForSomeGroup(client, mw, main, dtoGroup);
        }

        private void DeleteTestFromGroup_Click(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    if (((System.Windows.FrameworkElement)vis).DataContext is TestViewModel)
                    {
                        var testId = ((DTO.TestViewModel)((System.Windows.FrameworkElement)vis).DataContext).TestId;
                        if (testId != -1)
                        {
                            string answer = client.DeleteTestFromGroup(testId, dtoGroup.GroupId);
                            if (answer == "succesfull")
                            {
                                main.Content = new AdminStudentGroups(client, mw, main);
                            }
                        }
                    }
                }
        }
    }
}
