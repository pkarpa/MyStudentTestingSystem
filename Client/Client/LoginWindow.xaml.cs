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
        bool isAdmin;
        ClientObject client;
        public LoginWindow()
        {
            InitializeComponent();
            isAdmin = false;
            client = new ClientObject();
            client.Connect();
        }

        // кнопка входу в систему
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Text != "" && LoginBox.Text != "")
            {
                if (PasswordBox.Text.Length > 4)
                {
                    string answer = client.LogIn(isAdmin, LoginBox.Text, PasswordBox.Text);
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
                    PasswordBox.Text = "";
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
            RegistrarionWindow rw = new RegistrarionWindow(client);
            rw.ShowDialog();
        }

        // кнопка зміни користувача (студент/адміністратор)
        private void ChangeUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                isAdmin = false;
                this.ChangeUserButton.Content = "As Admin";
            }
            else 
            {
                isAdmin = true;
                this.ChangeUserButton.Content = "As Student";
            }
        }
    }
}
