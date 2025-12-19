using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization.Configuration;

namespace Model
{
    public abstract class User : BaseEntity
    {
        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private string userName;
        private string city;
        private string profilePicture;

        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
        public string City { get => city; set => city = value; }
        public string ProfilePicture { get => profilePicture; set => profilePicture = value; }
    }
}
