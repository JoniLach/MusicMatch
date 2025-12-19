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
    /// <summary>
    /// Interaction logic for StudentHomePage.xaml
    /// </summary>
    public partial class StudentHomePage : Page
    {
        private TeacherList allTeachers;

        public StudentHomePage()
        {
            InitializeComponent();
            InitializeComponent();
            LoadTeachers();
            DisplayUserInfo();
            LoadUpcoming();
        }

        private void LoadUpcoming()
        {
             if (MainWindow.LoggedInUser == null) return;
             try
             {
                 LessonDB db = new LessonDB();
                 
                 // Load Upcoming
                 var upcoming = db.GetUpcomingLessons(MainWindow.LoggedInUser.Id, false);
                 if (upcoming.Count > 0)
                 {
                     lstUpcoming.ItemsSource = upcoming;
                     bdUpcoming.Visibility = Visibility.Visible;
                 }
                 else
                 {
                     bdUpcoming.Visibility = Visibility.Collapsed;
                 }

                 // Load History
                 var history = db.GetPastLessons(MainWindow.LoggedInUser.Id, false);
                 if (history.Count > 0)
                 {
                     lstHistory.ItemsSource = history;
                     bdHistory.Visibility = Visibility.Visible;
                 }
                 else
                 {
                     bdHistory.Visibility = Visibility.Collapsed;
                 }
             }
             catch { }
        }

        private void btnRate_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Lesson lesson)
            {
                var dialog = new RateUserDialog(lesson.TeacherName);
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        TeacherDB teacherDB = new TeacherDB();
                        teacherDB.AddRating(lesson.TeacherId, dialog.SelectedRating);
                        MessageBox.Show("Rating submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        // Reload teachers to show updated rating
                        LoadTeachers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error submitting rating: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void DisplayUserInfo()
        {
            if (MainWindow.LoggedInUser != null)
            {
                txtWelcome.Text = $"Welcome, {MainWindow.LoggedInUser.FirstName}";
            }
        }

        private void LoadTeachers()
        {
            try
            {
                TeacherDB db = new TeacherDB();
                allTeachers = db.SelectAll();
                lvTeachers.ItemsSource = allTeachers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load teachers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (allTeachers == null) return;

            string query = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(query))
            {
                lvTeachers.ItemsSource = allTeachers;
            }
            else
            {
                var filtered = allTeachers.Where(t => 
                    t.FirstName.ToLower().Contains(query) || 
                    t.LastName.ToLower().Contains(query) ||
                    t.City.ToLower().Contains(query)
                ).ToList();
                lvTeachers.ItemsSource = filtered;
            }
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoggedInUser = null;
            NavigationService?.Navigate(new Login());
        }

        private void lvTeachers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lvTeachers.SelectedItem is Teacher selectedTeacher)
            {
                NavigationService?.Navigate(new TeacherProfilePage(selectedTeacher));
            }
        }
    }
}
