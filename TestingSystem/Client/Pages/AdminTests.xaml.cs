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
  /// Interaction logic for AdminTests.xaml
  /// </summary>
  public partial class AdminTests : Page
  {
    ClientObject client;
    List<DTOTest> tests = null;
    List<DTOTheme> theme = null;

    public AdminTests(ClientObject cl)
    {
      InitializeComponent();
      client = cl;
      AddItemsToComboBox();
      SetListOfTest();

      Style rowStyle = new Style(typeof(DataGridRow));
      rowStyle.Setters.Add(new EventSetter(DataGridRow.MouseDoubleClickEvent,
                               new MouseButtonEventHandler(Row_DoubleClick)));
      testsGrid.RowStyle = rowStyle;

    }

    // додає групи для перегляду
    public void AddItemsToComboBox()
    {
      theme = client.GetTheme();
      ThemeComboBox.Items.Clear();
      if (theme.Count > 0)
      {
        foreach (DTOTheme item in theme)
        {
          ComboBoxItem gnItem = new ComboBoxItem() { Content = item.ThemeName };
          ThemeComboBox.Items.Add(gnItem);
        }
      }
      else
      {
        ComboBoxItem gnItem = new ComboBoxItem() { Content = "No Groups" };
        ThemeComboBox.Items.Add(gnItem);
      }

    }

    public void SetListOfTest()
    {
      tests = client.GetAllTests();
      if (tests.Count != 0)
        testsGrid.ItemsSource = tests;
    }

    private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
    {
      DataGridRow row = sender as DataGridRow;
      try
      {
        var rowId = ((DTO.DTOTest)row.DataContext).TestId;
        NavigationWindow navWIN = new NavigationWindow();
        navWIN.Content = new TestDesigner(client, rowId);
        navWIN.Show();
      }
      catch(Exception exeption)
      {
        var ex = exeption.Message;
      }
    }


    private void ButtonClick_PassTest(object sender, MouseButtonEventArgs e)
    {
      DataGridRow row = sender as DataGridRow;
      try
      {
        var rowId = ((DTO.DTOTest)row.DataContext).TestId;
        NavigationWindow navWIN = new NavigationWindow();
        navWIN.Content = new TestDesigner(client, rowId);
        navWIN.Show();
      }
      catch (Exception exeption)
      {
        var ex = exeption.Message;
      }
    }
  }
}
