using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Model;
using ViewModel;

namespace MusicMatch
{
    public partial class ManageSchedulePage : Page
    {
        private Teacher currentTeacher;
        private LessonDB lessonDB;
        private LessonList currentSchedule;
        private int? _editingLessonId = null; // null = Add mode, non-null = Edit mode
        private StudentList _allStudents;

        public ManageSchedulePage()
        {
            InitializeComponent();
            currentTeacher = MainWindow.LoggedInUser as Teacher;
            lessonDB = new LessonDB();

            InitializeForm();
            LoadSchedule();
            LoadStudents();
        }

        private void InitializeForm()
        {
            dpDate.DisplayDateStart = DateTime.Today;
            dpDate.SelectedDate = DateTime.Today;
            dpDirectDate.DisplayDateStart = DateTime.Today;
            dpDirectDate.SelectedDate = DateTime.Today;

            for (int i = 8; i <= 20; i++)
            {
                cmbTime.Items.Add($"{i:D2}:00");
                cmbTime.Items.Add($"{i:D2}:30");
                cmbDirectTime.Items.Add($"{i:D2}:00");
                cmbDirectTime.Items.Add($"{i:D2}:30");
            }
            cmbTime.SelectedIndex = 0;
            cmbDirectTime.SelectedIndex = 0;
        }

        private void LoadSchedule()
        {
            if (currentTeacher != null)
            {
                currentSchedule = lessonDB.GetTeacherScheduleWithStudents(currentTeacher.Id);
                lstSchedule.ItemsSource = currentSchedule;
            }
        }

        private void LoadStudents()
        {
            try
            {
                StudentDB studentDB = new StudentDB();
                _allStudents = studentDB.SelectAll();
                cmbStudent.ItemsSource = _allStudents;
                if (_allStudents.Count > 0)
                    cmbStudent.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load students: {ex.Message}", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // ── Tab 1: Add / Update Slot ─────────────────────────────────────────

        private void btnAddSlot_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.SelectedDate == null || cmbTime.SelectedItem == null)
            {
                MessageBox.Show("Please select a date and time.", "Missing Fields", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtDuration.Text, out int duration) || duration <= 0)
            {
                MessageBox.Show("Please enter a valid duration (positive number).", "Invalid Duration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime selectedDate = dpDate.SelectedDate.Value;
            string selectedTime = cmbTime.SelectedItem.ToString();

            try
            {
                if (_editingLessonId.HasValue)
                {
                    // Update existing slot
                    Lesson toEdit = currentSchedule?.FirstOrDefault(l => l.Id == _editingLessonId.Value);
                    if (toEdit != null)
                    {
                        toEdit.LessonDate = selectedDate;
                        toEdit.StartTime = selectedTime;
                        toEdit.Duration = duration;

                        lessonDB.Update(toEdit);
                        lessonDB.SaveChanges();
                        MessageBox.Show("Slot updated successfully!", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    ExitEditMode();
                }
                else
                {
                    // Add new slot — prevent duplicates
                    bool isDuplicate = currentSchedule != null && currentSchedule.Any(l =>
                        l.LessonDate.Date == selectedDate.Date && l.StartTime == selectedTime);

                    if (isDuplicate)
                    {
                        MessageBox.Show("A slot already exists at this date and time.", "Duplicate Slot", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    Lesson newLesson = new Lesson
                    {
                        TeacherId = currentTeacher.Id,
                        LessonDate = selectedDate,
                        StartTime = selectedTime,
                        Duration = duration,
                        StudentId = 0
                    };

                    lessonDB.Insert(newLesson);
                    lessonDB.SaveChanges();
                    MessageBox.Show("Slot added successfully!", "Added", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                LoadSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving slot: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditSlot_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Lesson lesson)
            {
                if (lesson.IsBooked)
                    return; // should not happen; button is disabled for booked slots

                _editingLessonId = lesson.Id;
                txtFormTitle.Text = "Edit Slot";
                btnAddSlot.Content = "Update Slot";
                btnCancelEdit.Visibility = Visibility.Visible;

                // Pre-fill form
                dpDate.SelectedDate = lesson.LessonDate;
                int timeIndex = cmbTime.Items.IndexOf(lesson.StartTime);
                if (timeIndex >= 0) cmbTime.SelectedIndex = timeIndex;
                txtDuration.Text = lesson.Duration.ToString();

                // Switch to Tab 1 and scroll to form
                tabMain.SelectedIndex = 0;
            }
        }

        private void btnCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            ExitEditMode();
        }

        private void ExitEditMode()
        {
            _editingLessonId = null;
            txtFormTitle.Text = "Add a New Slot";
            btnAddSlot.Content = "Add Slot";
            btnCancelEdit.Visibility = Visibility.Collapsed;
            dpDate.SelectedDate = DateTime.Today;
            cmbTime.SelectedIndex = 0;
            txtDuration.Text = "60";
        }

        private void btnDeleteSlot_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Lesson lesson)
            {
                string message = lesson.IsBooked
                    ? $"This slot is booked by {lesson.StudentName ?? "a student"}. Deleting it will cancel their booking. Continue?"
                    : "Delete this available slot?";

                if (MessageBox.Show(message, "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;

                try
                {
                    lessonDB.Delete(lesson);
                    lessonDB.SaveChanges();

                    // Clear edit mode if we deleted the lesson being edited
                    if (_editingLessonId == lesson.Id)
                        ExitEditMode();

                    LoadSchedule();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting slot: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnRate_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Lesson lesson)
            {
                var dialog = new RateUserDialog(lesson.StudentName ?? "the student");
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        StudentDB studentDB = new StudentDB();
                        studentDB.AddRating(lesson.StudentId, dialog.SelectedRating);

                        // Mark lesson as rated so the Rate button disappears after refresh
                        lesson.TeacherRating = dialog.SelectedRating;
                        lessonDB.Update(lesson);
                        lessonDB.SaveChanges();

                        MessageBox.Show("Rating submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadSchedule();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error submitting rating: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        // ── Tab 2: Schedule Directly ─────────────────────────────────────────

        private void btnScheduleDirect_Click(object sender, RoutedEventArgs e)
        {
            if (cmbStudent.SelectedItem == null)
            {
                MessageBox.Show("Please select a student.", "Missing Student", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dpDirectDate.SelectedDate == null || cmbDirectTime.SelectedItem == null)
            {
                MessageBox.Show("Please select a date and time.", "Missing Fields", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtDirectDuration.Text, out int duration) || duration <= 0)
            {
                MessageBox.Show("Please enter a valid duration (positive number).", "Invalid Duration", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Student selectedStudent = cmbStudent.SelectedItem as Student;
            DateTime selectedDate = dpDirectDate.SelectedDate.Value;
            string selectedTime = cmbDirectTime.SelectedItem.ToString();

            // Check for conflict with existing slots
            bool hasConflict = currentSchedule != null && currentSchedule.Any(l =>
                l.LessonDate.Date == selectedDate.Date && l.StartTime == selectedTime);

            if (hasConflict)
            {
                MessageBox.Show("You already have a slot at this date and time.", "Conflict", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Lesson newLesson = new Lesson
                {
                    TeacherId = currentTeacher.Id,
                    StudentId = selectedStudent.Id,
                    LessonDate = selectedDate,
                    StartTime = selectedTime,
                    Duration = duration
                };

                lessonDB.Insert(newLesson);
                lessonDB.SaveChanges();

                // Notify the student
                try
                {
                    NotificationDB noteDB = new NotificationDB();
                    Notification note = new Notification()
                    {
                        UserId = selectedStudent.Id,
                        Message = $"New Lesson Scheduled: {currentTeacher.FirstName} scheduled a lesson with you on {selectedDate:d} at {selectedTime}",
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    };
                    noteDB.Insert(note);
                    noteDB.SaveChanges();
                }
                catch { /* notification failure should not block the main action */ }

                MessageBox.Show($"Lesson scheduled with {selectedStudent.FirstName} on {selectedDate:d} at {selectedTime}!",
                    "Scheduled", MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh list and switch to My Slots tab
                LoadSchedule();
                tabMain.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error scheduling lesson: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ── Navigation ────────────────────────────────────────────────────────

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TeacherHomePage());
        }
    }
}
