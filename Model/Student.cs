using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Student : User
    {
        private double rating;
        private int sessionsCompleted;
        public double Rating { get => rating; set => rating = value; }
        public int SessionsCompleted { get => sessionsCompleted; set => sessionsCompleted = value; }
    }
}
