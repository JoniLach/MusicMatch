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
using MusicMatch;
using Model;
using ViewModel;

namespace MusicMatch
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        // Handles login button click event
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Password;
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username and password.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                // Try teacher login first
                TeacherDB tdb = new TeacherDB();
                Teacher teacher = tdb.Login(username, password);

                if (teacher != null)
                {
                    MainWindow.LoggedInUser = teacher;
                    MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                    if (mainWindow != null)
                    {
                        mainWindow.MainFrame.Navigate(new TeacherHomePage());
                    }
                }
                else
                {
                    // If not a teacher, try student login
                    StudentDB sdb = new StudentDB(); // Re-declare sdb here as it's in a new scope
                    Student student = sdb.Login(username, password);
                    if (student != null)
                    {
                        MainWindow.LoggedInUser = student;
                        MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                        if (mainWindow != null)
                        {
                            mainWindow.MainFrame.Navigate(new StudentHomePage());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // Switch to the sign-up page
        private void SwitchToSignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Signup());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e) { }
    }
}