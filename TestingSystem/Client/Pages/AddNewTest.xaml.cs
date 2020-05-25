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
    /// Interaction logic for AddNewTest.xaml
    /// </summary>
    public partial class AddNewTest : Page
    {
        ClientObject client;
        Frame main = null;
        Window mw = null;
        int themeId = -1;
        bool mixQuestionOrder = false;
        bool mixAnswersOrder = false;
        List<DTOSubject> subjects = null;
        List<DTOTheme> themes = null;
        public DateTime time;

        

        public AddNewTest(ClientObject cl, Window _mw, Frame _mn)
        {
            InitializeComponent();
            client = cl;
            main = _mn;
            mw = _mw;          
            this.AddItemsToSubjectComboBox();
        }

        private void CreateTestButton_Click(object sender, RoutedEventArgs e)
        {
            if (TestName.Text != "")
            {
                if (themeId != -1)
                {
                    int num1;
                    bool isNum = int.TryParse(TestTime.Text, out num1);
                    if (TestTime.Text != "" && isNum != false)
                    {
                        int min = int.Parse(TestTime.Text);
                        time = new DateTime(2020, 5, 24, 01, min, 00);
                        DTOTest test = new DTOTest() { TestName = TestName.Text, TestTime = time, MixQuestionsOrder = mixQuestionOrder, MixAnswersOrder = mixAnswersOrder, ThemeId = themeId };
                        client.AddTest(test);
                        main.Content = new AdminTests(client,mw, main);
                    }
                    else
                    {
                        MessageBox.Show("Please, enter correct test time!!!");
                    }
                }
                else
                {
                    MessageBox.Show("Please select test theme!!!");
                }
            }
            else
            {
                MessageBox.Show("Please enter name!!!");
            }
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var themeComboBox = sender as ComboBox;
            var selectTheme = themeComboBox.SelectedIndex;
            themeId = themes[selectTheme].Id;
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

        private void MixQuestionsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            mixQuestionOrder = true;
        }

        private void MixQuestionsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            mixQuestionOrder = false;
        }

        private void MixAnswersCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            mixAnswersOrder = true;
        }

        private void MixAnswersCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            mixAnswersOrder = false;
        }
    }
}
