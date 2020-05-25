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
    /// Interaction logic for AddStudentToGroup.xaml
    /// </summary>
    public partial class AddStudentToGroup : Page
    {
        ClientObject client;
        Window mw;
        Frame main = null;
        DTOGroup dTOGroup = null;
        List<DTOStudent> students = null;
        List<DTOStudent> avaibleStudents = null;
        public AddStudentToGroup(ClientObject cl, Window _mw, Frame _mn, DTOGroup _group)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            main = _mn;
            dTOGroup = _group;
            avaibleStudents = new List<DTOStudent>();
            SetListStudents();
        }

        public void SetListStudents()
        {
            students = client.GetStudents();
            foreach (var st in students)
            {
                if (st.GroupId == 6)
                {
                    avaibleStudents.Add(st);
                }
            }
            if (avaibleStudents.Count > 0)
            {
                studentsGrid.ItemsSource = avaibleStudents;
            }
        }

        private void AddStudentToGroup_Click(object sender, RoutedEventArgs e)
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
                            string answer = client.AddStudentToGroup(studentId, dTOGroup.GroupId);
                            if (answer == "succesfull")
                            {
                                main.Content = new StudentsSomeGroup(client, mw, main, dTOGroup);
                            }
                        }
                    }
                }
        }
    }
}
