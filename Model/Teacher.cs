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
        private string description;

        public int Rating { get => rating; set => rating = value; }
        public int Price { get => price; set => price = value; }
        public int AmountOfJobs { get => amountOfJobs; set => amountOfJobs = value; }
        public string Description { get => description; set => description = value; }
    }
}
