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
            Teacher teacher = entity as Teacher;
            string sqlStr = $@"INSERT INTO tblTeacher 
                                (id, Rating, Price, AmountOfJobs)
                                VALUES 
                                ({teacher.Id}, {teacher.Rating}, {teacher.Price}, {teacher.AmountOfJobs})";
            return sqlStr;
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
        public override void Insert(BaseEntity entity)
        {
            Teacher teacher = entity as Teacher;

            if (teacher != null)
            {
                this.inserted.Add(new ChangeEntity(base.CreateInsertSQL, entity));
                this.inserted.Add(new ChangeEntity(this.CreateInsertSQL, entity));
            }
        }
        public Teacher Login(string username, string password)
        {
            // Corrected SQL query with the missing FROM clause
            this.command.CommandText = $@"
        SELECT  tblUsers.id, tblUsers.Username, tblUsers.[Password], tblUsers.Email,
                tblUsers.FirstName, tblUsers.LastName, tblUsers.City,
                tblTeacher.Rating, tblTeacher.Price, tblTeacher.AmountOfJobs
        FROM    tblTeacher 
        INNER JOIN tblUsers ON tblTeacher.id = tblUsers.id
        WHERE tblUsers.Username = '{username}' AND tblUsers.[Password] = '{password}'";

            // Execute the query and populate the TeacherList
            TeacherList teachers = new TeacherList(base.Select());

            if (teachers.Count > 0)
                return teachers[0];

            return null;
        }

        public TeacherList SelectAll()
        {
            this.command.CommandText = @"
                SELECT  tblUsers.id, tblUsers.Username, tblUsers.[Password], tblUsers.Email,
                        tblUsers.FirstName, tblUsers.LastName, tblUsers.City,
                        tblTeacher.Rating, tblTeacher.Price, tblTeacher.AmountOfJobs
                FROM    tblTeacher 
                INNER JOIN tblUsers ON tblTeacher.id = tblUsers.id";

            return new TeacherList(base.Select());
        }
    }
}