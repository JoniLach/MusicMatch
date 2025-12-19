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
    public partial class TeacherHomePage : Page
    {
        private Teacher currentTeacher;

        public TeacherHomePage()
        {
            InitializeComponent();
            currentTeacher = MainWindow.LoggedInUser as Teacher;
            LoadTeacherData();
        }

        private void LoadTeacherData()
        {
            if (currentTeacher != null)
            {
                // Welcome message
                txtWelcome.Text = $"Welcome back, {currentTeacher.FirstName}!";

                // Stats
                txtTotalJobs.Text = currentTeacher.AmountOfJobs.ToString();
                txtRating.Text = currentTeacher.Rating.ToString("0.0");
                txtPrice.Text = currentTeacher.Price.ToString();

                // Load instruments
                LoadInstruments();
            }
        }

        private void LoadInstruments()
        {
            try
            {
                InstrumentDB db = new InstrumentDB();
                InstrumentList instruments = db.GetUserInstruments(currentTeacher.Id);
                
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

        private void btnViewProfile_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to a profile view page (could be created later)
            MessageBox.Show("Profile view coming soon!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnEditProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new UserPage());
        }

        private void btnManageInstruments_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InstrumentSelectionDialog(currentTeacher);
            if (dialog.ShowDialog() == true)
            {
                LoadInstruments(); // Refresh the list
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoggedInUser = null;
            NavigationService?.Navigate(new Login());
        }
    }
}
