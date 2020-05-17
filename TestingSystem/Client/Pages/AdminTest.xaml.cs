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
  /// Interaction logic for AdminTest.xaml
  /// </summary>
  public partial class AdminTest : Page
  {
    ClientObject client;
    List<DTOTest> tests = null;

    public AdminTest(ClientObject cl)
    {
      InitializeComponent();
      client = cl;
      //tests = client.GetGroupTests(1);
    }
  }
}
