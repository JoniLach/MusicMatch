using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Notification : BaseEntity
    {
        private int userId;
        private string message;
        private bool isRead;
        private DateTime createdAt;

        public int UserId { get => userId; set => userId = value; }
        public string Message { get => message; set => message = value; }
        public bool IsRead { get => isRead; set => isRead = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
    }
}
