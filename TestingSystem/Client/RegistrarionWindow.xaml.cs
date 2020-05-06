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
using DTO;

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
                this.SurNameLabel.Visibility = Visibility.Collapsed;
                this.GroupNameComboBox.Visibility = Visibility.Collapsed;
                this.GroupLabel.Visibility = Visibility.Collapsed;
                this.MinHeight = 490;
            }
            else
            {
                isAdmin = false;
                this.ChangeRegistrationButton.Content = "New Admin";
                this.SurNameBox.Visibility = Visibility.Visible;
                this.SurNameLabel.Visibility = Visibility.Visible;
                this.GroupNameComboBox.Visibility = Visibility.Visible;
                this.GroupLabel.Visibility = Visibility.Visible;
                this.MinHeight = 590;
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
                if (NameBox.Text != "" && SurNameBox.Text != "" && LoginBox.Text != "" && PasswordBox.Password != "" && ConfirmPasswordBox.Password != "" && selectedName != null)
                {
                    if (PasswordBox.Password.Length > 4)
                    {
                        if (PasswordBox.Password == ConfirmPasswordBox.Password)
                        {
                            student = new DTOStudent { Name = NameBox.Text, SurName = SurNameBox.Text, Login = LoginBox.Text, Password = PasswordBox.Password, GroupId = groups.FirstOrDefault(g => g.GroupName == selectedName).GroupId };
                            answer = client.Registrate(student);
                            if (answer == "successfully")
                            {
                                client.LogOut();
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
                                PasswordBox.Password = "";
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
                            MessageBox.Show("Error in password or confirm password!!!");
                            PasswordBox.Foreground = Brushes.Red;
                            ConfirmPasswordBox.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error in password or confirm password!!!");
                        PasswordBox.Foreground = Brushes.Red;
                        ConfirmPasswordBox.Foreground = Brushes.Red;
                    }
                }
                else
                {
                    MessageBox.Show("Fill all the gaps!!!");
                }
            }
            else
            {
                if (NameBox.Text != "" && LoginBox.Text != "" && PasswordBox.Password != "" && ConfirmPasswordBox.Password != "")
                {
                    if (PasswordBox.Password.Length > 4)
                    {
                        if (PasswordBox.Password == ConfirmPasswordBox.Password)
                        {
                            admin = new DTOAdministrator { Name = NameBox.Text, Login = LoginBox.Text, Password = PasswordBox.Password };
                            answer = client.Registrate(admin);
                            if (answer == "successfully")
                            {
                                client.LogOut();
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
                                PasswordBox.Password = "";
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
                            MessageBox.Show("Error in password or confirm password!!!");
                            PasswordBox.Foreground = Brushes.Red;
                            ConfirmPasswordBox.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error in password or confirm password!!!");
                        PasswordBox.Foreground = Brushes.Red;
                        ConfirmPasswordBox.Foreground = Brushes.Red;
                    }
                }
                else
                {
                    MessageBox.Show("Fill all the gaps!!!");
                }
            }

        }
    }
}
