using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace ViewModel
{
    public class TeacherDB : UserDB
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

        protected override BaseEntity NewEntity()
        {
            return new Teacher();
        }

        protected override void CreateModel(BaseEntity entity)
        {
            Teacher teacher = entity as Teacher;

            base.CreateModel(entity);
            teacher.Rating = (int)this.reader["Rating"];
            teacher.Price = (int)this.reader["Price"];
            teacher.AmountOfJobs = (int)this.reader["AmountOfJobs"];
        }

        public Teacher Login(string username, string password)
        {
            // Corrected SQL query with the missing FROM clause and table validation
            this.command.CommandText = @"
            SELECT tblUsers.id, tblUsers.Username, tblUsers.Password, tblUsers.FirstName, tblUsers.LastName, tblUsers.Email, tblUsers.City, tblTeachers.Rating, tblTeacher.Price, tblTeacher.AmountOfJobs
            FROM tblUsers
            INNER JOIN tblTeacher ON tblUsers.id = tblTeacher.UserId
            WHERE tblUsers.Username = ? AND tblUsers.Password = ?";

            // Add parameters to the command
            this.command.Parameters.Clear();
            this.command.Parameters.AddWithValue("?", username);
            this.command.Parameters.AddWithValue("?", password);

            // Execute the query and populate the TeacherList
            TeacherList teachers = new TeacherList(base.Select());

            if (teachers.Count > 0)
                return teachers[0];

            return null;
        }

    }
}