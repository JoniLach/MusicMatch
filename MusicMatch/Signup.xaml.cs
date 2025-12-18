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
using Model;

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
            string email = txtEmail.Text?.Trim();
            string firstName = txtFirstName.Text?.Trim();
            string lastName = txtLastName.Text?.Trim();
            string city = txtCity.Text?.Trim();
            bool isTeacher = chkIsTeacher.IsChecked == true;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(city))
            {
                MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (isTeacher)
            {
                var teacher = new Teacher
                {
                    UserName = username,
                    Password = password,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    City = city
                };

                try
                {
                    TeacherDB db = new TeacherDB();
                    db.Insert(teacher);
                    db.SaveChanges(); // This updates teacher.Id
                    MainWindow.LoggedInUser = teacher; // Set globally
                    NavigationService?.Navigate(new InstrumentSelectionPage(teacher));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                var student = new Student
                {
                    UserName = username,
                    Password = password,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    City = city
                };

                try 
                {
                    StudentDB db = new StudentDB();
                    db.Insert(student);
                    db.SaveChanges(); // This updates student.Id
                    MainWindow.LoggedInUser = student; // Set globally
                    NavigationService?.Navigate(new InstrumentSelectionPage(student));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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