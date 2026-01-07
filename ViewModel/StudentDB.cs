using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class StudentDB : UserDB
    {
        public override string CreateDeleteSQL(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override string CreateInsertSQL(BaseEntity entity)
        {
            Student student = entity as Student;
            string sqlStr = $@"INSERT INTO tblStudent 
                                (id, StudentRating)
                                VALUES 
                                ({student.Id}, {student.Rating})";
            return sqlStr;
        }

        public override string CreateUpdateSQL(BaseEntity entity)
        {
            Student student = entity as Student;
            string sqlStr = $"UPDATE tblStudent SET StudentRating = {student.Rating} WHERE id = {student.Id}";
            return sqlStr;
        }

        public override void Update(BaseEntity entity)
        {
            Student student = entity as Student;

            if (student != null)
            {
                this.updated.Add(new ChangeEntity(base.CreateUpdateSQL, entity));
                this.updated.Add(new ChangeEntity(this.CreateUpdateSQL, entity));
            }
        }


        protected override void CreateModel(BaseEntity entity)
        {
            Student student = entity as Student;

            base.CreateModel(student);
            student.Rating = Convert.ToDouble(this.reader["StudentRating"]);
            student.SessionsCompleted = (int)this.reader["SessionsCompleted"];
        }

        public override void Insert(BaseEntity entity)
        {
            Student student = entity as Student;

            if (student != null)
            {
                this.inserted.Add(new ChangeEntity(base.CreateInsertSQL, entity));
                this.inserted.Add(new ChangeEntity(this.CreateInsertSQL, entity));
            }
        }

        protected override BaseEntity NewEntity()
        {
            return new Student();
        }

        public StudentList SelectAll()
        {
            this.command.CommandText = $@"SELECT  tblUsers.id, tblUsers.Username, tblUsers.[Password], tblUsers.Email, 
                                                tblUsers.FirstName, tblUsers.LastName, tblUsers.City,
                                                tblStudent.StudentRating, tblStudent.SessionsCompleted
                                         FROM   (tblStudent INNER JOIN
                                                       tblUsers ON tblStudent.id = tblUsers.id)";

            return new StudentList(base.Select());
        }

        public Student Login(string username, string password)
        {
            this.command.CommandText = $@"
        SELECT  tblUsers.id, tblUsers.Username, tblUsers.[Password], tblUsers.Email,
                tblUsers.FirstName, tblUsers.LastName, tblUsers.City,
                tblStudent.StudentRating, tblStudent.SessionsCompleted
        FROM    tblStudent 
        INNER JOIN tblUsers ON tblStudent.id = tblUsers.id
        WHERE tblUsers.Username = '{username}' AND tblUsers.[Password] = '{password}'";

            StudentList students = new StudentList(base.Select());

            if (students.Count > 0)
                return students[0];

            return null;
        }
        public void AddRating(int studentId, int newRating)
        {
            // Get current rating info
            this.command.CommandText = $"SELECT StudentRating FROM tblStudent WHERE id = {studentId}";
            var list = base.Select();
            if (list.Count > 0)
            {
                Student student = list[0] as Student;
                if (student != null)
                {
                    double currentRating = student.Rating;
                    
                    double finalRating = currentRating == 0 ? newRating : (currentRating + newRating) / 2;
                    
                    this.command.CommandText = $"UPDATE tblStudent SET StudentRating = {finalRating} WHERE id = {studentId}";
                    this.command.ExecuteNonQuery();
                }
            }
        }
    }
}
