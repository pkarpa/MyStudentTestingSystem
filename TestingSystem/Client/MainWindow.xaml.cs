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
        Button myAccount = new Button() { Name = "MyAccountButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Кабінет", Padding = new Thickness(25, 0, 25, 7) };
        myAccount.Click += MyAdminAccountButton_Click;
        Button studentsButton = new Button() { Name = "StudentsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Студенти", Padding = new Thickness(25, 0, 25, 7) };
        studentsButton.Click += StudentsButton_Click;
        Button groupsButton = new Button() { Name = "GroupsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Групи", Padding = new Thickness(25, 0, 25, 7) };
        groupsButton.Click += GroupsButton_Click;
        Button testsButton = new Button() { Name = "TestsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Тести", Padding = new Thickness(25, 0, 25, 7) };
        testsButton.Click += TestsButton_Click;
        Button subjectsButton = new Button() { Name = "SubjectsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Предмети", Padding = new Thickness(25, 0, 25, 7) };
        subjectsButton.Click += AdminSubjectButton_Click;
        Button testSessionsButton = new Button() { Name = "TestSessionsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Тестові сесії", Padding = new Thickness(25, 0, 25, 7) };
        testSessionsButton.Click += AdminTestSessionsButton_Click;
        Button logOutButton = new Button() { Name = "LogOutButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Вихід", Padding = new Thickness(25, 0, 25, 7) };
        logOutButton.Click += LogOutButton_Click;
        buttonsStackPanel.Children.Add(myAccount);
        buttonsStackPanel.Children.Add(studentsButton);
        buttonsStackPanel.Children.Add(groupsButton);
        buttonsStackPanel.Children.Add(subjectsButton);
        buttonsStackPanel.Children.Add(testsButton);
        buttonsStackPanel.Children.Add(testSessionsButton);
        buttonsStackPanel.Children.Add(logOutButton);
      }
      else
      {
        Button myStudentAccount = new Button() { Name = "MyStudentAccountButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Кабінет", Padding = new Thickness(25, 0, 25, 7) };
        myStudentAccount.Click += MyStudentAccountButton_Click;
        Button testsButton = new Button() { Name = "testsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Тести", Padding = new Thickness(25, 0, 25, 7) };
        testsButton.Click += TestsButton_Click;
        Button resultsButton = new Button() { Name = "resultsButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Результати", Padding = new Thickness(25, 0, 25, 7) };
        resultsButton.Click += ResultsButton_Click;
        Button logOutButton = new Button() { Name = "LogOutButton", Background = Brushes.Blue, FontSize = 15, Foreground = Brushes.White, Content = "Вихід", Padding = new Thickness(25, 0, 25, 7) };
        buttonsStackPanel.Children.Add(myStudentAccount);
        buttonsStackPanel.Children.Add(testsButton);
        buttonsStackPanel.Children.Add(resultsButton);
        buttonsStackPanel.Children.Add(logOutButton);
        logOutButton.Click += LogOutButton_Click;
      }
    }

    // кнопка для переходу на сторінку профілю адміністратора
    private void MyAdminAccountButton_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Content = new AdminAccount(client, mw, MainFrame);
    }

    // кнопка для переходу на сторінку студентів
    private void StudentsButton_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Content = new AdminStudentPage(client, mw, MainFrame);
    }

    // кнопка для переходу на сторінку груп студентів
    private void GroupsButton_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Content = new AdminStudentGroups(client, mw, MainFrame);
    }

    private void AdminSubjectButton_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new AdminSubjects(client, mw, MainFrame);
    }
        
    private void MyStudentAccountButton_Click(object sender, RoutedEventArgs e)
    {
      MainFrame.Content = new StudentAccount(client);
    }

    private void AdminTestSessionsButton_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new AdminTestSessions(client);
    }

    private void TestsButton_Click(object sender, RoutedEventArgs e)
    {
        if (client.IsAdmin == true)
        {
            MainFrame.Content = new AdminTests(client, mw, MainFrame);
        }
        else 
        {
            MainFrame.Content = new StudentTests(client, mw, MainFrame);
        }
    }

    private void ResultsButton_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Content = new StudentResults(client);
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
