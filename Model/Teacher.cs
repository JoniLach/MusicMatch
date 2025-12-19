using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Teacher : User
    {
        private double rating;
        private double price;
        private int amountOfJobs;
        private string description;

        public double Rating { get => rating; set => rating = value; }
        public double Price { get => price; set => price = value; }
        public int AmountOfJobs { get => amountOfJobs; set => amountOfJobs = value; }
        public string Description { get => description; set => description = value; }
    }
}
