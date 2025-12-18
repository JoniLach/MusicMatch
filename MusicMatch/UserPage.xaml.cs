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
using Model;
using ViewModel;

namespace MusicMatch
{
    public partial class UserPage : Page
    {
        private User currentUser;

        public UserPage()
        {
            InitializeComponent();
            currentUser = MainWindow.LoggedInUser;
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (currentUser != null)
            {
                txtFirstName.Text = currentUser.FirstName;
                txtLastName.Text = currentUser.LastName;
                txtEmail.Text = currentUser.Email;
                txtCity.Text = currentUser.City;
                txtUsername.Text = currentUser.UserName;
                txtPassword.Password = currentUser.Password;

                if (currentUser is Teacher teacher)
                {
                    pnlTeacherSettings.Visibility = Visibility.Visible;
                    txtPrice.Text = teacher.Price.ToString();
                    txtJobs.Text = teacher.AmountOfJobs.ToString();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser == null) return;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || 
                string.IsNullOrWhiteSpace(txtLastName.Text) || 
                string.IsNullOrWhiteSpace(txtEmail.Text) || 
                string.IsNullOrWhiteSpace(txtCity.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Update user object
            currentUser.FirstName = txtFirstName.Text;
            currentUser.LastName = txtLastName.Text;
            currentUser.Email = txtEmail.Text;
            currentUser.City = txtCity.Text;

            // Only update password if changed (and not empty if that's a requirement, but here we allow keeping old one if logic allows, 
            // though the simple binding above sets it to current. If they clear it, we might want to check. 
            // For now, let's assume whatever is in the box is the desired password)
            if (!string.IsNullOrEmpty(txtPassword.Password))
            {
                currentUser.Password = txtPassword.Password;
            }

            try
            {
                if (currentUser is Teacher teacher)
                {
                    // Update specific teacher fields
                    if (int.TryParse(txtPrice.Text, out int price))
                    {
                        teacher.Price = price;
                    }
                    else
                    {
                        MessageBox.Show("Price must be a valid number.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    TeacherDB db = new TeacherDB();
                    db.Update(teacher);
                    db.SaveChanges();
                }
                else if (currentUser is Student student)
                {
                    StudentDB db = new StudentDB();
                    db.Update(student);
                    db.SaveChanges();
                }

                MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Navigate back
                if (currentUser is Student)
                    NavigationService?.Navigate(new StudentHomePage());
                else
                    NavigationService?.Navigate(new SearchPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating profile: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser is Student)
                NavigationService?.Navigate(new StudentHomePage());
            else
                NavigationService?.Navigate(new SearchPage());
        }
    }
}
