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
    public partial class TeacherProfilePage : Page
    {
        private Teacher teacher;
        private bool isEditMode = false;
        private bool isOwnProfile = false;
        private string path;

        public TeacherProfilePage(Teacher selectedTeacher)
        {
            InitializeComponent();
            teacher = selectedTeacher;
            
            // Check if viewing own profile
            if (MainWindow.LoggedInUser is Teacher loggedTeacher && loggedTeacher.Id == teacher.Id)
            {
                isOwnProfile = true;
                btnChangePic.Visibility = Visibility.Visible;
            }
            else
            {
                // Hide edit button if not own profile
                btnEditDescription.Visibility = Visibility.Collapsed;
                btnChangePic.Visibility = Visibility.Collapsed;
            }
            
            LoadTeacherProfile();
        }

        private void LoadTeacherProfile()
        {
            if (teacher != null)
            {
                // Basic Info
                txtName.Text = $"{teacher.FirstName} {teacher.LastName}";
                txtCity.Text = teacher.City;
                txtEmail.Text = teacher.Email;
                txtRating.Text = teacher.Rating.ToString("0.0");
                txtPrice.Text = $"{teacher.Price} ILS/hr";
                txtJobs.Text = $"{teacher.AmountOfJobs} jobs completed";
                path = ImageHelper.GetImagePath(teacher.ProfilePicture);

                // Initial for avatar
                if (!string.IsNullOrEmpty(teacher.FirstName))
                {
                    txtInitial.Text = teacher.FirstName.Substring(0, 1).ToUpper();
                }
                else
                {
                    txtInitial.Text = "U";
                }

                // Description
                if (!string.IsNullOrEmpty(teacher.Description))
                {
                    txtDescription.Text = teacher.Description;
                    txtDescriptionEdit.Text = teacher.Description;
                }
                else
                {
                    txtDescription.Text = $"{teacher.FirstName} is an experienced music teacher ready to help you achieve your musical goals.";
                    txtDescriptionEdit.Text = "";
                }

                // Profile Picture (if available)
                if (!string.IsNullOrEmpty(teacher.ProfilePicture))
                {
                    try
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(path);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        imgProfileBrush.ImageSource = bitmap;
                        imgProfile.Visibility = Visibility.Visible;
                        txtInitial.Visibility = Visibility.Collapsed;
                    }
                    catch
                    {
                        // If image fails to load, keep the initial visible
                    }
                }

                // Load Instruments
                LoadInstruments();
            }
        }

        private void LoadInstruments()
        {
            try
            {
                InstrumentDB db = new InstrumentDB();
                InstrumentList instruments = db.GetUserInstruments(teacher.Id);

                if (instruments.Count > 0)
                {
                    icInstruments.ItemsSource = instruments;
                    txtNoInstruments.Visibility = Visibility.Collapsed;
                }
                else
                {
                    txtNoInstruments.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading instruments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditDescription_Click(object sender, RoutedEventArgs e)
        {
            if (!isOwnProfile) return;

            if (!isEditMode)
            {
                // Switch to edit mode
                isEditMode = true;
                txtDescription.Visibility = Visibility.Collapsed;
                pnlEditDescription.Visibility = Visibility.Visible;
                txtEditLabel.Text = "Save";
                txtEditLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 165, 0)); // Orange
            }
            else
            {
                // Save changes
                try
                {
                    teacher.Description = txtDescriptionEdit.Text.Trim();
                    
                    TeacherDB db = new TeacherDB();
                    db.Update(teacher);
                    db.SaveChanges();

                    // Switch back to view mode
                    isEditMode = false;
                    txtDescription.Text = string.IsNullOrEmpty(teacher.Description) 
                        ? $"{teacher.FirstName} is an experienced music teacher ready to help you achieve your musical goals." 
                        : teacher.Description;
                    txtDescription.Visibility = Visibility.Visible;
                    pnlEditDescription.Visibility = Visibility.Collapsed;
                    txtEditLabel.Text = "Edit";
                    txtEditLabel.Foreground = new SolidColorBrush(Color.FromRgb(29, 185, 84)); // Green

                    MessageBox.Show("Description updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save description: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void txtDescriptionEdit_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtCharCount.Text = $"{txtDescriptionEdit.Text.Length}/500 characters";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void btnContact_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Contact feature coming soon!\n\nYou can reach {teacher.FirstName} at:\n{teacher.Email}", 
                "Contact Teacher", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnBookLesson_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new BookLessonPage(teacher.Id));
        }

        private void btnChangePic_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            dlg.Title = "Select New Profile Picture";

            if (dlg.ShowDialog() == true)
            {
                string savedPath = ImageHelper.SaveImageLocally(dlg.FileName);
                if (savedPath != null)
                {
                    try
                    {
                        teacher.ProfilePicture = savedPath;
                        
                        TeacherDB db = new TeacherDB();
                        db.Update(teacher);
                        db.SaveChanges(); // Persist to DB using new UserDB logic

                        LoadTeacherProfile(); // Refresh UI

                        MessageBox.Show("Profile picture updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to update picture: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}
