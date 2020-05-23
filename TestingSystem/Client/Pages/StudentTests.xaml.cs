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
    /// Interaction logic for StudentTests.xaml
    /// </summary>
    /// 

    public partial class StudentTests : Page
    {
        ClientObject client;
        List<DTOTest> tests = null;
        List<DTOTheme> theme = null;
        Window mw;
        public StudentTests(ClientObject cl, Window _mw)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            AddItemsToComboBox();
            SetListOfTest();
        }

        // додає групи для перегляду
        public void AddItemsToComboBox()
        {
            theme = client.GetTheme();
            ThemeComboBox.Items.Clear();
            if (theme.Count > 0)
            {
                foreach (DTOTheme item in theme)
                {
                    ComboBoxItem gnItem = new ComboBoxItem() { Content = item.ThemeName };
                    ThemeComboBox.Items.Add(gnItem);
                }
            }
            else
            {
                ComboBoxItem gnItem = new ComboBoxItem() { Content = "No Themes" };
                ThemeComboBox.Items.Add(gnItem);
            }
        }

        public void SetListOfTest()
        {
            tests = client.GetTestsForSomeGroup();
            if (tests.Count != 0)
            {
                testsGrid.ItemsSource = tests;
            }         
        }

        private void Pass_Button_Click(object sender, RoutedEventArgs e)
        {
            //DataGridRow row = sender as DataGridRow;
            //try
            //{
            //    var rowId = ((DTO.DTOTest)row.DataContext).TestId;
            //    NavigationWindow navWIN = new NavigationWindow();
            //    navWIN.Content = new StudentPassTest(client, rowId);
            //    navWIN.Show();
            //}
            //catch (Exception exeption)
            //{
            //    var ex = exeption.Message;
            //}

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    if (((System.Windows.FrameworkElement)vis).DataContext is DTOTest)
                    {
                        int testId = ((DTO.DTOTest)((System.Windows.FrameworkElement)vis).DataContext).TestId;
                        NavigationWindow navWIN = new NavigationWindow();
                        navWIN.Content = new StudentPassTest(client, testId, mw, this);
                        navWIN.Show();
                        mw.Visibility = Visibility.Hidden;
                    }
                }
            }
        }
    }
}
