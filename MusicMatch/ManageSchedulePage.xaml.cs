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
    public partial class ManageSchedulePage : Page
    {
        private Teacher currentTeacher;
        private LessonDB lessonDB;

        public ManageSchedulePage()
        {
            InitializeComponent();
            currentTeacher = MainWindow.LoggedInUser as Teacher;
            lessonDB = new LessonDB();
            
            InitializeForm();
            LoadSchedule();
        }

        private void InitializeForm()
        {
            dpDate.DisplayDateStart = DateTime.Today;
            dpDate.SelectedDate = DateTime.Today;

            // Populate time slots (example: 08:00 to 20:00)
            for (int i = 8; i <= 20; i++)
            {
                cmbTime.Items.Add($"{i:D2}:00");
                cmbTime.Items.Add($"{i:D2}:30");
            }
            cmbTime.SelectedIndex = 0;
        }

        private void LoadSchedule()
        {
            if (currentTeacher != null)
            {
               LessonList lessons = lessonDB.GetTeacherSchedule(currentTeacher.Id);
               lstSchedule.ItemsSource = lessons;
            }
        }

        private void btnAddSlot_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.SelectedDate == null || cmbTime.SelectedItem == null)
            {
                MessageBox.Show("Please select date and time.");
                return;
            }

            if (!int.TryParse(txtDuration.Text, out int duration))
            {
                MessageBox.Show("Invalid duration.");
                return;
            }

            try
            {
                Lesson newLesson = new Lesson
                {
                    TeacherId = currentTeacher.Id,
                    LessonDate = dpDate.SelectedDate.Value,
                    StartTime = cmbTime.SelectedItem.ToString(),
                    Duration = duration,
                    StudentId = 0 // Available
                };

                lessonDB.Insert(newLesson);
                lessonDB.SaveChanges();

                MessageBox.Show("Slot added successfully!");
                LoadSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding slot: {ex.Message}");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new TeacherHomePage());
        }
    }
}
