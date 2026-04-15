using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class LessonDB : BaseDB
    {
        public LessonDB() : base()
        {
        }

        protected override BaseEntity NewEntity()
        {
            return new Lesson();
        }

        protected override void CreateModel(BaseEntity entity)
        {
            Lesson lesson = entity as Lesson;
            lesson.Id = (int)this.reader["id"];
            lesson.TeacherId = (int)this.reader["TeacherId"];

            if(this.reader["StudentId"] != DBNull.Value)
                lesson.StudentId = (int)this.reader["StudentId"];

            lesson.LessonDate = (DateTime)this.reader["LessonDate"];
            lesson.StartTime = this.reader["StartTime"].ToString();
            lesson.Duration = (int)this.reader["Duration"];

            if (this.reader["StudentRating"] != DBNull.Value)
                lesson.StudentRating = (int)this.reader["StudentRating"];
            if (this.reader["TeacherRating"] != DBNull.Value)
                lesson.TeacherRating = (int)this.reader["TeacherRating"];

            // Present in JOIN queries (GetUpcomingLessons, GetPastLessons, GetStudentBookedSessions)
            try { lesson.TeacherName = this.reader["TeacherName"]?.ToString(); } catch (IndexOutOfRangeException) { }
            try { lesson.StudentName = this.reader["StudentName"]?.ToString(); } catch (IndexOutOfRangeException) { }
        }

        public override string CreateInsertSQL(BaseEntity entity)
        {
            Lesson lesson = entity as Lesson;
            string dateStr = lesson.LessonDate.ToString("MM/dd/yyyy");
            string studentVal = lesson.StudentId == 0 ? "NULL" : lesson.StudentId.ToString();
            string studentRating = lesson.StudentRating == 0 ? "NULL" : lesson.StudentRating.ToString();
            string teacherRating = lesson.TeacherRating == 0 ? "NULL" : lesson.TeacherRating.ToString();
            string sqlStr = $"INSERT INTO tblLessons (TeacherId, StudentId, LessonDate, StartTime, Duration, TeacherRating, StudentRating) VALUES ({lesson.TeacherId}, {studentVal}, #{dateStr}#, '{lesson.StartTime}', {lesson.Duration}, {teacherRating}, {studentRating})";
            return sqlStr;
        }

        public override string CreateUpdateSQL(BaseEntity entity)
        {
            Lesson lesson = entity as Lesson;
            string dateStr = lesson.LessonDate.ToString("MM/dd/yyyy");
            string studentVal = lesson.StudentId == 0 ? "NULL" : lesson.StudentId.ToString();
            string studentRating = lesson.StudentRating == 0 ? "NULL" : lesson.StudentRating.ToString();
            string teacherRating = lesson.TeacherRating == 0 ? "NULL" : lesson.TeacherRating.ToString();
            string sqlStr = $"UPDATE tblLessons SET TeacherId={lesson.TeacherId}, StudentId={studentVal}, LessonDate=#{dateStr}#, StartTime='{lesson.StartTime}', Duration={lesson.Duration}, StudentRating={studentRating}, TeacherRating={teacherRating} WHERE id={lesson.Id}";
            return sqlStr;
        }

        public override string CreateDeleteSQL(BaseEntity entity)
        {
             Lesson lesson = entity as Lesson;
             return $"DELETE FROM tblLessons WHERE id={lesson.Id}";
        }
        
        public LessonList GetTeacherSchedule(int teacherId)
        {
            this.command.CommandText = $"SELECT * FROM tblLessons WHERE TeacherId = {teacherId} ORDER BY LessonDate, StartTime";
            return new LessonList(base.Select());
        }

        public LessonList GetFreeSlots(int teacherId)
        {
             this.command.CommandText = $"SELECT * FROM tblLessons WHERE TeacherId = {teacherId} AND (StudentId IS NULL OR StudentId = 0) ORDER BY LessonDate, StartTime";
            return new LessonList(base.Select());
        }

        public LessonList GetAllSlots(int teacherId)
        {
             // Returns ALL slots (booked or free) for the student view to see "red" ones
             this.command.CommandText = $"SELECT * FROM tblLessons WHERE TeacherId = {teacherId} ORDER BY LessonDate, StartTime";
            return new LessonList(base.Select());
        }

        public LessonList GetUpcomingLessons(int userId, bool isTeacher)
        {
            // Access expects MM/dd/yyyy for date literals
            string currentDate = DateTime.Now.ToString("MM/dd/yyyy");
            string currentTime = DateTime.Now.ToString("HH:mm");

            if (isTeacher)
            {
                // For teachers: get future lessons that are booked (StudentId not null and not 0)
                this.command.CommandText = $@"
                    SELECT tblLessons.*, 
                           TeacherUser.FirstName AS TeacherName, 
                           StudentUser.FirstName AS StudentName
                    FROM ((tblLessons 
                    LEFT JOIN tblUsers AS TeacherUser ON tblLessons.TeacherId = TeacherUser.id)
                    LEFT JOIN tblUsers AS StudentUser ON tblLessons.StudentId = StudentUser.id)
                    WHERE tblLessons.TeacherId = {userId} 
                      AND (tblLessons.StudentId IS NOT NULL AND tblLessons.StudentId <> 0)
                      AND ((tblLessons.LessonDate > #{currentDate}#) 
                           OR (tblLessons.LessonDate = #{currentDate}# AND tblLessons.StartTime >= '{currentTime}')) 
                    ORDER BY tblLessons.LessonDate, tblLessons.StartTime";
            }
            else
            {
                // For students: get their booked future lessons
                this.command.CommandText = $@"
                    SELECT tblLessons.*, 
                           TeacherUser.FirstName AS TeacherName, 
                           StudentUser.FirstName AS StudentName
                    FROM ((tblLessons 
                    LEFT JOIN tblUsers AS TeacherUser ON tblLessons.TeacherId = TeacherUser.id)
                    LEFT JOIN tblUsers AS StudentUser ON tblLessons.StudentId = StudentUser.id)
                    WHERE tblLessons.StudentId = {userId} 
                      AND ((tblLessons.LessonDate > #{currentDate}#) 
                           OR (tblLessons.LessonDate = #{currentDate}# AND tblLessons.StartTime >= '{currentTime}')) 
                    ORDER BY tblLessons.LessonDate, tblLessons.StartTime";
            }
            return new LessonList(base.Select());
        }

        public LessonList GetPastLessons(int userId, bool isTeacher)
        {
            string currentDate = DateTime.Now.ToString("MM/dd/yyyy");
            string currentTime = DateTime.Now.ToString("HH:mm");
            
            if (isTeacher)
            {
                 this.command.CommandText = $@"
                    SELECT tblLessons.*, 
                           TeacherUser.FirstName AS TeacherName, 
                           StudentUser.FirstName AS StudentName
                    FROM ((tblLessons 
                    LEFT JOIN tblUsers AS TeacherUser ON tblLessons.TeacherId = TeacherUser.id)
                    LEFT JOIN tblUsers AS StudentUser ON tblLessons.StudentId = StudentUser.id)
                    WHERE tblLessons.TeacherId = {userId} 
                      AND (tblLessons.StudentId IS NOT NULL AND tblLessons.StudentId <> 0) 
                      AND ((tblLessons.LessonDate < #{currentDate}#) OR (tblLessons.LessonDate = #{currentDate}# AND tblLessons.StartTime < '{currentTime}')) 
                    ORDER BY tblLessons.LessonDate DESC, tblLessons.StartTime DESC";
            }
            else
            {
                 this.command.CommandText = $@"
                    SELECT tblLessons.*, 
                           TeacherUser.FirstName AS TeacherName, 
                           StudentUser.FirstName AS StudentName
                    FROM ((tblLessons 
                    LEFT JOIN tblUsers AS TeacherUser ON tblLessons.TeacherId = TeacherUser.id)
                    LEFT JOIN tblUsers AS StudentUser ON tblLessons.StudentId = StudentUser.id)
                    WHERE tblLessons.StudentId = {userId} 
                      AND ((tblLessons.LessonDate < #{currentDate}#) OR (tblLessons.LessonDate = #{currentDate}# AND tblLessons.StartTime < '{currentTime}')) 
                    ORDER BY tblLessons.LessonDate DESC, tblLessons.StartTime DESC";
            }
            return new LessonList(base.Select());
        }

        public LessonList GetStudentBookedSessions(int studentId)
        {
            this.command.CommandText = $@"
                SELECT tblLessons.*,
                       TeacherUser.FirstName AS TeacherName,
                       StudentUser.FirstName AS StudentName
                FROM ((tblLessons
                LEFT JOIN tblUsers AS TeacherUser ON tblLessons.TeacherId = TeacherUser.id)
                LEFT JOIN tblUsers AS StudentUser ON tblLessons.StudentId = StudentUser.id)
                WHERE tblLessons.StudentId = {studentId}
                ORDER BY tblLessons.LessonDate, tblLessons.StartTime";
            return new LessonList(base.Select());
        }

        public void BookLesson(Lesson lesson, int studentId)
        {
            lesson.StudentId = studentId;
            this.Update(lesson);
        }

        public LessonList GetTeacherScheduleWithStudents(int teacherId)
        {
            this.command.CommandText = $@"
                SELECT tblLessons.*,
                       StudentUser.FirstName AS StudentName
                FROM (tblLessons
                LEFT JOIN tblUsers AS StudentUser ON tblLessons.StudentId = StudentUser.id)
                WHERE tblLessons.TeacherId = {teacherId}
                ORDER BY tblLessons.LessonDate, tblLessons.StartTime";
            return new LessonList(base.Select());
        }
    }
}
