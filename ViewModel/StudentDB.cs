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
            throw new NotImplementedException();
        }

        public override string CreateUpdateSQL(BaseEntity entity)
        {
            throw new NotImplementedException();
        }


        protected override void CreateModel(BaseEntity entity)
        {
            Student student = entity as Student;

            base.CreateModel(student);

            // Helper to check if the reader has a given column
            Func<string, bool> hasColumn = name =>
            {
                for (int i = 0; i < this.reader.FieldCount; i++)
                    if (string.Equals(this.reader.GetName(i), name, StringComparison.OrdinalIgnoreCase))
                        return true;
                return false;
            };

            // Map StudentRating (database column) to Student.Rating
            if (hasColumn("StudentRating") && this.reader["StudentRating"] != DBNull.Value)
                student.Rating = Convert.ToInt32(this.reader["StudentRating"]);

            // Map InstId if present
            if (hasColumn("InstId") && this.reader["InstId"] != DBNull.Value)
                student.InstId = Convert.ToInt32(this.reader["InstId"]);
        }


        protected override BaseEntity NewEntity()
        {
            return new Student();
        }

        public StudentList SelectAll()
        {
            this.command.CommandText = $@"SELECT  tblUsers.id, tblUsers.Username, tblUsers.[Password], tblUsers.Email, 
                                                tblUsers.FirstName, tblUsers.LastName, tblUsers.City,
                                                tblStudent.StudentRating, tblStudent.InstId
                                         FROM   (tblStudent INNER JOIN
                                                       tblUsers ON tblStudent.id = tblUsers.id)";

            return new StudentList(base.Select());
        }

        public Student Login(string username, string password)
        {
            this.command.CommandText = $@"
        SELECT  tblUsers.id, tblUsers.Username, tblUsers.[Password], tblUsers.Email,
                tblUsers.FirstName, tblUsers.LastName, tblUsers.City,
                tblStudent.StudentRating
        FROM    tblStudent 
        INNER JOIN tblUsers ON tblStudent.id = tblUsers.id
        WHERE tblUsers.Username = '{username}' AND tblUsers.[Password] = '{password}'";

            StudentList students = new StudentList(base.Select());

            if (students.Count > 0)
                return students[0];

            return null;
        }
    }
}
