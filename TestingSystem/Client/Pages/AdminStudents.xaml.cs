using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for AdminStudentPage.xaml
    /// </summary>
    public partial class AdminStudentPage : Page
    {
        ClientObject client;
        Window mw;
        Frame main = null;
        List<DTOStudent> students = null;
        List<DTOGroup> groups = null;

        public AdminStudentPage(ClientObject cl, Window _mw, Frame _mn)
        {
            InitializeComponent();
            mw = _mw;
            main = _mn;
            client = cl;
            AddItemsToGroupComboBox();

            //SetListOfTest();

            //Style rowStyle = new Style(typeof(DataGridRow));
            //rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent,
            //                         new MouseButtonEventHandler(Row_DoubleClick)));
            //StudentsGrid.RowStyle = rowStyle;

        }

        // додає групи для перегляду
        public void AddItemsToGroupComboBox()
        {
            groups = client.GetGroups();
            GroupsComboBox.Items.Clear();
            if (groups.Count > 0)
            {
                foreach(DTOGroup item in groups)
                {
                    ComboBoxItem gnItem = new ComboBoxItem() { Content = item.GroupName };
                    GroupsComboBox.Items.Add(gnItem);
                }
            }
            else
            {
                ComboBoxItem gnItem = new ComboBoxItem() { Content = "No Groups" };
                GroupsComboBox.Items.Add(gnItem);
            }

        }

        public void SetListOfStudents(int groupId)
        {
            students = client.GetGroupStudents(groupId);
            if (students.Count != 0)
                StudentsGrid.ItemsSource = students;
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
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

        private void GroupsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var groupComboBox = sender as ComboBox;
            var selectGroup = groupComboBox.SelectedIndex;
            int groupId = groups[selectGroup].GroupId;
            this.SetListOfStudents(groupId);
        }

        private void ChangeStudent_Click(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    if (((System.Windows.FrameworkElement)vis).DataContext is DTOStudent)
                    {
                        var studentId = ((DTO.DTOStudent)((System.Windows.FrameworkElement)vis).DataContext).StudentId;
                        if (studentId != -1)
                        {
                            DTOStudent st = students.FirstOrDefault(x => x.StudentId == studentId);
                            main.Content = new AdminChangeStudent(client, mw, main, st);
                        }
                    }
                }
           
        }
    }
}
