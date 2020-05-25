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
    /// Interaction logic for AdminChangeStudent.xaml
    /// </summary>
    public partial class AdminChangeStudent : Page
    {
        ClientObject client;
        Window mw;
        Frame main = null;
        DTOStudent student = null;
        public AdminChangeStudent(ClientObject cl, Window _mw, Frame _mn, DTOStudent _student)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            main = _mn;
            student = _student;
            ChangeTextBox();
        }

        public void ChangeTextBox()
        {
            StudentName.Text = student.Name;
            StudentSurName.Text = student.SurName;
            StudentLogin.Text = student.Login;
            StudentPassword.Text = student.Password;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            student.Name = StudentName.Text;
            student.SurName = StudentSurName.Text;
            student.Login = StudentLogin.Text;
            student.Password = StudentPassword.Text;
            string answer = client.EditStudent(student);
            if (answer == "succesfull")
            {
                main.Content = new AdminStudentPage(client,mw, main);
            }
        }
    }
}
