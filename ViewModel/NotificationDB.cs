using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class NotificationDB : BaseDB
    {
        protected override BaseEntity NewEntity()
        {
            return new Notification();
        }

        protected override void CreateModel(BaseEntity entity)
        {
            Notification notification = entity as Notification;
            notification.Id = (int)this.reader["id"];
            notification.UserId = (int)this.reader["UserId"];
            notification.Message = this.reader["Message"].ToString();
            notification.IsRead = (bool)this.reader["IsRead"];
            notification.CreatedAt = (DateTime)this.reader["CreatedAt"];
        }

        public override string CreateInsertSQL(BaseEntity entity)
        {
            Notification notification = entity as Notification;
            string dateStr = notification.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss");
            string message = notification.Message?.Replace("'", "''") ?? "";
            string sqlStr = $"INSERT INTO tblNotifications (UserId, Message, IsRead, CreatedAt) VALUES ({notification.UserId}, '{message}', 0, #{dateStr}#)";
            return sqlStr;
        }

        public override string CreateUpdateSQL(BaseEntity entity)
        {
            Notification notification = entity as Notification;
            string isReadVal = notification.IsRead ? "True" : "False";
            string sqlStr = $"UPDATE tblNotifications SET IsRead = {isReadVal} WHERE id = {notification.Id}";
            return sqlStr;
        }

        public override string CreateDeleteSQL(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public NotificationList GetUnreadNotifications(int userId)
        {
            this.command.CommandText = $"SELECT * FROM tblNotifications WHERE UserId = {userId} AND IsRead = False ORDER BY CreatedAt DESC";
            return new NotificationList(base.Select());
        }

        public void MarkAsRead(int notificationId)
        {
            try
            {
                this.connection.Open();
                this.command.CommandText = $"UPDATE tblNotifications SET IsRead = True WHERE id = {notificationId}";
                this.command.ExecuteNonQuery();
            }
            finally
            {
                if (this.connection.State == System.Data.ConnectionState.Open)
                    this.connection.Close();
            }
        }
    }
}
