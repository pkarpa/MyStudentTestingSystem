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
  /// Interaction logic for AdminTests.xaml
  /// </summary>
  public partial class AdminTests : Page
  {
    ClientObject client;
    Window mw;
    Frame main = null;
    List<DTOTest> tests = null;
    List<DTOTheme> themes = null;
    List<TestViewModel> testsV = null;
   

    public AdminTests(ClientObject cl, Window _mw, Frame _mn)
    {
      InitializeComponent();
      client = cl;
      mw = _mw;
      main = _mn;
      themes = client.GetTheme();
      testsV = new List<TestViewModel>();
      //AddItemsToComboBox();
      SetListOfTest();

      Style rowStyle = new Style(typeof(DataGridRow));
      rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent,
                               new MouseButtonEventHandler(Row_DoubleClick)));
      testsGrid.RowStyle = rowStyle;

    }

        // додає групи для перегляду
        //public void AddItemsToComboBox()
        //{
        //  theme = client.GetTheme();
        //  ThemeComboBox.Items.Clear();
        //  if (theme.Count > 0)
        //  {
        //    foreach (DTOTheme item in theme)
        //    {
        //      ComboBoxItem gnItem = new ComboBoxItem() { Content = item.ThemeName };
        //      ThemeComboBox.Items.Add(gnItem);
        //    }
        //  }
        //  else
        //  {
        //    ComboBoxItem gnItem = new ComboBoxItem() { Content = "No Groups" };
        //    ThemeComboBox.Items.Add(gnItem);
        //  }

        //}

        //public void SetListOfTest()
        //{
        //    tests = client.GetAllTests();
        //    if (tests.Count != 0)
        //        testsGrid.ItemsSource = tests;
        //}

        public void SetListOfTest()
        {
            tests = client.GetAllTests();
            if (tests.Count != 0)
            {
                foreach (var t in tests)
                {
                    string theme = themes.FirstOrDefault(f => f.Id == t.ThemeId).ThemeName;
                    TestViewModel testV = new TestViewModel() { TestId = t.TestId, TestName = t.TestName, MixQuestionsOrder = t.MixQuestionsOrder, MixAnswersOrder = t.MixAnswersOrder, TestTime = t.TestTime, ThemeId = t.ThemeId, ThemeName = theme };
                    testsV.Add(testV);
                }
                testsGrid.ItemsSource = testsV;
            }

        }

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

        //private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    DataGridRow row = sender as DataGridRow;
        //    try
        //    {
        //        int testId = ((DTO.TestViewModel)row.DataContext).TestId;
        //        DTOTest test = tests.FirstOrDefault(x => x.TestId == testId);
        //        var rowId = test.TestId;
        //        NavigationWindow navWIN = new NavigationWindow();
        //        navWIN.Content = new TestDesigner(client, rowId);
        //        navWIN.Show();
        //    }
        //    catch (Exception exeption)
        //    {
        //        var ex = exeption.Message;
        //    }
        //}

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            try
            {
                int testId = ((DTO.TestViewModel)row.DataContext).TestId;
                DTOTest test = tests.FirstOrDefault(x => x.TestId == testId);
                var rowId = test.TestId;
                main.Content = new AdminTestQuestions(client, mw, main, rowId);
                //NavigationWindow navWIN = new NavigationWindow();
                //navWIN.Content = new TestDesigner(client, rowId);
                //navWIN.Show();
            }
            catch (Exception exeption)
            {
                var ex = exeption.Message;
            }
        }


    private void ButtonClick_PassTest(object sender, MouseButtonEventArgs e)
    {
      DataGridRow row = sender as DataGridRow;
      try
      {
        var rowId = ((DTO.DTOTest)row.DataContext).TestId;
        NavigationWindow navWIN = new NavigationWindow();
        navWIN.Content = new TestDesigner(client, rowId);
        navWIN.Show();
      }
      catch (Exception exeption)
      {
        var ex = exeption.Message;
      }
    }

    private void AddTestButton_Click(object sender, RoutedEventArgs e)
    {
        main.Content = new AddNewTest(client, mw, main);
    }

    private void DeleteTest_Click(object sender, RoutedEventArgs e)
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
                        client.DeleteTest(testId);
                        main.Content = new AdminTests(client, mw, main);
                    }
                }
            }
    }
  }
}
