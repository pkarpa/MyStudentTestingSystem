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
    /// Interaction logic for AdminTestSessions.xaml
    /// </summary>
    public partial class AdminTestSessions : Page
    {
        ClientObject client;
        List<DTOSubject> subjects = null;
        List<DTOTheme> themes = null;
        List<DTOTest> tests = null;
        List<DTOTestSession> testSessions = null;

        public AdminTestSessions(ClientObject cl)
        {
            InitializeComponent();
            client = cl;
            AddItemsToSubjectComboBox();
            //SetListOfTest();

            //Style rowStyle = new Style(typeof(DataGridRow));
            //rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent,
            //                         new MouseButtonEventHandler(Row_DoubleClick)));
            //tests.RowStyle = rowStyle;

        }

        // додає групи для перегляду
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

        public void AddItemsToTestComboBox(int themeId)
        {
            tests = client.GetTestsForTheme(themeId);
            TestComboBox.Items.Clear();
            if (themes.Count > 0)
            {
                foreach (DTOTheme item in themes)
                {
                    ComboBoxItem gnItem = new ComboBoxItem() { Content = item.ThemeName };
                    TestComboBox.Items.Add(gnItem);
                }
            }
            else
            {
                ComboBoxItem gnItem = new ComboBoxItem() { Content = "No Test" };
                TestComboBox.Items.Add(gnItem);
            }
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var themeComboBox = sender as ComboBox;
            var selectTheme = themeComboBox.SelectedIndex;
            int themeId = themes[selectTheme].Id;
            this.AddItemsToTestComboBox(themeId);
        }

        //public void SetListOfTest()
        //{
        //    tests = client.GetAllTests();
        //    if (tests.Count != 0)
        //        testsGrid.ItemsSource = tests;
        //}

        //private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    DataGridRow row = sender as DataGridRow;
        //    try
        //    {
        //        var rowId = ((DTO.DTOTest)row.DataContext).TestId;
        //        NavigationWindow navWIN = new NavigationWindow();
        //        navWIN.Content = new TestDesigner(client, rowId);
        //        navWIN.Show();
        //    }
        //    catch (Exception exeption)
        //    {
        //        var ex = exeption.Message;
        //    }
        //}
    }
}
