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
using DTOLibraryTestingSystem;

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
                //name = administrator.Name;
                //login = administrator.Login;
                //password = administrator.Password;
                StudentName.Text = student.Name;
                StudentSurName.Text = student.SurName;
                StudentGroup.Text = student.GroupName;
                StudentLogin.Text = student.Login;
                StudentPassword.Password = student.Password;
                StudentPasswordTextBox.Text = student.Password;
            }
        }
    }
}
