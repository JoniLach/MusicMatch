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
            string desc = teacher.Description?.Replace("'", "''") ?? ""; // Escape single quotes
            string sqlStr = $@"INSERT INTO tblTeacher 
                                (id, Rating, Price, AmountOfJobs, Description)
                                VALUES 
                                ({teacher.Id}, {teacher.Rating}, {teacher.Price}, {teacher.AmountOfJobs}, '{desc}')";
            return sqlStr;
        }

        public override string CreateUpdateSQL(BaseEntity entity)
        {
            Teacher teacher = entity as Teacher;
            string desc = teacher.Description?.Replace("'", "''") ?? ""; // Escape single quotes
            string sqlStr = $"UPDATE tblTeacher SET Price = {teacher.Price}, AmountOfJobs = {teacher.AmountOfJobs}, Rating = {teacher.Rating}, Description = '{desc}' WHERE id = {teacher.Id}";
            return sqlStr;
        }

        public override void Update(BaseEntity entity)
        {
            Teacher teacher = entity as Teacher;

            if (teacher != null)
            {
                this.updated.Add(new ChangeEntity(base.CreateUpdateSQL, entity));
                this.updated.Add(new ChangeEntity(this.CreateUpdateSQL, entity));
            }
        }

        protected override BaseEntity NewEntity()
        {
            return new Teacher();
        }

        protected override void CreateModel(BaseEntity entity)
        {
            Teacher teacher = entity as Teacher;

            base.CreateModel(entity);

            teacher.Rating = Convert.ToDouble(this.reader["Rating"]);
            teacher.Price = Convert.ToDouble(this.reader["Price"]);
            teacher.AmountOfJobs = (int)this.reader["AmountOfJobs"];
            teacher.Description = this.reader["Description"]?.ToString() ?? "";
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
                tblUsers.FirstName, tblUsers.LastName, tblUsers.City, tblUsers.ProfilePicture,
                tblTeacher.Rating, tblTeacher.Price, tblTeacher.AmountOfJobs, tblTeacher.Description
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
                        tblUsers.FirstName, tblUsers.LastName, tblUsers.City, tblUsers.ProfilePicture,
                        tblTeacher.Rating, tblTeacher.Price, tblTeacher.AmountOfJobs, tblTeacher.Description
                FROM    tblTeacher 
                INNER JOIN tblUsers ON tblTeacher.id = tblUsers.id";

            return new TeacherList(base.Select());
        }
        public void AddRating(int teacherId, int newRating)
        {
            // Get current rating info
            this.command.CommandText = $"SELECT Rating, AmountOfJobs FROM tblTeacher WHERE id = {teacherId}";
            var list = base.Select();
            if (list.Count > 0)
            {
                Teacher teacher = list[0] as Teacher;
                if (teacher != null)
                {
                    double currentRating = teacher.Rating;
                    int jobs = teacher.AmountOfJobs;

                    // Calculate new average
                    double updatedRating = ((currentRating * jobs) + newRating) / (jobs + 1);
                    int updatedJobs = jobs + 1;

                    // Update DB
                    this.command.CommandText = $"UPDATE tblTeacher SET Rating = {updatedRating}, AmountOfJobs = {updatedJobs} WHERE id = {teacherId}";
                    this.command.ExecuteNonQuery();
                }
            }
        }
    }
}