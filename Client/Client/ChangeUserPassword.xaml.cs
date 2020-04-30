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
    /// Interaction logic for ChangeUserLogin.xaml
    /// </summary>
    public partial class ChangeUserPassword : Window
    {
        ClientObject client;
        string password;
        public ChangeUserPassword(ClientObject cl, string passw)
        {
            InitializeComponent();
            client = cl;
            password = passw;
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            //if (oldPassword.Text == password)
            //{
            //    if (newPassword.Text.Length > 4)
            //    {
            //        if (newPassword.Text == confirmNewPassword.Text)
            //        {
            //            string answer = client.EditAdminAccount("password", newPassword.Text);
            //            if (answer == "succesfully")
            //            {
            //                this.Close();
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show("Confirm password is bad!");
            //            newPassword.Foreground = Brushes.Red;
            //            confirmNewPassword.Foreground = Brushes.Red;
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Password is short, minimum 5 characters");
            //        newPassword.Foreground = Brushes.Red;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Bad Password");
            //    oldPassword.Foreground = Brushes.Red;
            //}
        }
    }
}
