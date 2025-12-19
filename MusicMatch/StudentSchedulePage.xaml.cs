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
    public partial class StudentSchedulePage : Page
    {
        private LessonDB lessonDB;
        private TeacherDB teacherDB;

        public StudentSchedulePage()
        {
            InitializeComponent();
            lessonDB = new LessonDB();
            teacherDB = new TeacherDB();
            LoadSchedule();
        }

        private void LoadSchedule()
        {
            if (MainWindow.LoggedInUser is Student student)
            {
                var bookedLessons = lessonDB.GetStudentBookedSessions(student.Id);
                
                if (bookedLessons.Count > 0)
                {
                    lstSchedule.ItemsSource = bookedLessons;
                    lstSchedule.Visibility = Visibility.Visible;
                    pnlEmpty.Visibility = Visibility.Collapsed;
                }
                else
                {
                    lstSchedule.Visibility = Visibility.Collapsed;
                    pnlEmpty.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
