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
using DTOLibraryTestingSystem;

namespace Client.Pages
{
    /// <summary>
    /// Interaction logic for AdminAccount.xaml
    /// </summary>
    public partial class AdminAccount : Page
    {
        private string name;
        private string login;
        private string password;
        ClientObject client;
        DTOAdministrator administrator = null;
        public AdminAccount(ClientObject cl)
        {
            InitializeComponent();
            client = cl;
            this.AdminInfo();
        }


        // Метод який викликає метод  який повертає інформацію про адміністратора
        private void AdminInfo()
        {
            administrator = client.GetInfoAboutAdmin();
            if (administrator != null)
            {
                name = administrator.Name;
                login = administrator.Login;
                password = administrator.Password;
                AdminName.Text = name;
                AdminLogin.Text = login;
                AdminPassword.Password = password;
                AdminPasswordTextBox.Text = password;
            }
        }
       
        // Кнопка зміни імені адміністратора
        private void ChangeNameButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeNameButton.IsEnabled = false; // вимикаємо кнопку зміни імені
            if (SaveNameButton.Visibility == Visibility.Hidden) // виводимо кнопку для збереження зміненого імені
            {
                SaveNameButton.Visibility = Visibility.Visible;
            }
            AdminName.IsEnabled = true; // дозволяємо змінювати дані в стрічці з іменем
        }

        // Кнопка зміни логіна адміністратора
        private void ChangeLoginButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeLoginButton.IsEnabled = false; // вимикаємо кнопку зміни логіна
            if (SaveLoginButton.Visibility == Visibility.Hidden)// виводимо кнопку для збереження зміненого логіна
            {
                SaveLoginButton.Visibility = Visibility.Visible;
            }
            AdminLogin.IsEnabled = true; // дозволяємо змінювати дані в стрічці з логіном
        }

        // Кнопка зміни паролю адміністратора
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordButton.IsEnabled = false; // вимикаємо кнопку зміни паролю
            if (SavePasswordButton.Visibility == Visibility.Hidden) // виводимо кнопку для збереження зміненого паролю
            {
                SavePasswordButton.Visibility = Visibility.Visible;
            }
            ShowPasswordCheckBox.IsChecked = true; // робимо пароль видимий і дозволяємо його змінювати
        }

        // Показ паролю
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            AdminPasswordTextBox.Text = AdminPassword.Password;
            AdminPasswordTextBox.Visibility = Visibility.Visible;
            AdminPassword.Visibility = Visibility.Hidden;
            AdminPassword.IsEnabled = false;
        }

        // Пароль показується крапками
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            AdminPassword.Password = AdminPasswordTextBox.Text;
            AdminPasswordTextBox.Visibility = Visibility.Hidden;
            AdminPassword.Visibility = Visibility.Visible;
        }

        // Зберігаємо змінене ім'я 
        private void SaveNameButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdminName.Text != "")
            {
                if (AdminName.Text != name)
                {
                    name = AdminName.Text;
                    string answer = client.EditAdminName(name);
                    if (answer == "succesfully")
                    {
                        ChangeNameButton.IsEnabled = true;
                        AdminName.IsEnabled = false;
                        SaveNameButton.Visibility = Visibility.Hidden;
                    }
                    else if (answer == "Exist admin with this name")
                    {
                        MessageBox.Show(answer);
                    }
                    else
                    {
                        MessageBox.Show("Error! Something wrong!");
                    }
                }
                else
                {
                    MessageBox.Show("Error! You must enter another name!");
                }
            }
            else
            {
                MessageBox.Show("Error! Enter name!");
            }
        }

        // Зберігаємо змінений пароль
        private void SavePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdminPasswordTextBox.Text != password)
            {
                if (AdminPasswordTextBox.Text.Length > 4)
                {
                    password = AdminPasswordTextBox.Text;
                    string answer = client.EditAdminPassword(password);
                    if (answer == "succesfully")
                    {
                        ChangePasswordButton.IsEnabled = true;
                        ShowPasswordCheckBox.IsChecked = false;
                        SavePasswordButton.Visibility = Visibility.Hidden;
                    }
                    else if (answer == "Exist admin with this password")
                    {
                        MessageBox.Show(answer);
                    }
                    else
                    {
                        MessageBox.Show("Error! Something wrong!");
                    }
                }
                else
                {
                    MessageBox.Show("Error! Minimum password length is 5 characters!");
                }
            }
            else
            {
                MessageBox.Show("Error! You must enter another password!");
            }

        }

        // Зберігаємо змінений логін
        private void SaveLoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (AdminLogin.Text != "")
            {
                if (AdminLogin.Text != login)
                {
                    login = AdminLogin.Text;
                    string answer = client.EditAdminLogin(login);
                    if (answer == "succesfully")
                    {
                        ChangeLoginButton.IsEnabled = true;
                        AdminLogin.IsEnabled = false;
                        SaveLoginButton.Visibility = Visibility.Hidden;
                    }
                    else if (answer == "Exist admin with this login")
                    {
                        MessageBox.Show(answer);
                    }
                    else
                    {
                        MessageBox.Show("Error! Something wrong!");
                    }
                }
                else
                {
                    MessageBox.Show("Error! You must enter another login!");
                }
            }
            else
            {
                MessageBox.Show("Error! Enter login!");
            }
        }
    }
}
