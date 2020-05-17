using System;
using System.Collections;
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
  /// Interaction logic for TestDesigner.xaml
  /// </summary>
  public partial class TestDesigner : Page
  {
    readonly ClientObject _client;
    List<QueDTOQuestionstion> questions = null;
    List<DTOQuestionType> questionType = null;
    int _testId;

    public TestDesigner(ClientObject cl, int testId)
    {
      InitializeComponent();
      _client = cl;
      _testId = testId;
      questions = _client.GetQuestions(testId);
      questionType = _client.GetQuestionTypes();
      if (questions.Count != 0)
        testsGrid.ItemsSource = questions;
    }

    private IEnumerable<DataGridRow> GetDataGridRowsForButtons(DataGrid grid)
    { //IQueryable 
      var itemsSource = grid.ItemsSource as IEnumerable;
      if (null == itemsSource) yield return null;
      foreach (var item in itemsSource)
      {
        var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
        if (null != row & row.IsSelected) yield return row;
      }
    }

    void Button_Click_dgvs(object sender, RoutedEventArgs e)
    {

      for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
        if (vis is DataGridRow)
        {
          if (((System.Windows.FrameworkElement)vis).DataContext is QueDTOQuestionstion)
          {
            var id = ((DTO.QueDTOQuestionstion)((System.Windows.FrameworkElement)vis).DataContext).Id;
            if (id == 0)
            {
              _client.SaveQuestion(((DTO.QueDTOQuestionstion)((System.Windows.FrameworkElement)vis).DataContext));
            } else
            {
              _client.UpdateQuestion(((DTO.QueDTOQuestionstion)((System.Windows.FrameworkElement)vis).DataContext));
            }
          }
        }
    }
  }
}
