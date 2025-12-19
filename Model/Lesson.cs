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

        public int TeacherId { get => teacherId; set => teacherId = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public DateTime LessonDate { get => lessonDate; set => lessonDate = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public int Duration { get => duration; set => duration = value; }

        public bool IsBooked => StudentId != 0;
    }
}
