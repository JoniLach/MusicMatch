using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public abstract class UserDB : BaseDB
    {
        protected override void CreateModel(BaseEntity entity)
        {
            User user = entity as User;

            // -- Users
            user.Id = (int)this.reader["id"];
            user.FirstName = this.reader["FirstName"].ToString();
            user.LastName = this.reader["LastName"].ToString();
            user.UserName = this.reader["UserName"].ToString();
            user.Password = this.reader["Password"].ToString();
            user.Email = this.reader["Email"].ToString();
            user.Email = this.reader["Email"].ToString();
            user.City = this.reader["City"].ToString();
            user.ProfilePicture = this.reader["ProfilePicture"] != DBNull.Value ? this.reader["ProfilePicture"].ToString() : "";
        }

        public override string CreateInsertSQL(BaseEntity entity)
        {
            User user = entity as User;
            string pic = user.ProfilePicture ?? "";
            string sqlStr = $"INSERT INTO tblUsers (FirstName, LastName, UserName, [Password], Email, City, ProfilePicture) VALUES ('{user.FirstName}', '{user.LastName}', '{user.UserName}', '{user.Password}', '{user.Email}', '{user.City}', '{pic}')";
            return sqlStr;
        }

        public override string CreateUpdateSQL(BaseEntity entity)
        {
            User user = entity as User;
            string pic = user.ProfilePicture ?? "";
            string sqlStr = $"UPDATE tblUsers SET FirstName = '{user.FirstName}', LastName = '{user.LastName}', UserName = '{user.UserName}', [Password] = '{user.Password}', Email = '{user.Email}', City = '{user.City}', ProfilePicture = '{pic}' WHERE id = {user.Id}";
            return sqlStr;
        }

        public override string CreateDeleteSQL(BaseEntity entity)
        {
            User user = entity as User;
            string sqlStr = $"DELETE FROM tblUsers WHERE id = {user.Id}";
            return sqlStr;
        }
    }
}
