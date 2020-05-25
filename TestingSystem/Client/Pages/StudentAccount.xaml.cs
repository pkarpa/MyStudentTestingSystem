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
    /// Interaction logic for StudentAccount.xaml
    /// </summary>
    public partial class StudentAccount : Page
    {
        ClientObject client;
        DTOStudent student = null;
        public StudentAccount(ClientObject cl)
        {
            InitializeComponent();
            client = cl;
            this.StudentInfo();
        }

        // Метод який викликає метод  який повертає інформацію про адміністратора
        private void StudentInfo()
        {
            student = client.GetInfoAboutStudent();
            if (student != null)
            {
                StudentName.Text = student.Name;
                StudentSurName.Text = student.SurName;
                StudentLogin.Text = student.Login;
                StudentPassword.Text = student.Password;
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            StudentName.IsEnabled = true;
            StudentSurName.IsEnabled = true;
            StudentLogin.IsEnabled = true;
            StudentPassword.IsEnabled = true;
            SaveButton.IsEnabled = true;
            ChangeButton.IsEnabled = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StudentName.IsEnabled = false;
            StudentSurName.IsEnabled = false;
            StudentLogin.IsEnabled = false;
            StudentPassword.IsEnabled = false;
            SaveButton.IsEnabled = false;
            ChangeButton.IsEnabled = true;

            student.Name = StudentName.Text;
            student.SurName = StudentSurName.Text;
            student.Login = StudentLogin.Text;
            student.Password = StudentPassword.Text;
            string answer = client.EditStudent(student);
            if (answer == "succesfull")
            {
            }
        }
    }
}
