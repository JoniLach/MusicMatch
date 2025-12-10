using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Teacher : User
    {
        private int rating;
        private int price;
        private int amountOfJobs;

        public int Rating { get => rating; set => rating = value; }
        public int Price { get => price; set => price = value; }
        public int AmountOfJobs { get => amountOfJobs; set => amountOfJobs = value; }
    }
}
