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
    public partial class BookLessonPage : Page
    {
        private LessonDB lessonDB;
        private int teacherId;
        private List<Lesson> allFreeSlots;

        public BookLessonPage(int teacherId)
        {
            InitializeComponent();
            this.teacherId = teacherId;
            lessonDB = new LessonDB();
            
            calDate.DisplayDateStart = DateTime.Today;
            LoadSlots();
        }

        private void LoadSlots()
        {
            // Now we get ALL slots to show booked ones too
            allFreeSlots = lessonDB.GetAllSlots(teacherId); // Renamed variable kept for minimal code churn, but it now holds all slots
            
            // Group by date for the sidebar (Availability)
            // We only want to show dates that have AT LEAST ONE free slot or show "Full"
            // For simplicity, let's show all dates that have slots, but count only free ones
            var availability = allFreeSlots
                .GroupBy(l => l.LessonDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count(s => !s.IsBooked) })
                .Where(x => x.Count > 0) // Only show dates with free slots in Quick Select? Or show all? User said "Available Dates Obvious", so let's stick to free count > 0
                .OrderBy(x => x.Date)
                .ToList();

            lstAvailableDates.ItemsSource = availability;

            // Load My Sessions
            if (MainWindow.LoggedInUser is Student student)
            {
                var mySessions = lessonDB.GetStudentBookedSessions(student.Id);
                lstMySessions.ItemsSource = mySessions;
            }
        }

        private void lstAvailableDates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstAvailableDates.SelectedItem != null)
            {
                // Reflection to get properties from anonymous type
                dynamic selected = lstAvailableDates.SelectedItem;
                calDate.SelectedDate = selected.Date;
                calDate.DisplayDate = selected.Date; // Move calendar view
            }
        }

        private void calDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (calDate.SelectedDate.HasValue)
            {
                DateTime selectedDate = calDate.SelectedDate.Value;
                txtSelectedDate.Text = $"Available slots for {selectedDate:D}";

                var slotsForDate = allFreeSlots.Where(l => l.LessonDate.Date == selectedDate.Date).ToList();
                lstSlots.ItemsSource = slotsForDate;

                if (slotsForDate.Count == 0)
                {
                    lstSlots.Visibility = Visibility.Collapsed;
                    txtNoSlots.Visibility = Visibility.Visible;
                }
                else
                {
                     lstSlots.Visibility = Visibility.Visible;
                     txtNoSlots.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void btnBook_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.LoggedInUser == null || !(MainWindow.LoggedInUser is Student))
            {
                MessageBox.Show("Only students can book lessons.");
                return;
            }

            Button btn = sender as Button;
            int lessonId = (int)btn.Tag;
            Lesson lessonToBook = allFreeSlots.FirstOrDefault(l => l.Id == lessonId);

            if (lessonToBook != null)
            {
                try
                {
                    lessonDB.BookLesson(lessonToBook, MainWindow.LoggedInUser.Id);
                    lessonDB.SaveChanges(); // Persist changes!
                    
                    // NOTIFY TEACHER
                    try {
                        NotificationDB noteDB = new NotificationDB();
                        Notification note = new Notification() {
                            UserId = lessonToBook.TeacherId,
                            Message = $"New Booking: {MainWindow.LoggedInUser.FirstName} booked a lesson on {lessonToBook.LessonDate:d} at {lessonToBook.StartTime}",
                            IsRead = false,
                            CreatedAt = DateTime.Now
                        };
                        noteDB.Insert(note);
                        noteDB.SaveChanges();
                    } catch (Exception ex) {
                        // Show error to user so they know the notification failed
                        MessageBox.Show($"Lesson booked, but notification failed: {ex.Message}\n\nMake sure you created the tblNotifications table in your database.", 
                            "Notification Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        System.Diagnostics.Debug.WriteLine("Failed to notify teacher: " + ex.Message);
                    }

                    MessageBox.Show("Lesson booked successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    // Refresh
                    LoadSlots();
                    calDate_SelectedDatesChanged(null, null); // Re-filter
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error booking lesson: {ex.Message}");
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new StudentHomePage());
        }
    }
}
