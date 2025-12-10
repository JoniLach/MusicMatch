using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Student : User
    {
        private int rating;
        private int instId;
        public int Rating { get => rating; set => rating = value; }
        public int InstId { get => instId; set => instId = value; }
    }
}
