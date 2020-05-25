using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for AddTestForSomeGroup.xaml
    /// </summary>
    public partial class AddTestForSomeGroup : Page
    {
        ClientObject client;
        Window mw;
        Frame main = null;
        DTOGroup dtoGroup = null;
        List<DTOSubject> subjects = null;
        List<DTOTheme> themes = null;
        List<DTOTest> tests = null;
        List<TestViewModel> testsV;

        public AddTestForSomeGroup(ClientObject cl, Window _mw, Frame _mn, DTOGroup _group)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            main = _mn;
            dtoGroup = _group;
            testsV = new List<TestViewModel>();
            AddItemsToSubjectComboBox();
        }

        public void AddItemsToSubjectComboBox()
        {
            subjects = client.GetSubjectsForAdmin();
            SubjectComboBox.Items.Clear();
            if (subjects.Count > 0)
            {
                foreach (DTOSubject item in subjects)
                {
                    ComboBoxItem gnItem = new ComboBoxItem() { Content = item.SubjectName };
                    SubjectComboBox.Items.Add(gnItem);
                }
            }
            else
            {
                ComboBoxItem gnItem = new ComboBoxItem() { Content = "No Subjects" };
                SubjectComboBox.Items.Add(gnItem);
            }
        }
        private void SubjectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var subjectComboBox = sender as ComboBox;
            var selectSubject = subjectComboBox.SelectedIndex;
            int subjectId = subjects[selectSubject].SubjectId;
            this.AddItemsToThemeComboBox(subjectId);
        }

        public void AddItemsToThemeComboBox(int subjectId)
        {
            themes = client.GetThemesForSubject(subjectId);
            ThemeComboBox.Items.Clear();
            if (themes.Count > 0)
            {
                foreach (DTOTheme item in themes)
                {
                    ComboBoxItem gnItem = new ComboBoxItem() { Content = item.ThemeName };
                    ThemeComboBox.Items.Add(gnItem);
                }
            }
            else
            {
                ComboBoxItem gnItem = new ComboBoxItem() { Content = "No Theme" };
                ThemeComboBox.Items.Add(gnItem);
            }
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var themeComboBox = sender as ComboBox;
            var selectTheme = themeComboBox.SelectedIndex;
            int themeId = themes[selectTheme].Id;
            this.SetListOfAvaibleTests(themeId);
        }

        public void SetListOfAvaibleTests(int themeId)
        {
            List<DTOTest> avaibleTests = client.GetListAvaibleTests(dtoGroup.GroupId, themeId);
            if (avaibleTests.Count != 0)
            {
                foreach (var t in avaibleTests)
                {
                    string theme = themes.FirstOrDefault(f => f.Id == t.ThemeId).ThemeName;
                    TestViewModel testV = new TestViewModel() { TestId = t.TestId, TestName = t.TestName, MixQuestionsOrder = t.MixQuestionsOrder, MixAnswersOrder = t.MixAnswersOrder, TestTime = t.TestTime, ThemeId = t.ThemeId, ThemeName = theme };
                    testsV.Add(testV);
                }
                avaibleTestsGrid.ItemsSource = testsV;
            }
        }

        private void AddTest_Click(object sender, RoutedEventArgs e)
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
                            string answer = client.AddTestToGroup(testId, dtoGroup.GroupId);
                            if (answer == "succesfull")
                            {
                                main.Content = new AdminStudentGroups(client, mw, main); ;
                            }
                        }
                    }
                }
        }
    }
}
