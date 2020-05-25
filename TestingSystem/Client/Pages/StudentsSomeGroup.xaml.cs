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
    /// Interaction logic for StudentsSomeGroup.xaml
    /// </summary>
    public partial class StudentsSomeGroup : Page
    {
        ClientObject client;
        Window mw;
        Frame main = null;
        DTOGroup dtoGroup = null;
        List<DTOStudent> students = null;
        
        public StudentsSomeGroup(ClientObject cl, Window _mw, Frame _mn, DTOGroup _group)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            main = _mn;
            dtoGroup = _group;
            StudentGroupsLabel.Content = "Студенти групи " + dtoGroup.GroupName + ":";
            SetListOfStudents();
        }

        public void SetListOfStudents()
        {
            students = client.GetGroupStudents(dtoGroup.GroupId);
            if (students.Count > 0)
                studentsOfGroupGrid.ItemsSource = students;
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            main.Content = new AddStudentToGroup(client, mw, main, dtoGroup);
        }

        private void DeleteStudentFromGroup_Click(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    if (((System.Windows.FrameworkElement)vis).DataContext is DTOStudent)
                    {
                        var studentId = ((DTO.DTOStudent)((System.Windows.FrameworkElement)vis).DataContext).StudentId;
                        if (studentId != -1)
                        {
                            string answer = client.DeleteStudentFromGroup(studentId);
                            if (answer == "successfully")
                            {
                                SetListOfStudents();
                            }
                        }
                    }
                }
            }
            
        }
    }
    
}
