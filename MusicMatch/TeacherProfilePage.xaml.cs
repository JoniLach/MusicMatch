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

        public TeacherProfilePage(Teacher selectedTeacher)
        {
            InitializeComponent();
            teacher = selectedTeacher;
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

                // Initial for avatar
                txtInitial.Text = teacher.FirstName.Substring(0, 1).ToUpper();

                // Description
                if (!string.IsNullOrEmpty(teacher.Description))
                {
                    txtDescription.Text = teacher.Description;
                }
                else
                {
                    txtDescription.Text = $"{teacher.FirstName} is an experienced music teacher ready to help you achieve your musical goals.";
                }

                // Profile Picture (if available)
                if (!string.IsNullOrEmpty(teacher.ProfilePicture) && System.IO.File.Exists(teacher.ProfilePicture))
                {
                    try
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(teacher.ProfilePicture, UriKind.RelativeOrAbsolute);
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void btnContact_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Contact feature coming soon!\n\nYou can reach {teacher.FirstName} at:\n{teacher.Email}", 
                "Contact Teacher", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
