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
        public int Rating { get => rating; set => rating = value; }

    }
}
