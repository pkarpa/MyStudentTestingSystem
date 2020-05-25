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
  /// Interaction logic for AdminSubjects.xaml
  /// </summary>
  public partial class AdminSubjects : Page
  {
    ClientObject client;
    Window mw;
    Frame main = null;
    List<DTOSubject> subjects;
    public AdminSubjects(ClientObject cl, Window _mw, Frame _mn)
    {
      InitializeComponent();
      client = cl;
      mw = _mw;
      main = _mn;
      SetListOfSubjects();

      Style rowStyle = new Style(typeof(DataGridRow));
      rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent,
                                new MouseButtonEventHandler(Row_DoubleClick)));
      subjectsGrid.RowStyle = rowStyle;
    }


    public void SetListOfSubjects()
    {
        subjects = client.GetSubjectsForAdmin();
        if (subjects.Count > 0)
        {
            subjectsGrid.ItemsSource = subjects;
        }
    }

    public void Row_DoubleClick(object sender, MouseButtonEventArgs e)
    {
        DataGridRow row = sender as DataGridRow;
        try
        {
            int subjectId = ((DTO.DTOSubject)row.DataContext).SubjectId;       
            main.Content = new AdminThemes(client, mw, main, subjectId);
            //NavigationWindow navWIN = new NavigationWindow();
            //navWIN.Content = new TestDesigner(client, rowId);
            //navWIN.Show();
        }
        catch (Exception exeption)
        {
            var ex = exeption.Message;
        }
    }

    private void AddSubjectButton_Click(object sender, RoutedEventArgs e)
    {
        main.Content = new AddSubject(client, mw, main);
    }

    private void DeleteSubject_Click(object sender, RoutedEventArgs e)
    {
    var dg = sender as DataGrid;

    for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
        if (vis is DataGridRow)
        {
            if (((System.Windows.FrameworkElement)vis).DataContext is DTOSubject)
            {
                var subjectId = ((DTO.DTOSubject)((System.Windows.FrameworkElement)vis).DataContext).SubjectId;
                if (subjectId != -1)
                {
                    client.DeleteSubject(subjectId);
                    SetListOfSubjects();
                }
            }
        }
    }
  }
}
