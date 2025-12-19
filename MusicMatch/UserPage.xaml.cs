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
        private string selectedProfilePicturePath = null;

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

                // Profile picture
                if (!string.IsNullOrEmpty(currentUser.FirstName))
                {
                    txtProfileInitial.Text = currentUser.FirstName.Substring(0, 1).ToUpper();
                }
                else
                {
                    txtProfileInitial.Text = "U";
                }
                
                if (!string.IsNullOrEmpty(currentUser.ProfilePicture) && System.IO.File.Exists(currentUser.ProfilePicture))
                {

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(currentUser.ProfilePicture, UriKind.RelativeOrAbsolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    imgProfileBrush.ImageSource = bitmap;
                    imgProfilePreview.Visibility = Visibility.Visible;
                    txtProfileInitial.Visibility = Visibility.Collapsed;
                }

                if (currentUser is Teacher teacher)
                {
                    pnlTeacherSettings.Visibility = Visibility.Visible;
                    txtPrice.Text = teacher.Price.ToString();
                    txtJobs.Text = teacher.AmountOfJobs.ToString();
                }
            }
        }

        private void btnBrowseProfilePic_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            dlg.Title = "Select Profile Picture";

            if (dlg.ShowDialog() == true)
            {
                selectedProfilePicturePath = dlg.FileName;
                // Update preview
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedProfilePicturePath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    imgProfileBrush.ImageSource = bitmap;
                    imgProfilePreview.Visibility = Visibility.Visible;
                    txtProfileInitial.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

            // Update profile picture if changed
            if (!string.IsNullOrEmpty(selectedProfilePicturePath))
            {
                currentUser.ProfilePicture = selectedProfilePicturePath;
            }

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
                    NavigationService?.Navigate(new TeacherHomePage());
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
                NavigationService?.Navigate(new TeacherHomePage());
        }
    }
}
