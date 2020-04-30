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
using DTOLibraryTestingSystem;

namespace Client
{
    /// <summary>
    /// Interaction logic for RegistrarionWindow.xaml
    /// </summary>
    public partial class RegistrarionWindow : Window
    {
        bool isAdmin;
        DTOAdministrator admin;
        DTOStudent student;
        List<DTOGroup> groups; // список груп  чтудентів для реєстрації студента
        string selectedName;
        ClientObject client;
        public RegistrarionWindow(ClientObject cl)
        {
            InitializeComponent();
            client = cl;
            groups = client.GetGroups(); // отримуємо список груп 
            foreach (DTOGroup item in groups)
            {
                ComboBoxItem gnItem = new ComboBoxItem() { Content = item.GroupName };
                GroupNameComboBox.Items.Add(gnItem);
            }
        }

        // кнопка зміни користувача (студент/адміністратор)
        private void ChangeRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isAdmin)
            {
                isAdmin = true;
                this.ChangeRegistrationButton.Content = "New Student";
                this.SurNameBox.Visibility = Visibility.Collapsed;
                this.SurNameBlock.Visibility = Visibility.Collapsed;
                this.GroupNameComboBox.Visibility = Visibility.Collapsed;
                this.GroupNameBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                isAdmin = false;
                this.ChangeRegistrationButton.Content = "New Admin";
                this.SurNameBox.Visibility = Visibility.Visible;
                this.SurNameBlock.Visibility = Visibility.Visible;
                this.GroupNameComboBox.Visibility = Visibility.Visible;
                this.GroupNameBlock.Visibility = Visibility.Visible;
            }

        }

        // вибір групии студентів для студента
        private void GroupNameComboBox_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox groupNames = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)groupNames.SelectedItem;
            selectedName = selectedItem.Content.ToString();
        }

        // кнопка реєстрації, в залежності від обраного користувача відбувається реєстрація адміністратора або студента
        private void RegistrateButton_Click(object sender, RoutedEventArgs e)
        {
            string answer = null;
            if (!isAdmin)
            {
                if (NameBox.Text != "" && SurNameBox.Text !="" && LoginBox.Text != "" && PasswordBox.Text != "" && ConfirmPasswordBox.Text != "" && selectedName != null)
                {
                    if (PasswordBox.Text.Length > 4)
                    {
                        if (PasswordBox.Text == ConfirmPasswordBox.Text)
                        {
                            student = new DTOStudent { Name = NameBox.Text, SurName = SurNameBox.Text, Login = LoginBox.Text, Password = PasswordBox.Text, GroupId = groups.FirstOrDefault(g => g.GroupName == selectedName).GroupId };
                            answer = client.Registrate(student);
                        }
                        else
                        {
                            MessageBox.Show("Error in password or confirm password!!!");
                            PasswordBlock.Foreground = Brushes.Red;
                            ConfirmPasswordBlock.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error in password or confirm password!!!");
                        PasswordBlock.Foreground = Brushes.Red;
                        ConfirmPasswordBlock.Foreground = Brushes.Red;
                    }
                }
                else
                {
                    MessageBox.Show("Fill all the gaps!!!");
                }
            }
            else 
            {
                if (NameBox.Text != "" && LoginBox.Text != "" && PasswordBox.Text != "" && ConfirmPasswordBox.Text != "")
                {
                    if (PasswordBox.Text.Length > 4)
                    {
                        if (PasswordBox.Text == ConfirmPasswordBox.Text)
                        {
                            admin = new DTOAdministrator { Name = NameBox.Text, Login = LoginBox.Text, Password = PasswordBox.Text };
                            answer = client.Registrate(admin);
                        }
                        else
                        {
                            MessageBox.Show("Error in password or confirm password!!!");
                            PasswordBlock.Foreground = Brushes.Red;
                            ConfirmPasswordBlock.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error in password or confirm password!!!");
                        PasswordBlock.Foreground = Brushes.Red;
                        ConfirmPasswordBlock.Foreground = Brushes.Red;
                    }
                }
                else
                {
                    MessageBox.Show("Fill all the gaps!!!");
                }
            }
            if (answer == "successfully")
            {
                Close();
            }
            else if (answer == "login already exists")
            {
                MessageBox.Show("Already exist user with this login!!!");
                LoginBox.Text = "";
            }
            else if (answer == "password already exists")
            {
                MessageBox.Show("Already exist user with this password!!!");
                PasswordBox.Text = "";
            }
            else
            {
                MessageBox.Show("Something wrong!!!");
                client.Disconnect();
                Close();
            }
        }
    }
}
