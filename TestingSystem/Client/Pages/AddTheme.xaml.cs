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
    /// Interaction logic for AddTheme.xaml
    /// </summary>
    public partial class AddTheme : Page
    {
        ClientObject client;
        Window mw;
        Frame main = null;
        int subjectId = -1;
        public AddTheme(ClientObject cl, Window _mw, Frame _mn, int _subjectId)
        {
            InitializeComponent();
            client = cl;
            mw = _mw;
            main = _mn;
            subjectId = _subjectId;
        }

        private void CreateThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeName.Text != "")
            {
                string themeName = ThemeName.Text;
                string answer = client.AddTheme(subjectId,themeName);
                if (answer == "succesfull")
                {
                    main.Content = new AdminThemes(client, mw, main, subjectId);
                }
            }
            else
            {
                MessageBox.Show("Please enter subject name!");
            }
        }
    }
}
