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
    /// Interaction logic for AdminThemes.xaml
    /// </summary>
    public partial class AdminThemes : Page
    {
        ClientObject client;
        Window mw;
        Frame main = null;
        int subjectId = -1;
        List<DTOTheme> themes;
        public AdminThemes(ClientObject cl, Window _mw, Frame _mn, int _subjectId)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            main = _mn;
            subjectId = _subjectId;
            SetListOfThemes();
        }

        public void SetListOfThemes()
        {
            themes = client.GetThemesForSubject(subjectId);
            if (themes.Count > 0)
            {
                themesGrid.ItemsSource = themes;
            }
        }

        private void AddThemeButton_Click(object sender, RoutedEventArgs e)
        {
            main.Content = new AddTheme(client, mw, main, subjectId);
        }

        private void DeleteTheme_Click(object sender, RoutedEventArgs e)
        {
            var dg = sender as DataGrid;

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    if (((System.Windows.FrameworkElement)vis).DataContext is DTOTheme)
                    {
                        var themeId = ((DTO.DTOTheme)((System.Windows.FrameworkElement)vis).DataContext).Id;
                        if (themeId != -1)
                        {
                            client.DeleteTheme(themeId);
                            this.SetListOfThemes();
                        }
                    }
                }
        }
    }
}
