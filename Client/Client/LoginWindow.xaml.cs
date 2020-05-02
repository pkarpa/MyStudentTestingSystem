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
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        ClientObject client;
        public LoginWindow()
        {
            InitializeComponent();
            client = new ClientObject();
        }

        // кнопка входу в систему
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password != "" && LoginBox.Text != "")
            {
                if (PasswordBox.Password.Length > 4)
                {
                    client.Connect();
                    string answer = client.LogIn(LoginBox.Text, PasswordBox.Password);
                    if (answer == "succesfully")
                    {
                        MainWindow mw = new MainWindow(client);
                        Close();
                        mw.ShowDialog();
                    }
                    else if (answer == "No user with this password")
                    {
                        MessageBox.Show(answer);
                        PasswordBox.Foreground = Brushes.Red;
                    }
                    else if (answer == "No user with this login")
                    {
                        MessageBox.Show(answer);
                        LoginBox.Foreground = Brushes.Red;
                    }
                    else
                    {
                        MessageBox.Show("Something wrong!!!");
                        client.Disconnect();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Password is short, minimum 5 characters!!!");
                    PasswordBox.Password = "";
                }
            }
            else
            {
                MessageBox.Show("Fill all the gaps!!!");
            }
        }

        // Кнопка переходу до реєстрації
        private void RegistrateButton_Click(object sender, RoutedEventArgs e)
        {
            client.Connect();
            RegistrarionWindow rw = new RegistrarionWindow(client);
            rw.ShowDialog();
        }
    }
}
