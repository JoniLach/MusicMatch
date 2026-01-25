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
            this.Loaded += (s, e) => LoadTeacherData();
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
                // Load instruments
                LoadInstruments();
                
                // Load Notifications
                LoadNotifications();

                // Load Upcoming
                LoadUpcomingSessions();
                
                // Check for session reminders
                CheckSessionReminders();
            }
        }

        private void Notification_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Notification notification)
            {
                try
                {
                    NotificationDB db = new NotificationDB();
                    notification.IsRead = true;
                    db.Update(notification);
                    db.SaveChanges();
                    
                    // Refresh notifications
                    LoadNotifications();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to mark notification as read: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CheckSessionReminders()
        {
            try
            {
                LessonDB lessonDB = new LessonDB();
                NotificationDB noteDB = new NotificationDB();
                
                // Get sessions starting within 30 minutes
                var upcomingSessions = lessonDB.GetUpcomingLessons(currentTeacher.Id, true);
                
                foreach (var lesson in upcomingSessions)
                {
                    // Parse the lesson time
                    if (DateTime.TryParse($"{lesson.LessonDate:yyyy-MM-dd} {lesson.StartTime}", out DateTime lessonDateTime))
                    {
                        TimeSpan timeUntilLesson = lessonDateTime - DateTime.Now;
                        
                        // If lesson is within 30 minutes and hasn't started yet
                        if (timeUntilLesson.TotalMinutes > 0 && timeUntilLesson.TotalMinutes <= 30)
                        {
                            // Check if we already created a reminder for this lesson
                            var existingReminders = noteDB.GetUnreadNotifications(currentTeacher.Id);
                            bool reminderExists = existingReminders.Any(n => n.Message.Contains($"lesson at {lesson.StartTime}"));
                            
                            if (!reminderExists)
                            {
                                Notification reminder = new Notification()
                                {
                                    UserId = currentTeacher.Id,
                                    Message = $"⏰ Reminder: You have a lesson at {lesson.StartTime} in {(int)timeUntilLesson.TotalMinutes} minutes!",
                                    IsRead = false,
                                    CreatedAt = DateTime.Now
                                };
                                noteDB.Insert(reminder);
                                noteDB.SaveChanges();
                            }
                        }
                    }
                }
                
                // Reload notifications if any reminders were added
                LoadNotifications();
            }
            catch { }
        }

        private void LoadNotifications()
        {
            NotificationDB db = new NotificationDB();
            var list = db.GetUnreadNotifications(currentTeacher.Id);
                
            if (list.Count > 0)
            {
                lstNotifications.ItemsSource = list;
                txtNoNotifications.Visibility = Visibility.Collapsed;
                bdBadge.Visibility = Visibility.Visible;
                txtBadgeCount.Text = list.Count.ToString();
            }
            else
            {
                lstNotifications.ItemsSource = null;
                txtNoNotifications.Visibility = Visibility.Visible;
                bdBadge.Visibility = Visibility.Collapsed;
            }
            
        }

        private void LoadUpcomingSessions()
        {
            try
            {
                LessonDB db = new LessonDB();
                
                // Upcoming
                var upcoming = db.GetUpcomingLessons(currentTeacher.Id, true);
                 if (upcoming.Count > 0)
                {
                    lstUpcoming.ItemsSource = upcoming;
                    txtNoUpcoming.Visibility = Visibility.Collapsed;
                }
                else
                {
                    lstUpcoming.ItemsSource = null;
                    txtNoUpcoming.Visibility = Visibility.Visible;
                }

                // History
                var history = db.GetPastLessons(currentTeacher.Id, true);
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
                var dialog = new RateUserDialog(lesson.StudentName);
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        StudentDB studentDB = new StudentDB();
                        studentDB.AddRating(lesson.StudentId, dialog.SelectedRating);
                        MessageBox.Show("Student rating submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error submitting rating: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
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
            NavigationService?.Navigate(new TeacherProfilePage(currentTeacher));
        }

        private void btnManageSchedule_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ManageSchedulePage());
        }

        private void btnManageInstruments_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new InstrumentSelectionPage(currentTeacher, true));
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoggedInUser = null;
            NavigationService?.Navigate(new Login());
        }
    }
}
