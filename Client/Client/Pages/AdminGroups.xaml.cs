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
    /// Interaction logic for AdminGroups.xaml
    /// </summary>
    public partial class AdminGroups : Page
    {
        ClientObject client;
        List<DTOGroup> groups;

        public AdminGroups(ClientObject cl)
        {
            InitializeComponent();
            client = cl;
            AddItemsToComboBox();
        }

        // додає групи для перегляду
        public void AddItemsToComboBox()
        {
            groups = client.GetGroupsForAdmin();
            GroupsComboBox.Items.Clear();
            if (groups.Count > 0)
            {
                foreach (DTOGroup item in groups)
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

        //Тут ще не докінця дороблено, і саме тут виникає ця помилка яку я не можу знайти
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (GroupsComboBox.SelectedItem != null)
            {
                DetailsGrid.Visibility = Visibility.Visible;
                DTOGroup group = groups.FirstOrDefault(g => g.GroupName == GroupsComboBox.Text);
                List<DTOStudent> students = client.GetGroupStudents(group.StudentsId.ToList<int>());
                List<DTOTest> tests = client.GetGroupTests(group.TestsId.ToList<int>());
                studentsList.ItemsSource = students;
                testsList.ItemsSource = tests;
            }
            else
            {
                MessageBox.Show("Please select group!");
            }
        }

        //Видаляємо обрану групу
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (GroupsComboBox.SelectedItem != null)
            {
                int groupId = groups.FirstOrDefault(g => g.GroupName == GroupsComboBox.Text).GroupId;
                string answer = client.DeleteGroup(groupId);

                if (answer != "successfully")
                {
                    MessageBox.Show("Something wrong!");
                }
                else
                {
                    AddItemsToComboBox();
                }
            }
            else
            {
                MessageBox.Show("Please select group!");
            }
        }

        // показує стрічку для вводу імені нової групи
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (DetailsGrid.Visibility == Visibility.Visible)
            {
                DetailsGrid.Visibility = Visibility.Hidden;
            }
            AddGroupLabel.Visibility = Visibility.Visible;
            AddGroupTextBox.Visibility = Visibility.Visible;
            SaveButton.Visibility = Visibility.Visible;
            AddButton.IsEnabled = false;
        }

        // збереження нової групи
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddGroupTextBox.Text != "")
            {
                string answer = client.AddGroup(AddGroupTextBox.Text);
                if (answer == "successfully")
                {
                    AddItemsToComboBox();
                    AddGroupLabel.Visibility = Visibility.Hidden;
                    AddGroupTextBox.Visibility = Visibility.Hidden;
                    SaveButton.Visibility = Visibility.Hidden;
                    AddButton.IsEnabled = true;
                }
                else if (answer == "ExistName")
                {
                    AddGroupTextBox.Foreground = Brushes.Red;
                    MessageBox.Show("A group with this name already exists, please enter another name!");
                }
                else
                {
                    MessageBox.Show("Something wrong!");
                }
            }
            else 
            {
                MessageBox.Show("Please, enter group name!");
            }
        }

        private void ChangeNameButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DeleteStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (studentsList.SelectedItem != null)
            {
                DTOStudent student = (DTOStudent)studentsList.SelectedItem;
                ComboBoxItem gnItem = new ComboBoxItem() { Content = GroupsComboBox.Text }; ;
                string answer = client.DeleteStudentFromGroup(student.StudentId);
                if(answer == "successfully")
                {
                    AddItemsToComboBox();
                    DTOGroup group = groups.FirstOrDefault(g => g.GroupName == gnItem.Content.ToString());
                    List<DTOStudent> students = client.GetGroupStudents(group.StudentsId.ToList<int>());
                    studentsList.ItemsSource = students;      
                }
                else 
                {
                    MessageBox.Show("Something wrong!");
                }
            }
            else 
            {
                MessageBox.Show("Pleaіe select student!");
            }    
        }

        private void DeleteTestButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
