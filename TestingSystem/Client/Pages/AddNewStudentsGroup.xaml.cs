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
    /// Interaction logic for AddNewStudentsGroup.xaml
    /// </summary>
    public partial class AddNewStudentsGroup : Page
    {
        ClientObject client;
        Frame main = null;
        Window mw = null;
        List<DTOGroup> groups;
        public AddNewStudentsGroup(ClientObject cl, Window _mw, Frame _mn)
        {
            InitializeComponent();
            client = cl;
            main = _mn;
            mw = _mw;
            groups = client.GetGroups();
        }

        private void CreateGroupButton_Click(object sender, RoutedEventArgs e)
        {
            if (GroupName.Text != "")
            {
                bool isCorrect = true;
                foreach (var n in groups)
                {
                    if (GroupName.Text == n.GroupName)
                    {
                        isCorrect = false;
                    }
                }
                if (isCorrect)
                {
                    string groupName = GroupName.Text;
                    string answer = client.AddGroup(groupName);
                    if (answer == "successfull")
                    {
                        main.Content = new AdminStudentGroups(client, mw, main);
                    }
                }
                else
                {
                    MessageBox.Show("This item already exists!");
                }
            }
            else
            {
                MessageBox.Show("Please enter group name!");
            }
        }
    }
}
