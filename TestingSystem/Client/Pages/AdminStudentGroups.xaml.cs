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
    /// Interaction logic for AdminStudentGroups.xaml
    /// </summary>
    public partial class AdminStudentGroups : Page
    {
        ClientObject client;
        Window mw;
        Frame main = null;
        List<DTOGroup> groups = null;
        List<DTOTheme> themes = null;
        List<TestViewModel> testsV = null;

        public AdminStudentGroups(ClientObject cl, Window _mw, Frame _mn)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            main = _mn;
            SetListOfGroups();
        }

        public void SetListOfGroups()
        {
            groups = client.GetGroups();
            if(groups.Count > 0)
                groupsGrid.ItemsSource = groups;
        }

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            main.Content = new AddNewStudentsGroup(client, mw, main);
        }

        private void GroupStudents_Click(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    if (((System.Windows.FrameworkElement)vis).DataContext is DTOGroup)
                    {
                        var groupId = ((DTO.DTOGroup)((System.Windows.FrameworkElement)vis).DataContext).GroupId;
                        if (groupId != -1)
                        {
                            DTOGroup g = groups.FirstOrDefault(x => x.GroupId == groupId);
                            main.Content = new StudentsSomeGroup(client, mw, main, g);
                        }
                    }
                }
        }

        private void GroupTest_Click(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    if (((System.Windows.FrameworkElement)vis).DataContext is DTOGroup)
                    {
                        var groupId = ((DTO.DTOGroup)((System.Windows.FrameworkElement)vis).DataContext).GroupId;
                        if (groupId != -1)
                        {
                            DTOGroup g = groups.FirstOrDefault(x => x.GroupId == groupId);
                            main.Content = new TestSomeGroup(client, mw, main, g);
                        }
                    }
                }
        }

        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
