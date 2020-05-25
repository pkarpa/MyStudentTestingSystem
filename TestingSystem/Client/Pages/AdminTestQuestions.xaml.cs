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
    /// Interaction logic for AdminTestQuestions.xaml
    /// </summary>
    public partial class AdminTestQuestions : Page
    {
        readonly ClientObject _client;
        List<QueDTOQuestionstion> questions = null;
        List<DTOQuestionType> questionType = null;
        int _testId;
        Window mw;
        Frame main = null;

        public AdminTestQuestions(ClientObject cl, Window _mw, Frame _mn, int testId)
        {
            InitializeComponent();
            _client = cl;
            _testId = testId;
            mw = _mw;
            main = _mn;
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

        void Button_Click(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    if (((System.Windows.FrameworkElement)vis).DataContext is QueDTOQuestionstion)
                    {
                        //if (((DTO.QueDTOQuestionstion)((System.Windows.FrameworkElement)vis).DataContext).)
                        var id = ((DTO.QueDTOQuestionstion)((System.Windows.FrameworkElement)vis).DataContext).Id;
                        if (id == 0)
                        {
                            var new_id = _client.SaveQuestion(((DTO.QueDTOQuestionstion)((System.Windows.FrameworkElement)vis).DataContext));
                            ((DTO.QueDTOQuestionstion)((System.Windows.FrameworkElement)vis).DataContext).Id = new_id;
                        }
                        else
                        {
                            _client.UpdateQuestion(((DTO.QueDTOQuestionstion)((System.Windows.FrameworkElement)vis).DataContext));
                        }
                    }
                }
        }

        private void DataGrid_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var datag = (DataGrid)sender;
            var p = (QueDTOQuestionstion)datag.SelectedValue;
            var p1 = (QueDTOQuestionstion)e.Row.Item;
            //Debug.WriteLine(p.Name + ", " + p.Age);
            //Debug.WriteLine(p1.Name + ", " + p1.Age);
        }

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            main.Content = new AddNewQuestion(_client, mw, main, _testId);
        }

        private void DeleteQuestion_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
