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
using static System.Collections.Specialized.BitVector32;
using ViewModel;

namespace MusicMatch
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Page
    {
        public Signup()
        {
            InitializeComponent();
        }
        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text?.Trim();
            string password = txtPass.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //TeacherDB tdb = new TeacherDB();
            //bool created = tdb.CreateTeacherAccount(username, password);

            //if (created)
            //{
            //    MessageBox.Show("Account created. You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            //    NavigationService?.Navigate(new Login());
            //}
            //else
            //{
            //    MessageBox.Show("Failed to create account. The username may already exist or the database is inaccessible.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void SwitchToLogIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Login());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e) 
        {

        }
    }
}