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
using Client.Pages;
using DTO;

namespace Client
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    ClientObject client;
    Window mw;
    public MainWindow(ClientObject cl)
    {
      InitializeComponent();
      client = cl;
      mw = this;
      if (client.IsAdmin) // в залежості від користувача який входить в систему на головному меню відображаються різні кнопки
      {
        Button myAccount = new Button() { Name = "MyAccountButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "My Account", Padding = new Thickness(25, 0, 25, 7) };
        myAccount.Click += MyAdminAccountButton_Click;
        Button studentsButton = new Button() { Name = "StudentsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Students", Padding = new Thickness(25, 0, 25, 7) };
        studentsButton.Click += StudentsButton_Click;
        Button groupsButton = new Button() { Name = "GroupsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Groups", Padding = new Thickness(25, 0, 25, 7) };
        groupsButton.Click += GroupsButton_Click;
        Button testsButton = new Button() { Name = "TestsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Tests", Padding = new Thickness(25, 0, 25, 7) };
        testsButton.Click += TestsButton_Click;
        Button subjectsButton = new Button() { Name = "SubjectsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Subjects", Padding = new Thickness(25, 0, 25, 7) };
        Button testSessionsButton = new Button() { Name = "TestSessionsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Test Sessions", Padding = new Thickness(25, 0, 25, 7) };
        Button logOutButton = new Button() { Name = "LogOutButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Log out", Padding = new Thickness(25, 0, 25, 7) };
        logOutButton.Click += LogOutButton_Click;
        buttonsStackPanel.Children.Add(myAccount);
        buttonsStackPanel.Children.Add(studentsButton);
        buttonsStackPanel.Children.Add(groupsButton);
        buttonsStackPanel.Children.Add(testsButton);
        buttonsStackPanel.Children.Add(testSessionsButton);
        buttonsStackPanel.Children.Add(logOutButton);
      }
      else
      {
        Button myStudentAccount = new Button() { Name = "MyStudentAccountButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "My Account", Padding = new Thickness(25, 0, 25, 7) };
        myStudentAccount.Click += MyStudentAccountButton_Click;
        Button testsButton = new Button() { Name = "testsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Tests", Padding = new Thickness(25, 0, 25, 7) };
        testsButton.Click += TestsButton_Click;
        Button logOutButton = new Button() { Name = "LogOutButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Log out", Padding = new Thickness(25, 0, 25, 7) };
        buttonsStackPanel.Children.Add(myStudentAccount);
        buttonsStackPanel.Children.Add(testsButton);
        buttonsStackPanel.Children.Add(logOutButton);
        logOutButton.Click += LogOutButton_Click;
      }
    }

    // кнопка для переходу на сторінку профілю адміністратора
    private void MyAdminAccountButton_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Content = new AdminAccount(client);
    }

    // кнопка для переходу на сторінку студентів
    private void StudentsButton_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Content = new AdminStudentPage();
    }

    // кнопка для переходу на сторінку груп студентів
    private void GroupsButton_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Content = new AdminGroups(client);
    }

    private void MyStudentAccountButton_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Content = new StudentAccount(client);
    }

    private void TestsButton_Click(object sender, RoutedEventArgs e)
    {
        if (client.IsAdmin == true)
        {
            MainFrame.Content = new AdminTests(client);
        }
        else 
        {
                MainFrame.Content = new StudentTests(client, mw);
        }
    }

    // кнопка виходу з системи
    private void LogOutButton_Click(object sender, RoutedEventArgs e)
    {
      client.LogOut();
      LoginWindow lg = new LoginWindow();
      Close();
      lg.ShowDialog();
    }
  }
}
