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
    /// Interaction logic for AddSubject.xaml
    /// </summary>
    public partial class AddSubject : Page
    {
        ClientObject client;
        Window mw;
        Frame main = null;
        List<DTOSubject> subjects;

        public AddSubject(ClientObject cl, Window _mw, Frame _mn)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            main = _mn;
            subjects = client.GetSubjectsForAdmin();
        }

        private void CreateSubjectButton_Click(object sender, RoutedEventArgs e)
        {
            if (SubjectName.Text != "")
            {
                bool isCorrect = true;
                foreach (var n in subjects)
                {
                    if (SubjectName.Text == n.SubjectName)
                    {
                        isCorrect = false;
                    }
                }
                if (isCorrect)
                {
                    string subjectName = SubjectName.Text;
                    string answer = client.AddSubject(subjectName);
                    if (answer == "succesfull")
                    {
                        main.Content = new AdminSubjects(client, mw, main);
                    }
                }
                else
                {
                    MessageBox.Show("This item already exists!");
                }
            }
            else
            {
                MessageBox.Show("Please enter subject name!");
            }
        }
    }
}
