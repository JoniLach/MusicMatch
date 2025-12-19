using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class NotificationList : List<Notification>
    {
        public NotificationList() { }
        public NotificationList(IEnumerable<Notification> list) : base(list) { }
        public NotificationList(IEnumerable<BaseEntity> list) : base(list.Cast<Notification>().ToList()) { }
    }
}
