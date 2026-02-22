using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Lesson : BaseEntity
    {
        private int teacherId;
        private int studentId;
        private DateTime lessonDate;
        private string startTime;
        private int duration; // in minutes
        private int studentRating; 
        private int teacherRating; 

        public int TeacherId { get => teacherId; set => teacherId = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public DateTime LessonDate { get => lessonDate; set => lessonDate = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public int Duration { get => duration; set => duration = value; }
        public int StudentRating { get => studentRating; set => studentRating = value; }
        public int TeacherRating { get => teacherRating; set => teacherRating = value; }

        // Non-mapped properties for display
        public string TeacherName { get; set; }
        public string StudentName { get; set; }

        public bool IsBooked => StudentId != 0;

    }
}
