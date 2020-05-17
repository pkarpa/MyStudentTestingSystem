using DTO;
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

namespace Client.Pages
{
  /// <summary>
  /// Interaction logic for AdminSubjects.xaml
  /// </summary>
  public partial class AdminSubjects : Page
  {
    ClientObject client;
    List<DTOSubject> subjects;
    public AdminSubjects()
    {
      InitializeComponent();
      AddItemsToComboBox();
    }

    // додає subject для перегляду
    public void AddItemsToComboBox()
    {
      subjects = client.GetSubjects();
      SubjectComboBox.Items.Clear();
      if (subjects.Count > 0)
      {
        foreach (DTOSubject item in subjects)
        {
          ComboBoxItem gnItem = new ComboBoxItem() { Content = item.SubjectName };
          SubjectComboBox.Items.Add(gnItem);
        }
      }
      else
      {
        ComboBoxItem gnItem = new ComboBoxItem() { Content = "No Groups" };
        SubjectComboBox.Items.Add(gnItem);
      }

    }

    // показує стрічку для вводу імені нової групи
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      //if (DetailsGrid.Visibility == Visibility.Visible)
      //{
      //  DetailsGrid.Visibility = Visibility.Hidden;
      //}
      //AddGroupLabel.Visibility = Visibility.Visible;
      //AddGroupTextBox.Visibility = Visibility.Visible;
      //SaveButton.Visibility = Visibility.Visible;
      //AddButton.IsEnabled = false;
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      //if (studentsList.SelectedItem != null)
      //{
      //  DTOStudent student = (DTOStudent)studentsList.SelectedItem;
      //  ComboBoxItem gnItem = new ComboBoxItem() { Content = GroupsComboBox.Text }; ;
      //  string answer = client.DeleteStudentFromGroup(student.StudentId);
      //  if (answer == "successfully")
      //  {
      //    AddItemsToComboBox();
      //    DTOGroup group = groups.FirstOrDefault(g => g.GroupName == gnItem.Content.ToString());
      //    List<DTOStudent> students = client.GetGroupStudents(group.GroupId);
      //    studentsList.ItemsSource = students;
      //  }
      //  else
      //  {
      //    MessageBox.Show("Something wrong!");
      //  }
      //}
      //else
      //{
      //  MessageBox.Show("Pleaіe select student!");
      //}
    }
  }
}
